
namespace YouMe{
    
    public class TalkChannel : IChannel
    {
        string channelID;

        public string ChannelID{
            get{
                return channelID;
            }
        }

        public TalkChannel(string channelID){
            this.channelID = channelID;
        }
    }
}