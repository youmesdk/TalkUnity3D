using UnityEngine;
using System.Collections;
using System;

namespace YouMe
{
    public class TalkPlusClient : IClient
    {

        static TalkPlusClient _ins;
        public static TalkPlusClient Instance
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new TalkPlusClient();
                }
                return _ins;
            }
        }

        private string _userID = "";
        private TalkPlusConfig _config;
        private IMClient im;
        private TalkClient talk;

        private TalkPlusClient()
        {
            im = IMClient.Instance;
            talk = TalkClient.Instance;
        }

        public void Initialize(string appKey, string secretKey, TalkPlusConfig config, Action<InitEvent> initCallBack)
        {
            if (config == null)
            {
                Log.e("config can't not be null.");
                return;
            }
            im.Initialize(appKey, secretKey, PlusConfigToIMConfig(config));//IM目前不需要等回调通知
            talk.Initialize(appKey, secretKey, PlusConfigToTalkConfig(config), initCallBack);
        }

        public void Login(string userID, string token, Action<LoginEvent> callback)
        {
            _userID = userID;
            im.Login(userID, token, callback);
        }

        public string GetUserID()
        {
            return _userID;
        }

        public void JoinChannel(Channel channel, bool withIMChannel, Action<ChannelEvent> joinTalkChannelCallback, Action<ChannelEvent> joinIMChannelCallback, YouMeUserRole role = YouMeUserRole.YOUME_USER_TALKER_FREE)
        {
            talk.JoinChannel(channel, joinTalkChannelCallback, role);
            if (withIMChannel)
            {
                im.JoinChannel(channel, joinIMChannelCallback);
            }
        }

        public void LeaveChannel(Channel channel, bool withIMChannel, Action<ChannelEvent> leaveTalkChannelCallback, Action<ChannelEvent> leaveIMChannelCallback)
        {
            talk.LeaveChannel(channel, leaveTalkChannelCallback);
            if( withIMChannel )
            {
                im.LeaveChannel(channel, leaveIMChannelCallback);
            }
        }

        public void LeaveAllChannel(bool withIMChannel, Action<ChannelEvent> leaveTalkChannelCallback, Action<ChannelEvent> leaveIMChannelCallback)
        {
            talk.LeaveAllChannel(leaveTalkChannelCallback);
            if (withIMChannel)
            {
                im.LeaveAllChannel(leaveIMChannelCallback);
            }
        }

        public void Logout(Action<LogoutEvent> callback)
        {
            talk.LeaveAllChannel((ChannelEvent evt)=>{
                im.Logout(callback);
            });
        }

        public IMClient GetIMClient{
            get{
                return im;
            }
        }

        public TalkClient GetTalkClient{
            get{
                return talk;
            }
         }

        IMConfig PlusConfigToIMConfig(TalkPlusConfig conf)
        {
            var imConf = new IMConfig();
            imConf.ServerZone = conf.ServerZone;
            imConf.LogLevel = conf.LogLevel;
            return imConf;
        }

        TalkConfig PlusConfigToTalkConfig(TalkPlusConfig conf)
        {
            var talkConf = new TalkConfig();
            talkConf.AllowMultiChannel = conf.AllowMultiChannel;
            talkConf.ExtServerZone = conf.ExtServerZone;
            talkConf.LogLevel = conf.LogLevel;
            talkConf.ServerZone = (YOUME_RTC_SERVER_REGION)conf.ServerZone;
            return talkConf;
        }

    }
}