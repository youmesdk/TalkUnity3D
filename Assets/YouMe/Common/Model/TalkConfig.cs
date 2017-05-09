using YouMe;

namespace YouMe
{
    public class TalkConfig
    {
        public YOUME_RTC_SERVER_REGION ServerZone { get; set; }
        public string                  ExtServerZone { get; set; }
        public LogLevel                LogLevel { get; set; }
        public bool                    AllowMultiChannel { get; set; }

        public TalkConfig(){
            ServerZone = YOUME_RTC_SERVER_REGION.RTC_DEFAULT_SERVER;
            AllowMultiChannel = false;
            ExtServerZone = null;
        }
    }
}