using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

#if UNITY_IOS
using AOT;
#endif

namespace YouMe{

	public class YouMeVoiceAPI
	{
	    //////////////////////////////////////////////////////////////////////////////////////////////
		// 导出SDK所有的C接口API
	    //////////////////////////////////////////////////////////////////////////////////////////////
		
        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern int youme_init(string strAPPKey, string strAPPSecret, int serverRegionId, string strExtServerRegionName);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern int youme_unInit();

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern void youme_setcallback(string strObjName);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern void youme_setSpeakerMute (bool bOn);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern bool youme_getSpeakerMute ();

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern bool youme_getMicrophoneMute ();

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern void youme_setMicrophoneMute (bool mute);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern int youme_getVolume ();

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern void youme_setVolume (uint uiVolume);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern bool youme_getUseMobileNetworkEnabled ();

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern void youme_setUseMobileNetworkEnabled (bool bEnabled);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern int youme_joinChannelSingleMode (string strUserID, string strChannelID, int userRole);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
        private static extern int youme_joinChannelMultiMode (string strUserID, string strChannelID);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
        private static extern int youme_speakToChannel(string strChannelID);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
        private static extern int youme_leaveChannelMultiMode(string strChannelID);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
        private static extern int youme_leaveChannelAll();

		#if UNITY_IOS
			[DllImport("__Internal")]
		#else
			[DllImport("youme_voice_engine")]
		#endif
		private static extern int youme_avoidOtherVoiceStatus (string userID, bool isOn);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
        private static extern void youme_setServerRegion (int regionId, string strExtRegionId, bool bAppend);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern int youme_playBackgroundMusic (string pFilePath, bool bRepeat);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern int youme_stopBackgroundMusic ();

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern int youme_setBackgroundMusicVolume (int volume);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
	    private static extern int youme_setHeadsetMonitorOn(bool enabled);
	    
        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
    	private static extern int youme_setReverbEnabled(bool enabled);
        
        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
    	private static extern int youme_setVadCallbackEnabled(bool enabled);
        
        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
        private static extern int youme_pauseChannel();
        
        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
        private static extern int youme_resumeChannel();

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
        private static extern void youme_setRecordingTimeMs(uint timeMs);

        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
        private static extern void youme_setPlayingTimeMs(uint timeMs);
        
        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
        private static extern int youme_getSDKVersion();
        
        
       
	    //////////////////////////////////////////////////////////////////////////////////////////////
		// 导出SDK所有的C接口API -- end
	    //////////////////////////////////////////////////////////////////////////////////////////////
        

		//针对windows的回调设置
		#if UNITY_WINRT || UNITY_STANDALONE_WIN
 
		public delegate void UnitySendMessageDelegate([MarshalAs(UnmanagedType.LPStr)]string gameObjectName, [MarshalAs(UnmanagedType.LPStr)]string methodName, [MarshalAs(UnmanagedType.LPStr)]string message);
		
		private static UnitySendMessageDelegate s_SendMessageDelegate;
		
        #if UNITY_IOS
            [DllImport("__Internal")]
        #else
            [DllImport("youme_voice_engine")]
        #endif
		private static extern void SetUnitySendMessageCallback(UnitySendMessageDelegate sendMessage);
		
		#if UNITY_IOS
        	[MonoPInvokeCallback(typeof(UnitySendMessageDelegate))]
		#endif
		private static void UnitySendMessageWrapper(string gameObjectName, string methodName, string message)
		{
			callback.Add (()=>{
				var gameObject = GameObject.Find(gameObjectName);
				if (gameObject != null)
				{
					gameObject.SendMessage(methodName, message);
				}
			});
		}
		
		#endif //UNITY_WINRT || UNITY_STANDALONE_WIN

		//成员变量定义
		private static YouMeVoiceAPI mInstance;
 		#if UNITY_ANDROID
		private  bool mAndroidInited = false;
		private  bool mAndroidInitOK = false;
		private  AndroidJavaClass instance_youme_java;
		#endif

