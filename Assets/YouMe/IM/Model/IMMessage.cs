using System;
using YouMe;
using YIMEngine;

public class IMMessage{
    public MessageBodyType messageType;
    public string senderID;
    public string reciverID;
    public ChatType chatType;
    public bool isReceiveFromServer;
    public SendStatus sendStatus;
    public uint sendTime;

    /// <summary>
    /// 会话期间的消息唯一id
    /// </summary>
    public ulong requestID;
}

public enum MessageDownloadStatus{
    NOTDOWNLOAD, DOWNLOADING, DOWNLOADED, DOWNLOAD_FAIL
}

public enum ChatType{
    Unknow = 0,
    PrivateChat = 1,
    RoomChat = 2,
}

public enum SendStatus{
    NotStartSend = 0,
    Sending = 1,
    Sended = 2,
    Fail = 3,
}

public class AudioMessage:IMMessage{

    public MessageDownloadStatus downloadStatus;
    public bool isRecorgnizeText;
    public string audioFilePath;
    public string recognizedText;
    public string extraParam;
    public int audioDuration;

    public AudioMessage(string sender,string reciverID,ChatType chatType,string extraParam,bool isFromServer){
        this.senderID = sender;
        this.messageType = MessageBodyType.Voice;
        this.reciverID = reciverID;
        this.chatType = chatType;
        this.extraParam = extraParam;
        // this.isRecorgnizeText = isRecorgnizeText;
        this.isReceiveFromServer = isFromServer;

        if(isFromServer){
            sendStatus = SendStatus.Sended;
        }else{
            sendStatus = SendStatus.NotStartSend;
        }
    }

    public void Play(Action<AudioMessage> OnPlayCompletion){
        if( !string.IsNullOrEmpty( audioFilePath ) ){
            YIMEngine.IMAPI.Instance().StartPlayAudio(audioFilePath);
        }
        // TODO: ADD callback
    }
    public void PlayInQueue(){}

    public void Download(Action<StatusCode,AudioMessage> downloadCallback){
        Download(GetUniqAudioPath(),downloadCallback);
    }

    public void Download(string targetPath, Action<StatusCode,AudioMessage> downloadCallback){
        if(!isReceiveFromServer){
            Log.e("只能下载从服务器收到的语音消息，自己发送的语音消息不需要下载。");
            return;
        }
        if( this.downloadStatus == MessageDownloadStatus.DOWNLOADED ){
            if( downloadCallback!=null ) downloadCallback(StatusCode.Success,this);
            return;
        }
        this.downloadStatus = MessageDownloadStatus.DOWNLOADING;
        IMClient.Instance.DownloadFile( this.requestID, targetPath, (StatusCode code,string filePath )=>{
            if( code == StatusCode.Success ){
                this.downloadStatus = MessageDownloadStatus.DOWNLOADED;
            }else{
                this.downloadStatus = MessageDownloadStatus.DOWNLOAD_FAIL;
            }
            this.audioFilePath = filePath;
            if( downloadCallback!=null ) downloadCallback( code, this );
        });
    }

    private string GetUniqAudioPath()
    {
        return UnityEngine.Application.temporaryCachePath + "/YoumeIMAudioCache/"+ requestID + ".wav";
    }
}

public class TextMessage:IMMessage{
    public string content;

    public TextMessage(string sender,string reciver,ChatType chatType,string content,bool isFromServer){
        this.messageType = MessageBodyType.TXT;
        this.senderID = sender;
        this.reciverID = reciver;
        this.chatType = chatType;
        this.content = content;
        this.isReceiveFromServer = isFromServer;
        if(isFromServer){
            sendStatus = SendStatus.Sended;
        }else{
            sendStatus = SendStatus.NotStartSend;
        }
    }
}