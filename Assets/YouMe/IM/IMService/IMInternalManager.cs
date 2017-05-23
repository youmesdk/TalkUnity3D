using System;
using System.Collections.Generic;
using UnityEngine;
using YIMEngine;
using YouMe;

public class IMInternalManager :
    YIMEngine.LoginListen,
    YIMEngine.MessageListen,
    YIMEngine.ChatRoomListen,
    YIMEngine.DownloadListen,
    YIMEngine.ContactListen,
    YIMEngine.AudioPlayListen,
    YIMEngine.LocationListen
{

    private static IMInternalManager _instance;
    public static IMInternalManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new IMInternalManager();
            }
            return _instance;
        }
    }

    private IMUser _lastLoginUser;
    public IMUser LastLoginUser
    {
        get
        {
            return _lastLoginUser;
        }
    }

    private Dictionary<ulong, MessageCallbackObject> messageCallbackQueue = new Dictionary<ulong, MessageCallbackObject>(10);
    private Dictionary<ulong, Action<YouMe.StatusCode, string>> downloadCallbackQueue = new Dictionary<ulong, Action<YouMe.StatusCode, string>>(10);

    public bool AddMessageCallback(ulong reqID, MessageCallbackObject callback)
    {
        if (!messageCallbackQueue.ContainsKey(reqID))
        {
            messageCallbackQueue.Add(reqID, callback);
        }
        else
        {
            Log.e("message id is already in sending queue.");
            return false;
        }
        return true;
    }

    public bool AddDownloadCallback(ulong reqID, Action<YouMe.StatusCode, string> callback)
    {
        if (!downloadCallbackQueue.ContainsKey(reqID))
        {
            downloadCallbackQueue.Add(reqID, callback);
        }
        else
        {
            Log.e("file already in download queue.");
            return false;
        }
        return true;
    }

    private IMInternalManager()
    {
        var youmeObj = new GameObject("__YIMGameObjectV2__");
        GameObject.DontDestroyOnLoad(youmeObj);
        youmeObj.hideFlags = HideFlags.DontSave;
        youmeObj.AddComponent<YIMBehaviour>();

        YIMEngine.IMAPI.Instance().SetLoginListen(this);
        YIMEngine.IMAPI.Instance().SetMessageListen(this);
        YIMEngine.IMAPI.Instance().SetDownloadListen(this);
        YIMEngine.IMAPI.Instance().SetChatRoomListen(this);
        YIMEngine.IMAPI.Instance().SetContactListen(this);
        YIMEngine.IMAPI.Instance().SetAudioPlayListen(this);
        YIMEngine.IMAPI.Instance().SetLocationListen(this);
    }

    private class YIMBehaviour : MonoBehaviour
    {

        void OnApplicationQuit()
        {
#if UNITY_EDITOR
            IMAPI.Instance().Logout();
#else
            IMAPI.Instance().UnInit();
#endif
        }

        void OnApplicationPause(bool isPause)
        {
            if (isPause)
            {
                IMAPI.Instance().OnPause();
            }
            else
            {
                IMAPI.Instance().OnResume();
            }
        }

    }


    #region YouMeLoginListen implementation

    public void OnLogin(YIMEngine.ErrorCode errorcode, string iYouMeID)
    {
        if (errorcode == YIMEngine.ErrorCode.Success)
        {
            _lastLoginUser = new IMUser(iYouMeID);
        }
        if (IMClient.Instance.ConnectListener != null)
        {
            IMConnectEvent e = new IMConnectEvent(Conv.ErrorCodeConvert(errorcode), errorcode == 0 ?
            ConnectEventType.CONNECTED : ConnectEventType.CONNECT_FAIL, iYouMeID);
            IMClient.Instance.ConnectListener(e);
        }
    }

    public void OnLogout()
    {
        if (IMClient.Instance.ConnectListener != null)
        {
            IMConnectEvent e = new IMConnectEvent(YouMe.StatusCode.Success, ConnectEventType.DISCONNECTED, "");
            IMClient.Instance.ConnectListener(e);
        }
    }

    #endregion


    #region YouMeIMMessageListen implementation

    public void OnSendMessageStatus(ulong iRequestID, YIMEngine.ErrorCode errorcode)
    {
        MessageCallbackObject callbackObj = null;
        bool finded = messageCallbackQueue.TryGetValue(iRequestID, out callbackObj);
        if (finded)
        {
            if (callbackObj != null && callbackObj.callback != null)
            {
                try
                {
                    switch (callbackObj.msgType)
                    {
                        case MessageBodyType.TXT:
                            Action<YouMe.StatusCode, TextMessage> call = (Action<YouMe.StatusCode, TextMessage>)callbackObj.callback;
                            var msg = (TextMessage)callbackObj.message;
                            msg.sendTime = TimeUtil.ConvertToTimestamp(System.DateTime.Now);
                            if (errorcode == YIMEngine.ErrorCode.Success)
                            {
                                msg.sendStatus = SendStatus.Sended;
                            }
                            else
                            {
                                msg.sendStatus = SendStatus.Fail;
                            }
                            msg.isReceiveFromServer = false;
                            call(Conv.ErrorCodeConvert(errorcode), msg);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Log.e(e.ToString());
                }
            }
            messageCallbackQueue.Remove(iRequestID);
        }
    }

    public void OnRecvMessage(MessageInfoBase message)
    {
        if (IMClient.Instance.ReceiveMessageListener != null)
        {
            IMMessage messageObj = null;
            switch (message.MessageType)
            {
                case MessageBodyType.TXT:
                    {
                        var txtMsg = (YIMEngine.TextMessage)message;
                        var msg = new TextMessage(message.SenderID, message.RecvID, (ChatType)message.ChatType, txtMsg.Content, true);
                        msg.sendTime = (uint)message.CreateTime;
                        msg.sendStatus = SendStatus.Sended;
                        messageObj = msg;
                    }
                    break;
                case MessageBodyType.Voice:
                    {
                        var voiceMsg = (YIMEngine.VoiceMessage)message;
                        var msg = new AudioMessage(message.SenderID, voiceMsg.RecvID, (ChatType)message.ChatType, voiceMsg.Param, true);
                        msg.recognizedText = voiceMsg.Text;
                        msg.audioDuration = voiceMsg.Duration;
                        msg.sendTime = (uint)message.CreateTime;
                        msg.sendStatus = SendStatus.Sended;
                        messageObj = msg;
                    }
                    break;
                default:
                    Log.e("unknown message type:" + message.MessageType.ToString());
                    break;
            }

            if (messageObj != null)
            {
                messageObj.requestID = message.RequestID;
                IMClient.Instance.ReceiveMessageListener(messageObj);
            }
        }

    }

    public void OnRecvNewMessage(YIMEngine.ChatType chatType, string targetID)
    {

    }

    /*录音结束 */
    public void OnStartSendAudioMessage(ulong iRequestID, YIMEngine.ErrorCode errorcode, string strText, string strAudioPath, int iDuration)
    {
        OnSendAudioMessageStatusChange(iRequestID, errorcode, strText, strAudioPath, iDuration, false);
    }

    /*发送结束 */
    public void OnSendAudioMessageStatus(ulong iRequestID, YIMEngine.ErrorCode errorcode, string strText, string strAudioPath, int iDuration)
    {
        OnSendAudioMessageStatusChange(iRequestID, errorcode, strText, strAudioPath, iDuration, true);
    }

    private void OnSendAudioMessageStatusChange(ulong iRequestID, YIMEngine.ErrorCode errorcode, string strText, string strAudioPath, int iDuration, bool isFinish)
    {
        MessageCallbackObject callbackObj = null;
        bool finded = messageCallbackQueue.TryGetValue(iRequestID, out callbackObj);
        if (finded)
        {
            if (callbackObj != null && callbackObj.callback != null)
            {

                Action<YouMe.StatusCode, AudioMessage> call = (Action<YouMe.StatusCode, AudioMessage>)callbackObj.callback;
                var msg = (AudioMessage)callbackObj.message;
                msg.recognizedText = strText;
                msg.audioFilePath = strAudioPath;
                msg.audioDuration = iDuration;
                if (!isFinish)
                {
                    msg.sendTime = TimeUtil.ConvertToTimestamp(System.DateTime.Now);
                }
                if (errorcode == YIMEngine.ErrorCode.Success)
                {
                    msg.sendStatus = isFinish ? SendStatus.Sended : SendStatus.Sending;
                    if (isFinish)
                    {
                        msg.downloadStatus = MessageDownloadStatus.DOWNLOADED;
                    }
                }
                else
                {
                    msg.sendStatus = SendStatus.Fail;
                }
                msg.isReceiveFromServer = false;
                call(Conv.ErrorCodeConvert(errorcode), msg);

            }
            messageCallbackQueue.Remove(iRequestID);
        }
    }

    //获取消息历史纪录回调
    public void OnQueryHistoryMessage(YIMEngine.ErrorCode errorcode, string targetID, int remain, List<YIMEngine.HistoryMsg> messageList)
    {

    }


    //语音上传后回调
    public void OnStopAudioSpeechStatus(YIMEngine.ErrorCode errorcode, ulong iRequestID, string strDownloadURL, int iDuraton, int iFileSize, string strLocalPath, string strText)
    {

    }

    //举报结果通知
    public void OnAccusationResultNotify(AccusationDealResult result, string userID, uint accusationTime)
    {

    }

    #endregion

    #region OnJoinGroupRequest implementation

    public void OnJoinRoom(YIMEngine.ErrorCode errorcode, string iChatRoomID)
    {
        if (IMClient.Instance.ChannelEventListener != null)
        {
            ChannelEventType et = errorcode == YIMEngine.ErrorCode.Success ? ChannelEventType.JOIN_SUCCESS : ChannelEventType.JOIN_FAIL;
            IMClient.Instance.ChannelEventListener(new ChannelEvent(Conv.ErrorCodeConvert(errorcode), et, iChatRoomID));
        }
    }
    public void OnLeaveRoom(YIMEngine.ErrorCode errorcode, string iChatRoomID)
    {
        if (IMClient.Instance.ChannelEventListener != null)
        {
            ChannelEventType et = errorcode == YIMEngine.ErrorCode.Success ? ChannelEventType.LEAVE_SUCCESS : ChannelEventType.LEAVE_FAIL;
            IMClient.Instance.ChannelEventListener(new ChannelEvent(Conv.ErrorCodeConvert(errorcode), et, iChatRoomID));
        }
    }
    //其他成员进出频道的通知，需要联系游密开启通知功能
    public void OnUserJoinChatRoom(string strRoomID, string strUserID)
    {

    }
    public void OnUserLeaveChatRoom(string strRoomID, string strUserID)
    {

    }
    #endregion

    #region DownloadListen implementation
    public void OnDownload(YIMEngine.ErrorCode errorcode, YIMEngine.MessageInfoBase message, string strSavePath)
    {

        Action<YouMe.StatusCode, string> callbackObj = null;
        bool finded = downloadCallbackQueue.TryGetValue(message.RequestID, out callbackObj);
        if (finded)
        {
            if (callbackObj != null)
            {
                try
                {
                    callbackObj(Conv.ErrorCodeConvert(errorcode), strSavePath);
                }
                catch (Exception e)
                {
                    Log.e("OnDownload error:" + e.ToString());
                }
            }
            downloadCallbackQueue.Remove(message.RequestID);
        }
    }

    public void OnDownloadByUrl(YIMEngine.ErrorCode errorcode, string strFromUrl, string strSavePath)
    {

    }

    #endregion

    #region ContactListen implementation
    public void OnGetContact(List<string> contactLists)
    {

    }
    public void OnGetUserInfo(YIMEngine.ErrorCode code, IMUserInfo userInfo)
    {

    }
    public void OnQueryUserStatus(YIMEngine.ErrorCode code, string userID, UserStatus status)
    {

    }
    #endregion

    #region YIMEngine.LocationListen implementation

    public void OnUpdateLocation(YIMEngine.ErrorCode errorcode, YIMEngine.GeographyLocation location)
    {

    }

    public void OnGetNearbyObjects(YIMEngine.ErrorCode errorcode, List<YIMEngine.RelativeLocation> neighbourList, uint startDistance, uint endDistance)
    {

    }

    #endregion

    #region YIMEngine.AudioPlayListen implementation
    public void OnPlayCompletion(YIMEngine.ErrorCode errorcode, string path)
    {

    }
    #endregion
}

public class MessageCallbackObject
{
    public object callback;
    public IMMessage message;
    public MessageBodyType msgType;
    public MessageCallbackObject(IMMessage msg, MessageBodyType msgType, object call)
    {
        this.callback = call;
        this.message = msg;
        this.msgType = msgType;
    }
}