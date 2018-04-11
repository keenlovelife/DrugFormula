using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
public class AboutViewController : MonoBehaviour {

    public GameObject TopPanel;
    public Button BackButton, TelButton, EmailButton, NetButton, NullButton, NullButton1;
    public Image LogoImage, TelButton_Image, EmailButton_Image, NetButton_Image, TelButton_BgImage, EmailButton_BgImage, NetButton_BgImage,NullButton_BgImage;
    public Text TitleText, NameText, ButtomText, TelButton_NameText, TelButton_InfoText, EmailButton_NameText, EmailButton_InfoText, NetButton_NameText, NetButton_InfoText;
    public GameObject WebPanel;
    public Text WebPanelText;
   
    private void OnEnable()
    {
        canShow = true;
        canHide = false;
    }

    [DllImport("__Internal")]
    static extern void callPhone();
    [DllImport("__Internal")]
    static extern void useEmail();
    [DllImport("__Internal")]
    static extern void officalWebsite();
    void Start () {
        ui_layout();
        ui_enevts();
    }
    void webOnloadCompleted(UniWebView webView, bool success, string errormeesge)
    {
        if (success)
        {
            WebPanelText.gameObject.SetActive(false);
        }else
        {
            //Application.OpenURL("http://" + NetButton_InfoText.text);
            WebPanelText.text = "加载失败! 请在浏览器中重试";
        }
    }

