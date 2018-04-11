using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideViewController : MonoBehaviour {

    public UnityEngine.UI.Button NextButton,JumpButton;
    public UnityEngine.UI.Image Image1, Image2, Image3;
    private void OnEnable()
    {
        canShow = true;
        canHide = false;
    }
    int num = 1;
    void Start()
    {
        ui_layout();
        JumpButton.onClick.AddListener(() => {
            Debug.Log(" 点击了返回按钮。");
            if (transform.parent.gameObject.GetComponent<SettingViewController>().isFirstOpen)
            {
                transform.parent.gameObject.GetComponent<SettingViewController>().isFirstOpen = false;
                transform.parent.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
            if (!canHide && !canShow)
            {
                canHide = true;
               
            }
        });
        NextButton.onClick.AddListener(() => {
            switch(num)
            {
                case 1:
                    {
                        ++num;
                        startImage2Animation = true;
                        var next_to_buttom = (float)(102 / 667.0) * Image1.rectTransform.rect.height;
                        NextButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, next_to_buttom, 0);
                        NextButton.gameObject.SetActive(false);
                        JumpButton.gameObject.SetActive(false);
                    }
                    break;
                case 2:
                    {
                        ++num;
                        startImage2Animation = true;
                        var next_to_buttom = (float)(109 / 667.0) * Image1.rectTransform.rect.height;
                        NextButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, next_to_buttom, 0);
                        NextButton.gameObject.SetActive(false);
                        JumpButton.gameObject.SetActive(false);

                    }
                    break;
                case 3:
                    {
                        num = 1;
                        var next_to_buttom = (float)(126 / 667.0) * Image1.rectTransform.rect.height;
                        NextButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, next_to_buttom, 0);
                        Image2.rectTransform.anchoredPosition3D = new Vector3(Display.main.systemWidth, 0, 0);
                        Image3.rectTransform.anchoredPosition3D = new Vector3(Display.main.systemWidth, 0, 0);
                        Debug.Log(" 点击了完成按钮。");
                        firstOpenApp();
                        if (transform.parent.gameObject.GetComponent<SettingViewController>().isFirstOpen)
                        {
                            transform.parent.gameObject.GetComponent<SettingViewController>().isFirstOpen = false;
                            transform.parent.gameObject.SetActive(false);
                            gameObject.SetActive(false);
                        }
                        if (!canHide && !canShow)
                            canHide = true;
                    }
                    break;
            }
        });
    }
    bool canShow = false, canHide = false;
    bool startImage2Animation = false;
    void Update()
    { // 按了返回按钮
        if (!Helper.Updatecanvas.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!canHide && !canShow)
                canHide = true;
        }

        if (canHide)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(gameObject.GetComponent<RectTransform>().anchoredPosition3D, new Vector3(Display.main.systemWidth, 0, 0),
                20 * Time.deltaTime);
            if (gameObject.GetComponent<RectTransform>().anchoredPosition3D.x > Display.main.systemWidth - 1)
            {
                canHide = false;
                gameObject.SetActive(false);
                num = 1;
                var next_to_buttom = (float)(126 / 667.0) * Image1.rectTransform.rect.height;
                NextButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, next_to_buttom, 0);
                Image2.rectTransform.anchoredPosition3D = new Vector3(Display.main.systemWidth, 0, 0);
                Image3.rectTransform.anchoredPosition3D = new Vector3(Display.main.systemWidth, 0, 0);
            }
        }
        if (canShow)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(gameObject.GetComponent<RectTransform>().anchoredPosition3D, new Vector3(0, 0, 0), 
                20 * Time.deltaTime);
            if (gameObject.GetComponent<RectTransform>().anchoredPosition3D.x < 1)
            {
                canShow = false;
                gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            }
        }
        if (startImage2Animation)
        {
            switch (num)
            {
                case 2:
                    {
                        Image2.rectTransform.anchoredPosition3D = Vector3.Lerp(Image2.rectTransform.anchoredPosition3D, Image1.rectTransform.anchoredPosition3D, 10 * Time.deltaTime);
                        if (Image2.rectTransform.anchoredPosition3D.x < Image1.rectTransform.anchoredPosition3D.x + 1)
                        {
                            startImage2Animation = false;
                           
                            NextButton.gameObject.SetActive(true);
                            JumpButton.gameObject.SetActive(true);

                        }
                    }
                    break;
                case 3:
                    {
                        Image3.rectTransform.anchoredPosition3D = Vector3.Lerp(Image3.rectTransform.anchoredPosition3D, Image1.rectTransform.anchoredPosition3D, 10 * Time.deltaTime);
                        if (Image3.rectTransform.anchoredPosition3D.x < Image1.rectTransform.anchoredPosition3D.x + 1)
                        {
                            startImage2Animation = false;
                           
                            // NextButton.image.sprite = DoenSprite;
                            NextButton.gameObject.SetActive(true);
                            JumpButton.gameObject.SetActive(true);
                        }
                    }
                    break;
            }
        }
    }
    void ui_layout()
    {
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Display.main.systemWidth, Display.main.systemHeight);

        float image_h = Display.main.systemHeight;
        var image_w = (float)(750 / 1334.0) * image_h;
        if(image_w > Display.main.systemWidth)
        {
            image_w = Display.main.systemWidth;
            image_h = (float)(1334 / 750.0) * image_w;
        }
        var image_to_left = (float)(Display.main.systemWidth - image_w) / 2.0f;
        Image1.rectTransform.sizeDelta = new Vector2(image_w, image_h);
        Image2.rectTransform.sizeDelta = new Vector2(image_w, image_h);
        Image3.rectTransform.sizeDelta = new Vector2(image_w, image_h);
        Image1.rectTransform.anchoredPosition3D = new Vector3(image_to_left, 0, 0);
        Image2.rectTransform.anchoredPosition3D = new Vector3(Display.main.systemWidth, 0, 0);
        Image3.rectTransform.anchoredPosition3D = new Vector3(Display.main.systemWidth, 0, 0);

        var to_buttom = (float)(126 / 667.0) * Image1.rectTransform.rect.height + (float)(Display.main.systemHeight - image_h) / 2.0f;
        var next_w = Image1.rectTransform.rect.width - (float)(122 / 375.0) * Image1.rectTransform.rect.width * 2;
        var next_h = (float)(90 / 262.0) * next_w;
        NextButton.GetComponent<RectTransform>().sizeDelta = new Vector2(next_w, next_h);
        NextButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, to_buttom, 0);

        var jump_w = (float)(124 / 750.0) * Image1.rectTransform.rect.width;
        var jump_h = (float)(60 / 124.0) * jump_w;
        var jump_to_right = (float)(25 / 375.0) * Image1.rectTransform.rect.width + image_to_left;
        var jump_to_top = (float)(25 / 375.0) * Image1.rectTransform.rect.width + (float)(Display.main.systemHeight - image_h) / 2.0f;
        JumpButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-jump_to_right, -jump_to_top, 0);
        JumpButton.GetComponent<RectTransform>().sizeDelta = new Vector2(jump_w, jump_h);
    }
    void firstOpenApp()
    {
        string path = "/firstOpenApp";
        path = Application.persistentDataPath + path;
        if (!System.IO.File.Exists(path))
        {
            System.IO.File.Create(path).Close();
            Debug.Log(" 创建文件firstOpenApp: " + path);
        }
    }

}
