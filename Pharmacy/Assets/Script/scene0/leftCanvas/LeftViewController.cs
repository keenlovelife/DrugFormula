using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftViewController : MonoBehaviour {

    public Button BgButton, Panel_UserButton, Panel_AboutButton, Panel_ExitButton;
    public GameObject BgPanel, TopPanel, Panel;
    public Image TopPanel_LogoImage;
    public Text TopPanel_Text;

    public bool isRecognizeHandleVC;

    bool canShow = false, canHide = false;
    private void OnEnable()
    {
        canShow = true;
        canHide = false;
    }

    void Start () {
        ui_layout();
        BgButton.onClick.AddListener(() => {
            Debug.Log("点击了背景按钮");
            if (!canHide && !canShow)
            {
                canHide = true;
            }
        });
        Panel_ExitButton.onClick.AddListener(() => {
            if (!isOnClick)
                isOnClick = true;
            Application.Quit();
        });
        Panel_AboutButton.onClick.AddListener(() => {
            Debug.Log("点击了关于我们按钮");
            if (!isOnClick)
                isOnClick = true;
            transform.Find("aboutPanel").gameObject.SetActive(true);
        });
        Panel_UserButton.onClick.AddListener(() => {
            Debug.Log("点击了用户指南按钮");
            if (!isOnClick)
                isOnClick = true;
        });
	}
    bool isOnClick = false;
	void Update () {
        if (canShow)
        {
            if (!BgButton.gameObject.activeSelf)
                BgButton.gameObject.SetActive(true);
            BgPanel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(BgPanel.GetComponent<RectTransform>().anchoredPosition3D, new Vector3(0, 0, 0), 10 * Time.deltaTime);
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
            BgPanel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(BgPanel.GetComponent<RectTransform>().anchoredPosition3D, new Vector3(-BgPanel.GetComponent<RectTransform>().rect.width, 0, 0), 10 * Time.deltaTime);
            if (BgPanel.GetComponent<RectTransform>().anchoredPosition3D.x < -BgPanel.GetComponent<RectTransform>().rect.width+1)
            {
                canHide = false;
                if (!isRecognizeHandleVC)
                    HomeViewController.Instance.HomeViewHided();
                isRecognizeHandleVC = false;
                gameObject.SetActive(false);
            }
        }

        // 左划隐藏侧边栏
        if(!canShow && !canHide && !transform.Find("aboutPanel").gameObject.activeSelf && !GameObject.Find("logCanvas").transform.Find("guidePanel").gameObject.activeSelf)
        {
            if(Input.touchCount == 1)
            {


                if(Input.touches[0].phase == TouchPhase.Began)
                {
                    startPos = Input.touches[0].position;
                    
                }else if(Input.touches[0].phase == TouchPhase.Moved)
                {
                    movePos = Input.touches[0].position;
                    deltaPos = Input.touches[0].deltaPosition;

                }else if(Input.touches[0].phase == TouchPhase.Ended)
                {
                    endPos = Input.touches[0].position;
                    if(endPos.x - startPos.x < 0)
                    {
                        // 左划
                        if (!isOnClick)
                            canHide = true;
                        else
                            isOnClick = false;
                    }
                    startPos = Vector2.zero;
                    endPos = Vector2.zero;
                    deltaPos = Vector2.zero;
                }
            }
        }
		
	}
    Vector2 startPos, endPos, movePos, deltaPos;


    void ui_layout()
    {
        var bgw = (float)(317 / 375.0) * Display.main.systemWidth;
        var topPanel_hieght = (float)(176 / 667.0) * Display.main.systemHeight;
        var panel_button_height = (float)(60 / 667.0) * Display.main.systemHeight;
        var panel_image_to_left = (float)(24 / 375.0) * Display.main.systemWidth;
        var panel_image_w = (float)(58 / 750.0) * Display.main.systemWidth;
        var topPanel_logo_w = (float)((317 - 131 * 2) / 375.0) * Display.main.systemWidth;
        var topPanel_logo_to_top = (float)(35 / 667.0) * Display.main.systemHeight;
        var topPanel_name_h = (float)(41 / 1334.0) * Display.main.systemHeight;
        var panel_button_text_h = (float)(30 / 1334.0) * Display.main.systemHeight;
        var pane_exitButton_to_buttom = (float)(8 / 667.0) * Display.main.systemHeight;

        BgPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(bgw, Display.main.systemHeight);
        TopPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(bgw, topPanel_hieght);
        TopPanel_LogoImage.rectTransform.sizeDelta = new Vector2(topPanel_logo_w, topPanel_logo_w * 386 / 302.0f);
        TopPanel_Text.rectTransform.sizeDelta = new Vector2(bgw, topPanel_name_h);
        TopPanel_LogoImage.rectTransform.anchoredPosition3D = new Vector3(0, -topPanel_logo_to_top, 0);
        TopPanel_Text.rectTransform.anchoredPosition3D = new Vector3(0, (topPanel_hieght - topPanel_logo_to_top - TopPanel_LogoImage.rectTransform.rect.height - TopPanel_Text.rectTransform.rect.height) / 2.0f, 0);
        Panel.GetComponent<RectTransform>().sizeDelta = new Vector2(bgw, Display.main.systemHeight - topPanel_hieght);
        Panel_UserButton.GetComponent<RectTransform>().sizeDelta = new Vector2(bgw, panel_button_height);
        Panel_AboutButton.GetComponent<RectTransform>().sizeDelta = new Vector2(bgw, panel_button_height);
        Panel_AboutButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(
             Panel_AboutButton.GetComponent<RectTransform>().anchoredPosition3D.x, -panel_button_height, 0);
        Panel_ExitButton.transform.GetChild(0).GetComponent<Text>().rectTransform.sizeDelta = new Vector2(bgw, panel_button_text_h);
        Panel_ExitButton.GetComponent<RectTransform>().sizeDelta = new Vector2(bgw, panel_button_text_h + pane_exitButton_to_buttom * 2);
        for (int i = 0; i < 2; ++i)
        {
            var panel_button = Panel.transform.GetChild(i);
            panel_button.GetChild(1).GetComponent<Image>().rectTransform.anchoredPosition3D = new Vector3(panel_image_to_left, 0, 0);
            panel_button.GetChild(1).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(panel_image_w, panel_image_w);
            panel_button.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(panel_image_to_left + panel_image_w * 2, 0, 0);
            panel_button.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(bgw - panel_image_to_left * 2 - panel_image_w, panel_button_text_h);
            
        }
    }
}