		//单实例对象
		public static YouMeVoiceAPI GetInstance()
		{
			if (mInstance == null)
			{
				mInstance = new YouMeVoiceAPI();

				var youmeObj = new GameObject ("__YouMeRTCSDkGameObject__");
				GameObject.DontDestroyOnLoad (youmeObj);
				youmeObj.hideFlags = HideFlags.DontSave;
				youmeObj.AddComponent<YoumeTalkObj> ();

			}
			return mInstance;
		}

		private static List<System.Action> callback=new List<System.Action>();
		private class YoumeTalkObj:MonoBehaviour{
			void Update(){
				if (callback.Count > 0) {
					try {
						callback [0] ();
					} catch (System.Exception) {
					}
					callback.RemoveAt (0);
				}
			}
		}
		YouMeVoiceAPI()
		{
			#if UNITY_ANDROID
				instance_youme_java = new AndroidJavaClass ("com.youme.voiceengine.api");					
			#endif
		}

        //////////////////////////////////////////////////////////////////////////////////////////////
        // C# 对外接口定义
        //////////////////////////////////////////////////////////////////////////////////////////////

		// 初始化Android Java部分，并load so库，同时启动Android Service
		#if UNITY_ANDROID
		private void InitAndroidJava()
		{
			try {
				if (!mAndroidInited) {
					mAndroidInited = true;

					AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
					AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
					AndroidJavaClass YouMeManager = new AndroidJavaClass ("com.youme.voiceengine.mgr.YouMeManager");
					mAndroidInitOK = YouMeManager.CallStatic<bool> ("Init", currentActivity);
					if (mAndroidInitOK) {
						AndroidJavaClass VoiceEngineService = new AndroidJavaClass ("com.youme.voiceengine.VoiceEngineService");
						AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent", currentActivity, VoiceEngineService);
						currentActivity.Call<AndroidJavaObject> ("startService", intentObject);
					}
				} 
			} catch {
				Debug.Log("android init exception!!!");
			}
		}
		#endif

        /// <summary>
        /// 设置接收回调消息的对象名。这个函数必须最先调用，这样才能收到后面所有调用的回调消息，包括Init(...)函数的回调。
        /// </summary>
        /// <param name="strObjName">用于接收回调消息的对象名</param>
        ///
		public void SetCallback(string strObjName)
		{
			#if UNITY_ANDROID
				InitAndroidJava();
				if (!mAndroidInitOK) {
					return;
				}
			#endif 

			#if (UNITY_STANDALONE_WIN)
				s_SendMessageDelegate = UnitySendMessageWrapper;
				SetUnitySendMessageCallback(s_SendMessageDelegate);
			#endif
			
			#if (UNITY_IOS || UNITY_STANDALONE_WIN)
				youme_setcallback (strObjName);
			#elif UNITY_ANDROID
				// Android下回调的方式不一样，需要通过JNI转换对象（如string转为jsting），直接使用 C 的回调方式会导致垃圾回收时崩溃。
				if(null != instance_youme_java)
				{
					try
					{
						instance_youme_java.CallStatic("SetCallback",strObjName);
					}
					catch
					{
						Debug.Log("SetCallback exception!!!");
					}
				}
			#endif
		}

