using System;
using YIMEngine;

namespace YouMe
{

    public class IMClient : IClient
    {

        static IMClient _ins;
        public static IMClient Instance{
            get{
                if(_ins==null){
                    _ins = new IMClient();
                }
                return _ins;
            }
        }

        private IMClient(){
            IMManager = IMInternalManager.Instance;
            ConnectListener = OnConnect;
            ChannelEventListener = OnChannelEvent;
        }

        public static string FAKE_PAPSSWORD = "123456";

        public IMInternalManager IMManager;

        // event 
        public Action<IMConnectEvent> ConnectListener{set;get;}
        public Action<ChannelEvent> ChannelEventListener{set;get;}
        public Action<IMMessage> ReceiveMessageListener{get;set; }

        private Action<LoginEvent> loginCallback;
        private Action<LogoutEvent> logoutCallback;
        private Action<KickOffEvent> kickOffCallback;
        private Action<DisconnectEvent> disconnectCallback;

        private Action<ChannelEvent> joinChannelCallback;
        private Action<ChannelEvent> leaveChannelCallback;

        private AudioMessage lastRecordAudioMessage;

        /// <summary>
        /// 设置配置信息
        /// </summary>
        /// <param name="appKey">游密官方分配的APP唯一标识，在www.youme.im注册后可以自助获取</param>
        /// <param name="secretKey">游密官方分配的接入密钥，和appkey配对使用，在www.youme.im注册后可以自助获取</param>
        /// <param name="config">SDK配置对象，可以通过该对象的参数设置SDK的可选参数，比如服务器区域、日志级别、方言识别</param>
        /// <returns>IMClient对象</returns>
        public IMClient Initialize(string appKey, string secretKey, Config config)
        {
            if (config != null)
            {
                IMAPI.Instance().SetServerZone((YIMEngine.ServerZone)config.ServerZone);
            }
            IMAPI.Instance().Init(appKey, secretKey);
            return this;
        }

        /// <summary>
        /// 登录IM系统。成功登录IM系统后就可以进行私聊消息的收发，以及进出聊天频道，进行频道消息的收发。
        /// </summary>
        /// <param name="userID">用户ID或者游戏角色ID，唯一标识一个用户在应用里的身份</param>
        /// <param name="token">使用服务器token验证模式时使用该参数，否则使用空字符串""即可</param>
        /// <param name="callback">登录结果的回调通知，在此回调里判读登录是否成功</param>
        public void Login(string userID, string token, Action<LoginEvent> callback)
        {
            // login 
            loginCallback = callback;
            YIMEngine.ErrorCode code = IMAPI.Instance().Login(userID, FAKE_PAPSSWORD, token);
            if( code!=YIMEngine.ErrorCode.Success && ConnectListener != null ){
                IMConnectEvent e = new IMConnectEvent(Conv.ErrorCodeConvert(code),ConnectEventType.CONNECT_FAIL,userID);
                ConnectListener( e );
            }
            
        }

        public void SetReceiveMessageListener(Action<IMMessage> listener){
            ReceiveMessageListener = listener;
        }

        public void Logout(Action<LogoutEvent> callback)
        {
            // logout
            logoutCallback = callback;
            YIMEngine.ErrorCode code = IMAPI.Instance().Logout();
            if( code!=YIMEngine.ErrorCode.Success && ConnectListener != null ){
                IMConnectEvent e = new IMConnectEvent(Conv.ErrorCodeConvert(code),ConnectEventType.DISCONNECTED,"");
                ConnectListener( e );
            }
        }

        /// <summary>
        /// 被踢下线事件监听器，在同时登录多个相同userID的情况下，先登录的帐号会被后登录的帐号踢下线。
        /// </summary>
        /// <param name="callback">
        /// 回调绑定
        /// KickOffEvent：响应结果对象
        ///     KickOffEvent.UserID  用户ID
        ///     KickOffEvent.Code    事件结果，断线通知的情况下始终为SUCCESS
        /// </param>
        public void SetKickOffListener(Action<KickOffEvent> callback){
            kickOffCallback = callback;
        }

        /// <summary>
        /// 断线事件监听器，一般发生在网络异常的时候。
        /// </summary>
        /// <param name="callback">
        /// 回调绑定
        /// DisconnectEvent：响应结果对象
        ///     DisconnectEvent.UserID  用户ID
        ///     DisconnectEvent.Code    事件结果，断线通知的情况下始终为SUCCESS
        /// </param>
        public void SetDisconnectListener(Action<DisconnectEvent> callback) {
            disconnectCallback = callback;
        }

