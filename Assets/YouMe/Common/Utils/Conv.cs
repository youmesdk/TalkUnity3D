using YIMEngine;

namespace YouMe{
    public static class Conv{
        public static StatusCode ErrorCodeConvert(YIMEngine.ErrorCode errorcode){
            return (StatusCode)errorcode;
        }

        public static StatusCode ErrorCodeConvert(YouMeErrorCode errorcode){
            return (StatusCode)errorcode;
        }
    }
}