        /// <summary>
        /// 初始化语音引擎，做APP验证和资源初始化
        /// 这是一个异步调用接口，如果函数返回 YOUME_SUCCESS， 则需要等待以下事件回调达到才表明初始化完成。只有初始化成功，才能进行。
        /// 其他的操作，如进入/退出频道，开关麦克风等。
        /// YouMeEvent.YOUME_EVENT_INIT_OK - 表明初始化成功
        /// YouMeEvent.YOUME_EVENT_INIT_NOK - 表明初始化失败，最常见的失败原因是网络错误或者 AppKey/AppSecret 错误
        /// </summary>
        /// <param name="strAPPKey">从游密申请到的 app key, 这个你们应用程序的唯一标识</param>
        /// <param name="strAPPKey">对应 strAPPKey 的私钥, 这个需要妥善保存，不要暴露给其他人</param>
        /// <param name="serverRegionId">
        /// 设置首选连接服务器的区域码
        /// 如果在初始化时不能确定区域，可以填RTC_DEFAULT_SERVER，后面确定时通过 SetServerRegion 设置。
        /// 如果YOUME_RTC_SERVER_REGION定义的区域码不能满足要求，可以把这个参数设为 RTC_EXT_SERVER，然后
        /// 通过后面的参数 strExtServerRegionName 设置一个自定的区域值（如中国用 "cn" 或者 “ch"表示），然后把这个自定义的区域值同步给游密。
        /// 我们将通过后台配置映射到最佳区域的服务器。
        /// </param>
        /// <param name="strExtServerRegionName">扩展的服务器区域
        /// </param>
        ///
        /// <returns>返回接口调用是否成功的状态码，YouMeErrorCode.YOUME_SUCCESS表示成功</returns>
        ///
		public YouMeErrorCode Init(string strAppKey,string strAPPSecret, 
									YOUME_RTC_SERVER_REGION serverRegionId, string strExtServerRegionName)
		{
			#if UNITY_ANDROID
				InitAndroidJava();
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif 

			#if (UNITY_IOS || UNITY_STANDALONE_WIN)
				return (YouMeErrorCode)youme_init (strAppKey, strAPPSecret, (int)serverRegionId, strExtServerRegionName);
			#elif UNITY_ANDROID
				// Android下回调的方式不一样，需要通过JNI转换对象（如string转为jsting），直接使用 C 的回调方式会导致垃圾回收时崩溃。
				if(null == instance_youme_java)
				{
					return YouMeErrorCode.YOUME_ERROR_INVALID_PARAM;
				}
				try
				{
					return (YouMeErrorCode)instance_youme_java.CallStatic<int>("init",strAppKey,strAPPSecret, (int)serverRegionId, strExtServerRegionName);
				}
				catch
				{
					Debug.Log("init exception!!!");
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#else
				return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
			#endif
		}

        /// <summary>
        /// 功能描述:反初始化引擎，在应用退出之前需要调用这个接口释放资源
        /// 这是一个同步调用接口，函数返回时表明操作已经完成
        ///
        /// <returns>返回接口调用是否成功的状态码，YouMeErrorCode.YOUME_SUCCESS表示成功</returns>
        ///
		public YouMeErrorCode UnInit ()
		{
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

			#if !UNITY_EDITOR
				return (YouMeErrorCode)youme_unInit();
			#else
				return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
			#endif
		}

        /// <summary>
        /// 设置服务器区域，默认是中国
        /// </summary>
        ///
        /// <param name="regionId">
        /// 设置首选连接服务器的区域码
        /// 如果YOUME_RTC_SERVER_REGION定义的区域码不能满足要求，可以把这个参数设为 RTC_EXT_SERVER，然后
        /// 通过下面的参数 strExtRegionName 设置一个自定的区域值，然后把这个自定义的区域值同步给游密。
        /// 我们将通过后台配置映射到最佳区域的服务器。
        /// </param>
        /// <param name="strExtRegionName">扩展的服务器区域
        /// </param>
        ///
        public void SetServerRegion(YOUME_RTC_SERVER_REGION regionId, string strExtRegionName){
			#if UNITY_ANDROID
				InitAndroidJava();
				if (!mAndroidInitOK) {
					return;
				}
			#endif 

			#if !UNITY_EDITOR
				youme_setServerRegion((int)regionId, strExtRegionName, false);
			#endif
        }

        /// <summary>
        /// 设置参与通话各方所在的区域
        /// 这个接口适合于分布区域比较广的应用。最简单的做法是只设定前用户所在区域。但如果能确定其他参与通话的应用所在的区域，则能使服务器选择更优。
        /// </summary>
        ///
        /// <param name="regionNames">
        /// 	指定参与通话各方区域的数组，数组里每个元素为一个区域代码。用户可以自行定义代表各区域的字符串（如中国用 "cn" 或者 “ch"表示），
        ///     然后把定义好的区域表同步给游密，游密会把这些定义配置到后台，在实际运营时选择最优服务器。
        /// </param>
        ///
        public void SetServerRegion(string[] regionNames){
			#if UNITY_ANDROID
				InitAndroidJava();
				if (!mAndroidInitOK) {
					return;
				}
			#endif

			if ((regionNames != null) && (regionNames.Length > 0)) {
				youme_setServerRegion ((int)YouMe.YOUME_RTC_SERVER_REGION.RTC_EXT_SERVER, regionNames[0], false);
			}
			for (int i = 1; i < regionNames.Length; i++) {
				youme_setServerRegion ((int)YouMe.YOUME_RTC_SERVER_REGION.RTC_EXT_SERVER, regionNames[i], true);
	        }
        }

        /// <summary>
        /// 设置扬声器是否静音
        /// 这是一个同步调用接口，函数返回时表明操作已经完成
        /// </summary>
        ///
        /// <param name="bMute">true为麦克风静音，false为打开扬声器</param>
        ///
		public void SetSpeakerMute (bool bMute)
		{
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return;
				}
			#endif

			#if !UNITY_EDITOR
				youme_setSpeakerMute (bMute);
			#endif
		}

