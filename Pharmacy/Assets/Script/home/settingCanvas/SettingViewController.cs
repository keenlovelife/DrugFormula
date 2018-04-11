using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingViewController : MonoBehaviour {
    public Button BgButton;
    public GameObject BgPanel,TopPanel, Panel, UserButton, AboutButton, ExitButton,
        AboutPanel, GuidePanel;
    public UnityEngine.UI.Image logoImage, UserButton_Image, UserButton_BgImage, AboutButton_Iamge,AboutButton_BgImage;
    public Text AppNameText,UserButton_Text, AboutButton_Text, ExitButton_Text;
    public bool isFirstOpen = false;
    bool canShow = false, canHide = false;
    private void OnEnable()
    {
        canShow = true;
        canHide = false;
    }
    private void Start()
    {
        _ui();
        _ui_events();
    }
    Vector2 startPos, endPos, movePos, deltaPos;
    void Update()
    {
        // 按了返回按钮
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Helper.Updatecanvas.activeSelf && !AboutPanel.activeSelf &&!GuidePanel.activeSelf)
            {
                if (!canHide && !canShow)
                    canHide = true;
            }
        }
        if (canShow)
        {
            if (!BgButton.gameObject.activeSelf)
                BgButton.gameObject.SetActive(true);
            BgPanel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(BgPanel.GetComponent<RectTransform>().anchoredPosition3D, new Vector3(0, 0, 0), 20 * Time.deltaTime);
            if (BgPanel.GetComponent<RectTransform>().anchoredPosition3D.x > -1)
            {
                BgPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                canShow = false;
            }
        }
        if (canHide)
        {
            if (BgButton.gameObject.activeSelf)
                BgButton.gameObject.SetActive(false);
            BgPanel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(BgPanel.GetComponent<RectTransform>().anchoredPosition3D, new Vector3(-BgPanel.GetComponent<RectTransform>().rect.width, 0, 0), 20 * Time.deltaTime);
            if (BgPanel.GetComponent<RectTransform>().anchoredPosition3D.x < -BgPanel.GetComponent<RectTransform>().rect.width + 1)
            {
                canHide = false;
                gameObject.SetActive(false);
            }
        }
        // 左划隐藏侧边栏
        //if (!canShow && !canHide && !transform.Find("aboutPanel").gameObject.activeSelf && !GameObject.Find("logCanvas").transform.Find("guidePanel").gameObject.activeSelf)
        //{
        //    if (Input.touchCount == 1)
        //    {
        //        if (Input.touches[0].phase == TouchPhase.Began)
        //        {
        //            startPos = Input.touches[0].position;
        //        }
        //        else if (Input.touches[0].phase == TouchPhase.Moved)
        //        {
        //            movePos = Input.touches[0].position;
        //            deltaPos = Input.touches[0].deltaPosition;
        //        }
        //        else if (Input.touches[0].phase == TouchPhase.Ended)
        //        {
        //            endPos = Input.touches[0].position;
        //            if (endPos.x - startPos.x < 0)
        //            {
        //                // 左划
        //                //if (!isOnClick)
        //                //    canHide = true;
        //                //else
        //                //    isOnClick = false;
        //            }
        //            startPos = Vector2.zero;
        //            endPos = Vector2.zero;
        //            deltaPos = Vector2.zero;
        //        }
        //    }
        //}
    }
    void _ui()
    {
        // bgPanel
        float bg_panel_width = (float)(317 / 375.0) * Display.main.systemWidth;
        BgPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(bg_panel_width, Display.main.systemHeight);
        // topPanel
        float top_panel_height = (float)(176 / 667.0) * Display.main.systemHeight;
        TopPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(TopPanel.GetComponent<RectTransform>().sizeDelta.x, top_panel_height);
        // Panel
        float panel_height = Display.main.systemHeight - top_panel_height;
        Panel.GetComponent<RectTransform>().sizeDelta = new Vector2(Panel.GetComponent<RectTransform>().sizeDelta.x, panel_height);
        // logoImage
        float logo_image_posy = -(float)(35 / 667.0) * Display.main.systemHeight,
            logo_image_width = (float)(57 / 375.0) * Display.main.systemWidth,
            logo_image_height = (float)(74.5 / 57) * logo_image_width;
        logoImage.rectTransform.sizeDelta = new Vector2(logo_image_width, logo_image_height);
        logoImage.rectTransform.anchoredPosition3D = new Vector3(0, logo_image_posy, 0);
        // topPanel text
        float logo_panel_text_posy = (float)(33.5 / 667.0) * Display.main.systemHeight,
            logo_panel_text_height = (float)(20 / 667.0) * Display.main.systemHeight;
        AppNameText.rectTransform.sizeDelta = new Vector2(AppNameText.rectTransform.sizeDelta.x, logo_panel_text_height);
        AppNameText.rectTransform.anchoredPosition3D = new Vector3(0, logo_panel_text_posy, 0);
        // userButton、aboutButton
        float user_about_button_height = (float)(60 / 667.0) * Display.main.systemHeight,
            about_button_posy = -user_about_button_height,
            user_button_image_posx = (float)(25 / 375.0) * Display.main.systemWidth,
            user_button_image_width = (float)(17.8 / 375.0) * Display.main.systemWidth,
            user_button_text_left = (float)(60 / 375.0) * Display.main.systemWidth,
            user_button_text_height = (float)(20/667.0)*Display.main.systemHeight,
            user_button_bgImage_left = (float)(16 / 375.0) * Display.main.systemWidth,
            user_button_bgImage_height = Display.main.systemWidth > 375 ? (float)(1 / 375.0) * Display.main.systemWidth : 1f,
            about_button_image_width = (float)(17.5 / 375.0) * Display.main.systemWidth,
            about_button_image_height = (float)(16.9/17.5)*about_button_image_width;
        // userButton
        UserButton.GetComponent<RectTransform>().sizeDelta = new Vector2(UserButton.GetComponent<RectTransform>().sizeDelta.x, user_about_button_height);
        // userButton_image
        UserButton_Image.rectTransform.anchoredPosition3D = new Vector3(user_button_image_posx, 0, 0);
        UserButton_Image.rectTransform.sizeDelta = new Vector2(user_button_image_width, user_button_image_width);
        // userButton_bgIamge
        UserButton_BgImage.rectTransform.sizeDelta = new Vector2(UserButton_BgImage.rectTransform.sizeDelta.x, user_button_bgImage_height);
        UserButton_BgImage.rectTransform.offsetMin = new Vector2(user_button_bgImage_left, UserButton_BgImage.rectTransform.offsetMin.y);
        UserButton_BgImage.rectTransform.offsetMax = new Vector2(user_button_bgImage_left, UserButton_BgImage.rectTransform.offsetMax.y);
        // userButton_text
        UserButton_Text.rectTransform.sizeDelta = new Vector2(UserButton_Text.rectTransform.sizeDelta.x, user_button_text_height);
        UserButton_Text.rectTransform.offsetMin = new Vector2(user_button_text_left, UserButton_Text.rectTransform.offsetMin.y);
        // aboutButton
        AboutButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, about_button_posy, 0);
        AboutButton.GetComponent<RectTransform>().sizeDelta = new Vector2(AboutButton.GetComponent<RectTransform>().sizeDelta.x, user_about_button_height);
        // aboutButton_Image
        AboutButton_Iamge.rectTransform.sizeDelta = new Vector2(about_button_image_width, about_button_image_height);
        AboutButton_Iamge.rectTransform.anchoredPosition3D = new Vector3(user_button_image_posx, 0, 0);
        // aboutButton_bgImage
        AboutButton_BgImage.rectTransform.sizeDelta = new Vector2(AboutButton_BgImage.rectTransform.sizeDelta.x, user_button_bgImage_height);
        AboutButton_BgImage.rectTransform.offsetMin = new Vector2(user_button_bgImage_left, AboutButton_BgImage.rectTransform.offsetMin.y);
        AboutButton_BgImage.rectTransform.offsetMax = new Vector2(user_button_bgImage_left, AboutButton_BgImage.rectTransform.offsetMax.y);
        // aboutButton_text
        AboutButton_Text.rectTransform.sizeDelta = new Vector2(AboutButton_Text.rectTransform.sizeDelta.x, user_button_text_height);
        AboutButton_Text.rectTransform.offsetMin = new Vector2(user_button_text_left, AboutButton_Text.rectTransform.offsetMin.y);
        // exitButton
        float exit_button_height = (float)(50 / 667.0) * Display.main.systemHeight,
            exit_button_text_height = (float)(27 / 667.0) * Display.main.systemHeight;
        ExitButton.GetComponent<RectTransform>().sizeDelta = new Vector2(ExitButton.GetComponent<RectTransform>().sizeDelta.x, exit_button_height);
        ExitButton_Text.rectTransform.sizeDelta = new Vector2(ExitButton_Text.rectTransform.sizeDelta.x, exit_button_text_height);
        // 
        AboutPanel.SetActive(false);
        GuidePanel.SetActive(false);
    }
    void _ui_events()
    {
        AppNameText.text = Application.productName;
        BgButton.onClick.AddListener(() => {
            if (!canHide && !canShow)
                canHide = true;
        });
        ExitButton.GetComponent<Button>().onClick.AddListener(() => {
            Application.Quit();
        });
        UserButton.GetComponent<Button>().onClick.AddListener(() => {
            Debug.Log(" 点击了用户指南按钮！");
            GuidePanel.SetActive(true);
        });
        AboutButton.GetComponent<Button>().onClick.AddListener(() => {
            Debug.Log(" 点击了关于我们按钮！");
            AboutPanel.SetActive(true);
        });
    }
}
