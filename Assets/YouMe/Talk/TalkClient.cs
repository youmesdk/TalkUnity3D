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

        private string _userID = "";
        private TalkConfig _config;

        /// <summary>
        /// 设置配置信息
        /// </summary>
        /// <param name="appKey">游密官方分配的APP唯一标识，在www.youme.im注册后可以自助获取</param>
        /// <param name="secretKey">游密官方分配的接入密钥，和appkey配对使用，在www.youme.im注册后可以自助获取</param>
        /// <param name="config">SDK配置对象，可以通过该对象的参数设置SDK的可选参数，比如服务器区域、日志级别</param> <summary>
        /// <param name="initCallBack">初始化结果回调</param>
        /// 
        public void Initialize(string appKey, string secretKey, TalkConfig config, Action<InitEvent> initCallBack)
        {
            YouMeErrorCode errorCode;
            _config = config;
            talkManager.initCallBack = initCallBack;
            if (config != null)
            {
                if (!string.IsNullOrEmpty(config.ExtServerZone))
                {
                    errorCode = YouMeVoiceAPI.GetInstance().Init(appKey, secretKey, YOUME_RTC_SERVER_REGION.RTC_EXT_SERVER, config
                   .ExtServerZone);
                }
                else
                {
                    errorCode = YouMeVoiceAPI.GetInstance().Init(appKey, secretKey, config.ServerZone, "");
                }
            }
            else
            {
                errorCode = YouMeVoiceAPI.GetInstance().Init(appKey, secretKey, YOUME_RTC_SERVER_REGION.RTC_DEFAULT_SERVER, "");
            }
            if (errorCode != YouMeErrorCode.YOUME_SUCCESS && initCallBack != null)
            {
                talkManager.initCallBack = null;
                initCallBack(new InitEvent(errorCode));
            }
        }

        public TalkClient SetUserID(string userID)
        {
            _userID = userID;
            return this;
        }

        public string GetUserID()
        {
            return _userID;
        }

        public void JoinChannel(Channel channel, Action<ChannelEvent> callback, YouMeUserRole role = YouMeUserRole.YOUME_USER_TALKER_FREE)
        {
            if (string.IsNullOrEmpty(GetUserID()))
            {
                Log.e("Need SetUserID(string userid).");
                if (callback != null)
                {
                    callback(new ChannelEvent(StatusCode.USER_ID_IS_EMPTY, ChannelEventType.JOIN_FAIL, channel.ChannelID));
                }
                return;
            }
            talkManager.AddJoinChannelCallback(channel.ChannelID, callback);
            YouMeErrorCode errorCode;
            if (_config.AllowMultiChannel)
            {
                errorCode = YouMeVoiceAPI.GetInstance().JoinChannelMultiMode(GetUserID(), channel.ChannelID);
            }
            else
            {
                errorCode = YouMeVoiceAPI.GetInstance().JoinChannelSingleMode(GetUserID(), channel.ChannelID, (int)role);
            }

            if (errorCode != YouMeErrorCode.YOUME_SUCCESS && callback != null)
            {
                talkManager.RemoveJoinChannelCallback(channel.ChannelID);
                callback(new ChannelEvent(Conv.ErrorCodeConvert(errorCode), ChannelEventType.JOIN_FAIL, channel.ChannelID));
            }
        }

        public void LeaveChannel(Channel channel, Action<ChannelEvent> callback)
        {
            if (channel.ChannelID == null)
            {
                Log.e("channel ID can't be null.");
                return;
            }
            if (_config.AllowMultiChannel)
            {
                YouMeErrorCode errorCode = YouMeVoiceAPI.GetInstance().LeaveChannelMultiMode(channel.ChannelID);
                if (errorCode != YouMeErrorCode.YOUME_SUCCESS && callback != null)
                {
                    callback(new ChannelEvent(Conv.ErrorCodeConvert(errorCode), ChannelEventType.LEAVE_FAIL, channel.ChannelID));
                }
                else
                {
                    talkManager.AddLeaveChannelCallback(channel.ChannelID, callback);
                }
            }
            else
            {
                LeaveAllChannel(callback);
            }
        }

        public void LeaveAllChannel(Action<ChannelEvent> callback)
        {
            YouMeErrorCode errorCode = YouMeVoiceAPI.GetInstance().LeaveChannelAll();
            if (errorCode != YouMeErrorCode.YOUME_SUCCESS && callback != null)
            {
                callback(new ChannelEvent(Conv.ErrorCodeConvert(errorCode), ChannelEventType.LEAVE_FAIL, ""));
            }
            else
            {
                talkManager.leaveAllChannelCallback = callback;
            }
        }

        public void UnInitialize()
        {
            YouMeVoiceAPI.GetInstance().UnInit();
        }

        public void SetServerRegion(string[] regionNames)
        {
            YouMeVoiceAPI.GetInstance().SetServerRegion(regionNames);
        }

        public void OpenSpeaker()
        {
            YouMeVoiceAPI.GetInstance().SetSpeakerMute(false);
        }

        public void CloseSpeaker()
        {
            YouMeVoiceAPI.GetInstance().SetSpeakerMute(true);
        }

        public void OpenMicrophone()
        {
            YouMeVoiceAPI.GetInstance().SetMicrophoneMute(false);
        }

        public void CloseMicrophone()
        {
            YouMeVoiceAPI.GetInstance().SetMicrophoneMute(true);
        }

        public bool IsSpeakerOpen()
        {
            return !YouMeVoiceAPI.GetInstance().GetSpeakerMute();
        }

        public bool IsMicrophoneOpen()
        {
            return !YouMeVoiceAPI.GetInstance().GetSpeakerMute();
        }

        /// <summary>
        /// 设置通话音量
        /// 这是一个同步调用接口，函数返回时表明操作已经完成
        /// </summary>
        ///
        /// <param name="uiVolume"> 取值范围是[0-100] 100表示最大音量， 默认音量是100</param>
        ///
        public void SetVolume(int uiVolume)
        {
            uiVolume = Mathf.Clamp(uiVolume, 0, 100);
            YouMeVoiceAPI.GetInstance().SetVolume((uint)uiVolume);
        }

        /// <summary>
        /// 获取音量大小,此音量值为程序内部的音量，与系统音量相乘得到程序使用的实际音量
        /// 这是一个同步调用接口，函数返回时表明操作已经完成
        /// </summary>
        ///
        /// <returns>当前音量值，范围 [0-100]</returns>
        ///
		public int GetVolume()
        {
            return YouMeVoiceAPI.GetInstance().GetVolume();
        }

        /// <summary>
        /// 多频道模式下，指定当前要讲话的频道
        /// </summary>
        ///
        /// <param name="strChannelID"> 全局唯一的频道标识，全局指在当前应用程序的范围内 </param>
        /// <param name="callback">设置当前讲话频道是否成功</param>
        /// </returns>
        ///
        public void SpeakToChannel(string strChannelID, Action<SpeakToChannelEvent> callback)
        {
            talkManager.speakToChannelCallback = callback;
            var code = YouMeVoiceAPI.GetInstance().SpeakToChannel(strChannelID);
            if (code != YouMeErrorCode.YOUME_SUCCESS)
            {
                //接口调用时机错误，不会收到回调结果
                callback(new SpeakToChannelEvent(code, strChannelID));
                talkManager.speakToChannelCallback = null;
            }
        }

        /// <summary>
		/// 设置不接收指定用户的语音
		/// </summary>
		///
		/// <param name="userID">要屏蔽的用户的ID</param>
		/// <param name="isOn">true为打开，false为关闭</param>
		///
		/// <returns>返回接口调用是否成功的状态码，YouMeErrorCode.YOUME_SUCCESS表示成功</returns>
		///
		public StatusCode AvoidOtherVoiceStatus(string userID, bool isOn)
        {
            return Conv.ErrorCodeConvert(YouMeVoiceAPI.GetInstance().AvoidOtherVoiceStatus(userID, isOn));
        }

        /// <summary>
        /// 播放指定的音乐文件。播放的音乐将会通过扬声器输出，并和语音混合后发送给接收方。这个功能适合于主播/指挥等使用。
        /// 如果当前已经有一个音乐文件在播放，正在播放的音乐会被停止，然后播放新的文件。
        /// 这是一个异步调用接口，函数返回YouMeErrorCode.YOUME_SUCCESS后，将通过如下回调事件通知音乐播放的状态：
        /// YouMeEvent.YOUME_EVENT_BGM_STOPPED - 音乐播放结束了
        /// YouMeEvent.YOUME_EVENT_BGM_FAILED - 音乐文件无法播放（比如文件损坏，格式不支持等）
        /// </summary>
        ///
        /// <param name="strFilePath"> 音乐文件的路径 </param>
        /// <param name="bRepeat"> true 重复播放这个文件， false 只播放一次就停止播放 </param>
        ///
        /// <returns>
        /// YouMeErrorCode.YOUME_SUCCESS 表明成功启动了音乐播放流程
        /// 返回其他值时请看 YouMeErrorCode 的定义
        /// </returns>
        ///
		public StatusCode PlayBackgroundMusic(string strFilePath, bool bRepeat)
        {
            return Conv.ErrorCodeConvert(YouMeVoiceAPI.GetInstance().PlayBackgroundMusic(strFilePath, bRepeat));
        }

        /// <summary>
        /// 停止播放当前正在播放的背景音乐。
        /// 这是一个同步调用接口，函数返回时，音乐播放也就停止了。
        /// </summary>
        ///
        /// <returns>
        /// YouMeErrorCode.YOUME_SUCCESS 表明成功停止了音乐播放流程
        /// 返回其他值时请看 YouMeErrorCode 的定义
        /// </returns>
        ///
		public StatusCode StopBackgroundMusic()
        {
            return Conv.ErrorCodeConvert(YouMeVoiceAPI.GetInstance().StopBackgroundMusic());
        }

        /// <summary>
        /// 设定背景音乐的音量。这个接口用于调整背景音乐和语音之间的相对音量，使得背景音乐和语音混合听起来协调。
        /// 这是一个同步调用接口
        /// </summary>
        ///
        /// <param name="volume"> 背景音乐的音量，范围 [0-100], 100为最大音量 </param>
        ///
        /// <returns>
        /// YouMeErrorCode.YOUME_SUCCESS 表明成功设置了背景音乐的音量
        /// 返回其他值时请看 YouMeErrorCode 的定义
        /// </returns>
        ///
		public StatusCode SetBackgroundMusicVolume(int volume)
        {
            return Conv.ErrorCodeConvert(YouMeVoiceAPI.GetInstance().SetBackgroundMusicVolume(volume));
        }

        /// <summary>
        /// 设置插耳机的情况下开启或关闭语音监听（即通过耳机听到自己说话的内容）
        /// 这是一个同步调用接口
        /// </summary>
        ///
        /// <param name="enabled"> true 开启监听，false 关闭监听 </param>
        ///
        /// <returns>
        /// YouMeErrorCode.YOUME_SUCCESS 表明成功设置了语音监听
        /// 返回其他值时请看 YouMeErrorCode 的定义
        /// </returns>
        ///
        public StatusCode SetHeadsetMonitorOn(bool enabled)
        {
            return Conv.ErrorCodeConvert(YouMeVoiceAPI.GetInstance().SetHeadsetMonitorOn(enabled));
        }

        /// <summary>
        /// 设置是否开启混响音效，这个主要对主播/指挥有用
        /// 这是一个同步调用接口
        /// </summary>
        ///
        /// <param name="enabled"> true 开启混响，false 关闭混响 </param>
        ///
        /// <returns>
        /// YouMeErrorCode.YOUME_SUCCESS 表明成功设置了混响音效
        /// 返回其他值时请看 YouMeErrorCode 的定义
        /// </returns>
        ///
        public StatusCode SetReverbEnabled(bool enabled)
        {
            return Conv.ErrorCodeConvert(YouMeVoiceAPI.GetInstance().SetReverbEnabled(enabled));
        }

        /// <summary>
        /// 设置是否开启语音检测回调
        /// 这是一个同步调用接口
        /// </summary>
        ///
        /// <param name="enabled"> true 开启语音检测，false 关闭语音检测 </param>
        ///
        /// <returns>
        /// YouMeErrorCode.YOUME_SUCCESS 表明成功设置了语音检测回调
        /// 返回其他值时请看 YouMeErrorCode 的定义
        /// </returns>
        ///
        public StatusCode SetVadCallbackEnabled(bool enabled)
        {
            return Conv.ErrorCodeConvert(YouMeVoiceAPI.GetInstance().SetVadCallbackEnabled(enabled));
        }

        /// <summary>
        /// 设置麦克风音量回调参数
        /// 你可以在初始化成功后随时调用这个接口。在整个APP生命周期只需要调用一次，除非你想修改参数。
        /// 设置成功后，当用户讲话时，你将收到回调事件 MY_MIC_LEVEL， 回调参数 iStatus 表示当前讲话的音量级别。
        /// </summary>
        ///
        /// <param name="maxMicLevel">
        /// 设为 0 表示关闭麦克风音量回调
        /// 设为 大于0的值表示音量最大时对应的值，这个可以根据你们的UI设计来设定。
        /// 比如你用10级的音量条来表示音量变化，则传10。这样当底层回传音量是10时，则表示当前mic音量达到最大值。
        /// </param>
        ///
        /// <returns>
        /// YouMeErrorCode.YOUME_SUCCESS 表示设置成功
        /// 返回其他值时请看 YouMeErrorCode 的定义
        /// </returns>
        ///
        public StatusCode SetMicLevelCallback(int maxMicLevel)
        {
            return Conv.ErrorCodeConvert(YouMeVoiceAPI.GetInstance().SetMicLevelCallback(maxMicLevel));
        }

        /// <summary>
        /// 暂停通话，释放对麦克风等设备资源的占用。当需要用第三方模块临时录音时，可调用这个接口。
        /// 这是一个异步调用接口，函数返回YouMeErrorCode.YOUME_SUCCESS后，还需要等待如下事件回调
        /// YouMeEvent.YOUME_EVENT_PAUSED - 成功暂停语音
        /// YouMeEvent.YOUME_EVENT_CONNECT_FAILED - 暂停语音失败
        /// </summary>
        ///
        /// <returns>
        /// YouMeErrorCode.YOUME_SUCCESS 表明暂停通话正在进行当中
        /// 返回其他值时请看 YouMeErrorCode 的定义
        /// </returns>
        ///
        public StatusCode PauseChannel()
        {
            return Conv.ErrorCodeConvert(YouMeVoiceAPI.GetInstance().PauseChannel());
        }

        /// <summary>
        /// 恢复通话，调用PauseChannel暂停通话后，可调用这个接口恢复通话
        /// 这是一个异步调用接口，函数返回YouMeErrorCode.YOUME_SUCCESS后，还需要等待如下事件回调
        /// YouMeEvent.YOUME_EVENT_RESUMED - 成功恢复语音
        /// YouMeEvent.YOUME_EVENT_RESUME_FAILED - 恢复语音失败
        /// </summary>
        ///
        /// <returns>
        /// YouMeErrorCode.YOUME_SUCCESS 表明恢复通话正在进行当中
        /// 返回其他值时请看 YouMeErrorCode 的定义
        /// </returns>
        ///
        public StatusCode ResumeChannel()
        {
            return Conv.ErrorCodeConvert(YouMeVoiceAPI.GetInstance().ResumeChannel());
        }

        /// <summary>
        /// 设置当前录音的时间戳。当通过录游戏脚本进行直播时，要保证观众端音画同步，在主播端需要进行时间对齐。
        /// 这个接口设置的就是当前游戏画面录制已经进行到哪个时间点了。
        /// </summary>
        ///
		/// <param name="timeMs"> 当前游戏画面对应的时间点，单位为毫秒 </param>
        /// 
        ///
        public void SetRecordingTimeMs(uint timeMs)
        {
            YouMeVoiceAPI.GetInstance().SetRecordingTimeMs(timeMs);
        }

        /// <summary>
        /// 设置当前声音播放的时间戳。当通过录游戏脚本进行直播时，要保证观众端音画同步，游戏画面的播放需要和声音播放进行时间对齐。
        /// 这个接口设置的就是当前游戏画面播放已经进行到哪个时间点了。
        /// </summary>
        ///
		/// <param name="timeMs"> 当前游戏画面播放对应的时间点，单位为毫秒 </param>
        /// <returns> void </returns>
        ///
        public void SetPlayingTimeMs(uint timeMs)
        {
            YouMeVoiceAPI.GetInstance().SetPlayingTimeMs(timeMs);
        }

        /**
 		 *  功能描述:Rest API , 向服务器请求额外数据
   		 *  @param strCommand: 请求的命令字符串
  		 *  @param strQueryBody: 请求需要的数据,json格式，内容参考restAPI
   		 *  @param requestID: 回传id,回调的时候传回，标识消息。
   		 *  @return YOUME_SUCCESS - 成功
     	 *          其他 - 具体错误码
		 */
        public void RequestRestApi(string command, string queryBody)
        {
            int requestID = 0;
            var code = YouMeVoiceAPI.GetInstance().RequestRestApi(command, queryBody, ref requestID);
        }

        /**
   		*  功能描述:查询频道的用户列表(必须在频道中)
   		*  @param channelID:要查询的频道ID
    	*  @param maxCount:想要获取的最大数量，-1表示获取全部
     	*  @param notifyMemChagne: 其他用户进出房间时，是否要收到通知
     	*  @return 错误码，详见YouMeConstDefine.h定义
     	*/
        public void GetChannelUserList(string channelID, int maxCount, bool notifyMemChange)
        {

        }

        /**
        *  功能描述:设置身份验证的token
        *  @param strToken: 身份验证用token，设置为NULL或者空字符串，清空token值,则不验证。
        *  @return 无
        */
        public void SetToken(string strToken)
        {
            YouMeVoiceAPI.GetInstance().SetToken(strToken);
        }
    }
}