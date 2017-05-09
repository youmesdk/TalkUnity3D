using UnityEngine;
using UnityEngine.UI;
using YouMe;
using YIMEngine;

public class TalkTest : MonoBehaviour {

    public InputField userIDInput;
    public InputField channelInput;
    public Button joinButton;
    public Button leaveButton;
    public Button microphoneButton;
    public Button speakerButton;
    public Text microphoneText;
    public Text speakerText;
    public Text logText;

    TalkClient talk;

    // Use this for initialization
    void Start () {
        UpdateUninitUI();
        talk = TalkClient.Instance;
        Log2UI("开始初始化请求");
        talk.Initialize("YOUME670584CA1F7BEF370EC7780417B89BFCC4ECBF78", "yYG7XY8BOVzPQed9T1/jlnWMhxKFmKZvWSFLxhBNe0nR4lbm5OUk3pTAevmxcBn1mXV9Z+gZ3B0Mv/MxZ4QIeDS4sDRRPzC+5OyjuUcSZdP8dLlnRV7bUUm29E2CrOUaALm9xQgK54biquqPuA0ZTszxHuEKI4nkyMtV9sNCNDMBAAE=",new TalkConfig(),(InitEvent evt)=>{
            if(evt.Code == StatusCode.Success){
                Log2UI("初始化成功");
                UpdateInintedUI();
            }else{
                Log2UI("初始化失败，状态码："+evt.Code);
            }
        });
    
        userIDInput.text = Random.Range(10000,999999).ToString();
        channelInput.text = "channel_888";
    }

    public void OnJoinChannelClick()
    {
        UpdateJoinChannelUI();
        var channelID = channelInput.text;
        //开始进语音频道
        Log2UI("开始进入语音频道");

        talk.SetUserID(userIDInput.text);
        talk.JoinChannel(new TalkChannel(channelID), (ChannelEvent evt) => { 
            if(evt.code == StatusCode.Success)
            {
                talk.OpenMicrophone();
                microphoneText.text = "麦克风：开";
                talk.OpenSpeaker();
                speakerText.text = "扬声器：开";
                Log2UI("进入语音频道成功");
            }else{
                UpdateLeavedChannelUI();
                Log2UI("进入语音频道失败，错误码："+evt.code);
            } 
       });
    }

    public void OnLeaveClick(){
        Log2UI("发起离开语音频道请求");
        leaveButton.interactable = false;
        talk.LeaveAllChannel((ChannelEvent evt)=>{
            UpdateLeavedChannelUI();
            if(evt.code == StatusCode.Success)
            {
                Log2UI("成功离开语音频道");
            }else{
                Log2UI("不在语音频道，不需要调用离开频道");
            } 
        });
    }

    void Log2UI(string log){
        logText.text = log + "\n" + logText.text;
    }

    public void SwitchMicrophone(){
        if(talk.IsMicrophoneOpen()){
            talk.CloseMicrophone();
            microphoneText.text = "麦克风：关";
        }else{
            talk.OpenMicrophone();
            microphoneText.text = "麦克风：开";
        }
    }

    public void SwitchSpeaker(){
        if(talk.IsSpeakerOpen()){
            talk.CloseSpeaker();
            speakerText.text = "扬声器：关";
        }else{
            talk.OpenSpeaker();
            speakerText.text = "扬声器：开";
        }
    }

    void UpdateJoinChannelUI(){
        userIDInput.interactable = false;
        channelInput.interactable = false;
        joinButton.interactable = false;
        leaveButton.interactable = true;
        microphoneButton.interactable = true;
        speakerButton.interactable = true;
    }

    void UpdateLeavedChannelUI(){
        userIDInput.interactable = true;
        channelInput.interactable = true;
        joinButton.interactable = true;
        leaveButton.interactable = false;
        microphoneButton.interactable = false;
        speakerButton.interactable = false;
    }

    void UpdateUninitUI(){
        joinButton.interactable = false;
        leaveButton.interactable = false;
        microphoneButton.interactable = false;
        speakerButton.interactable = false;
    }

    void UpdateInintedUI(){
        joinButton.interactable = true;
        leaveButton.interactable = false;
    }
}
