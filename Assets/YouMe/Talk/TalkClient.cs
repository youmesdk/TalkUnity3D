using UnityEngine;
using System.Collections;
using System;

namespace YouMe
{
    public class TalkClient : IClient
    {

        static TalkClient _ins;
        public static TalkClient Instance
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new TalkClient();
                }
                return _ins;
            }
        }

		private TalkInternalManager talkManager;
        private TalkClient()
        {
            talkManager = TalkInternalManager.Instance;
        }

        private string _userID="";
        private TalkConfig _config;

        /// <summary>
        /// 设置配置信息
        /// </summary>
        /// <param name="appKey">游密官方分配的APP唯一标识，在www.youme.im注册后可以自助获取</param>
        /// <param name="secretKey">游密官方分配的接入密钥，和appkey配对使用，在www.youme.im注册后可以自助获取</param>
        /// <param name="config">SDK配置对象，可以通过该对象的参数设置SDK的可选参数，比如服务器区域、日志级别</param> <summary>
        /// <param name="initCallBack">初始化结果回调</param>
        /// 
        public void Initialize(string appKey, string secretKey, TalkConfig config , Action<InitEvent> initCallBack)
        {
            YouMeErrorCode errorCode;
            _config = config;
            talkManager.initCallBack = initCallBack;
            if ( config != null )
            {
               if( !string.IsNullOrEmpty(config.ExtServerZone)){
				    errorCode = YouMeVoiceAPI.GetInstance ().Init (appKey, secretKey,YOUME_RTC_SERVER_REGION.RTC_EXT_SERVER,config
				   .ExtServerZone);
			   }else{
                    errorCode = YouMeVoiceAPI.GetInstance ().Init (appKey, secretKey,config.ServerZone,"");
			   }
            }else { 
				 errorCode = YouMeVoiceAPI.GetInstance ().Init (appKey, secretKey,YOUME_RTC_SERVER_REGION.RTC_DEFAULT_SERVER,"");
			}
            if( errorCode != YouMeErrorCode.YOUME_SUCCESS && initCallBack!=null){
                talkManager.initCallBack = null;
                initCallBack(new InitEvent(errorCode));
            }
        }

        public TalkClient SetUserID(string userID){
            _userID = userID;
            return this;
        }

        public string GetUserID(){
            return _userID;
        }

        public void JoinChannel(TalkChannel channel,Action<ChannelEvent> callback,YouMeUserRole role = YouMeUserRole.YOUME_USER_TALKER_FREE)
        {
            if(string.IsNullOrEmpty(GetUserID())){
                Log.e("Need SetUserID(string userid).");
                if(callback!=null){
                    callback(new ChannelEvent(StatusCode.USER_ID_IS_EMPTY, ChannelEventType.JOIN_FAIL, channel.ChannelID));
                }
                return;
            }
            talkManager.AddJoinChannelCallback(channel.ChannelID,callback);
            YouMeErrorCode errorCode;
            if(_config.AllowMultiChannel){
                errorCode = YouMeVoiceAPI.GetInstance().JoinChannelMultiMode(GetUserID(), channel.ChannelID);
            }else{
                errorCode = YouMeVoiceAPI.GetInstance().JoinChannelSingleMode(GetUserID(), channel.ChannelID, (int)role);
            }

            if( errorCode != YouMeErrorCode.YOUME_SUCCESS && callback!=null ){
                talkManager.RemoveJoinChannelCallback(channel.ChannelID);
                callback(new ChannelEvent( Conv.ErrorCodeConvert(errorCode),ChannelEventType.JOIN_FAIL,channel.ChannelID ));
            }
        }

        public void LeaveChannel(TalkChannel channel,Action<ChannelEvent> callback){
            if(channel.ChannelID==null){
                Log.e("channel ID can't be null.");
                return;
            }
            if(_config.AllowMultiChannel){
                YouMeErrorCode errorCode = YouMeVoiceAPI.GetInstance().LeaveChannelMultiMode(channel.ChannelID);
                if( errorCode != YouMeErrorCode.YOUME_SUCCESS && callback!=null ){
                    callback(new ChannelEvent( Conv.ErrorCodeConvert(errorCode),ChannelEventType.LEAVE_FAIL,channel.ChannelID ));
                }else{
                    talkManager.AddLeaveChannelCallback(channel.ChannelID,callback);
                }
            }else{
                LeaveAllChannel(callback);
            }
        }

        public void LeaveAllChannel(Action<ChannelEvent> callback){
            YouMeErrorCode errorCode = YouMeVoiceAPI.GetInstance().LeaveChannelAll();
            if( errorCode != YouMeErrorCode.YOUME_SUCCESS && callback!=null ){
                callback(new ChannelEvent( Conv.ErrorCodeConvert(errorCode),ChannelEventType.LEAVE_FAIL,"" ));
            }else{
                talkManager.leaveAllChannelCallback = callback;
            }
        }

        public void SetServerRegion(string[] regionNames)
        {
            YouMeVoiceAPI.GetInstance().SetServerRegion(regionNames);
        }

        public void OpenSpeaker(){
            YouMeVoiceAPI.GetInstance().SetSpeakerMute(false);
        }

        public void CloseSpeaker(){
            YouMeVoiceAPI.GetInstance().SetSpeakerMute(true);
        }

        public void OpenMicrophone(){
            YouMeVoiceAPI.GetInstance().SetMicrophoneMute(false);
        }

        public void CloseMicrophone(){
            YouMeVoiceAPI.GetInstance().SetMicrophoneMute(true);
        }

        public bool IsSpeakerOpen(){
            return !YouMeVoiceAPI.GetInstance().GetSpeakerMute();
        }

        public bool IsMicrophoneOpen(){
            return !YouMeVoiceAPI.GetInstance().GetSpeakerMute();
        }

    }
}