using YouMe;

namespace YouMe
{
    public class TalkPlusConfig
    {
        public YIMEngine.ServerZone ServerZone { get; set; }
        public string                  ExtServerZone { get; set; }
        public LogLevel                LogLevel { get; set; }
        public bool                    AllowMultiChannel { get; set; }

        public TalkPlusConfig(){
            ServerZone = YIMEngine.ServerZone.China;
            AllowMultiChannel = false;
            ExtServerZone = null;
        }
    }
}