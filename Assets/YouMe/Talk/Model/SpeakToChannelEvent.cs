namespace YouMe
{
    public class SpeakToChannelEvent
    {
        private StatusCode _code = StatusCode.UnknowError;
        private string _channel="";

        public StatusCode Code
        {
            get
            {
                return _code;
            }
        }

        public bool IsSuccess
        {
            get
            {
                return ( _code == 0 );
            }
        }

        public string ChannelID{
            get{
                return _channel;
            }
        }

        public SpeakToChannelEvent(StatusCode code,string channel)
        {
            _code = code;
            _channel = channel;
        }

        public SpeakToChannelEvent(YouMeErrorCode code, string channel)
        {
            _code = Conv.ErrorCodeConvert(code);
            _channel = channel;
        }

    }
}