using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
public class AppHomeViewController : MonoBehaviour {
    public bool IsTest = false;
    public static AppHomeViewController Instance { get { return instance; } }
    static AppHomeViewController instance;
    public static bool IsShowGuide = true;
    private void Awake()
    {
        instance = this;
    }
    public GameObject SearchCanvas, SettingCanvas, GuidePanel, DrugInfoCanvas, UpdateCanvas;
    public Text LogText;
    public Button ShengJiButton;
    public Button ArButton, SearchButton, SettingButton;
    public Text AppNameText, ComNameText;
    public Image LogoImage, TopImage, LeftLineImage, RightLineImage;
    void Start()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
            downloadButtonClick();
        Helper.Settingcanvas = SettingCanvas;
        Helper.Searchcanvas = SearchCanvas;
        Helper.Guidepanel = GuidePanel;
        Helper.DrugInfocanvas = DrugInfoCanvas;
        Helper.Updatecanvas = UpdateCanvas;
        Helper.Init();
        StartCoroutine(IsFisrtOpenApp());
        _ui_events();
        _ui();
        if (Application.platform == RuntimePlatform.Android)
        {
            LogText.text = "持久路径：\n" + Application.persistentDataPath;
            var Build = new AndroidJavaClass("android.os.Build");
            var brand = Build.GetStatic<string>("BRAND");
            Debug.Log(" 厂商：" + brand + " 应用版本：" + Application.version);

            if (IsTest)
                LogText.gameObject.SetActive(true);
            LogText.text += "\n" + " 厂商：" + brand;

        }
        Debug.Log(" 应用版本：" + Application.version + "\n持久路径：" + Application.persistentDataPath);
    }
    void Update () {
		// 按了返回按钮
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Helper.Updatecanvas.activeSelf && !SettingCanvas.activeSelf && !SearchCanvas.activeSelf)
                Application.Quit();
        }
	}
    void _ui()
    {
        // topImage
        float top_image_height = (float)(55.5 / 667.0) * Display.main.systemHeight;
        TopImage.rectTransform.sizeDelta = new Vector2(Display.main.systemWidth, top_image_height);
        // logoImage
        float logoImage_posx = (float)(24.5 / 375.0) * Display.main.systemWidth,
            logoImage_posy = -(float)(13 / 667.0) * Display.main.systemHeight,
            logo_width = (float)(22.5 / 375.0) * Display.main.systemWidth,
            logo_height = (float)(579 / 453.0) * logo_width;
        LogoImage.rectTransform.sizeDelta = new Vector2(logo_width, logo_height);
        LogoImage.rectTransform.anchoredPosition3D = new Vector3(logoImage_posx, logoImage_posy, 0);
        // 设置按钮
        float set_button_posx = -(float)(25 / 375.0) * Display.main.systemWidth,
            set_button_width = (float)(20.5 / 375.0) * Display.main.systemWidth;
        SettingButton.GetComponent<RectTransform>().sizeDelta = new Vector2(set_button_width, set_button_width);
        SettingButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(set_button_posx, 0, 0);
        // 应用文本
        float app_posx = (float)(34 / 375.0) * Display.main.systemWidth,
            app_width = Display.main.systemWidth - app_posx - logoImage_posx + set_button_posx - set_button_width,
            app_height = (float)(14 / 667.0) * Display.main.systemHeight;
        AppNameText.rectTransform.sizeDelta = new Vector2(app_width, app_height);
        AppNameText.rectTransform.anchoredPosition3D = new Vector3(app_posx, 0, 0);
        // 搜索按钮、扫一扫按钮
        float search_button_width = (float)(136 / 375.0) * Display.main.systemWidth,
            search_button_posy = -(float)(163 / 667.0) * Display.main.systemHeight,
            scan_button_posy = (float)(149.5 / 667.0) * Display.main.systemHeight;
        ArButton.GetComponent<RectTransform>().sizeDelta = new Vector2(search_button_width, search_button_width);
        ArButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, scan_button_posy, 0);
        SearchButton.GetComponent<RectTransform>().sizeDelta = new Vector2(search_button_width, search_button_width);
        SearchButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, search_button_posy, 0);
        // 公司文本
        float com_text_posy = (float)(23 / 667.0) * Display.main.systemHeight,
            com_text_width = (float)(128 / 375.0) * Display.main.systemWidth,
            com_text_height = (float)(12.5 / 128.0) * com_text_width;
        ComNameText.rectTransform.sizeDelta = new Vector2(com_text_width, com_text_height);
        ComNameText.rectTransform.anchoredPosition3D = new Vector3(0, com_text_posy, 0);
        // 线
        float line_posx = (float)(8.5 / 375.0) * Display.main.systemWidth,
            line_width = (float)(49 / 375.0) * Display.main.systemWidth,
            line_height = Display.main.systemHeight > 667.0 ? (float)(0.5 / 667.0) * Display.main.systemHeight : 0.5f;
        LeftLineImage.rectTransform.sizeDelta = new Vector2(line_width, line_height);
        LeftLineImage.rectTransform.anchoredPosition3D = new Vector3(-line_posx, 0, 0);
        RightLineImage.rectTransform.sizeDelta = new Vector2(line_width, line_height);
        RightLineImage.rectTransform.anchoredPosition3D = new Vector3(line_posx, 0, 0);
        // 隐藏界面
        SettingCanvas.SetActive(false);
        SearchCanvas.SetActive(false);
        DrugInfoCanvas.SetActive(false);
        UpdateCanvas.SetActive(false);
    }
    void _ui_events()
    {
        ArButton.onClick.AddListener(arButtonClick);
        SearchButton.onClick.AddListener(searchButtonClick);
        //ShengJiButton.onClick.AddListener(downloadButtonClick);
        SettingButton.onClick.AddListener(_settingButtonClick);
    }
    void arButtonClick()
    {
        SearchCanvas.GetComponent<SearchViewController>().InputField.text = string.Empty;

        LoadingViewController.SearchCanvas = SearchCanvas;
        LoadingViewController.DrugInfoCanvas = DrugInfoCanvas;
        LoadingViewController.UpdateCanvas = UpdateCanvas;
        Helper.Searchcanvas.GetComponent<SearchViewController>().InputField.text = string.Empty;
        for (int i = 0; i < Helper.Searchcanvas.GetComponent<SearchViewController>().ContentPanel.transform.childCount; ++i)
            Destroy(Helper.Searchcanvas.GetComponent<SearchViewController>().ContentPanel.transform.GetChild(i).gameObject);
        SceneManager.LoadScene("Scene/start");
    }
    void searchButtonClick()
    {
        Debug.Log(" AppHomeViewController 点击了搜索按钮。");
        var dh = SearchCanvas.GetComponent<DataHelper>();
        if (dh == null)
            gameObject.AddComponent<DataHelper>();
        SearchCanvas.SetActive(true);
    }
    void _settingButtonClick()
    {
        Debug.Log(" 点击设置！");
        SettingCanvas.SetActive(true);
    }
    //IEnumerator loadjson()
    //{
    //    WWW www = new WWW("http://kuwan.jia1du.com/appcall/mall/plandetail.do?tid=33");
    //    yield return www;
    //    File.WriteAllText(Application.dataPath + "/json.txt", www.text);／／保存本地

    //       StreamReader sr = File.OpenText(Application.dataPath + "/json.txt");／／读取

    //           Root josn = LitJson.JsonMapper.ToObject <</ span > Root > (sr.ReadLine());／／解析

    //    Debug.Log(josn.result.title);
    //}
    void downloadButtonClick()
    {
        Debug.Log("点击升级！");
        string url = "http://update.haopeixun.org/BTSJARAppUpdate/checkUpdate";
        string apkPathName = string.Empty;
        apkPathName = Application.persistentDataPath + "/testapk.apk";
        WWWForm wwwform = new WWWForm();
        if (Application.platform == RuntimePlatform.Android)
            wwwform.AddField("type", "android");
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            wwwform.AddField("type", "iphone");
        wwwform.AddField("version", Application.version);
        wwwform.AddField("device_id", SystemInfo.deviceUniqueIdentifier);
        wwwform.AddField("channel", "蒲公英");
        Debug.Log("准备post数据，并发出请求！, 设备号："+ SystemInfo.deviceUniqueIdentifier);
        tabfun.helper.Coroutine.Instance.StartWWW(new WWW(url, wwwform), (www) =>
        {
            if (www.error == null)
            {
                Debug.Log(" 服务器返回数据：" + www.text);

                File.WriteAllText(Application.dataPath + "/json.txt", www.text);
                StreamReader sr = File.OpenText(Application.dataPath + "/json.txt");
                var jsondata = LitJson.JsonMapper.ToObject(sr.ReadLine());
                if (jsondata != null && jsondata.IsObject)
                {
                    tabfun.helper.LitJson.FindKey(jsondata, "need_update", (jd) => {
                        if (jd != null)
                        {
                            Debug.Log(" need_update = " + jd.ToString());
                            if (jd.ToString() == "1")
                            {
                                if (Application.platform == RuntimePlatform.IPhonePlayer)
                                {
                                    var updateUIObj = Helper.Updatecanvas.GetComponent<UpdateController>();
                                    updateUIObj.UpdateButton.transform.GetChild(0).GetComponent<Text>().text = "AppStore";
                                    updateUIObj.ContentText.text = "请转到 App Store 中下载更新";
                                    updateUIObj.UpdateButton.enabled = true;
                                    updateUIObj.UpdateButton.onClick.RemoveAllListeners();
                                    updateUIObj.UpdateButton.onClick.AddListener(() =>
                                    {
                                        
                                    });
                                }
                                else if (Application.platform == RuntimePlatform.Android)
                                {

                                    Debug.Log(jsondata["up_title"] + " 内容：" + jsondata["up_content"]);
                                    var updateUIObj = Helper.Updatecanvas.GetComponent<UpdateController>();
                                    updateUIObj.TitleText.text = jsondata["up_title"].ToString();
                                    updateUIObj.ContentText.text = jsondata["up_content"].ToString();
                                    updateUIObj.UpdateButton.onClick.AddListener(() =>
                                    {
                                        updateUIObj.UpdateButton.enabled = false;
                                        updateUIObj.UpdateButton.transform.GetChild(0).GetComponent<Text>().text = "等待下载";
                                        url = jsondata["url"].ToString();
                                        StartCoroutine(ApkController.Instance.DownloadFile(url, apkPathName,
                                           (url_arg, downloadpathmame, bytes) =>
                                           {

                                               updateUIObj.ContentText.text = "100%";
                                               updateUIObj.UpdateButton.transform.GetChild(0).GetComponent<Text>().text = "等待安装";
                                               Debug.Log(" 文件已经下载完毕！" + bytes);
                                               if (File.Exists(apkPathName))
                                                   File.Delete(apkPathName);
                                               File.WriteAllBytes(apkPathName, bytes);
                                               Debug.Log(" 文件写入路径：" + apkPathName);
                                           },
                                           (propress) =>
                                           {
                                               Debug.Log(" 进度：" + propress);
                                               updateUIObj.ContentText.text = System.Math.Round(propress, 4) * 100 + "%";
                                               if (propress > 0.99f)
                                               {
                                                   updateUIObj.ContentText.text = "100%";
                                                   updateUIObj.UpdateButton.transform.GetChild(0).GetComponent<Text>().text = "等待安装";
                                               }
                                           },
                                           (isError, errorOrText, downloadpathname) =>
                                           {
                                               if (isError)
                                               {
                                                   updateUIObj.ContentText.text = errorOrText;
                                                   updateUIObj.UpdateButton.enabled = true;
                                                   updateUIObj.UpdateButton.onClick.RemoveAllListeners();
                                                   updateUIObj.UpdateButton.onClick.AddListener(() =>
                                                   {
                                                       Application.Quit();
                                                   });
                                                   return;
                                               }
                                               else
                                               {

                                                   updateUIObj.UpdateButton.transform.GetChild(0).GetComponent<Text>().text = "完成";
                                                   updateUIObj.UpdateButton.enabled = true;
                                                   updateUIObj.UpdateButton.onClick.RemoveAllListeners();
                                                   updateUIObj.UpdateButton.onClick.AddListener(() =>
                                                   {
                                                       Application.Quit();
                                                   });
                                                   ApkController.Instance.InstallAPK(downloadpathname, LogText);
                                               }
                                           }));
                                    });
                                    updateUIObj.gameObject.SetActive(true);
                                }else
                                {
                                    var updateUIObj = Helper.Updatecanvas.GetComponent<UpdateController>();

                                    updateUIObj.UpdateButton.transform.GetChild(0).GetComponent<Text>().text = "完成";
                                    updateUIObj.ContentText.text = "安装不了：" + Application.platform;
                                    Debug.Log(" 安装不了：" + Application.platform);
                                    updateUIObj.UpdateButton.enabled = true;
                                    updateUIObj.UpdateButton.onClick.RemoveAllListeners();
                                    updateUIObj.UpdateButton.onClick.AddListener(() =>
                                    {
                                        Application.Quit();
                                    });
                                }
                            }
                        }
                    });
                }
                Debug.Log(" 数据请求成功！" + www.text);
            }
            else
            {
                Debug.Log(" 数据请求失败");
            }
        });
    }
    IEnumerator IsFisrtOpenApp()
    {
        string path = "/firstOpenApp";
        path = Application.persistentDataPath + path;
        yield return new WaitForEndOfFrame();
        if (!File.Exists(path) && IsShowGuide)
        {
            Debug.Log(" 第一次打开应用");
            SettingCanvas.SetActive(true);
            SettingCanvas.GetComponent<SettingViewController>().isFirstOpen = true;
            StartCoroutine(openGuide());
        }
    }
    IEnumerator openGuide()
    {
        yield return new WaitForEndOfFrame();
        GuidePanel.SetActive(true);
    }
}
