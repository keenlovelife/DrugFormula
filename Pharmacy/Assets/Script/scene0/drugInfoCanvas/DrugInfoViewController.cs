using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrugInfoViewController : MonoBehaviour {

    public static DrugInfoViewController Instance;

    public Drug Drug;
    public Classes ClassesObj;

    public Button BackButton, ScreenShotButton, Button1, Button2;
    public Image DrugBoxImage, DrugIamge, ScreenShotImage;
    public Text TitleText, ScreenshotText, LogText;
    public GameObject BoxPanel, InfoPanel, InfoDataPanel, Panel, ScreenshotPanel;

    public Sprite MainStructureSprite_nor, MainStructureSprite_seceted,
        InfoSprite_nor, InfoSprite_seceted,
        KnowledgePoint_1_nor, KnowledgePoint_1_seceted,
        KnowledgePoint_2_nor, KnowledgePoint_2_seceted,
        ShowDrugSprite, ShowDrugDefaultSprite;
    public bool IsSearch;
    private void Awake()
    {
        Debug.Log(" DrugInfoViewController awake()");
        Instance = this;
    }
    public void Endable()
    {
        OnEnable();
    }
    private void OnEnable()
    {
        Debug.Log(" DrugInfoViewController OnEnable()");
        if (ClassesObj == null)
            return;

        canShow = true;
        canHide = false;
        if (LogText.gameObject.activeSelf)
            LogText.gameObject.SetActive(false);
        // 数据修改
        re();

        InfoPanel.SetActive(false);

        if (ShowDrugDefaultSprite == null)
            ShowDrugDefaultSprite = DrugIamge.sprite;
        if (!isMainStructureButtonClick)
        {
            DrugBoxImage.gameObject.GetComponent<MainStruetureController>().SetPngCount(1, false);
            DrugBoxImage.transform.Find("animationImage").gameObject.SetActive(true);

            if (IsSearch)
            {
                DrugIamge.gameObject.SetActive(false);

                TitleText.text = ClassesObj.Drugs[0].Name;
                Drug = ClassesObj.Drugs[0];
                if (ShowDrugSprite == null)
                {
                    if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        var filepath = "/sdcard/" + Application.productName + "/" + Drug.AttachedInfoList[0].PngName;
                        if (System.IO.File.Exists(filepath))
                        {
                            filepath = "file://" + filepath;
                            tabfun.helper.Coroutine.Instance.StartWWW(new WWW(filepath), (www) =>
                            {
                                if (www.error == null)
                                {

                                    var sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(.5f, .5f));
                                    sprite.name = www.url.Substring(www.url.LastIndexOf('/') + 1);
                                    if (!sprite.name.EndsWith(".png"))
                                        sprite.name += ".png";
                                    DrugIamge.sprite = sprite;
                                    ShowDrugSprite = sprite;
                                    DrugIamge.gameObject.SetActive(true);
                                    DrugBoxImage.transform.Find("animationImage").gameObject.SetActive(false);
                                }
                                else
                                {
                                    if (!LogText.gameObject.activeSelf)
                                        LogText.gameObject.SetActive(true);
                                    LogText.text = "加载失败:" + www.error;
                                }
                            });
                        }
                        else
                        {
                            var path = "http://www.baitongshiji.com/images/" + Drug.AttachedInfoList[0].PngName + ".png";
                            tabfun.helper.Coroutine.Instance.StartWWW(new WWW(path), (w) =>
                            {
                                if (w.error == null)
                                {
                                    var savefilepath = "/sdcard/" + Application.productName;
                                    if (!System.IO.Directory.Exists(savefilepath))
                                        System.IO.Directory.CreateDirectory(savefilepath);
                                    savefilepath += "/" + Drug.AttachedInfoList[0].PngName;
                                    System.IO.File.WriteAllBytes(savefilepath, w.bytes);
                                    var sprite = Sprite.Create(w.texture, new Rect(0, 0, w.texture.width, w.texture.height), new Vector2(.5f, .5f));
                                    sprite.name = w.url.Substring(w.url.LastIndexOf('/') + 1);
                                    DrugIamge.sprite = sprite;
                                    ShowDrugSprite = sprite;
                                    DrugIamge.gameObject.SetActive(true);
                                    DrugBoxImage.transform.Find("animationImage").gameObject.SetActive(false);
                                }
                                else
                                {
                                    if (!LogText.gameObject.activeSelf)
                                        LogText.gameObject.SetActive(true);
                                    LogText.text = "加载失败:" + w.error;
                                }
                            });
                        }
                    }else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        var path = "http://www.baitongshiji.com/images/" + Drug.AttachedInfoList[0].PngName + ".png";
                        tabfun.helper.Coroutine.Instance.StartWWW(new WWW(path), (w) =>
                        {
                            if (w.error == null)
                            {
                                //var savefilepath = "/sdcard/" + Application.productName;
                                //if (!System.IO.Directory.Exists(savefilepath))
                                //    System.IO.Directory.CreateDirectory(savefilepath);
                                //savefilepath += "/" + Drug.AttachedInfoList[0].PngName;
                                //if (Application.platform != RuntimePlatform.IPhonePlayer)
                                //    System.IO.File.WriteAllBytes(savefilepath, w.bytes);
                                var sprite = Sprite.Create(w.texture, new Rect(0, 0, w.texture.width, w.texture.height), new Vector2(.5f, .5f));
                                sprite.name = w.url.Substring(w.url.LastIndexOf('/') + 1);
                                DrugIamge.sprite = sprite;
                                ShowDrugSprite = sprite;
                                DrugIamge.gameObject.SetActive(true);
                                DrugBoxImage.transform.Find("animationImage").gameObject.SetActive(false);
                            }
                            else
                            {
                                if (!LogText.gameObject.activeSelf)
                                    LogText.gameObject.SetActive(true);
                                LogText.text = "加载失败:" + w.error;
                            }
                        });
                    }
                }
                else
                {
                    DrugIamge.sprite = ShowDrugSprite;
                    DrugIamge.gameObject.SetActive(true);
                    DrugBoxImage.transform.Find("animationImage").gameObject.SetActive(false);
                }
            }
            else
            {
                DrugIamge.sprite = ShowDrugSprite;
                DrugIamge.gameObject.SetActive(true);
                DrugBoxImage.transform.Find("animationImage").gameObject.SetActive(false);
                var dd = ClassesObj.Drugs.Find((d) =>
                {
                    foreach (var a in d.AttachedInfoList)
                        if (a.PngName + ".png" == ShowDrugSprite.name)
                            return true;
                    return false;
                });
                TitleText.text = dd.Name;
                Drug = dd;
            }
        }
        else
        {
            TitleText.text = string.Empty;
            if (ClassesObj.Mtype == Classes.ModelType.UnknownType)
                TitleText.text = "无内容！";
            else
                TitleText.text = ClassesObj.Name;
            // 加载主体结构图片
            DrugBoxImage.transform.Find("animationImage").gameObject.SetActive(true);

            var num = -1;
            DrugBoxImage.gameObject.GetComponent<MainStruetureController>().SetPngCount(num);
            if (ClassesObj.Mtype == Classes.ModelType.UnknownType)
                num = 0;
            else if (ClassesObj.Mtype == Classes.ModelType.OnBehalfOfDrugModelType)
            {
                var dd = ClassesObj.Drugs.Find((d) => { return d.Name == ClassesObj.OnBehalfOfTheDrugName; });
                num = dd.AttachedInfoList.Count;
                for (var i = 0; i < dd.AttachedInfoList.Count; ++i)
                    DrugBoxImage.transform.GetChild(2 + i).GetComponent<DrugImageContoller>().StartLoadPng(dd.AttachedInfoList[i].PngName);
            }
            else
            {
                if (ClassesObj.AttachedInfoList != null && ClassesObj.AttachedInfoList.Count > 0)
                {
                    num = ClassesObj.AttachedInfoList.Count;
                    for (var i = 0; i < ClassesObj.AttachedInfoList.Count; ++i)
                        DrugBoxImage.transform.GetChild(2 + i).GetComponent<DrugImageContoller>().StartLoadPng(ClassesObj.AttachedInfoList[i].PngName);
                }
                else if (ClassesObj.OnBehalfOfTheDrugName != string.Empty)
                {
                    var dd = ClassesObj.Drugs.Find((d) => { return d.Name == ClassesObj.OnBehalfOfTheDrugName; });
                    num = dd.AttachedInfoList.Count;
                    for (var i = 0; i < dd.AttachedInfoList.Count; ++i)
                        DrugBoxImage.transform.GetChild(2 + i).GetComponent<DrugImageContoller>().StartLoadPng(dd.AttachedInfoList[i].PngName);
                }
                else
                    num = 0;
            }
            DrugBoxImage.gameObject.GetComponent<MainStruetureController>().SetPngCount(num);
        }

        // 按钮修改
        BackButton.onClick.RemoveAllListeners();
        Button1.onClick.RemoveAllListeners();
        Button2.onClick.RemoveAllListeners();

        if (isMainStructureButtonClick)
            Button1.image.sprite = MainStructureSprite_seceted;
        else
            Button1.image.sprite = MainStructureSprite_nor;
        Button2.image.sprite = InfoSprite_nor;
        var b1st = new SpriteState();
        b1st.pressedSprite = MainStructureSprite_seceted;
        Button1.spriteState = b1st;
        var b2st = new SpriteState();
        b2st.pressedSprite = InfoSprite_seceted;
        Button2.spriteState = b2st;

        BackButton.onClick.AddListener(() =>
        {
            Debug.Log(" DrugInfoViewController 返回按钮点击了");
            if (!isMainStructureButtonClick)
            {
                if (!canHide && !canShow)
                    canHide = true;
            }
            else
            {
                isMainStructureButtonClick = false;
                OnEnable();
            }
        });
        Button1.onClick.AddListener(() =>
        {
            Debug.Log("DrugInfoViewController 点击了主体结构按钮");
            if (!isMainStructureButtonClick)
            {
                isMainStructureButtonClick = true;
                OnEnable();
            }
        });
        Button2.onClick.AddListener(() =>
        {
            Debug.Log("DrugInfoViewController 点击了详细信息按钮");

            if (TitleText.text != "无内容！")
            {
                re();
                InfoPanel.SetActive(true);
            }
        });
    }
    public  bool isMainStructureButtonClick = false;
    private void OnDisable()
    {
        Debug.Log(" DrugInfoViewController OnDisable()");
        if (!IsSearch)
        {
            var all = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
            GameObject obj = null;
            foreach (var o in all)
                if (o.transform.parent == null && o.name == "Canvas")
                {
                    obj = o.transform.Find("RecognizeHandleCanvas").gameObject;
                    break;
                }
            if (obj != null)
            {
                if (!obj.activeSelf)
                    obj.SetActive(true);
                obj.GetComponent<RecognizeHandleViewController>().DrugsPanel.GetComponent<DrugsViewController>().canScals = true;
            }
        }
        isMainStructureButtonClick = false;
        IsSearch = false;
        Drug = null;
        ClassesObj = null;
        ShowDrugSprite = null;
        var alll = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
        foreach (var o in alll)
            if (o.transform.parent == null && o.name == "searchCanvas")
            {
                o.GetComponent<SearchViewController>().IsItemClicked = false;
                break;
            }
    }
    void Start () {
        Debug.Log(" DrugInfoViewController Start ()");
        startPos = Vector3.zero;
        startSize = Vector2.zero;
        startPos3D = Vector3.zero;
        movePos = Vector3.zero;
        ScreenShotButton.onClick.AddListener(() => {

            if (ScreenshotPanel.activeSelf)
                return;

            ScreenshotPanel.gameObject.SetActive(true);
            var t2d = DataHelper.Instance.SreenShot();
            if (t2d != null)
            {
                ScreenShotImage.sprite = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
                ScreenShotImage.rectTransform.sizeDelta = new Vector2(t2d.width, t2d.height);
                screenshotAnimaiton();
            }
            else
            {
                ScreenshotPanel.gameObject.SetActive(false);
            }
        });
        ui_layout();
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            ScreenShotButton.gameObject.SetActive(false);
    }
    bool canScreenshotAnimation = false;
    void screenshotAnimaiton()
    {
        if(!ScreenshotPanel.gameObject.activeSelf)
            ScreenshotPanel.gameObject.SetActive(true);
        canScreenshotAnimation = true;
    }

    bool canShow = false, canHide = false;
    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!InfoPanel.activeSelf)
            {
                if (!isMainStructureButtonClick)
                {
                    if (!canHide && !canShow)
                        canHide = true;
                }
                else
                {
                    isMainStructureButtonClick = false;
                    OnEnable();
                }
            }
        }

        if (canHide)
        {
            Panel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(Panel.GetComponent<RectTransform>().anchoredPosition3D, 
                new Vector3(Display.main.systemWidth, 0, 0), 
                20 * Time.deltaTime);
            if (Panel.GetComponent<RectTransform>().anchoredPosition3D.x > Display.main.systemWidth - 1)
            {
                canHide = false;
                gameObject.SetActive(false);
            }
        }
        if (canShow)
        {
            Panel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(Panel.GetComponent<RectTransform>().anchoredPosition3D, 
                new Vector3(0, 0, 0), 
                20 * Time.deltaTime);
            if (Panel.GetComponent<RectTransform>().anchoredPosition3D.x < 1)
            {
                canShow = false;
                Panel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            }
        }

        if (!InfoPanel.gameObject.activeSelf)
            pngEvents();

        if(canScreenshotAnimation)
        {
            float w = Display.main.systemWidth / 2.0f;
            float h = (Display.main.systemHeight / (float)Display.main.systemWidth) * w;
            ScreenShotImage.rectTransform.sizeDelta = Vector2.Lerp(ScreenShotImage.rectTransform.sizeDelta, new Vector2(w, h), 2 * Time.deltaTime);
            if(ScreenShotImage.rectTransform.rect.width < w + 15)
            {
                canScreenshotAnimation = false;
                ScreenShotImage.rectTransform.sizeDelta = new Vector2(w, h);
                ScreenshotPanel.gameObject.SetActive(false);
            }
        }
	}
    void ui_layout()
    {
        // 整个界面Panel
        float panel_posx = Display.main.systemWidth,
            panel_width = Display.main.systemWidth,
            panel_height = Display.main.systemHeight;
        Panel.GetComponent<RectTransform>().sizeDelta = new Vector2(panel_width, panel_height);
        Panel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(panel_posx, 0, 0);

        var to_top = (float)(25 / 667.0) * Display.main.systemHeight;
        var to_left = (float)(25 / 375.0) * Display.main.systemWidth;
        var top_icon_w = (float)(93 / 1080.0) * Display.main.systemWidth;
        var to_buttom = (float)(43 / 667.0) * Display.main.systemHeight;
        var buttom_icon_w = (float)(210 / 750.0) * Display.main.systemWidth;
        var buttom_iocn_h = (float)(80 / 210.0) * buttom_icon_w;

        BackButton.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(top_icon_w, top_icon_w);
        ScreenShotButton.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(top_icon_w, top_icon_w);
        BackButton.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(to_left, -to_top, 0);
        ScreenShotButton.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-to_left, -to_top, 0);
        TitleText.rectTransform.anchoredPosition3D = new Vector3(0, -to_top-(float)(10/93.0)*top_icon_w, 0);
        TitleText.rectTransform.sizeDelta = new Vector2(Display.main.systemWidth - (to_left + top_icon_w) * 2, top_icon_w - (float)(10 / 93.0) * top_icon_w * 2);

        Button1.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(buttom_icon_w, buttom_iocn_h);
        Button2.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(buttom_icon_w, buttom_iocn_h);
        Button1.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(to_left, to_buttom, 0);
        Button2.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-to_left, to_buttom, 0);

        // 截图panel
        float h = (float)(27 / 667.0) * Display.main.systemHeight;
        ScreenshotText.rectTransform.sizeDelta = new Vector2(Display.main.systemWidth, h);
        ScreenshotPanel.SetActive(false);

        // logText
        float log_text_height = (float)(22 / 667.0) * Display.main.systemHeight;
        LogText.rectTransform.sizeDelta = new Vector2(Display.main.systemWidth, log_text_height);
    }
    // png图片的放缩拖拽旋转
    public  bool canRotate = false,
        canDragAndDrop = false,
        canZoom = false;
    void pngEvents()
    {
        if(Input.touchCount == 1)
        {
            if(!canRotate)
            {
                pngzero();
                canRotate = true;
                canDragAndDrop = false;
                canZoom = false;
            }
        }else if(Input.touchCount == 2)
        {
            canRotate = false;
            if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved)
            {
                if ((Input.touches[0].deltaPosition.x > 0 && Input.touches[1].deltaPosition.x > 0 && (Input.touches[0].deltaPosition.y > 0 && Input.touches[1].deltaPosition.y > 0 || Input.touches[0].deltaPosition.y < 0 && Input.touches[1].deltaPosition.y < 0)) ||
                    (Input.touches[0].deltaPosition.x < 0 && Input.touches[1].deltaPosition.x < 0 && (Input.touches[0].deltaPosition.y > 0 && Input.touches[1].deltaPosition.y > 0 || Input.touches[0].deltaPosition.y < 0 && Input.touches[1].deltaPosition.y < 0)))
                {
                    if (!canDragAndDrop)
                    {
                        pngzero();
                        canDragAndDrop = true;
                        canZoom = false;
                    }
                }else
                {
                    if (!canZoom) {
                        pngzero();
                        canZoom = true;
                        canDragAndDrop = false;
                    }
                }
            }
        }
        else
        {
            pngzero();
        }

        // 执行事件：放缩、拖拽、旋转
        if(canZoom)
        {
            pngZoom();
        }else if (canDragAndDrop)
        {
            pngDragAndDrop();
        }else if(canRotate)
        {
            pngRotate();
        }
    }
    void pngRotate()
    {
        Debug.Log("旋转");
        if(startPos == Vector3.zero)
        {
            startPos = Input.touches[0].position;
            movePos = startPos;
        }
        float deltaX = Input.touches[0].position.x - movePos.x;
        if (Input.touches[0].position.y > Display.main.systemHeight / 2.0f)
        {
            deltaX = -deltaX;
        }
        BoxPanel.transform.Rotate(new Vector3(0, 0, 10 * deltaX * Time.deltaTime));
        movePos = Input.touches[0].position;
    }
    Vector3 startPos3D;
    Vector3 startPos, movePos;
    Vector2 posDalta;
    void pngDragAndDrop()
    {
        Debug.Log("拖拽");

        if (startPos == Vector3.zero)
        {
            startPos = (Input.touches[0].position + Input.touches[1].position) / 2.0f;
            posDalta = new Vector2(startPos.x, startPos.y) - new Vector2(Display.main.systemWidth / 2.0f, Display.main.systemHeight) - new Vector2(startPos3D.x, startPos3D.y);
            movePos = startPos;
        }
        else
        {
            movePos = (Input.touches[0].position + Input.touches[1].position) / 2.0f;
        }

        var boxpos3d = new Vector2(movePos.x, movePos.y) - new Vector2(Display.main.systemWidth / 2.0f, Display.main.systemHeight) - posDalta;
        BoxPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(boxpos3d.x, boxpos3d.y, 0);
    }
    float startDistance = 0;
    Vector2 startSize;
    public float sizesize;
    void pngZoom()
    {
        Debug.Log("缩放");

        Debug.Log(" 两个手指之间的距离：" + Vector3.Distance(Input.touches[0].position, Input.touches[1].position));
        if (startDistance == 0)
        {
            startDistance = Vector3.Distance(Input.touches[0].position, Input.touches[1].position);
            startSize = BoxPanel.GetComponent<RectTransform>().sizeDelta;
        }
        if (startDistance != 0)
        {
            sizesize = Vector3.Distance(Input.touches[0].position, Input.touches[1].position) / startDistance;
            var size = startSize * sizesize;
            BoxPanel.GetComponent<RectTransform>().sizeDelta = size;
        }
    }
    void pngzero()
    {
        canRotate = false;
        canDragAndDrop = false;
        canZoom = false;
        startDistance = 0;
        startSize = BoxPanel.GetComponent<RectTransform>().sizeDelta;
        startPos = Vector3.zero;
        startPos3D = BoxPanel.GetComponent<RectTransform>().anchoredPosition3D;
        movePos = Vector3.zero;
        posDalta = Vector2.zero;
    }

    public void re()
    {
        var drug_to_top = (float)(170 / 667.0) * Display.main.systemHeight;
        var drug_w = (float)((375 - 50) / 375.0) * Display.main.systemWidth;
        BoxPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(drug_w, drug_w);
        BoxPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -drug_to_top - drug_w / 2.0f, 0);
        BoxPanel.transform.rotation = Quaternion.identity;
        StartCoroutine(ree());
    }
    IEnumerator ree()
    {
        yield return new WaitForSeconds(0.01f);
        BoxPanel.transform.rotation = Quaternion.identity;
    }
}
