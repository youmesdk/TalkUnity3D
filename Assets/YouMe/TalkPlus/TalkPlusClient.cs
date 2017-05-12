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

        public void JoinIMChannel(Channel channel, Action<ChannelEvent> joinChannelCallback)
        {
            im.JoinChannel(channel, joinChannelCallback);
        }

        public void JoinTalkChannel(Channel channel, Action<ChannelEvent> joinChannelCallback, YouMeUserRole role = YouMeUserRole.YOUME_USER_TALKER_FREE)
        {
            talk.JoinChannel(channel, joinChannelCallback, role);
        }

        public void LeaveIMChannel(Channel channel, Action<ChannelEvent> leaveIMChannelCallback)
        {
                im.LeaveChannel(channel, leaveIMChannelCallback);
        }

        public void LeaveTalkChannel(Channel channel, Action<ChannelEvent> leaveTalkChannelCallback)
        {
            talk.LeaveChannel(channel, leaveTalkChannelCallback);
        }

        public void LeaveAllTalkChannel(Action<ChannelEvent> leaveTalkChannelCallback)
        {
            talk.LeaveAllChannel(leaveTalkChannelCallback);
        }

        public void LeaveAllIMChannel(Action<ChannelEvent> leaveIMChannelCallback)
        {
            im.LeaveAllChannel(leaveIMChannelCallback);
        }

        public void Logout(Action<LogoutEvent> callback)
        {
            talk.LeaveAllChannel((ChannelEvent evt) =>
            {
                im.Logout(callback);
            });
        }

        public IMClient GetIMClient
        {
            get
            {
                return im;
            }
        }

        public TalkClient GetTalkClient
        {
            get
            {
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