        /// <summary>
        /// 获取扬声器的静音状态
        /// 这是一个同步调用接口，函数返回时表明操作已经完成
        /// </summary>
        ///
        /// <returns>true 扬声器当前处于静音状态，false 扬声器当前处于打开状态， 默认情况下扬声器是打开的</returns>
        ///
		public bool GetSpeakerMute ()
		{
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return false;
				}
			#endif

			#if !UNITY_EDITOR
				return youme_getSpeakerMute ();
			#else
				return true;
			#endif
		}


        /// <summary>
        /// 设置麦克风是否静音
        /// 这是一个同步调用接口，函数返回时表明操作已经完成
        /// </summary>
        ///
        /// <param name="bMute">true为麦克风静音，false为打开麦克风，默认情况下麦克风是关闭的</param>
        ///
        public void SetMicrophoneMute (bool mute)
        {
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return;
				}
			#endif

            #if !UNITY_EDITOR
            youme_setMicrophoneMute (mute);
            #endif
        }


        /// <summary>
        /// 获取麦克风的静音状态
        /// 这是一个同步调用接口，函数返回时表明操作已经完成
        /// </summary>
        ///
        /// <returns>true 麦克风当前处于静音状态，false 麦克风当前处于打开状态</returns>
        ///
		public bool GetMicrophoneMute ()
		{
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return false;
				}
			#endif

			#if !UNITY_EDITOR
				return youme_getMicrophoneMute ();
			#else
				return false;
			#endif
		}

        /// <summary>
        /// 设置通话音量
        /// 这是一个同步调用接口，函数返回时表明操作已经完成
        /// </summary>
        ///
        /// <param name="uiVolume"> 取值范围是[0-100] 100表示最大音量， 默认音量是100</param>
        ///
        public void SetVolume (uint uiVolume)
        {
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return;
				}
			#endif

            #if !UNITY_EDITOR
                youme_setVolume (uiVolume);
            #endif
        }

        /// <summary>
        /// 获取音量大小,此音量值为程序内部的音量，与系统音量相乘得到程序使用的实际音量
        /// 这是一个同步调用接口，函数返回时表明操作已经完成
        /// </summary>
        ///
        /// <returns>当前音量值，范围 [0-100]</returns>
        ///
		public int GetVolume ()
		{
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return 0;
				}
			#endif

			#if !UNITY_EDITOR
				return youme_getVolume ();
			#else
				return 0;
			#endif

		}


        /// <summary>
        /// 设置是否允许使用移动网络(2G/3G/4G)进行通话。如果当前已经进入了语音频道，这个设置是不生效的，它只对下次通话有效。
        /// 这是一个同步调用接口，函数返回时表明操作已经完成
        /// </summary>
        ///
        /// <param name="bEnabled"> true-移动网络下允许通话，false-移动网络下不允许通话，默认允许 </param>
        ///
        public void SetUseMobileNetworkEnabled (bool bEnabled)
        {
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return;
				}
			#endif

            #if !UNITY_EDITOR
                youme_setUseMobileNetworkEnabled (bEnabled);
            #endif
        }

        /// <summary>
        /// 获取当前是否允许使用移动网络(2G/3G/4G)进行通话
        /// 这是一个同步调用接口，函数返回时表明操作已经完成
        /// </summary>
        ///
        /// <returns>true-移动网络下允许通话，false-移动网络下不允许通话</returns>
        ///
		public bool GetUseMobileNetworkEnabled ()
		{
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return false;
				}
			#endif

			#if !UNITY_EDITOR
				return youme_getUseMobileNetworkEnabled ();
			#else
				return true;
			#endif


		}


        /// <summary>
        /// 加入语音频道（单频道模式，每个时刻只能在一个语音频道里面）
        /// 这是一个异步调用接口，函数返回YouMeErrorCode.YOUME_SUCCESS后，还需要等待如下事件回调
        /// YouMeEvent.YOUME_EVENT_CONNECTED - 成功进入语音频道
        /// YouMeEvent.YOUME_EVENT_CONNECT_FAILED - 进入语音频道失败，可能原因是网络或服务器有问题
        /// </summary>
        ///
        /// <param name="strUserID"> 全局唯一的用户标识，全局指在当前应用程序的范围内 </param>
        /// <param name="strChannelID"> 全局唯一的频道标识，全局指在当前应用程序的范围内 </param>
        /// <param name="userRole"> 用户在语音频道里面的角色，见YouMeUserRole定义 </param>
        ///
        /// <returns>
        /// YouMeErrorCode.YOUME_SUCCESS 表明成功了启动了进入语音频道的流程
        /// 返回其他值时请看 YouMeErrorCode 的定义
        /// </returns>
        ///
		public YouMeErrorCode JoinChannelSingleMode (string strUserID, string strChannelID, int userRole)
		{
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

			#if !UNITY_EDITOR
				return (YouMeErrorCode)youme_joinChannelSingleMode (strUserID, strChannelID, userRole);
			#else
				return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
			#endif


		}
				
        /// <summary>
        /// 加入语音频道（多频道模式，可以同时听多个语音频道的内容，但每个时刻只能对着一个频道讲话）
        /// 这是一个异步调用接口，函数返回YouMeErrorCode.YOUME_SUCCESS后，还需要等待如下事件回调
        /// YouMeEvent.YOUME_EVENT_CONNECTED - 成功进入语音频道
        /// YouMeEvent.YOUME_EVENT_CONNECT_FAILED - 进入语音频道失败，可能原因是网络或服务器有问题
        /// </summary>
        ///
        /// <param name="strUserID"> 全局唯一的用户标识，全局指在当前应用程序的范围内 </param>
        /// <param name="strChannelID"> 全局唯一的频道标识，全局指在当前应用程序的范围内 </param>
        ///
        /// <returns>
        /// YouMeErrorCode.YOUME_SUCCESS 表明成功了启动了进入语音频道的流程
        /// 返回其他值时请看 YouMeErrorCode 的定义
        /// </returns>
        ///
        public YouMeErrorCode JoinChannelMultiMode (string strUserID, string strChannelID)
        {
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

            #if !UNITY_EDITOR
                return (YouMeErrorCode)youme_joinChannelMultiMode (strUserID, strChannelID);
            #else
                return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
            #endif
        }

        /// <summary>
        /// 多频道模式下，指定当前要讲话的频道
        /// 这是一个异步调用接口，函数返回YouMeErrorCode.YOUME_SUCCESS后，还需要等待如下事件回调
        /// YouMeEvent.YOUME_EVENT_SPEAK_SUCCESS - 成功进入语音频道
        /// YouMeEvent.YOUME_EVENT_SPEAK_FAILED - 进入语音频道失败，可能原因是网络或服务器有问题
        /// </summary>
        ///
        /// <param name="strUserID"> 全局唯一的用户标识，全局指在当前应用程序的范围内 </param>
        /// <param name="strChannelID"> 全局唯一的频道标识，全局指在当前应用程序的范围内 </param>
        ///
        /// <returns>
        /// YouMeErrorCode.YOUME_SUCCESS 表明成功了启动了进入语音频道的流程
        /// 返回其他值时请看 YouMeErrorCode 的定义
        /// </returns>
        ///
        public YouMeErrorCode SpeakToChannel (string strChannelID)
        {
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

            #if !UNITY_EDITOR
                return (YouMeErrorCode)youme_speakToChannel (strChannelID);
            #else
                return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
            #endif
        }

        /// <summary>
        /// 退出指定的语音频道
        /// 这是一个异步调用接口，函数返回YouMeErrorCode.YOUME_SUCCESS后，还需要等待如下事件回调
        /// YouMeEvent.YOUME_EVENT_TERMINATED - 成功退出语音频道
        /// YouMeEvent.YOUME_EVENT_TERMINATE_FAILED - 退出语音频道失败，可能原因是网络或服务器有问题。
        ///     只对多频道模式有意义，单频道模式下，退出总是成功的。
        /// </summary>
        ///
        /// <param name="strChannelID"> 指定要退出的频道的标识符 </param>
        ///
        /// <returns>
        /// YouMeErrorCode.YOUME_SUCCESS 表明成功了启动了退出语音频道的流程
        /// 返回其他值时请看 YouMeErrorCode 的定义
        /// </returns>
        ///
		public YouMeErrorCode LeaveChannelMultiMode (string strChannelID)
		{
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

            #if !UNITY_EDITOR
				return (YouMeErrorCode)youme_leaveChannelMultiMode (strChannelID);
			#else
                return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
			#endif

		}

        /// <summary>
        /// 退出所有的语音频道
        /// 这是一个异步调用接口，函数返回YouMeErrorCode.YOUME_SUCCESS后，还需要等待如下事件回调
        /// YouMeEvent.YOUME_EVENT_TERMINATED。
        /// </summary>
        ///
        /// <returns>
        /// YouMeErrorCode.YOUME_SUCCESS 表明成功了启动了退出所有语音频道的流程
        /// 返回其他值时请看 YouMeErrorCode 的定义
        /// </returns>
        ///
        public YouMeErrorCode LeaveChannelAll ()
        {
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

            #if !UNITY_EDITOR
                return (YouMeErrorCode)youme_leaveChannelAll ();
            #else
                return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
            #endif
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
		public YouMeErrorCode AvoidOtherVoiceStatus (string userID,bool isOn){
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

			#if !UNITY_EDITOR
				return (YouMeErrorCode)youme_avoidOtherVoiceStatus(userID, isOn);
			#else
				return YouMeErrorCode.YOUME_SUCCESS;
			#endif
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
		public YouMeErrorCode PlayBackgroundMusic (string strFilePath, bool bRepeat){
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

			#if !UNITY_EDITOR
                return (YouMeErrorCode)youme_playBackgroundMusic(strFilePath, bRepeat);
			#else
                return YouMeErrorCode.YOUME_SUCCESS;
			#endif
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
		public YouMeErrorCode StopBackgroundMusic (){
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

            #if !UNITY_EDITOR
                return (YouMeErrorCode)youme_stopBackgroundMusic();
			#else
                return YouMeErrorCode.YOUME_SUCCESS;
			#endif
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
		public YouMeErrorCode SetBackgroundMusicVolume (int volume){
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

			#if !UNITY_EDITOR
                return (YouMeErrorCode)youme_setBackgroundMusicVolume(volume);
			#else
                return YouMeErrorCode.YOUME_SUCCESS;
			#endif
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
        public YouMeErrorCode SetHeadsetMonitorOn(bool enabled){
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

            #if !UNITY_EDITOR
                return (YouMeErrorCode)youme_setHeadsetMonitorOn(enabled);
            #else
                return YouMeErrorCode.YOUME_SUCCESS;
            #endif
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
        public YouMeErrorCode SetReverbEnabled(bool enabled){
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

            #if !UNITY_EDITOR
                return (YouMeErrorCode)youme_setReverbEnabled(enabled);
            #else
                return YouMeErrorCode.YOUME_SUCCESS;
            #endif
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
        public YouMeErrorCode SetVadCallbackEnabled(bool enabled){
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

            #if !UNITY_EDITOR
                return (YouMeErrorCode)youme_setVadCallbackEnabled(enabled);
            #else
                return YouMeErrorCode.YOUME_SUCCESS;
            #endif
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
        public YouMeErrorCode PauseChannel(){
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

            #if !UNITY_EDITOR
                return (YouMeErrorCode)youme_pauseChannel();
            #else
                return YouMeErrorCode.YOUME_SUCCESS;
            #endif
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
        public YouMeErrorCode ResumeChannel(){
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return YouMeErrorCode.YOUME_ERROR_UNKNOWN;
				}
			#endif

            #if !UNITY_EDITOR
                return (YouMeErrorCode)youme_resumeChannel();
            #else
                return YouMeErrorCode.YOUME_SUCCESS;
            #endif
        }

        /// <summary>
        /// 设置当前录音的时间戳。当通过录游戏脚本进行直播时，要保证观众端音画同步，在主播端需要进行时间对齐。
        /// 这个接口设置的就是当前游戏画面录制已经进行到哪个时间点了。
        /// </summary>
        ///
		/// <param name="timeMs"> 当前游戏画面对应的时间点，单位为毫秒 </param>
        /// <returns> void </returns>
        ///
        public void SetRecordingTimeMs(uint timeMs){
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return;
				}
			#endif

            #if !UNITY_EDITOR
                youme_setRecordingTimeMs(timeMs);
            #endif
        }

        /// <summary>
        /// 设置当前声音播放的时间戳。当通过录游戏脚本进行直播时，要保证观众端音画同步，游戏画面的播放需要和声音播放进行时间对齐。
        /// 这个接口设置的就是当前游戏画面播放已经进行到哪个时间点了。
        /// </summary>
        ///
		/// <param name="timeMs"> 当前游戏画面播放对应的时间点，单位为毫秒 </param>
        /// <returns> void </returns>
        ///
        public void SetPlayingTimeMs(uint timeMs){
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return;
				}
			#endif

            #if !UNITY_EDITOR
                youme_setPlayingTimeMs(timeMs);
            #endif
        }

        /// <summary>
        /// 获取SDK版本号，版本号分为4段，如 2.5.0.0，这4段在int里面的分布如下
        /// | 4 bits | 6 bits | 8 bits | 14 bits|
        /// </summary>
        ///
        /// <returns>
        /// 压缩过的版本号
        /// </returns>
        ///
        public int GetSDKVersion(){
			#if UNITY_ANDROID
				if (!mAndroidInitOK) {
					return 0;
				}
			#endif

            #if !UNITY_EDITOR
                return youme_getSDKVersion();
            #else
                return 0;
            #endif
        }

	} //YouMeVoiceAPI
}
