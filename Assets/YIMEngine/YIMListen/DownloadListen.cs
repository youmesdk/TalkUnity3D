
namespace YIMEngine
{
	public interface DownloadListen
	{
		void OnDownload(ulong iRequestID,  YIMEngine.ErrorCode errorcode,string strSavePath);	
	}
}