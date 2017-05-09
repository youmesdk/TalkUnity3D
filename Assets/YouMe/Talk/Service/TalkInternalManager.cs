using UnityEngine;
using System.Collections.Generic;
using YouMe;
using System;

public class TalkInternalManager : MonoBehaviour
{

    public Action<InitEvent> initCallBack = null;
    public Dictionary<string, Action<ChannelEvent>> joinChannelCallbacks = new Dictionary<string, Action<ChannelEvent>>();
    public Dictionary<string, Action<ChannelEvent>> leaveChannelCallbacks = new Dictionary<string, Action<ChannelEvent>>();
    public Action<ChannelEvent> leaveAllChannelCallback = null;

    private static TalkInternalManager _instance;
    public static TalkInternalManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var youmeObj = new GameObject("__YTalkGameObjectV2__");
                GameObject.DontDestroyOnLoad(youmeObj);
                youmeObj.hideFlags = HideFlags.DontSave;
                _instance = youmeObj.AddComponent<TalkInternalManager>();
            }
            return _instance;
        }
    }

    // Use this for initialization
    void Awake()
    {
        YouMe.YouMeVoiceAPI.GetInstance().SetCallback(gameObject.name);
    }

    void CallbackProcess(string strParam)
    {
        string[] strSections = strParam.Split(new char[] { ',' });
        if (strSections == null)
        {

            return;
        }
        Log.e("strParam:" + strParam);
        //解析后得到两个字段，第一个为事件类型，第二个为错误码类型
        YouMe.YouMeEvent eventType = (YouMeEvent)int.Parse(strSections[0]);
        YouMe.YouMeErrorCode errorCode = (YouMeErrorCode)int.Parse(strSections[1]);
        string channelID = strSections[2];
        string param = strSections[3];

        switch (eventType)
        {
            case YouMe.YouMeEvent.YOUME_EVENT_INIT_OK:
                // 初始化成功
                if (initCallBack != null)
                {
                    initCallBack(new InitEvent(errorCode));
                }
                break;
            case YouMe.YouMeEvent.YOUME_EVENT_INIT_FAILED:
                // 初始化成功
                if (initCallBack != null)
                {
                    initCallBack(new InitEvent(errorCode));
                }
                break;

            case YouMe.YouMeEvent.YOUME_EVENT_JOIN_OK:
                // if (string.Equals (channelID, ARoomID)) 
                CallJoinChannelCallback(channelID,new ChannelEvent( Conv.ErrorCodeConvert(errorCode),ChannelEventType.JOIN_SUCCESS,channelID ));
                break;
			case YouMe.YouMeEvent.YOUME_EVENT_JOIN_FAILED:
				CallJoinChannelCallback(channelID,new ChannelEvent( Conv.ErrorCodeConvert(errorCode),ChannelEventType.JOIN_FAIL,channelID ));
                break;
            case YouMe.YouMeEvent.YOUME_EVENT_LEAVED_ONE:
                CallLeaveChannelCallback(channelID,new ChannelEvent( Conv.ErrorCodeConvert(errorCode),ChannelEventType.LEAVE_SUCCESS,channelID ));
                // if (string.Equals (channelID, ARoomID)) 
                break;
            case YouMe.YouMeEvent.YOUME_EVENT_LEAVED_ALL:
                if (leaveAllChannelCallback != null)
                {
                    leaveAllChannelCallback(new ChannelEvent(Conv.ErrorCodeConvert(errorCode), ChannelEventType.LEAVE_SUCCESS, ""));
                }
                // tipsText.text = "已退出所有频道";
                break;

            case YouMe.YouMeEvent.YOUME_EVENT_SPEAK_SUCCESS:

                // if (string.Equals (channelID, ARoomID)) {
                // tipsText.text = "可以对A房间说话";
                break;
            case YouMe.YouMeEvent.YOUME_EVENT_SPEAK_FAILED:
                // if (string.Equals (channelID, ARoomID)) {
                // 	tipsText.text = "对A房间说话的操作失败";
                break;
            case YouMe.YouMeEvent.YOUME_EVENT_OTHERS_VOICE_ON:
                Debug.LogError("YOUME_EVENT_OTHERS_VOICE_ON:" + param);
                break;
            case YouMe.YouMeEvent.YOUME_EVENT_OTHERS_VOICE_OFF:
                Debug.LogError("YOUME_EVENT_OTHERS_VOICE_OFF:" + param);
                break;
            default:
                // tipsText.text + "\n事件类型" + eventType + ",错误码" + errorCode;
                break;

        }
    }

    private List<System.Action> callbackQueue = new List<System.Action>(4);
    private readonly object syncAddCallback = new object();
    // Update is called once per frame
    void Update()
    {

        if (callbackQueue.Count > 0)
        {
            lock (syncAddCallback)
            {
                var call = callbackQueue[0];
                callbackQueue.RemoveAt(0);
                try
                {
                    call();
                }
                catch (System.Exception e)
                {
                    Log.e(e.StackTrace);
                }

            }
        }

    }

    //回调函数
    void OnEvent(string strParam)
    {
        lock (syncAddCallback)
        {
            callbackQueue.Add(() =>
            {
                try
                {
                    CallbackProcess( strParam );
                }catch(System.Exception e){
                    Log.e( e.StackTrace );
                }
            });
        }
    }

    public bool RemoveJoinChannelCallback(string channelID)
    {
        if (joinChannelCallbacks.ContainsKey(channelID))
        {
            return joinChannelCallbacks.Remove(channelID);
        }
        return false;
    }

    public void AddJoinChannelCallback(string channelID, Action<ChannelEvent> callback)
    {
        if (joinChannelCallbacks.ContainsKey(channelID))
        {
            joinChannelCallbacks[channelID] = callback;
        }
        else
        {
            joinChannelCallbacks.Add(channelID, callback);
        }
    }

    public bool CallJoinChannelCallback(string channelID,ChannelEvent evt){
        Action<ChannelEvent> call;
		if(joinChannelCallbacks.TryGetValue(channelID,out call)){
            try
            {
                call(evt);
            }catch(System.Exception e){
                Log.e(e.StackTrace);
            }
            joinChannelCallbacks.Remove(channelID);
            return true;
        }
        return false;
    }

     public bool RemoveLeaveChannelCallback(string channelID)
    {
        if (leaveChannelCallbacks.ContainsKey(channelID))
        {
            return leaveChannelCallbacks.Remove(channelID);
        }
        return false;
    }

    public void AddLeaveChannelCallback(string channelID, Action<ChannelEvent> callback)
    {
        if(channelID == null){
            Log.e("can not use null channel ID.");
            return;
        }
        if (leaveChannelCallbacks.ContainsKey(channelID))
        {
            leaveChannelCallbacks[channelID] = callback;
        }
        else
        {
            leaveChannelCallbacks.Add(channelID, callback);
        }
    }

    public bool CallLeaveChannelCallback(string channelID,ChannelEvent evt){
        Action<ChannelEvent> call;
		if(leaveChannelCallbacks.TryGetValue(channelID,out call)){
            try
            {
                call(evt);
            }catch(System.Exception e){
                Log.e(e.StackTrace);
            }
            leaveChannelCallbacks.Remove(channelID);
            return true;
        }
        return false;
    }

}
