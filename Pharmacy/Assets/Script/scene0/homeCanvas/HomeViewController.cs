using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
public class HomeViewController : MonoBehaviour {

    public static HomeViewController Instance;
    private void Awake()
    {
        Instance = this;
    }

    public static GameObject SearchCanvas;
    public static GameObject DrugInfoCanvas;
    public static GameObject UpdateCanvas;
    // 各个子控件
    public UnityEngine.UI.Image BgImage, TopImage, ButtonImage,
        ScanningImage, ScanningImage_ScanSticksImage,
        Top_BgImage, Buttom_BgImage, Left_BgImage, Right_BgImage,
        leftAddImage,rightAddImage,topAddImage,buttomAddImage;
    public Button Top_left_button, Top_right_button, Top_right_light_button,
        TimeOutButton;
    public Sprite[] LightSprites;
    public Camera ARCamera, HomeCamera;

    public UnityEngine.UI.Image FoundedImage;
    public float ScanSticksImageAnimationSpeed;
    public bool scanningImage_scanSticksImage_canAnimation;
    public UnityEngine.UI.Text LogText;

    private void _init()
    {
        var druginfo = Instantiate<GameObject>(Resources.Load<GameObject>("drugInfoCanvas"));
        var search = Instantiate<GameObject>(Resources.Load<GameObject>("searchCanvas"));
        var update = Instantiate<GameObject>(Resources.Load<GameObject>("updateCanvas"));
        druginfo.name = "drugInfoCanvas";
        search.name = "searchCanvas";
        update.name = "updateCanvas";
        druginfo.SetActive(false);
        search.SetActive(false);
        update.SetActive(false);
        Helper.Searchcanvas = null;
        Helper.Updatecanvas = null;
        Helper.DrugInfocanvas = null;
    }

