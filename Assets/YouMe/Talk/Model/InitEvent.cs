namespace YouMe
{
    public class InitEvent
    {
        private StatusCode _code = StatusCode.UnknowError;

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

        public InitEvent(StatusCode code)
        {
            _code = code;
        }

        public InitEvent(YouMeErrorCode code)
        {
            _code = Conv.ErrorCodeConvert(code);
        }
    }
}