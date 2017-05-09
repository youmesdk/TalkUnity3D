using UnityEngine;
using UnityEngine.UI;
using YouMe;
using YIMEngine;

public class IMTest : MonoBehaviour {

    public InputField userIDInput;
    public Button loginButton;
    public Button logoutButton;
    public Text logText;
    public Text recvMsgText;

    public InputField textMsgInput;

    IMClient IM;

    // Use this for initialization
    void Start () {
    
        IM = IMClient.Instance;
        IM.Initialize("YOUME670584CA1F7BEF370EC7780417B89BFCC4ECBF78", "yYG7XY8BOVzPQed9T1/jlnWMhxKFmKZvWSFLxhBNe0nR4lbm5OUk3pTAevmxcBn1mXV9Z+gZ3B0Mv/MxZ4QIeDS4sDRRPzC+5OyjuUcSZdP8dLlnRV7bUUm29E2CrOUaALm9xQgK54biquqPuA0ZTszxHuEKI4nkyMtV9sNCNDMBAAE=",new Config(){ServerZone=ServerZone.China});
    
        userIDInput.text = Random.Range(10000,999999).ToString();
    }

    public void OnLoginClick(){
        userIDInput.interactable = false;

        // 注册接收新消息的方法
        IM.SetReceiveMessageListener( OnReceiveMessage );
        // 断线事件监听
        IM.SetDisconnectListener((disconnectEvt)=>{
            Debug.Log("断线了");
        });
        // 被踢下线事件监听
        IM.SetKickOffListener((kickoffEvt)=>{
            Debug.Log("被踢下线了");
        });

        //开始登陆
        IM.Login(userIDInput.text,"",(evt)=>{
            if(evt.Code == StatusCode.Success){
                userIDInput.interactable = false; loginButton.interactable = false; logoutButton.interactable = true;
                //进入聊天频道
                IM.JoinChannel(new IMChannel("5678"),(channelEvt)=>{
                    if(channelEvt.code == StatusCode.Success){
                        Log2UI("进入频道:5678 成功");
                    }else{
                        Log2UI("进入频道:5678 失败");
                    }
                });
                Log2UI("登陆成功");
                Log2UI("开始进入频道:5678");
            }else{
                userIDInput.interactable = true; loginButton.interactable = true; logoutButton.interactable = false;
                Log2UI("登陆失败，错误码："+evt.Code);
            }
        });
        
    }

    public void OnLogoutClick(){
        IM.Logout((evt)=>{
            userIDInput.interactable = true; loginButton.interactable = true; logoutButton.interactable = false;
            Log2UI("注销登陆");
        });
    }

    public void OnSendTextMsgClick(){
        var msg = IM.SendTextMessage(userIDInput.text,ChatType.PrivateChat,textMsgInput.text,(code,textMsgObj)=>{
            if (code == StatusCode.Success)
            {
                Log2UI("发送：" + textMsgObj.content + " 成功");
            }else{
                Log2UI("发送：" + textMsgObj.content + " 失败");
            }
        });
    }

    public void OnStartRecordVoiceClick(){
        Log2UI("启动录音");
        var msg = IM.StartRecordAudio(userIDInput.text, ChatType.PrivateChat, "",false, (code, audioMsg) =>
        {
            if(audioMsg.sendStatus == SendStatus.Sending){
                Log2UI("语音发送中");
                Log2UI("开始播放自己发送的语音");
                // audioMsg.Play((audiomsg)=>{
                //     Log2UI("完成播放自己发送的语音");
                // });
            }else if(audioMsg.sendStatus == SendStatus.Sended){
                Log2UI("语音发送成功");
            }else if(audioMsg.sendStatus == SendStatus.Fail){
                Log2UI("语音发送失败");
            }else if (code != StatusCode.Success) { 
                 Log2UI("启动录音失败");
            }
        });
    }
	
    public void OnStopRecordAndSendClick(){

        bool ret = IMClient.Instance.StopRecordAndSendAudio();
        if(ret){
            Log2UI("停止录音成功");
        }else{
            Log2UI("停止录音失败，不会发送");
        }
    }

	void OnReceiveMessage(IMMessage msg){
		if(msg.messageType == MessageBodyType.Voice){
            var audioMsg = (AudioMessage)msg;
            ShowMsg(audioMsg);
            Log2UI("下载语音文件");
            audioMsg.Download((code,audiMsgObj)=>{
                Log2UI("播放接收到的语音消息");
                audiMsgObj.Play( null );
            });
        }else if(msg.messageType == MessageBodyType.TXT){
            ShowMsg(msg);
        }
	}

	void ShowMsg(IMMessage msg){
        if (msg.messageType == MessageBodyType.TXT)
        {
            recvMsgText.text = "收到文本："+((TextMessage)msg).content + "\n" + recvMsgText.text;
        }else{
            recvMsgText.text = "收到语音："+((AudioMessage)msg).recognizedText + "\n" + recvMsgText.text;
        }
    }

    void Log2UI(string log){
        logText.text = log + "\n" + logText.text;
    }
}
