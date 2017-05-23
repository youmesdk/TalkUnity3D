using UnityEngine;
using System.Collections;

namespace YouMe
{
    /// 频道状态
    public enum ChannelState
    {
        CHANNEL_STATE_JOINING = 0,  ///< 正在加入频道
        CHANNEL_STATE_JOINED,        ///< 已经加入频道
        CHANNEL_STATE_LEAVING_ONE,   ///< 正在离开单个频道
        CHANNEL_STATE_LEAVING_ALL,   ///< 正在离开所有频道
        CHANNEL_STATE_LEAVED         ///< 已经离开频道
    }

    /// 用户角色
    public enum YouMeUserRole
    {
		YOUME_USER_NONE = 0,         ///< 非法用户，调用API时不能传此参数
		YOUME_USER_TALKER_FREE,      ///< 自由讲话者，适用于小组通话（建议小组成员数最多10个），每个人都可以随时讲话, 同一个时刻只能在一个语音频道里面
		YOUME_USER_TALKER_ON_DEMAND, ///< 需要通过抢麦等请求麦克风权限之后才可以讲话，适用于较大的组或工会等（比如几十个人），同一个时刻只能有一个或几个人能讲话, 同一个时刻只能在一个语音频道里面
		YOUME_USER_LISTENER,         ///< 听众，主播/指挥/嘉宾的听众，同一个时刻只能在一个语音频道里面，只听不讲
		YOUME_USER_COMMANDER,        ///< 指挥，国家/帮派等的指挥官，同一个时刻只能在一个语音频道里面，可以随时讲话，可以播放背景音乐，戴耳机情况下可以监听自己语音
		YOUME_USER_HOST,             ///< 主播，广播型语音频道的主持人，同一个时刻只能在一个语音频道里面，可以随时讲话，可以播放背景音乐，戴耳机情况下可以监听自己语音
		YOUME_USER_GUSET,            ///< 嘉宾，主播或指挥邀请的连麦嘉宾，同一个时刻只能在一个语音频道里面， 可以随时讲话
    }

    /// 事件通知
    public enum YouMeEvent
    {
		YOUME_EVENT_INIT_OK              = 0,  ///< SDK初始化成功
		YOUME_EVENT_INIT_FAILED          = 1,  ///< SDK初始化失败
		YOUME_EVENT_JOIN_OK              = 2,  ///< 进入语音频道成功
		YOUME_EVENT_JOIN_FAILED          = 3,  ///< 进入语音频道失败
		YOUME_EVENT_LEAVED_ONE           = 4,  ///< 退出单个语音频道完成
		YOUME_EVENT_LEAVED_ALL           = 5,  ///< 退出所有语音频道完成
		YOUME_EVENT_PAUSED               = 6,  ///< 暂停语音频道完成
		YOUME_EVENT_RESUMED              = 7,  ///< 恢复语音频道完成
		YOUME_EVENT_SPEAK_SUCCESS        = 8,  ///< 切换对指定频道讲话成功（适用于多频道模式）
		YOUME_EVENT_SPEAK_FAILED         = 9,  ///< 切换对指定频道讲话失败（适用于多频道模式）
		YOUME_EVENT_RECONNECTING         = 10, ///< 断网了，正在重连
		YOUME_EVENT_RECONNECTED          = 11, ///< 断网重连成功
		YOUME_EVENT_REC_FAILED           = 12, ///< 通知录音启动失败（此时不管麦克风mute状态如何，都没有声音输出）
		YOUME_EVENT_BGM_STOPPED          = 13, ///< 通知背景音乐播放结束
		YOUME_EVENT_BGM_FAILED           = 14, ///< 通知背景音乐播放失败
		YOUME_EVENT_MEMBER_CHANGE        = 15, ///< 频道成员变化
		YOUME_EVENT_OTHERS_MIC_ON        = 16, ///< 其他用户麦克风打开
		YOUME_EVENT_OTHERS_MIC_OFF       = 17, ///< 其他用户麦克风关闭
		YOUME_EVENT_OTHERS_SPEAKER_ON    = 18, ///< 其他用户扬声器打开
		YOUME_EVENT_OTHERS_SPEAKER_OFF   = 19, ///< 其他用户扬声器关闭
		YOUME_EVENT_OTHERS_VOICE_ON      = 20, ///< 其他用户进入讲话状态
		YOUME_EVENT_OTHERS_VOICE_OFF     = 21, ///< 其他用户进入静默状态
	    YOUME_EVENT_MY_MIC_LEVEL         = 22, ///< 麦克风的语音级别
    }

    public enum YouMeErrorCode
    {
        YOUME_SUCCESS                            = 0,    ///< 成功

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
		YOUME_ERROR_QUERY_RESTAPI_FAIL			 = -107, ///< 请求RestApi信息失败了	

        // 麦克风错误
        YOUME_ERROR_REC_INIT_FAILED              = -201, ///< 录音模块初始化失败
        YOUME_ERROR_REC_NO_PERMISSION            = -202, ///< 没有录音权限
        YOUME_ERROR_REC_NO_DATA                  = -203, ///< 虽然初始化成功，但没有音频数据输出，比如oppo系列手机在录音权限被禁止的时候
        YOUME_ERROR_REC_OTHERS                   = -204, ///< 其他录音模块的错误
        YOUME_ERROR_REC_PERMISSION_UNDEFINED     = -205, ///< 录音权限未确定，iOS显示是否允许录音权限对话框时，返回的是这个错误码

        YOUME_ERROR_UNKNOWN = -1000,         ///< 未知错误

    }

	public enum YOUME_RTC_SERVER_REGION {
		RTC_CN_SERVER       = 0,  // 中国
		RTC_HK_SERVER       = 1,  // 香港
		RTC_US_SERVER       = 2,  // 美国东部
		RTC_SG_SERVER       = 3,  // 新加坡
		RTC_KR_SERVER       = 4,  // 韩国
		RTC_AU_SERVER       = 5,  // 澳洲
		RTC_DE_SERVER       = 6,  // 德国
		RTC_BR_SERVER       = 7,  // 巴西
		RTC_IN_SERVER       = 8,  // 印度
		RTC_JP_SERVER       = 9,  // 日本
		RTC_IE_SERVER       = 10, // 爱尔兰
		RTC_USW_SERVER      = 11, // 美国西部
		RTC_USM_SERVER      = 12, // 美国中部
		RTC_CA_SERVER       = 13, // 加拿大
		RTC_LON_SERVER      = 14, // 伦敦
		RTC_FRA_SERVER      = 15, // 法兰克福
		RTC_DXB_SERVER      = 16, // 迪拜
	
		RTC_EXT_SERVER     = 10000, // 使用扩展服务器
		RTC_DEFAULT_SERVER = 10001, // 缺省服务器
	}
}