        public IMChannel[] GetCurrentChannels()
        {
            return new IMChannel[]{};
        }

        public ConnectStatus GetCurrentStatus()
        {
            throw new NotImplementedException();
        }

        public IUser GetCurrentUserID()
        {
            return IMManager.LastLoginUser;
        }

        public void JoinChannel(IMChannel channel,Action<ChannelEvent> callback)
        {
            joinChannelCallback = callback;
            var code = IMAPI.Instance().JoinChatRoom( channel.ChannelID );
            if( code!=YIMEngine.ErrorCode.Success && ChannelEventListener!=null ){
                ChannelEventListener(new ChannelEvent( Conv.ErrorCodeConvert(code),ChannelEventType.JOIN_FAIL,channel.ChannelID ));
            }
        }

        public void JoinMultiChannel(IMChannel[] channels,Action<ChannelEvent> callback)
        {
            joinChannelCallback = callback;
            for (int i = 0; i < channels.Length;i++){
                var code = IMAPI.Instance().JoinChatRoom(channels[i].ChannelID);
                if( code!=YIMEngine.ErrorCode.Success && ChannelEventListener!=null ){
                    ChannelEventListener(new ChannelEvent( Conv.ErrorCodeConvert(code),ChannelEventType.LEAVE_FAIL,channels[i].ChannelID ));
                }
            }
        }

        public void LeaveAllChannel(Action<ChannelEvent> callback)
        {
            leaveChannelCallback = callback;
            throw new NotImplementedException();
        }

        public void LeaveChannel(IMChannel channel,Action<ChannelEvent> callback)
        {
            leaveChannelCallback = callback;
            var code = IMAPI.Instance().LeaveChatRoom( channel.ChannelID );
            if( code!=YIMEngine.ErrorCode.Success && ChannelEventListener!=null ){
                ChannelEventListener(new ChannelEvent( Conv.ErrorCodeConvert(code),ChannelEventType.LEAVE_FAIL,channel.ChannelID ));
            }
        }

        public void ReConnect()
        {
            throw new NotImplementedException();
        }

        public IMClient SetDebug(bool isDebug)
        {
            if(isDebug){
                IMAPI.SetMode(0);
            }else{
                IMAPI.SetMode(2);
            }
            return this;
        }

        public IMClient SetLogLevel(LogLevel logLevel)
        {

            return this;
        }

