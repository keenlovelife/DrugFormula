using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ApkController : MonoBehaviour {
    public static ApkController Instance { get { return _instance; } }
    static ApkController _instance;
    private void Awake()
    {
        _instance = this;
    }
    void Start () {
	}
    void Update () {
		
	}
    /// <summary>
    /// 下载并写入文件
    /// </summary>
    /// <param name="url"></param>
    /// <param name="downloadPathName"></param>
    /// <param name="actionBytes"></param>
    /// <param name="percentAction"></param>
    /// <param name="endAction"></param>
    /// <returns></returns>
    public IEnumerator DownloadFile(string url, string downloadPathName, System.Action<string, string, byte[]> actionBytes, System.Action<float> percentAction, System.Action<bool, string, string> endAction)
    {
        Debug.Log(" 正在下载文件：url=" + url);
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            if (null != percentAction)
                percentAction(www.progress);
            yield return null;
        }
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("WWW DownloadFile：" + www.error);
            if (null != endAction)
                endAction(true, www.error, downloadPathName);
        }
        if (null != actionBytes)
            actionBytes(url, downloadPathName, www.bytes);
        if (null != endAction)
            endAction(false, www.text, downloadPathName);
        www.Dispose();
    }
    /// <summary>
    /// 调用系统方法进行安装
    /// </summary>
    /// <param name="path"></param>
    /// <param name="bTrTry"></param>
    /// <returns></returns>
    public bool InstallAPK(string path, Text log)
    {
        try
        {
            var Intent = new AndroidJavaClass("android.content.Intent");
            var ACTION_VIEW = Intent.GetStatic<string>("ACTION_VIEW");
            var FLAG_ACTIVITY_NEW_TASK = Intent.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
            var intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);
            var file = new AndroidJavaObject("java.io.File", path);
            var Uri = new AndroidJavaClass("android.net.Uri");
            var uri = Uri.CallStatic<AndroidJavaObject>("fromFile", file);
            intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/vnd.android.package-archive");
            intent.Call<AndroidJavaObject>("addFlags", FLAG_ACTIVITY_NEW_TASK);
            intent.Call<AndroidJavaObject>("setClassName", "com.android.packageinstaller", "com.android.packageinstaller.PackageInstallerActivity");
            var UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", intent);
            if (!log.gameObject.activeSelf)
            {
                log.gameObject.SetActive(true);
                log.text += "Install Ok";
            }
            return true;
        }
        catch (System.Exception e)
        {
            try
            {
                var Intent = new AndroidJavaClass("android.content.Intent");
                var ACTION_VIEW = Intent.GetStatic<string>("ACTION_VIEW");
                var FLAG_ACTIVITY_NEW_TASK = Intent.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
                var intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);

                var file = new AndroidJavaObject("java.io.File", path);
                var Uri = new AndroidJavaClass("android.net.Uri");
                var uri = Uri.CallStatic<AndroidJavaObject>("fromFile", file);
                intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/vnd.android.package-archive");
                var UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                currentActivity.Call("startActivity", intent);
                if (!log.gameObject.activeSelf)
                {
                    log.gameObject.SetActive(true);
                    log.text += "Install Ok";
                }
                return true;
            }
            catch
            {
                Debug.LogError("Error Install APK:" + e.Message + " -- " + e.StackTrace);
                if (!log.gameObject.activeSelf)
                {
                    log.gameObject.SetActive(true);
                    log.text += "Error Install APK:" + e.Message + " -- " + e.StackTrace;
                }
                return false;
            }
        }
    }
}