    bool canShow = false, canHide = false;
    void Update () {

        // 按了返回按钮
        if (!Helper.Updatecanvas.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            if (WebPanel.activeSelf)
            {
                WebPanelText.gameObject.SetActive(true);
                WebPanel.SetActive(false);
                Destroy(WebPanel.GetComponent<UniWebView>());
            }
            else if (!canHide && !canShow)
                canHide = true;
        }

        if (canHide)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(gameObject.GetComponent<RectTransform>().anchoredPosition3D, new Vector3(Display.main.systemWidth, 0, 0), 20 * Time.deltaTime);
            if (gameObject.GetComponent<RectTransform>().anchoredPosition3D.x > Display.main.systemWidth - 1)
            {
                canHide = false;
                gameObject.SetActive(false);
            }
        }
        if (canShow)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(gameObject.GetComponent<RectTransform>().anchoredPosition3D, new Vector3(0, 0, 0), 20 * Time.deltaTime);
            if (gameObject.GetComponent<RectTransform>().anchoredPosition3D.x < 1)
            {
                canShow = false;
                gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            }
        }
    }

    void ui_layout()
    {
        // aboutPanel
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Display.main.systemWidth, Display.main.systemHeight);
        WebPanel.GetComponent<RectTransform>().sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
        // 顶部导航条
        var nav_h = (float)(64/667.0)*Display.main.systemHeight;
        TopPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(TopPanel.GetComponent<RectTransform>().sizeDelta.x, nav_h);
        // 返回按钮
        var backbutton_to_left = (float)(26.7 / 375.0) * Display.main.systemWidth;
        var backbutton_w = (float)(23.1/ 375.0) * Display.main.systemWidth;
        var backbutton_h = (float)((18 / 23.1) * backbutton_w);
        BackButton.GetComponent<RectTransform>().sizeDelta = new Vector2(backbutton_w, backbutton_h);
        BackButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(backbutton_to_left, 0, 0);
        // 标题文本
        var titletext_to_top = (float)(18 / 667.0) * Display.main.systemHeight;
        var titletext_h = (float)(22 / 667.0) * Display.main.systemHeight;
        TitleText.rectTransform.sizeDelta = new Vector2(TitleText.rectTransform.sizeDelta.x, titletext_h);
        // 中间logo
        var logo_to_top = (float)(128.1 / 667.0) * Display.main.systemHeight;
        var logo_w = (float)(76/375.0)*Display.main.systemWidth;
        var logo_h = (float)(98.4 / 76.0) * logo_w;
        LogoImage.rectTransform.anchoredPosition3D = new Vector3(0, -logo_to_top, 0);
        LogoImage.rectTransform.sizeDelta = new Vector2(logo_w, logo_h);
        // 名称
        var nametext_to_top = (float)(245.5 / 667.0) * Display.main.systemHeight;
        var nametext_h = (float)(20 / 667.0) * Display.main.systemHeight;
        NameText.rectTransform.sizeDelta = new Vector2(NameText.rectTransform.sizeDelta.x, nametext_h);
        NameText.rectTransform.anchoredPosition3D = new Vector3(0, -nametext_to_top, 0);
        // 站位button
        var null_button_height = (float)(35 / 667.0) * Display.main.systemHeight;
        var null_button_posy = -(float)(334 / 667.0) * Display.main.systemHeight;
      
        // 联系电话
        var tel_button_width = (float) Display.main.systemWidth;
        var tel_button_height = (float)(47 / 667.0) * Display.main.systemHeight;
        var tel_button_posy = /*-(float)(334 / 667.0) * Display.main.systemHeight*/null_button_posy-null_button_height;
        TelButton.GetComponent<RectTransform>().sizeDelta = new Vector2(tel_button_width, tel_button_height);
        TelButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0,tel_button_posy, 0.0f);
        // 邮件
        EmailButton.GetComponent<RectTransform>().sizeDelta = new Vector2(tel_button_width, tel_button_height);
        EmailButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, tel_button_posy - tel_button_height, 0.0f);
        // 网站
        NetButton.GetComponent<RectTransform>().sizeDelta = new Vector2(tel_button_width, tel_button_height);
        NetButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, tel_button_posy - tel_button_height* 2, 0.0f);
        // 站位button
        NullButton.GetComponent<RectTransform>().sizeDelta = new Vector2(tel_button_width, null_button_height);
        NullButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, tel_button_posy - tel_button_height * 3, 0.0f);
        NullButton1.GetComponent<RectTransform>().sizeDelta = new Vector2(tel_button_width, null_button_height);
        NullButton1.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, null_button_posy, 0.0f);
        // 联系电话内部的Image
        var tel_pic_posx = (float)((11 + 25) / 375.0) * Display.main.systemWidth;
        var tel_pic_w = (float)(14 / 375.0) * Display.main.systemWidth;
        var tel_pic_h = tel_pic_w;
        TelButton_Image.rectTransform.anchoredPosition3D = new Vector3(tel_pic_posx, 0, 0);
        TelButton_Image.rectTransform.sizeDelta = new Vector2(tel_pic_w, tel_pic_h);
        // 邮件内部的Image
        var email_image_w = (float)(17.2 / 375.0) * Display.main.systemWidth;
        var email_image_h = (float)(13.6 / 17.2) * email_image_w;
        EmailButton_Image.rectTransform.anchoredPosition3D = TelButton_Image.rectTransform.anchoredPosition3D;
        EmailButton_Image.rectTransform.sizeDelta = new Vector2(email_image_w, email_image_h);
        // 网站内部的Image
        var net_iamge_posx = (float)((12+25) / 375.0) * Display.main.systemWidth;
        var net_image_w = (float)(16.1 / 375.0) * Display.main.systemWidth;
        NetButton_Image.rectTransform.anchoredPosition3D = new Vector3(net_iamge_posx, 0, 0);
        NetButton_Image.rectTransform.sizeDelta = new Vector2(net_image_w, net_image_w);
        // 内部的Text、lineImage
        var tel_text_posx = (float)((35 +25) / 375.0) * Display.main.systemWidth;
        var tel_info_text_posx = -(float)(25 / 375.0) * Display.main.systemWidth;
        var tel_text_h = (float)(16 / 667.0) * Display.main.systemHeight;
        var tel_bgImage_height = (float)(Display.main.systemWidth > 375 ? (float)(1 / 667.0) * Display.main.systemHeight : 1f);
        var tel_bgImage_width = (float)(Display.main.systemWidth - (50 / 375.0) * Display.main.systemWidth);

        TelButton_NameText.rectTransform.sizeDelta = new Vector2(TelButton.GetComponent<RectTransform>().rect.width / 2.0f - tel_text_posx, tel_text_h);
        TelButton_NameText.rectTransform.anchoredPosition3D = new Vector3(tel_text_posx, 0, 0);
        TelButton_InfoText.rectTransform.sizeDelta = new Vector2(TelButton.GetComponent<RectTransform>().rect.width / 2.0f, tel_text_h);
        TelButton_InfoText.rectTransform.anchoredPosition3D = new Vector3(tel_info_text_posx, 0, 0);
        TelButton_BgImage.rectTransform.sizeDelta = new Vector2(tel_bgImage_width, tel_bgImage_height);
        EmailButton_NameText.rectTransform.sizeDelta = new Vector2(EmailButton.GetComponent<RectTransform>().rect.width / 2.0f - tel_text_posx, tel_text_h);
        EmailButton_NameText.rectTransform.anchoredPosition3D = new Vector3(tel_text_posx, 0, 0);
        EmailButton_InfoText.rectTransform.sizeDelta = new Vector2(EmailButton.GetComponent<RectTransform>().rect.width / 2.0f, tel_text_h);
        EmailButton_InfoText.rectTransform.anchoredPosition3D = new Vector3(tel_info_text_posx, 0, 0);
        EmailButton_BgImage.rectTransform.sizeDelta = new Vector2(tel_bgImage_width, tel_bgImage_height);
        NetButton_NameText.rectTransform.sizeDelta = new Vector2(NetButton.GetComponent<RectTransform>().rect.width / 2.0f - tel_text_posx, tel_text_h);
        NetButton_NameText.rectTransform.anchoredPosition3D = new Vector3(tel_text_posx, 0, 0);
        NetButton_InfoText.rectTransform.sizeDelta = new Vector2(NetButton.GetComponent<RectTransform>().rect.width / 2.0f, tel_text_h);
        NetButton_InfoText.rectTransform.anchoredPosition3D = new Vector3(tel_info_text_posx, 0, 0);
        NetButton_BgImage.rectTransform.sizeDelta = new Vector2(tel_bgImage_width, tel_bgImage_height);
        NullButton_BgImage.rectTransform.sizeDelta = new Vector2(tel_bgImage_width, tel_bgImage_height);
        // 底部公司
        var buttom_text_to_bottom = (float)(13.1 / 667.0) * Display.main.systemHeight;
        var buttom_text_h = (float)(17 / 667.0) * Display.main.systemHeight;
        ButtomText.rectTransform.sizeDelta = new Vector2(Display.main.systemWidth, buttom_text_h);
        ButtomText.rectTransform.anchoredPosition3D = new Vector3(0, buttom_text_to_bottom, 0);

        // web
        var web_text_height = (float)(25 / 667.0) * Display.main.systemHeight;
        WebPanelText.rectTransform.sizeDelta = new Vector2(Display.main.systemWidth, web_text_height);
        var web_panel_height = (float)Display.main.systemHeight - nav_h;
        var web_panel_posy = -(float)nav_h / 2.0f;
        WebPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, web_panel_posy, 0);
        WebPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(Display.main.systemWidth, web_panel_height);


    }
    void ui_enevts()
    {
        NameText.text = Application.productName;
        WebPanel.SetActive(false);
        NullButton.enabled = false;
        NullButton1.enabled = false;

        BackButton.onClick.AddListener(() => {
            Debug.Log(" 点击了返回按钮。");
            if (WebPanel.activeSelf)
            {
                WebPanelText.text = "加载中...";
                WebPanelText.gameObject.SetActive(true);
                WebPanel.SetActive(false);
                Destroy(WebPanel.GetComponent<UniWebView>());
            }
            else if (!canHide && !canShow)
                canHide = true;
        });
        NetButton.onClick.AddListener(() => {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                officalWebsite();
            }
            else if(Application.platform == RuntimePlatform.Android)
            {
                getJavaClass().CallStatic("turnNet", getCurrentActivity(), "http://www.baitongshiji.com");

            }
        });
        TelButton.onClick.AddListener(() => {
            Debug.Log("点击了电话");
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                callPhone();
            }
            else if(Application.platform == RuntimePlatform.Android)
            {
                getJavaClass().CallStatic("callPhone", getCurrentActivity(), "4000585123");
            }
        });
        EmailButton.onClick.AddListener(() => {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                useEmail();
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                getJavaClass().CallStatic("sendEmail", getCurrentActivity(), "baitongshiji@163.com");
            }
        });
    }
    AndroidJavaClass javaClass = null;
    AndroidJavaObject getJavaClass()
    {
        if (javaClass == null)
            javaClass = new AndroidJavaClass("com.bird.aband.testlibrary.TestUtil");
        return javaClass;
    }
    AndroidJavaObject currentAvtivity = null;
    AndroidJavaObject getCurrentActivity()
    {
        if(currentAvtivity == null)
        {
            AndroidJavaClass jclass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentAvtivity = jclass.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return currentAvtivity;
    }
}