        public void SwitchToChannels(IMChannel[] channel,Action<ChannelEvent> leaveCallback,Action<ChannelEvent> joinCallback)
        {
            LeaveAllChannel(leaveCallback);
            JoinMultiChannel(channel,joinCallback);
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="reciverID">接收者id，私聊就用用户id，频道聊天就用频道id</param>
        /// <param name="chatType">私聊消息还是频道消息</param>
        /// <param name="msgContent">文本消息内容</param>
        /// <param name="onSendCallBack">消息发送结果的回调通知</param>
        /// <returns>返回 TextMessage 实例</returns>
        public TextMessage SendTextMessage(string reciverID,ChatType chatType, string msgContent ,Action<StatusCode,TextMessage> onSendCallBack){
            ulong reqID = 0;
            YIMEngine.ErrorCode code = 0;
            code = IMAPI.Instance().SendTextMessage(reciverID, (YIMEngine.ChatType)chatType, msgContent, ref reqID);
            var msg = new TextMessage(GetCurrentUserID().UserID,reciverID,chatType,msgContent,false);
            if(code == YIMEngine.ErrorCode.Success){
                msg.sendStatus = SendStatus.Sending;
                msg.requestID = reqID;
                MessageCallbackObject callbackObj = new MessageCallbackObject(msg,MessageBodyType.TXT,onSendCallBack);
                IMInternalManager.Instance.AddMessageCallback(reqID, callbackObj);
            }else{
                msg.sendStatus = SendStatus.Fail;
                if(onSendCallBack!=null){
                    onSendCallBack(Conv.ErrorCodeConvert(code),msg);
                }
            }
            return msg;
        }

        /// <summary>
        /// 启动录音
        /// </summary>
        /// <param name="reciverID">接收者id，私聊就用用户id，频道聊天就用频道id</param>
        /// <param name="chatType">私聊消息还是频道消息</param>
        /// <param name="extraMsg">附带自定义文本消息内容</param>
        /// <param name="recognizeText">是否开启语音转文字识别功能</param>
        /// <param name="callback">语音消息发送回调通知，会通知多次，通过AudioMessage的sendStatus属性可以判断是哪个状态的回调</param>
        /// <returns></returns>
        public AudioMessage StartRecordAudio(string reciverID,ChatType chatType,string extraMsg,bool recognizeText,Action<StatusCode,AudioMessage> callback){
            ulong reqID = 0;
            YIMEngine.ErrorCode code = 0;
            if(recognizeText){
                code = IMAPI.Instance().SendAudioMessage(reciverID, (YIMEngine.ChatType)chatType, ref reqID);
            }else{
                code = IMAPI.Instance().SendOnlyAudioMessage(reciverID, (YIMEngine.ChatType)chatType, ref reqID);
            }
            var msg = new AudioMessage(GetCurrentUserID().UserID,reciverID,chatType,extraMsg,false);
            if(code == YIMEngine.ErrorCode.Success){
                msg.requestID = reqID;
                msg.sendStatus = SendStatus.NotStartSend;
                lastRecordAudioMessage = msg;
                MessageCallbackObject callbackObj = new MessageCallbackObject(msg,MessageBodyType.Voice,callback);
                IMInternalManager.Instance.AddMessageCallback(reqID, callbackObj);
            }else{
                msg.sendStatus = SendStatus.Fail;
                Log.e("Start Record Fail! code:"+code.ToString());
                if( callback!=null ){
                    callback(Conv.ErrorCodeConvert(code),msg);
                }
            }
            return msg;
        }

        /// <summary>
        /// 结束录音并发送语音消息
        /// </summary>
        /// <returns>false表示启动发送失败，true表示启动发送成功</returns>
        public bool StopRecordAndSendAudio(){
            if(lastRecordAudioMessage==null){
                return false;
            }
            var audioMsg = lastRecordAudioMessage;
            if(audioMsg.sendStatus == SendStatus.Fail){
                Log.e("StopRecordAndSendAudio Fail! SendStatus is Fail!");
                lastRecordAudioMessage = null;
                return false;
            }
            YIMEngine.ErrorCode code = IMAPI.Instance().StopAudioMessage(audioMsg.extraParam);
            lastRecordAudioMessage = null;
            if( code==YIMEngine.ErrorCode.Success ){
                return true;
            }else{
                Log.e("StopRecordAndSendAudio Fail! code:"+code.ToString());
                return false;
            }
        }

        public void DownloadFile(ulong requestID,string targetFilePath,Action<YouMe.StatusCode , string > downloadCallback){
            YIMEngine.ErrorCode code = IMAPI.Instance().DownloadAudioFile(requestID,targetFilePath);
            bool ret = false;
            if( code == YIMEngine.ErrorCode.Success ){
                ret = IMInternalManager.Instance.AddDownloadCallback( requestID, downloadCallback );
            }
            if(!ret && downloadCallback!=null){
                downloadCallback(YouMe.StatusCode.START_DOWNLOAD_FAIL,"");
            }
        }

        private void OnConnect(IMConnectEvent connectEvent)
        {
            switch ( connectEvent.EventType ){
                case ConnectEventType.CONNECTED: 
                    if( loginCallback != null ) loginCallback(new LoginEvent(connectEvent.Code,connectEvent.UserID));
                    break;
                case ConnectEventType.CONNECT_FAIL:
                     if( loginCallback != null ) loginCallback(new LoginEvent(connectEvent.Code,connectEvent.UserID));
                    break;
                case ConnectEventType.OFF_LINE:
                    if( disconnectCallback != null ) disconnectCallback(new DisconnectEvent(connectEvent.Code,connectEvent.UserID));
                    break;
                case ConnectEventType.DISCONNECTED:
                    if( logoutCallback != null ) logoutCallback(new LogoutEvent(connectEvent.Code,connectEvent.UserID));
                    break;
                case ConnectEventType.KICKED:
                    if( kickOffCallback != null ) kickOffCallback(new KickOffEvent(connectEvent.Code,connectEvent.UserID));
                    break;
                default:
                    break;
            }
        }

        void OnChannelEvent(ChannelEvent channelEvent){
            switch(channelEvent.EventType){
                case ChannelEventType.JOIN_SUCCESS:
                case ChannelEventType.JOIN_FAIL:
                    joinChannelCallback(channelEvent);
                    break;
                case ChannelEventType.LEAVE_FAIL:
                case ChannelEventType.LEAVE_SUCCESS:
                    leaveChannelCallback(channelEvent);
                    break;
            }
	    }
    }
}