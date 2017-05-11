
namespace YouMe{
    
    public class Channel : IChannel
    {
        string channelID;

        public string ChannelID{
            get{
                return channelID;
            }
        }

        public Channel(string channelID){
            this.channelID = channelID;
        }
    }
}