    bool isScanningImageSizeSetted = false;
    void Start() {


        _init();


        var dh = gameObject.GetComponent<DataHelper>();
        if (dh == null)
        {
            gameObject.AddComponent<DataHelper>();
        }
        static_ui_layout();

        ARCamera = GameObject.Find("ARCamera").transform.GetChild(0).GetComponent<Camera>();
        HomeCamera = ARCamera.transform.parent.GetChild(1).GetComponent<Camera>();
        HomeCamera.gameObject.SetActive(false);
        gameObject.transform.parent.GetComponent<Canvas>().worldCamera = ARCamera;
        ARCamera.transform.GetChild(0).GetComponent<BackgroundPlaneController>().SyncFileldOfView += HomeViewController_SyncFileldOfView;

        if (HomeCamera.fieldOfView == ARCamera.fieldOfView)
        {
            ScanningImage.transform.Find("rawPanel").GetChild(0).GetComponent<RawImage>().texture = HomeCamera.targetTexture;
            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                var w = ScanningImage.rectTransform.rect.width + 2 * addwidth;
                var h = (float)(1334 / 750.0) * w;
                ScanningImage.transform.Find("rawPanel").GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(ScanningImage.rectTransform.rect.width, h);
            }
            HomeCamera.gameObject.SetActive(true);
            TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
            CameraDevice.Instance.Start();
        }
        ui_events();
        scanningImage_scanSticksImage_canAnimation = true;
        TimeOutButton.onClick.AddListener(() => {
            scanningImage_scanSticksImage_canAnimation = true;
            ScanningImage_ScanSticksImage.gameObject.SetActive(true);
            TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
            TimeOutButton.gameObject.SetActive(false);
            StartCoroutine(TimeOut());
        });
        StartCoroutine(TimeOut());
    }                             

    private void HomeViewController_SyncFileldOfView(float obj)
    {
        if (obj != HomeCamera.fieldOfView)
        {
            HomeCamera.fieldOfView = obj;
            HomeCamera.nearClipPlane = ARCamera.nearClipPlane;
            HomeCamera.farClipPlane = ARCamera.farClipPlane;
            HomeCamera.depth = ARCamera.depth;
            var t = new RenderTexture(750, 1334, 1);
            HomeCamera.targetTexture = t;
            ScanningImage.transform.Find("rawPanel").GetChild(0).GetComponent<RawImage>().texture = t;
            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                var w = ScanningImage.rectTransform.rect.width + 2 * addwidth;
                var h = (float)(1334 / 750.0) * w;
                ScanningImage.transform.Find("rawPanel").GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
            }
            HomeCamera.gameObject.SetActive(true);
        }
    }
    public bool CanBack= true;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CanBack)
                _go_home();
        }

        

        scanSticksAnimation();
    }
    [DllImport("__Internal")]
    static extern void setHVCButtomImage();
    // 静态控件适应屏幕
    void static_ui_layout()
    {
        Vector2 topSize, scanningSize;
        GetComponent<RectTransform>().sizeDelta = new Vector2(Display.main.systemWidth, Display.main.systemHeight);
        
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
        var bh =(float)(97 / 667.0 * Display.main.systemHeight);
        //ButtonImage.rectTransform.sizeDelta = new Vector2(ButtonImage.rectTransform.sizeDelta.x, (float)bh);
        ButtonImage.rectTransform.sizeDelta = new Vector2(Display.main.systemWidth, bh);
        ButtonImage.rectTransform.anchoredPosition3D = new Vector3(0, bh, 0);
        // 顶部两图标
        var top_left_w = (93 / 1080.0) * Display.main.renderingWidth;
        var top_left_h = top_left_w;
        var posy = ((48+top_left_w/2) / 1334.0) * Display.main.renderingHeight;
        var posx = ((50 + top_left_w / 2) / 750.0) * Display.main.renderingWidth;
        Top_left_button.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((float)top_left_w, (float)top_left_h);
        Top_right_button.gameObject.GetComponent<RectTransform>().sizeDelta = Top_left_button.gameObject.GetComponent<RectTransform>().sizeDelta;
        Top_left_button.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3((float)posx, -(float)posy, 0);
        Top_right_button.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-(float)posx, -(float)posy, 0);
        Top_right_light_button.gameObject.GetComponent<RectTransform>().sizeDelta = Top_right_button.gameObject.GetComponent<RectTransform>().sizeDelta;
        var to_light_d = (float)(20 / 375.0) * Display.main.systemWidth;
        Top_right_light_button.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -(float)sw/2.0f - to_light_d, 0);

        // 背景遮罩image
        var bgh = (Display.main.systemHeight - scanningSize.y) / 2.0f;
        var bgw = (Display.main.systemWidth - scanningSize.x) / 2.0f;
        Top_BgImage.rectTransform.sizeDelta = new Vector2(Top_BgImage.rectTransform.sizeDelta.x, bgh + 5);
        Buttom_BgImage.rectTransform.sizeDelta = new Vector2(Buttom_BgImage.rectTransform.sizeDelta.x, bgh);
        Left_BgImage.rectTransform.sizeDelta = new Vector2(bgw, scanningSize.y);
        Right_BgImage.rectTransform.sizeDelta = new Vector2(bgw, scanningSize.y);

        // 超时按钮
        var timeoutw = ((750 - 77 * 4) / 750.0) * Display.main.systemWidth;
        var timeouth = 188 / 442.0 * timeoutw;
        TimeOutButton.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((float)timeoutw, (float)timeouth);


        //LogText.text = "视图size:" + GetComponent<RectTransform>().sizeDelta;
        //LogText.text += "屏幕size:" + Display.main.systemWidth + ',' + Display.main.systemHeight;

        leftAddImage.rectTransform.sizeDelta = new Vector2(Display.main.systemWidth / 2.0f, 2*Display.main.systemHeight);
        rightAddImage.rectTransform.sizeDelta = new Vector2(Display.main.systemWidth / 2.0f, 2*Display.main.systemHeight);
        topAddImage.rectTransform.sizeDelta = new Vector2(2 * Display.main.systemWidth, Display.main.systemWidth / 2.0f);
        buttomAddImage.rectTransform.sizeDelta = new Vector2(2 * Display.main.systemWidth, Display.main.systemWidth / 2.0f);

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            setHVCButtomImage();
        }
    }
    int addwidth = 0;
    public void HVCButtomImageWidthAdd(string addWidth)
    {
        int.TryParse(addWidth, out addwidth);
        ButtonImage.rectTransform.sizeDelta = new Vector2(ButtonImage.rectTransform.rect.width + addwidth, ButtonImage.rectTransform.rect.height + addwidth);

        var w = ScanningImage.rectTransform.rect.width + 2 * addwidth;
        var h = (float)(1334 / 750.0) * w;
        ScanningImage.transform.Find("rawPanel").GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
    }
    public void SetHVCButtomImagePosy(string posy)
    {
        int py = 20;
        if (int.TryParse(posy, out py))
        ButtonImage.rectTransform.anchoredPosition3D = new Vector3(0, -py, 0);
        else{
            Debug.Log("==>字符串："+posy+"转换失败！");
        }
    }
    // 扫描棒动画控制
    void scanSticksAnimation()
    {
        if(scanningImage_scanSticksImage_canAnimation)
        {
            if (!ScanningImage_ScanSticksImage.gameObject.activeSelf)
                ScanningImage_ScanSticksImage.gameObject.SetActive(true);
            if (ScanningImage_ScanSticksImage.rectTransform.anchoredPosition3D.y >= -ScanningImage.rectTransform.rect.height+10)
                ScanningImage_ScanSticksImage.rectTransform.anchoredPosition3D = new Vector3(
                    ScanningImage_ScanSticksImage.rectTransform.anchoredPosition3D.x, ScanningImage_ScanSticksImage.rectTransform.anchoredPosition3D.y - ScanSticksImageAnimationSpeed * Time.deltaTime, ScanningImage_ScanSticksImage.rectTransform.anchoredPosition3D.z);
            else
                ScanningImage_ScanSticksImage.rectTransform.anchoredPosition3D = new Vector3(
                    ScanningImage_ScanSticksImage.rectTransform.anchoredPosition3D.x, -10, ScanningImage_ScanSticksImage.rectTransform.anchoredPosition3D.z);
        }
    }
    // 控件事件
    public bool isLightClick = false;

    void searchButtonClick()
    {
        Debug.Log(" HomeViewController 点击了搜索按钮。");
        var dh = gameObject.GetComponent<DataHelper>();
        if (dh == null)
            gameObject.AddComponent<DataHelper>();
        
        if (isLightClick)
            CameraDevice.Instance.SetFlashTorchMode(false);
        CanBack = false;
        Helper.Searchcanvas.SetActive(true);
        Helper.Searchcanvas.GetComponent<SearchViewController>().IsFormScan = true;
        
    }
    void ui_events()
    {
        // 搜索按钮事件
        Top_right_button.onClick.AddListener(searchButtonClick);

        // 主页按钮事件
        Top_left_button.onClick.AddListener(_go_home);
        // 闪光灯按钮
        Top_right_light_button.onClick.AddListener(() => {
            if(!isLightClick)
            {
                Top_right_light_button.image.sprite = LightSprites[1];
                CameraDevice.Instance.SetFlashTorchMode(true);
                isLightClick = true;
            }
            else
            {
                Top_right_light_button.image.sprite = LightSprites[0];
                CameraDevice.Instance.SetFlashTorchMode(false);
                isLightClick = false;
            }
        });
    }
    void _go_home()
    {
        Debug.Log(" HomeViewController 点击了主页按钮。");
        if (!TimeOutButton.gameObject.activeSelf)
            TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();
        if (isLightClick)
            CameraDevice.Instance.SetFlashTorchMode(false);
        for (int i = 0; i < GameObject.Find("ARCamera").transform.childCount; ++i)
            GameObject.Find("ARCamera").transform.GetChild(i).gameObject.SetActive(false);
        AppHomeViewController.IsShowGuide = false;
        Instance = null;
        Helper.Searchcanvas.GetComponent<SearchViewController>().InputField.text = string.Empty;
        for (int i = 0; i < Helper.Searchcanvas.GetComponent<SearchViewController>().ContentPanel.transform.childCount; ++i)
            Destroy(Helper.Searchcanvas.GetComponent<SearchViewController>().ContentPanel.transform.GetChild(i).gameObject);
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        SceneManager.LoadScene("Scene/home");
    }
    public void HomeViewHided()
    {
        if (!TimeOutButton.gameObject.activeSelf)
            TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
    }
    // vuforia识别到目标图事件
    public void FoundTarget(string _string)
    {
        StopAllCoroutines();
        scanningImage_scanSticksImage_canAnimation = false;
        ScanningImage_ScanSticksImage.gameObject.SetActive(false);
        Resources.UnloadUnusedAssets();
        if (isLightClick)
            CameraDevice.Instance.SetFlashTorchMode(false);
        TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();
        CameraDevice.Instance.Stop();

        var rhc = transform.parent.Find("RecognizeHandleCanvas");
        rhc.gameObject.GetComponent<RecognizeHandleViewController>().FounedString = _string;
        if (!HomeCamera.gameObject.activeSelf)
            HomeCamera.gameObject.SetActive(true);
        StartCoroutine(MakeFounedImage());
        rhc.gameObject.SetActive(true);
        CanBack = false;
        rhc.gameObject.GetComponent<RecognizeHandleViewController>().Begin();
        rhc.gameObject.GetComponent<RecognizeHandleViewController>().addwidth = addwidth;
        rhc.gameObject.GetComponent<RecognizeHandleViewController>().ScanningImage.transform.Find("rawPanel").GetChild(0).GetComponent<RectTransform>().sizeDelta = ScanningImage.transform.Find("rawPanel").GetChild(0).GetComponent<RectTransform>().sizeDelta;
        rhc.gameObject.GetComponent<RecognizeHandleViewController>().ScanningImage.transform.Find("rawPanel").GetChild(0).GetComponent<RawImage>().texture = HomeCamera.targetTexture;
    }
    // 截取中心图片
    IEnumerator MakeFounedImage()
    {
        HomeCamera.fieldOfView = ARCamera.fieldOfView;
        HomeCamera.farClipPlane = ARCamera.farClipPlane;
        HomeCamera.nearClipPlane = ARCamera.nearClipPlane;
        HomeCamera.depth = ARCamera.depth;

        yield return new WaitForEndOfFrame();

        var pos = ARCamera.WorldToScreenPoint(FoundedImage.transform.position);
        //Rect mRect = new Rect(new Vector2(pos.x - FoundedImage.rectTransform.rect.size.x / 2,
        //    pos.y - FoundedImage.rectTransform.rect.size.y / 2),
        //    new Vector2((int)FoundedImage.rectTransform.rect.width,
        //    (int)FoundedImage.rectTransform.rect.height));
        Rect mRect = new Rect(0, (int)pos.y - Display.main.systemWidth / 2.0f, Display.main.systemWidth, Display.main.systemWidth);
        var mTexture = new Texture2D((int)mRect.width, (int)mRect.height);
        var rt = new RenderTexture((int)Display.main.systemWidth, (int)Display.main.systemHeight, (int)ARCamera.depth);
        var yt = HomeCamera.targetTexture;
        HomeCamera.targetTexture = rt;
        RenderTexture.active = rt;

        HomeCamera.Render();
        mTexture.ReadPixels(mRect, 0, 0);
        mTexture.Apply();
        FoundedImage.sprite = Sprite.Create(mTexture, new Rect(new Vector2(0, 0), mRect.size), new Vector2(0.5f, 0.5f));
        transform.parent.Find("RecognizeHandleCanvas").gameObject.GetComponent<RecognizeHandleViewController>().ScanningImage.transform.Find("foundedImage").GetComponent<UnityEngine.UI.Image>().sprite = FoundedImage.sprite;
        RenderTexture.active = null;
        HomeCamera.targetTexture = null;
        GameObject.Destroy(rt);
        HomeCamera.targetTexture = yt;
        HomeCamera.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    // 超时提醒 
    public IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(10);
        TimeOutButton.gameObject.SetActive(true);
        scanningImage_scanSticksImage_canAnimation = false;
        ScanningImage_ScanSticksImage.gameObject.SetActive(false);
        TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();
    }

}
