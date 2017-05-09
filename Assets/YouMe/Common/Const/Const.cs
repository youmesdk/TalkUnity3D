namespace YouMe
{
    public enum LogLevel
    {
        NONE = 0,
        FATAL  = 1,
        ERROR = 10,
        WARNING = 20,
        INFO = 40,
        ALL = 50
    };

    public enum ConnectStatus
    {
        DISCONNECTED = 0,
        CONNECTED  = 1,
        CONNECTING = 2,
        RECONNECTING = 3
    };

    public enum StatusCode{
        // =======================原始ERRORCODE保留========================
        Success = 0,
		EngineNotInit = 1,
		NotLogin = 2,	
		ParamInvalid = 3,
		TimeOut = 4,
		StatusError = 5,
		SDKInvalid = 6,
		AlreadyLogin = 7,
		ServerError = 8,
		NetError = 9,
		LoginSessionError = 10,
		NotStartUp = 11,
		FileNotExist = 12,
		SendFileError = 13,
		UploadFailed = 14,
		UsernamePasswordError = 15,
		UserStatusError = 16,	
		MessageTooLong = 17,	//消息太长
		ReceiverTooLong = 18,	//接收方ID过长(检查房间名)
		InvalidChatType = 19,	//无效聊天类型(私聊、聊天室)
		InvalidReceiver = 20,	//无效用户ID（私聊接受者为数字格式ID）
        UnknowError = 21,		//常见于发送房间消息，房间号并不存在。修正的方法是往自己joinRoom的房间id发消息，就可以发送成功。
        InvalidAppkey = 22,			//无效APPKEY
        ForbiddenSpeak = 23,			//被禁言
        CreateFileFailed = 24,     //无法创建文件
        UnsupportFormat = 25,			//不支持的文件格式
        ReceiverEmpty = 26,			//接收方为空
        RoomIDTooLong = 27,			//房间名太长
        ContentInvalid = 28,			//聊天内容严重非法
        NoLocationAuthrize = 29,		//未打开定位权限
        NoAudioDevice = 30,			//无音频设备
        AudioDriver = 31,				//音频驱动问题
        DeviceStatusInvalid = 32,		//设备状态错误
        ResolveFileError = 33,			//文件解析错误
        ReadWriteFileError = 34,		//文件读写错误
        NoLangCode = 35,				//语言编码错误
        TranslateUnable = 36,			//翻译接口不可用

		//服务器的错误码
		ALREADYFRIENDS = 1000,
		LoginInvalid = 1001,

		//语音部分错误码
		PTT_Start = 2000,
		PTT_Fail = 2001,
		PTT_DownloadFail = 2002,
		PTT_GetUploadTokenFail = 2003,
		PTT_UploadFail = 2004,
		PTT_NotSpeech = 2005,
        PTT_DeviceStatusError = 2006,		//音频设备状态错误
        PTT_IsSpeeching = 2007,			//已开启语音
        PTT_FileNotExist = 2008,
        PTT_ReachMaxDuration = 2009,		//达到语音最大时长限制
        PTT_SpeechTooShort = 2010,			//语音时长太短
        PTT_StartAudioRecordFailed = 2011,	//启动语音失败
        PTT_SpeechTimeout = 2012,			//音频输入超时
        PTT_IsPlaying = 2013,				//正在播放
        PTT_NotStartPlay = 2014,			//未开始播放
        PTT_CancelPlay = 2015,				//主动取消播放
        PTT_NotStartRecord = 2016,			//未开始语音
		Fail = 10000,

        // =======================END 原始ERRORCODE保留========================

        // ========================   TALK ERRORCODE   =======================
        // 参数和状态检查
        YOUME_ERROR_API_NOT_SUPPORTED            = -1,   ///< 正在使用的SDK不支持特定的API
        YOUME_ERROR_INVALID_PARAM                = -2,   ///< 传入参数错误
        YOUME_ERROR_ALREADY_INIT                 = -3,   ///< 已经初始化
        YOUME_ERROR_NOT_INIT                     = -4,   ///< 还没有初始化，在调用某函数之前要先调用初始化并且要根据返回值确保初始化成功
        YOUME_ERROR_CHANNEL_EXIST                = -5,   ///< 要加入的频道已经存在
        YOUME_ERROR_CHANNEL_NOT_EXIST            = -6,   ///< 要退出的频道不存在
        YOUME_ERROR_WRONG_STATE                  = -7,   ///< 状态错误
        YOUME_ERROR_NOT_ALLOWED_MOBILE_NETWROK   = -8,   ///< 不允许使用移动网络
		YOUME_ERROR_WRONG_CHANNEL_MODE           = -9,   ///< 在单频道模式下调用了多频道接口，或者反之
		YOUME_ERROR_TOO_MANY_CHANNELS            = -10,  ///< 同时加入了太多频道

        // 内部操作错误
        YOUME_ERROR_MEMORY_OUT                   = -100, ///< 内存错误
        YOUME_ERROR_START_FAILED                 = -101, ///< 启动引擎失败
        YOUME_ERROR_STOP_FAILED                  = -102, ///<  停止引擎失败
        YOUME_ERROR_ILLEGAL_SDK                  = -103, ///< 非法使用SDK
        YOUME_ERROR_SERVER_INVALID               = -104, ///< 语音服务不可用
        YOUME_ERROR_NETWORK_ERROR                = -105, ///< 网络错误
        YOUME_ERROR_SERVER_INTER_ERROR           = -106, ///< 服务器内部错误

        // 麦克风错误
        YOUME_ERROR_REC_INIT_FAILED              = -201, ///< 录音模块初始化失败
        YOUME_ERROR_REC_NO_PERMISSION            = -202, ///< 没有录音权限
        YOUME_ERROR_REC_NO_DATA                  = -203, ///< 虽然初始化成功，但没有音频数据输出，比如oppo系列手机在录音权限被禁止的时候
        YOUME_ERROR_REC_OTHERS                   = -204, ///< 其他录音模块的错误
        YOUME_ERROR_REC_PERMISSION_UNDEFINED     = -205, ///< 录音权限未确定，iOS显示是否允许录音权限对话框时，返回的是这个错误码
        
        YOUME_ERROR_UNKNOWN = -1000,         ///< 未知错误
        // ======================== END TALK ERRORCODE ======================

        // ==========================状态码扩充================================
        START_DOWNLOAD_FAIL = 20001,
        USER_ID_IS_EMPTY = 20002,
        // =========================END状态码扩充==============================
    }

}