using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrugsViewController : MonoBehaviour {
    public static DrugsViewController Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Image MiddleBoxImage;

    public Classes ClassesObj {
        get { return classes; }
        set
        {
            classes = value;
            loadPngs();
        }
    }
    Classes classes;

    float drugs_d;
    int itemCount = 0;
    public float Hider = 10;
    private void OnEnable()
    {
        drugs_d = (Display.main.systemWidth - MiddleBoxImage.rectTransform.rect.width) / 4.0f;
        StartCoroutine(showPngs());
    }
    IEnumerator showPngs()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < itemCount; ++i)
            transform.GetChild(i).gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        Debug.Log(" DrugsViewController OnDisable()");
        StopAllCoroutines();
        Resources.UnloadUnusedAssets();
    }
    public void Clear()
    {
        z();
        List<GameObject> l = new List<GameObject>();
        for (var i = 0; i < transform.childCount; ++i)
            l.Add(transform.GetChild(i).gameObject);
        transform.DetachChildren();
        foreach (var o in l)
            DestroyImmediate(o, true);
        AssetBundle.UnloadAllAssetBundles(true);
    }

    void Start () {
        canScals = true;
        md = new Vector3(0, 0, -1024);
    }

    void Update () {
        if (canScals)
            handleDrugsScorllEvents();
 
	}
    bool canClickItem = false;
    // 加载图片
    void loadPngs(bool isTest = true)
    {
        // 计算要显示药品的总数量
        itemCount = 0;
        Clear();
        foreach (var d in ClassesObj.Drugs)
            foreach (var a in d.AttachedInfoList)
            { // 生成每个png button
                var item = Instantiate<GameObject>(Resources.Load<GameObject>("itemButton"));
                item.transform.SetParent(transform);
                if (Application.platform != RuntimePlatform.IPhonePlayer)
                {
                    var filepath = "/sdcard/" + Application.productName + "/" + a.PngName;
                    if (System.IO.File.Exists(filepath))
                        item.SetActive(false);
                }
                else
                {
                    item.SetActive(false);
                }
                // 布局
                item.transform.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                item.transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                if (itemCount == 0)
                    item.transform.GetComponent<RectTransform>().sizeDelta = MiddleBoxImage.rectTransform.sizeDelta;
                else
                    item.transform.GetComponent<RectTransform>().sizeDelta = MiddleBoxImage.rectTransform.sizeDelta * 0.8f;
                float log_text_height = (float)(22 / 667.0) * Display.main.systemHeight;
                item.transform.Find("Text").GetComponent<Text>().rectTransform.sizeDelta = new Vector2(item.transform.GetComponent<RectTransform>().rect.width, log_text_height);
                item.transform.Find("Text").gameObject.SetActive(false);
                ++itemCount;
                item.GetComponent<ItemButtonData>().PngName = a.PngName;
                item.GetComponent<ItemButtonData>().DrugName = d.Name;
                item.GetComponent<ItemButtonData>().IsLoaded = false;

                item.GetComponent<Button>().onClick.AddListener(()=>{
                    if (canClickItem && item.GetComponent<ItemButtonData>().IsLoaded)
                    {
                        Debug.Log(" " + item.GetComponent<Button>().image.sprite.name + "被点击了");
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
                        drugInfo.GetComponent<DrugInfoViewController>().ClassesObj = ClassesObj;
                        drugInfo.gameObject.SetActive(true);
                        // transform.parent.gameObject.SetActive(false);
                        z();
                        canScals = false;
                    }
                });
              
            }
        canScals = true;
        RecognizeHandleViewController.Instance.DrugNameText.gameObject.SetActive(true);
        RecognizeHandleViewController.Instance.DrugNameText.text = ClassesObj.Drugs[0].Name;
        // 等待加载图片
        waitLoadPngs();
    }
    void waitLoadPngs()
    {
        foreach (var d in ClassesObj.Drugs)
            foreach (var a in d.AttachedInfoList)
            {
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    var filepath = "/sdcard/" + Application.productName + "/" + a.PngName;
                    if (System.IO.File.Exists(filepath))
                    {
                        Debug.Log(" path:" + filepath);
                        filepath = "file://" + filepath;
                        tabfun.helper.Coroutine.Instance.StartWWW(new WWW(filepath), (www) =>
                        {
                            GameObject item = null;
                            for (int i = 0; i < itemCount; ++i)
                                if (transform.GetChild(i).GetComponent<ItemButtonData>().PngName == a.PngName)
                                {
                                    item = transform.GetChild(i).gameObject;
                                    break;
                                }
                            if (www.error == null)
                            {
                                var sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(.5f, .5f));
                                sprite.name = www.url.Substring(www.url.LastIndexOf('/') + 1);
                                if (!sprite.name.EndsWith(".png"))
                                    sprite.name += ".png";

                                if (item != null)
                                {
                                    item.SetActive(true);
                                    item.GetComponent<ItemButtonData>().IsLoaded = true;
                                    item.GetComponent<Button>().image.sprite = sprite;
                                    item.GetComponent<Button>().image.color = new Color(item.GetComponent<Button>().image.color.r, item.GetComponent<Button>().image.color.g, item.GetComponent<Button>().image.color.b, 255);
                                    item.transform.Find("animationImage").gameObject.SetActive(false);
                                    // 检查所有图片都加载完毕没
                                    bool isAllLoaded = true;
                                    for (int i = 0; i < itemCount; ++i)
                                        if (!item.GetComponent<ItemButtonData>().IsLoaded)
                                        {
                                            isAllLoaded = false;
                                            break;
                                        }
                                    if (isAllLoaded)
                                    {
                                        // 所有图片全部都加载完成
                                        RecognizeHandleViewController.Instance.canStatusImageAnimation = false;
                                        RecognizeHandleViewController.Instance.StatusImage.rectTransform.rotation = Quaternion.identity;
                                        RecognizeHandleViewController.Instance.StatusImage.sprite = RecognizeHandleViewController.Instance.ConfirmSprite;
                                        StartCoroutine(status_buttom_image_animation_start());
                                    }
                                }
                            }
                            else
                            {
                                if (item != null)
                                {
                                    item.transform.Find("Text").gameObject.SetActive(true);
                                    item.transform.Find("Text").GetComponent<Text>().text = "加载失败:" + www.error;
                                }
                            }
                        });
                    }
                    else
                    {
                        var path = "http://www.baitongshiji.com/images/" + a.PngName + ".png";
                        tabfun.helper.Coroutine.Instance.StartWWW(new WWW(path), (w) =>
                        {
                            GameObject item = null;
                            for (int i = 0; i < itemCount; ++i)
                                if (transform.GetChild(i).GetComponent<ItemButtonData>().PngName == a.PngName)
                                {
                                    item = transform.GetChild(i).gameObject;
                                    break;
                                }
                            if (w.error == null)
                            {
                                var savefilepath = "/sdcard/" + Application.productName;
                                if (!System.IO.Directory.Exists(savefilepath))
                                    System.IO.Directory.CreateDirectory(savefilepath);
                                var filename = w.url.Substring(w.url.LastIndexOf('/') + 1);
                                filename = filename.Substring(0, filename.Length - filename.IndexOf('.'));
                                savefilepath += "/" + a.PngName;
                                System.IO.File.WriteAllBytes(savefilepath, w.bytes);
                                var sprite = Sprite.Create(w.texture, new Rect(0, 0, w.texture.width, w.texture.height), new Vector2(.5f, .5f));
                                sprite.name = w.url.Substring(w.url.LastIndexOf('/') + 1);

                                if (item != null)
                                {
                                    item.GetComponent<ItemButtonData>().IsLoaded = true;
                                    item.GetComponent<Button>().image.sprite = sprite;
                                    item.GetComponent<Button>().image.color = new Color(item.GetComponent<Button>().image.color.r, item.GetComponent<Button>().image.color.g, item.GetComponent<Button>().image.color.b, 255);
                                    item.transform.Find("animationImage").gameObject.SetActive(false);
                                    // 检查所有图片都加载完毕没
                                    bool isAllLoaded = true;
                                    for (int i = 0; i < itemCount; ++i)
                                        if (!item.GetComponent<ItemButtonData>().IsLoaded)
                                        {
                                            isAllLoaded = false;
                                            break;
                                        }
                                    if (isAllLoaded)
                                    {
                                        // 所有图片全部都加载完成
                                        RecognizeHandleViewController.Instance.canStatusImageAnimation = false;
                                        RecognizeHandleViewController.Instance.StatusImage.rectTransform.rotation = Quaternion.identity;
                                        RecognizeHandleViewController.Instance.StatusImage.sprite = RecognizeHandleViewController.Instance.ConfirmSprite;
                                        StartCoroutine(status_buttom_image_animation_start());
                                    }
                                }
                            }
                            else
                            {
                                if (item != null)
                                {
                                    item.transform.Find("Text").gameObject.SetActive(true);
                                    item.transform.Find("Text").GetComponent<Text>().text = "加载失败:" + w.error;
                                }
                            }
                        });
                    }
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    var path = "http://www.baitongshiji.com/images/" + a.PngName + ".png";
                    tabfun.helper.Coroutine.Instance.StartWWW(new WWW(path), (w) =>
                    {
                        GameObject item = null;
                        for (int i = 0; i < itemCount; ++i)
                            if (transform.GetChild(i).GetComponent<ItemButtonData>().PngName == a.PngName)
                            {
                                item = transform.GetChild(i).gameObject;
                                break;
                            }
                        if (w.error == null)
                        {
                            var sprite = Sprite.Create(w.texture, new Rect(0, 0, w.texture.width, w.texture.height), new Vector2(.5f, .5f));
                            sprite.name = w.url.Substring(w.url.LastIndexOf('/') + 1);

                            if (item != null)
                            {
                                if (!item.activeSelf)
                                    item.SetActive(true);
                                item.GetComponent<ItemButtonData>().IsLoaded = true;
                                item.GetComponent<Button>().image.sprite = sprite;
                                item.GetComponent<Button>().image.color = new Color(item.GetComponent<Button>().image.color.r, item.GetComponent<Button>().image.color.g, item.GetComponent<Button>().image.color.b, 255);
                                item.transform.Find("animationImage").gameObject.SetActive(false);
                                // 检查所有图片都加载完毕没
                                bool isAllLoaded = true;
                                for (int i = 0; i < itemCount; ++i)
                                    if (!item.GetComponent<ItemButtonData>().IsLoaded)
                                    {
                                        isAllLoaded = false;
                                        break;
                                    }
                                if (isAllLoaded)
                                {
                                    // 所有图片全部都加载完成
                                    RecognizeHandleViewController.Instance.canStatusImageAnimation = false;
                                    RecognizeHandleViewController.Instance.StatusImage.rectTransform.rotation = Quaternion.identity;
                                    RecognizeHandleViewController.Instance.StatusImage.sprite = RecognizeHandleViewController.Instance.ConfirmSprite;
                                    StartCoroutine(status_buttom_image_animation_start());
                                }
                            }
                        }
                        else
                        {
                            if (item != null)
                            {
                                item.transform.Find("Text").gameObject.SetActive(true);
                                item.transform.Find("Text").GetComponent<Text>().text = "加载失败:" + w.error;
                            }
                        }
                    });
                }
            }
    }
    IEnumerator status_buttom_image_animation_start()
    {
        yield return new WaitForSeconds(0.5f);
        RecognizeHandleViewController.Instance.CanButtomImageAnimation = true;
    }

    // 布局其他itembutton
    void layoutItembuttons()
    {
        for(int i = 1;i< transform.childCount;++i)
        {
            var pre = transform.GetChild(i - 1).GetComponent<RectTransform>();

            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D =
                new Vector3((float)(pre.anchoredPosition3D.x + pre.rect.width / 2.0f + drugs_d + transform.GetChild(i).GetComponent<RectTransform>().rect.width /2.0f), 0, 0);
        }
    }

    bool isFastSweep = false, isNormalSweep = false, isStart = false, isEnd = false, isFisrtTimeMouseDown = false;
    public bool canScals = false;
    Vector3 startPos, endPos, movedPos;
    long preTicks = 0;
    float offset_x = 0, middle_offset_to_left = 0, right_offset_to_left = 0;
    //float offset_all = 0;
    float d_whenReset = 0;
    Vector3 md;
    int index = -1;
    // 滚动事件函数： 滚动、拖动动画、复位到中心
    void handleDrugsScorllEvents()
    {
        if (Input.touchCount == 1)
            movedPos = Input.touches[0].position;
        else
            movedPos = Input.mousePosition;

        // 触碰开始
        if (Input.touchCount == 1)
        {
            if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
            {
                Debug.Log("刚开始触摸");
                var screenpos = HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.position);
                var wpos = HomeViewController.Instance.ARCamera.ScreenToWorldPoint(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, screenpos.z));
                if (Input.touches[0].position.y <= screenpos.y + MiddleBoxImage.rectTransform.rect.height / 2.0f && Input.touches[0].position.y >= screenpos.y - MiddleBoxImage.rectTransform.rect.height / 2.0f)
                {
                    startPos = Input.touches[0].position;
                    movedPos = startPos;
                    offset_x = wpos.x - transform.GetChild(0).position.x;
                    preTicks = System.DateTime.Now.Ticks;
                    isStart = true;
                    isEnd = false;
                    isFastSweep = false;
                    isNormalSweep = false;
                    canClickItem = true;
                }
            }

        }
        // 触碰结束
        if ((Input.touchCount == 1) && isStart)
        {
            if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Ended)
            {
                Debug.Log(" 触摸结束");
                endPos = Input.touches[0].position;
                isStart = false;
                isEnd = true;
                var time = System.DateTime.Now.Ticks - preTicks;
                if (time / 10000.0f <= 500)
                {
                    isFastSweep = true;
                    isNormalSweep = false;
                }
                else
                {
                    isNormalSweep = true;
                    isFastSweep = false;
                }
            }
        }

        // 布局itembutton
        layoutItembuttons();

        // 据中心距离的远近来缩放itemButton
        for (int i = 0; i < transform.childCount; ++i)
        {
            var d = Mathf.Abs(Vector3.Distance(HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.GetChild(i).position), HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.position)));
            var dd = MiddleBoxImage.rectTransform.rect.width * 0.9f;
            if (d < dd)
                transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = MiddleBoxImage.rectTransform.rect.size * (1 - 0.2f * d / dd);
            else
                transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = MiddleBoxImage.rectTransform.rect.size * 0.8f;
        }

        // 触碰开始时可以滑动items
        if (isStart && Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Moved)
        {
            canClickItem = false;
            var screenpos = HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.position);
            var wpos = HomeViewController.Instance.ARCamera.ScreenToWorldPoint(new Vector3(movedPos.x, movedPos.y, screenpos.z));

            if (transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D.x <= 10 && transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition3D.x >= -10)
            {
                transform.GetChild(0).position = new Vector3(wpos.x - offset_x, 0, wpos.z);
            }
        }

        // 复位            // 更新药品名称
        if (isEnd && (isNormalSweep | isFastSweep))
        {
            canClickItem = false;
            if (index == -1)
            {
                for (int i = 0; i < transform.childCount; ++i)
                {
                    if (transform.GetChild(i).GetComponent<ItemButtonData>().DrugName == RecognizeHandleViewController.Instance.DrugNameText.text)
                    {
                        index = i;
                        break;
                    }
                }
            }
            if (endPos.x - startPos.x > 0)
            {// 返回开头
                var d = Vector3.Distance(HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.GetChild((int)index).position), HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.position));
                if (d <= MiddleBoxImage.rectTransform.rect.width * 0.1f)
                {
                    var first = transform.GetChild(0);
                    if (index == 0)
                        md = Vector3.zero;
                    else
                        md = new Vector3(-MiddleBoxImage.rectTransform.rect.width * 0.5f - index * drugs_d - MiddleBoxImage.rectTransform.rect.width * 0.8f * (index - 0.5f), 0, 0);
                    first.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(first.GetComponent<RectTransform>().anchoredPosition3D, md, 15 * Time.deltaTime);
                    if (Mathf.Abs(d) < 0.5f)
                    {
                        first.GetComponent<RectTransform>().anchoredPosition3D = md;
                        isEnd = false;
                        md = new Vector3(0, 0, -1024);
                        RecognizeHandleViewController.Instance.DrugNameText.text = transform.GetChild((int)index).GetComponent<ItemButtonData>().DrugName;
                        index = -1;
                        canClickItem = true;
                    }
                }
                else
                {
                    if (index == 0 || index == 1)
                        d = Vector3.Distance(HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.GetChild(0).position), HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.position));
                    else
                        d = Vector3.Distance(HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.GetChild((int)index - 1).position), HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.position));
                    var first = transform.GetChild(0);
                    if (index == 0 || index == 1)
                        md = Vector3.zero;
                    else
                        md = new Vector3(-MiddleBoxImage.rectTransform.rect.width * 0.5f - (index - 1) * drugs_d - MiddleBoxImage.rectTransform.rect.width * 0.8f * (index - 1 - 0.5f), 0, 0);
                    first.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(first.GetComponent<RectTransform>().anchoredPosition3D, md, 15 * Time.deltaTime);
                    if (Mathf.Abs(d) < 0.5f)
                    {
                        first.GetComponent<RectTransform>().anchoredPosition3D = md;

                        isEnd = false;
                        md = new Vector3(0, 0, -1024);
                        RecognizeHandleViewController.Instance.DrugNameText.text = transform.GetChild((index == 0 ? 0 : index - 1)).GetComponent<ItemButtonData>().DrugName;
                        canClickItem = true;
                        index = -1;
                    }
                }
                Debug.Log(" d = " + d);

            }
            else if (endPos.x - startPos.x < 0)
            {// 向尾部前进

                var d = Vector3.Distance(HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.GetChild((int)index).position), HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.position));
                if (d <= MiddleBoxImage.rectTransform.rect.width * 0.1f)
                {
                    var first = transform.GetChild(0);
                    if (index == 0)
                        md = Vector3.zero;
                    else
                        md = new Vector3(-MiddleBoxImage.rectTransform.rect.width * 0.5f - index * drugs_d - MiddleBoxImage.rectTransform.rect.width * 0.8f * (index - 0.5f), 0, 0);
                    first.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(first.GetComponent<RectTransform>().anchoredPosition3D, md, 15 * Time.deltaTime);
                    if (Mathf.Abs(d) < 0.5f)
                    {
                        first.GetComponent<RectTransform>().anchoredPosition3D = md;

                        isEnd = false;
                        md = new Vector3(0, 0, -1024);
                        RecognizeHandleViewController.Instance.DrugNameText.text = transform.GetChild(index).GetComponent<ItemButtonData>().DrugName;
                        canClickItem = true;
                        index = -1;
                    }
                }
                else
                {
                    if (index == transform.childCount - 1)
                        d = Vector3.Distance(HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.GetChild((int)index).position), HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.position));
                    else
                        d = Vector3.Distance(HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.GetChild((int)index + 1).position), HomeViewController.Instance.ARCamera.WorldToScreenPoint(transform.position));
                    var first = transform.GetChild(0);
                    if (index == transform.childCount - 1)
                        md = new Vector3(-MiddleBoxImage.rectTransform.rect.width * 0.5f - index * drugs_d - MiddleBoxImage.rectTransform.rect.width * 0.8f * (index - 0.5f), 0, 0);
                    else
                        md = new Vector3(-MiddleBoxImage.rectTransform.rect.width * 0.5f - (index + 1) * drugs_d - MiddleBoxImage.rectTransform.rect.width * 0.8f * (index + 0.5f), 0, 0);
                    first.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(first.GetComponent<RectTransform>().anchoredPosition3D, md, 15 * Time.deltaTime);
                    if (Mathf.Abs(d) < 0.5f)
                    {
                        first.GetComponent<RectTransform>().anchoredPosition3D = md;

                        isEnd = false;
                        md = new Vector3(0, 0, -1024);
                        RecognizeHandleViewController.Instance.DrugNameText.text = transform.GetChild((index == transform.childCount - 1 ? index : index + 1)).GetComponent<ItemButtonData>().DrugName;
                        canClickItem = true;
                        index = -1;
                    }
                }
                Debug.Log(" d = " + d);
            }
        }
    }

    // 复原
    public void z()
    {
        isFastSweep = false;
        isNormalSweep = false;
        isStart = false;
        isEnd = false;
        isFisrtTimeMouseDown = false;
        itemCount = 0;
        canScals = false;
        startPos = Vector3.zero;
        endPos = Vector3.zero;
        movedPos = Vector3.zero;
        preTicks = 0;
        offset_x = 0;
        middle_offset_to_left = 0;
        right_offset_to_left = 0;
    }
}
