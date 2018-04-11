using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System.Runtime.InteropServices;

public class RecognizeHandleViewController : MonoBehaviour {

    public static RecognizeHandleViewController Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Sprite ConfirmSprite,
        WaittingSprite,
        HomeIconSprite,
        TitleSprite,
        RefreshSprite;

    // 各个子控件
    public UnityEngine.UI.Image TopImage, ButtomImage, ScanningImage, StatusImage, DrugsBgImage,BgImage;
    public Button Top_left_button, Top_right_button, DuibiButton, LearnButton;
    public GameObject DrugsPanel;
    public Text DrugNameText;
    public Sprite[] Duibi_learn_button_Sprites;

    public bool canStatusImageAnimation;

    public string FounedString;
    public UnityEngine.UI.Text LogText;

    public Sprite iosAddImageSprite;

    private void OnEnable()
    {
        Debug.Log(" RecognizeHandleViewController OnEnable()");
       
    }
    private void OnDisable()
    {
        Debug.Log(" RecognizeHandleViewController OnDisable()");
    }

    public void Begin()
    {
        DrugNameText.gameObject.SetActive(false);
        DrugsPanel.SetActive(false);
        canStatusImageAnimation = true;
        StartCoroutine(loadDataWithTargetImageName(FounedString));
    }

    void Start () {
        static_ui_layout();
        ui_events();
    }
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var all = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (var o in all)
                if (o.transform.parent == null)
                {
                    if (o.name == "drugInfoCanvas")
                    {
                        if (!o.activeSelf)
                            _re_scan();
                        break;
                    }
                }
        }

        statusImageAnimation();

        if(CanButtomImageAnimation)
        {
            if(!DuibiButton.gameObject.activeSelf)
            {
                DuibiButton.gameObject.SetActive(true);
                LearnButton.gameObject.SetActive(true);
            }
            ButtomImage.rectTransform.anchoredPosition3D = Vector3.Lerp(ButtomImage.rectTransform.anchoredPosition3D, new Vector3(ButtomImage.rectTransform.anchoredPosition3D.x, -ButtomImage.rectTransform.rect.height, 0), 4 * Time.deltaTime);

            if (ButtomImage.rectTransform.anchoredPosition3D.y < -ButtomImage.rectTransform.rect.height + 1)
            {
                CanButtomImageAnimation = false;
                StartCoroutine(buttomImageAnimationEnd());
            }
        }
	}
    IEnumerator buttomImageAnimationEnd()
    {
        yield return new WaitForEndOfFrame();
        ButtomImage.gameObject.SetActive(false);
        var bh = (float)(97 / 667.0 * Display.main.systemHeight);
        ButtomImage.rectTransform.anchoredPosition3D = new Vector3(0, bh, 0);
    }
    // 静态控件适应屏幕
    [DllImport("__Internal")]
    static extern void setRVCBgImageAndButtomImageSize();
    void static_ui_layout()
    {
        Vector2 topSize, scanningSize;
        GetComponent<RectTransform>().sizeDelta = new Vector2(Display.main.systemWidth, Display.main.systemHeight);

        // 背景图
        BgImage.rectTransform.sizeDelta = new Vector2(Display.main.systemWidth, Display.main.systemHeight);
        // 顶部提示图片
        var w = (375 - 77 * 2) / 375.0 * Display.main.systemWidth;
        var h = TopImage.sprite.textureRect.height / TopImage.sprite.textureRect.width * w;
        topSize = new Vector2((float)w, (float)h);
        var topimageTotop = 85 / 667.0 * Display.main.systemHeight;
        TopImage.rectTransform.sizeDelta = topSize;
        TopImage.rectTransform.anchoredPosition3D = new Vector3(0, -(float)topimageTotop, 0);

        // 中间扫描框
        var sw = (375 - 61 * 2) / 375.0 * Display.main.systemWidth;
        scanningSize = new Vector2((float)sw, (float)sw);
        ScanningImage.rectTransform.sizeDelta = scanningSize;

        // 底部图片
        var bh = (float)(97 / 667.0 * Display.main.systemHeight);
        ButtomImage.rectTransform.sizeDelta = new Vector2(Display.main.systemWidth, (float)bh);
        ButtomImage.rectTransform.anchoredPosition3D = new Vector3(0, bh, 0);

        // 顶部两图标
        var top_left_w = (93 / 1080.0) * Display.main.renderingWidth;
        var top_left_h = top_left_w;
        var posy = ((48 + top_left_w / 2) / 1334.0) * Display.main.renderingHeight;
        var posx = ((50 + top_left_w / 2 )/ 750.0) * Display.main.renderingWidth;
        Top_left_button.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((float)top_left_w, (float)top_left_h);
        Top_right_button.gameObject.GetComponent<RectTransform>().sizeDelta = Top_left_button.gameObject.GetComponent<RectTransform>().sizeDelta;
        Top_left_button.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3((float)posx, -(float)posy, 0);
        Top_right_button.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-(float)posx, -(float)posy, 0);

        // 状态image
        var sizew = (750 - 164 * 4) / 750.0 * Display.main.systemWidth;
        StatusImage.rectTransform.sizeDelta = new Vector2((float)sizew, (92 / 100.0f) * (float)sizew);
        var toButtom = (48 + sizew/ 2) / 1334.0 * Display.main.systemHeight;
        StatusImage.rectTransform.anchoredPosition3D = new Vector3(0, (float)toButtom, 0);

        // 具体药品名称text
        var drug_name_text_to_top = (float)(158 / 667.0) * Display.main.systemHeight;
        var drug_name_text_h = (float)(41 / 1334.0) * Display.main.systemHeight;
        DrugNameText.rectTransform.sizeDelta = new Vector2(Display.main.systemWidth, drug_name_text_h);
        DrugNameText.rectTransform.anchoredPosition3D = new Vector3(0, -drug_name_text_to_top, 0);

        TopImage.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2((float)w, drug_name_text_h > TopImage.rectTransform.rect.height ? TopImage.rectTransform.rect.height : drug_name_text_h);

        // 对比模式button、学习模式button
        var duibi_button_width = (float)(sw / 2.0f);
        var duibi_button_posy = (float)((Display.main.systemHeight / 2.0f - topimageTotop - TopImage.rectTransform.rect.height - sw/2.0f) / 2.2f + (sw * 1.2f) / 2.0f);
        var duibi_button_posx = (float)(sw * 1.2f / 4.0);
        DuibiButton.GetComponent<RectTransform>().sizeDelta = new Vector2(duibi_button_width, TopImage.rectTransform.rect.height * 1.2f);
        DuibiButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-duibi_button_posx, -duibi_button_posy, 0);
        DuibiButton.transform.GetComponentInChildren<Text>().rectTransform.sizeDelta = new Vector2(duibi_button_width, DuibiButton.GetComponent<RectTransform>().rect.height * 0.5f);
        LearnButton.GetComponent<RectTransform>().sizeDelta = DuibiButton.GetComponent<RectTransform>().sizeDelta;
        LearnButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(duibi_button_posx, -duibi_button_posy, 0);
        LearnButton.transform.GetComponentInChildren<Text>().rectTransform.sizeDelta = DuibiButton.transform.GetComponentInChildren<Text>().rectTransform.sizeDelta;

        // 药物背景透明图
        //DrugsBgImage.rectTransform.sizeDelta = new Vector2(ScanningImage.rectTransform.rect.width * 1.2f, ScanningImage.rectTransform.rect.width * 1.2f);

        DrugsBgImage.gameObject.SetActive(false);
        DuibiButton.gameObject.SetActive(false);
        LearnButton.gameObject.SetActive(false);
        //LogText.text = "视图size:" + GetComponent<RectTransform>().sizeDelta;
        //LogText.text += "屏幕size:" + Display.main.systemWidth + ',' + Display.main.systemHeight;

        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            setRVCBgImageAndButtomImageSize();
        }
    }
    public void SetRVCBgImageAddSize(string addsize)
    {
        var strs = addsize.Split('|');
        int width = 0;
        int height = 0;
        int.TryParse(strs[0],out width);
        int.TryParse(strs[1], out height);
        BgImage.rectTransform.sizeDelta = new Vector2(BgImage.rectTransform.rect.width + width,BgImage.rectTransform.rect.height+ height);
    }
    public void RVCButtomImageAddWidth(string addWidth)
    {
        int addwidth = 0;
        int.TryParse(addWidth, out addwidth);
        ButtomImage.rectTransform.sizeDelta = new Vector2(ButtomImage.rectTransform.rect.width + addwidth, ButtomImage.rectTransform.rect.height + addwidth);
    }
    float buttomposy = 0;
    public void SetRVCButtomImagePosy(string posy)
    {
        int py = 20;
        if(int.TryParse(posy, out py))
        ButtomImage.rectTransform.anchoredPosition3D = new Vector3(0, -py, 0);
        else
        {
            Debug.Log("==>字符串：" + posy + "转换失败！");
        }
        buttomposy = -py;
    }
    void ui_events()
    {
        // 对比模式按钮
        DuibiButton.onClick.AddListener(() =>
        {
            if (ScanningImage.transform.Find("foundedImage").gameObject.activeSelf)
            {
                float width = -1;
                GameObject item = null;
                for (int i = 0; i < DrugsPanel.transform.childCount; ++i)
                {
                    if (DrugsPanel.transform.GetChild(i).GetComponent<RectTransform>().rect.width > width)
                    {
                        width = DrugsPanel.transform.GetChild(i).GetComponent<RectTransform>().rect.width;
                        item = DrugsPanel.transform.GetChild(i).gameObject;
                    }
                }
                if (DrugsBgImage.rectTransform.rect.width != item.GetComponent<RectTransform>().rect.width)
                    DrugsBgImage.rectTransform.sizeDelta = item.GetComponent<RectTransform>().sizeDelta;

                DuibiButton.gameObject.transform.GetChild(0).GetComponent<Text>().color = new Color(0.0627f,0.557f,0.910f);
                DrugsBgImage.gameObject.SetActive(true);
                ScanningImage.transform.Find("foundedImage").gameObject.SetActive(false);
                DuibiButton.image.sprite = Duibi_learn_button_Sprites[0];
            }
            else
            {
                DuibiButton.gameObject.transform.GetChild(0).GetComponent<Text>().color = new Color(1, 1, 1);
                DrugsBgImage.gameObject.SetActive(false);
                ScanningImage.transform.Find("foundedImage").gameObject.SetActive(true);
                DuibiButton.image.sprite = Duibi_learn_button_Sprites[1];
            }
        });
        // 学习模式按钮
        LearnButton.onClick.AddListener(() => {

            float width = -1;
            GameObject item = null;
            for(int i = 0; i < DrugsPanel.transform.childCount; ++i)
            {
                if (DrugsPanel.transform.GetChild(i).GetComponent<RectTransform>().rect.width > width)
                {
                    width = DrugsPanel.transform.GetChild(i).GetComponent<RectTransform>().rect.width;
                    item = DrugsPanel.transform.GetChild(i).gameObject;
                }
            }
            Debug.Log("选择了：" + item.GetComponent<Button>().image.sprite.name);
            GameObject drugInfo = null;
            var all = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (GameObject obj in all)
            {
                if (obj.transform.parent == null)
                {
                    if (obj.name == "drugInfoCanvas")
                    {
                        drugInfo = obj;
                        break;
                    }
                }
            }
            drugInfo.GetComponent<DrugInfoViewController>().isMainStructureButtonClick = false;
            drugInfo.GetComponent<DrugInfoViewController>().IsSearch = false;
            drugInfo.GetComponent<DrugInfoViewController>().ShowDrugSprite = item.GetComponent<Button>().image.sprite;
            drugInfo.GetComponent<DrugInfoViewController>().ClassesObj = DrugsPanel.GetComponent<DrugsViewController>().ClassesObj;
            drugInfo.gameObject.SetActive(true);
            // transform.parent.gameObject.SetActive(false);
            DrugsPanel.GetComponent<DrugsViewController>().z();
            DrugsPanel.GetComponent<DrugsViewController>().canScals = false;
        });
    }
    // 状态图片的动画控制
    void statusImageAnimation()
    {
        if(canStatusImageAnimation)
        {
            if (StatusImage.sprite == WaittingSprite)
                StatusImage.sprite = WaittingSprite;
            StatusImage.transform.Rotate(new Vector3(0, 0, -200 * Time.deltaTime));
        }
    }
    public bool CanButtomImageAnimation = false;
    // 在数据库中找目标图片的相关信息
    IEnumerator loadDataWithTargetImageName(string _string)
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log(" 在数据库中加载相关数据："+_string);
        DataHelper.Instance.Query(_string, (classes) => {
            Debug.Log(" 数据库中" + _string + "相关数据加载完成："+ classes == null ? "null" : classes.ToString());
            
            StartCoroutine(loadedData(classes));
        });
    }
    // 数据库数据加载完成时触发的事件
    IEnumerator loadedData(Classes classes)
    {
        yield return new WaitForSeconds(0.5f);

        DrugsPanel.GetComponent<DrugsViewController>().ClassesObj = classes;
        TopImage.transform.GetChild(0).GetComponent<Text>().text = classes.Name;
        selectDrugs();
        DrugsPanel.SetActive(true);
    }
    // 开始选择合适的药品
    public int addwidth = 0;
    void selectDrugs()
    {
        Top_left_button.image.sprite = HomeIconSprite;
        Top_right_button.image.sprite = RefreshSprite;

        // 提示图标
        TopImage.sprite = TitleSprite;
        var w = (375 - 84 * 2) / 375.0 * Display.main.systemWidth;
        var h = TopImage.sprite.textureRect.height / TopImage.sprite.textureRect.width * w;
        var topSize = new Vector2((float)w, (float)h);
        var topimageTotop = 96 / 667.0 * Display.main.systemHeight;
        TopImage.rectTransform.sizeDelta = topSize;
        TopImage.rectTransform.anchoredPosition3D = new Vector3(0, -(float)topimageTotop, 0);
        var topImage_text_h = (float)(41 / 1334.0) * Display.main.systemHeight;
        TopImage.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2((float)w-20, topImage_text_h);

        // 中间扫描框
        var sw = (375 - 37 * 2) / 375.0 * Display.main.systemWidth;
        var scanningSize = new Vector2((float)sw, (float)sw);
        ScanningImage.rectTransform.sizeDelta = scanningSize;

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var addw = (float)(sw + 2 * addwidth);
            var addh = (float)(1334 / 750.0) * addw;
            ScanningImage.transform.Find("rawPanel").GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(addw, addh);
        }
        //ScanningImage.transform.Find("rawPanel").GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(ScanningImage.rectTransform.rect.width, (float)(1334 / 750.0) * ScanningImage.rectTransform.rect.width);

        // 重新识别按钮事件
        Top_right_button.onClick.RemoveAllListeners();
        Top_right_button.onClick.AddListener(_re_scan);
        // 主页按钮事件
        Top_left_button.onClick.RemoveAllListeners();
        Top_left_button.onClick.AddListener(() => {
            Debug.Log(" RecognizeHandleViewController 主页按钮点击了！");

            for (int i = 0; i < GameObject.Find("ARCamera").transform.childCount; ++i)
                GameObject.Find("ARCamera").transform.GetChild(i).gameObject.SetActive(false);
            StatusImage.sprite = WaittingSprite;
            Resources.UnloadUnusedAssets();
            gameObject.SetActive(false);
            TopImage.transform.GetChild(0).GetComponent<Text>().text = "";
            Resources.UnloadUnusedAssets();
            StopAllCoroutines();
            if (DrugsViewController.Instance != null)
                DrugsViewController.Instance.StopAllCoroutines();
            tabfun.helper.Coroutine.Instance.StopAllCoroutines();
            if (HomeViewController.Instance.isLightClick)
                CameraDevice.Instance.SetFlashTorchMode(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene/home");

        });
    }
    void _re_scan()
    {
        Debug.Log(" RecognizeHandleViewController 重新识别");
        transform.parent.Find("homeCanvas").gameObject.SetActive(true);
        DrugsBgImage.gameObject.SetActive(false);
        ScanningImage.transform.Find("foundedImage").gameObject.SetActive(true);
        DuibiButton.gameObject.SetActive(false);
        LearnButton.gameObject.SetActive(false);

        if (DrugsViewController.Instance != null)
        {
            DrugsViewController.Instance.Clear();
        }
        StatusImage.sprite = WaittingSprite;
        TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
        CameraDevice.Instance.Start();
        HomeViewController.Instance.StopAllCoroutines();
        HomeViewController.Instance.CanBack = true;
        HomeViewController.Instance.StartCoroutine(HomeViewController.Instance.TimeOut());
        HomeViewController.Instance.scanningImage_scanSticksImage_canAnimation = true;
        HomeViewController.Instance.ScanningImage_ScanSticksImage.gameObject.SetActive(true);
       ButtomImage.gameObject.SetActive(true);
        HomeViewController.Instance.HomeCamera.gameObject.SetActive(true);
        gameObject.SetActive(false);
        TopImage.transform.GetChild(0).GetComponent<Text>().text = "";
        if (DrugsViewController.Instance != null)
            DrugsViewController.Instance.StopAllCoroutines();
        if (HomeViewController.Instance.isLightClick)
            CameraDevice.Instance.SetFlashTorchMode(true);
        Resources.UnloadUnusedAssets();
        AssetBundle.UnloadAllAssetBundles(true);
        StopAllCoroutines();
    }
}
