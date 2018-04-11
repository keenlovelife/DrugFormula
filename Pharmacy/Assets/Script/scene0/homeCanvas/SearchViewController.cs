using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class SearchViewController : MonoBehaviour
{
    public static SearchViewController Instance;
    private void Awake()
    {
        Instance = this;
    }

    public GameObject Panel, ScorllPanel, ContentPanel, MaskPanel;
    public Button BackButton, SearchButton, MoreButton;
    public UnityEngine.UI.Image SearchImage, AnimationImage;
    public InputField InputField;
    public Text InputFieldText, HelloText;

    Classes ClassesObj;
    public bool IsItemClicked = false;
    public bool IsFormScan = false;
    GameObject drugInfoCanvas;

    private void OnEnable()
    {
        canShow = true;
        canHide = false;
    }
    private void OnDisable()
    {
        
    }
    void Start()
    {
        drugInfoCanvas = Helper.DrugInfocanvas;

        _ui();
        _ui_events();
    }

    bool canShow = false, canHide = false;
    void Update()
    {

        if (!drugInfoCanvas.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!canHide && !canShow)
            {

                _back();
            }
        }

        if (InputField.isFocused)
        {
            if (!ok)
            {
                ok = true;
            }
        }
        if (InputField.text == string.Empty)
            if (AnimationImage.gameObject.activeSelf)
                AnimationImage.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (canHide)
        {
            //Panel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(Panel.GetComponent<RectTransform>().anchoredPosition3D,
            //    new Vector3(Display.main.systemWidth, 0, 0),
            //    20 * Time.fixedDeltaTime);
            //if (Panel.GetComponent<RectTransform>().anchoredPosition3D.x > Display.main.systemWidth - 1)
            //{
            //    canHide = false;
            //    gameObject.SetActive(false);
            //    if (IsFormScan)
            //    {
            //        HomeViewController.Instance.CanBack = true;
            //        if (HomeViewController.Instance.isLightClick)
            //            CameraDevice.Instance.SetFlashTorchMode(true);
            //    }
            //}
            canHide = false;
            Panel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(Display.main.systemWidth, 0, 0);
            gameObject.SetActive(false);
            if (IsFormScan)
            {
                HomeViewController.Instance.CanBack = true;
                if (HomeViewController.Instance.isLightClick)
                    CameraDevice.Instance.SetFlashTorchMode(true);
            }
        }
        if (canShow)
        {
            //Panel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(Panel.GetComponent<RectTransform>().anchoredPosition3D,
            //    new Vector3(0, 0, 0),
            //    20 * Time.fixedDeltaTime);
            //if (Panel.GetComponent<RectTransform>().anchoredPosition3D.x < 1)
            //{
            //    canShow = false;
            //    Panel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            //}
            canShow = false;
            Panel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        }
    }
    void _ui()
    {
        // 整个界面Panel
        float panel_posx = Display.main.systemWidth,
            panel_width = Display.main.systemWidth,
            panel_height = Display.main.systemHeight;
        Panel.GetComponent<RectTransform>().sizeDelta = new Vector2(panel_width, panel_height);
        Panel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(panel_posx, 0, 0);

        // backButton
        float back_button_posx = (float)(15 / 375.0) * Display.main.systemWidth,
            back_button_posy = -(float)(36.5 / 667.0) * Display.main.systemHeight,
            back_button_width = (float)(31.5 / 375.0) * Display.main.systemWidth;
        BackButton.GetComponent<RectTransform>().sizeDelta = new Vector2(back_button_width, back_button_width);
        BackButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(back_button_posx, back_button_posy, 0);
        // helloText
        float hello_text_posy = -(float)(208.5 / 667.0) * Display.main.systemHeight,
            hello_text_width = (float)(97 / 375.0) * Display.main.systemWidth,
            hello_text_height = (float)(13 / 97.0) * hello_text_width;
        HelloText.rectTransform.sizeDelta = new Vector2(hello_text_width, hello_text_height);
        HelloText.rectTransform.anchoredPosition3D = new Vector3(0, hello_text_posy, 0);
        // 输入框
        float input_field_posx = -(float)(24 / 375.0) * Display.main.systemWidth,
            input_field_posy = -(float)(37.5 / 667.0) * Display.main.systemHeight,
            input_filed_width = (float)(294 / 375.0) * Display.main.systemWidth,
            input_filed_height = (float)(30 / 667.0) * Display.main.systemHeight;
        InputField.GetComponent<RectTransform>().sizeDelta = new Vector2(input_filed_width, input_filed_height);
        InputField.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(input_field_posx, input_field_posy, 0);
        // searchButton、image
        float search_button_width = (float)(36 / 375.0) * Display.main.systemWidth,
            search_button_height = (float)(34 / 36.0) * search_button_width,
            search_image_posx = -(float)(6 / 375.0) * Display.main.systemWidth,
            search_image_width = (float)(19.77 / 375.0) * Display.main.systemWidth,
            search_image_height = (float)(15.471 / 19.77) * search_image_width;
        SearchButton.GetComponent<RectTransform>().sizeDelta = new Vector2(search_button_width, search_button_height);
        SearchImage.rectTransform.anchoredPosition3D = new Vector3(search_image_posx, 0, 0);
        SearchImage.rectTransform.sizeDelta = new Vector2(search_image_width, search_image_height);
        // 输入框文本
        float inputfiled_text_posx = (float)(20 / 375.0) * Display.main.systemWidth,
            inputfiled_text_height = (float)(16 / 667.0) * Display.main.systemHeight;
        InputFieldText.rectTransform.anchoredPosition3D = new Vector3(inputfiled_text_posx, 0, 0);
        InputFieldText.rectTransform.sizeDelta = new Vector2(InputField.GetComponent<RectTransform>().rect.width - inputfiled_text_posx - SearchButton.GetComponent<RectTransform>().rect.width, inputfiled_text_height);

        // scorllPanel
        float scorll_panel_posy = -(float)(80 / 667.0) * Display.main.systemHeight,
            scorll_panel_height = Display.main.systemHeight + scorll_panel_posy;
        ScorllPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(Display.main.systemWidth, scorll_panel_height);
        ScorllPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, scorll_panel_posy, 0);

        // moreButton
        var more_button_height = (float)(44.5 / 667.0) * Display.main.systemHeight;
        var more_button_width = (float)((375.0 - 25.5 - 24.5) / 375.0) * Display.main.systemWidth;
        var more_button_text_height = (float)(12.5 / 667.0) * Display.main.systemHeight;
        MoreButton.GetComponent<RectTransform>().sizeDelta = new Vector2(more_button_width, more_button_height);
        MoreButton.transform.GetChild(0).GetComponent<Text>().rectTransform.sizeDelta = new Vector2(MoreButton.transform.GetChild(0).GetComponent<Text>().rectTransform.sizeDelta.x, more_button_text_height);
        // maskPanel
        var mask_panel_bottom = (float)(65.5 / 667.0) * Display.main.systemHeight;
        var mask_panel_height = (float)(Display.main.systemHeight - mask_panel_bottom - Mathf.Abs(ScorllPanel.GetComponent<RectTransform>().anchoredPosition3D.y));
        MaskPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(Display.main.systemWidth, mask_panel_height);
        // animation image
        var animation_image_posy = -(float)(Display.main.systemHeight / 4.0);
        if (animation_image_posy > InputField.GetComponent<RectTransform>().anchoredPosition3D.y - InputField.GetComponent<RectTransform>().rect.height)
            animation_image_posy = InputField.GetComponent<RectTransform>().anchoredPosition3D.y - InputField.GetComponent<RectTransform>().rect.height;
        //var animation_image_width = (float)(101 / 375.0) * Display.main.systemWidth;
        //var animation_image_height = (float)(107 / 101.0) * animation_image_width;
        //AnimationImage.rectTransform.sizeDelta = new Vector2(animation_image_width, animation_image_height);
        AnimationImage.rectTransform.anchoredPosition3D = new Vector3(0, animation_image_posy, 0);
    }
    void _ui_events()
    {
        InputField.keyboardType = TouchScreenKeyboardType.NamePhonePad;
        ScorllPanel.SetActive(false);
        AnimationImage.gameObject.SetActive(false);
        BackButton.onClick.AddListener(_back);
        InputField.onEndEdit.AddListener((str) => { searchHandle_plus(str); });
        SearchButton.onClick.AddListener(_searchButtonClick);
        MoreButton.onClick.AddListener(() => {
            MoreButton.gameObject.SetActive(false);
            MaskPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(Display.main.systemWidth, Display.main.systemHeight - Mathf.Abs(ScorllPanel.GetComponent<RectTransform>().anchoredPosition3D.y));
        });
        ScorllPanel.GetComponent<ScrollRect>().onValueChanged.AddListener((v2) => {
            if (MoreButton.gameObject.activeSelf && v2.y < 1.0f)
            {
                ScorllPanel.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
            }
        });
    }
    void _back()
    {
        InputField.text = string.Empty;
        Debug.Log(" 点击了返回按钮。");
        if (IsFormScan)
        {
            Destroy(HomeViewController.Instance.GetComponent<DataHelper>());
        }
        else
        {
            Destroy(AppHomeViewController.Instance.GetComponent<DataHelper>());
        }
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < ContentPanel.transform.childCount; ++i)
            list.Add(ContentPanel.transform.GetChild(i).gameObject);
        foreach (var obj in list)
            DestroyImmediate(obj, true);
        //Destroy(ContentPanel.transform.GetChild(i).gameObject);
        StopAllCoroutines();
        DataHelper.Instance.StopAllCoroutines();
        tabfun.helper.Coroutine.Instance.StopAllCoroutines();
        ScorllPanel.SetActive(false);
        AssetBundle.UnloadAllAssetBundles(true);
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        if (!canHide && !canShow)
            canHide = true;

    }
    void _searchButtonClick()
    {
        Debug.Log(" 点击了搜索按钮!");
        if (InputField.text == string.Empty)
        {
            if (!InputField.isFocused)
                InputField.ActivateInputField();
            else if (!ok)
            {
                searchHandle_plus(InputField.text);
            }

        }
    }
    public void Clear()
    {
        InputField.text = string.Empty;
    }
    public bool ok = true;
    void searchHandle_plus(string _string)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < ContentPanel.transform.childCount; ++i)
            list.Add(ContentPanel.transform.GetChild(i).gameObject);
        foreach (var obj in list)
            DestroyImmediate(obj, true);
            //Destroy(ContentPanel.transform.GetChild(i).gameObject);
        StopAllCoroutines();
        DataHelper.Instance.StopAllCoroutines();
        tabfun.helper.Coroutine.Instance.StopAllCoroutines();
        ScorllPanel.SetActive(false);
        AssetBundle.UnloadAllAssetBundles(true);
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        if (_string == string.Empty)
        {
            HelloText.text = "请输入搜索内容";
            HelloText.gameObject.SetActive(true);
            return;
        }
        AnimationImage.gameObject.SetActive(true);
        StartCoroutine(_searchHandle_plus(_string));
    }
    IEnumerator _searchHandle_plus(string _string)
    {
        yield return new WaitForEndOfFrame();
        if (ok)
        {
            MoreButton.gameObject.SetActive(true);
            HelloText.gameObject.SetActive(false);
            AnimationImage.gameObject.SetActive(true);
            // maskPanel
            var mask_panel_bottom = (float)(65.5 / 667.0) * Display.main.systemHeight;
            var mask_panel_height = (float)(Display.main.systemHeight - mask_panel_bottom - Mathf.Abs(ScorllPanel.GetComponent<RectTransform>().anchoredPosition3D.y));
            MaskPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(Display.main.systemWidth, mask_panel_height);
            // scorll panel
            ScorllPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(Display.main.systemWidth, ScorllPanel.GetComponent<RectTransform>().anchoredPosition3D.y, 0);
            try
            {
                DataHelper.Instance.Query_Plus(_string);
            }
            catch
            {
                Debug.Log(" 程序有异常！");
            }
        }
    }
    public void FilterMatchResults()
    {// 对匹配结果进行过滤
     //StartCoroutine(filterMatchResults());

        MainJsonData.MatchResultList.Sort((x, y) => {
            if (x.LengthWeight > y.LengthWeight)
                return -1;
            else if (x.LengthWeight == y.LengthWeight)
            {
                if (x.IndexWeight < y.IndexWeight)
                    return -1;
                else if (x.IndexWeight > y.IndexWeight)
                    return 1;
                else
                    return 0;
            }
            else
                return 1;
        });
        if (MainJsonData.MatchResultList.Count > 200)
            MainJsonData.MatchResultList.RemoveRange(199, MainJsonData.MatchResultList.Count - 200);
        foreach (var i in MainJsonData.MatchResultList)
            Debug.Log(" 匹配的长度数：" + i.LengthWeight);
        Debug.Log(" 匹配结果：" + MainJsonData.MatchResultList.Count);
    }
    IEnumerator filterMatchResults()
    {
        yield return new WaitForSeconds(0.1f);
       
    }
    public void UpdateMatchUI(bool isUpdateClass = false)
    {// 显示匹配结果
        //StartCoroutine(updataMatchUI(isUpdateClass));
        if (isUpdateClass)
        {
            for (int i = 0; i < ContentPanel.transform.childCount; ++i)
                Destroy(ContentPanel.transform.GetChild(i).gameObject);
        }

        ContentPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(ContentPanel.GetComponent<RectTransform>().sizeDelta.x, 0);
        var classList = new List<MatchResult>();
        var drugList = new List<MatchResult>();
        var classExtractList = new List<MatchResult>();
        var drugExtracList = new List<MatchResult>();

        if (isUpdateClass)
        {
            foreach (var r in MainJsonData.MatchResultList)
                drugList.Add(r);
        }
        else
        {
            foreach (var r in MainJsonData.MatchResultList)
                foreach (var i in r.MatchItemList)
                    if (i.MResultType == MatchResult.Type.Drug)
                        drugList.Add(r);
            foreach (var r in MainJsonData.MatchResultList)
                foreach (var i in r.MatchItemList)
                    if (i.MResultType == MatchResult.Type.Class)
                        classList.Add(r);
            foreach (var r in MainJsonData.MatchResultList)
                foreach (var i in r.MatchItemList)
                    if (i.MResultType == MatchResult.Type.Extract_Class)
                        classExtractList.Add(r);
            foreach (var r in MainJsonData.MatchResultList)
                foreach (var i in r.MatchItemList)
                    if (i.MResultType == MatchResult.Type.Extract_Drug)
                        drugExtracList.Add(r);
        }
        if (classList.Count > 0)
        {
            var headerPanel = Instantiate(Resources.Load<GameObject>("headerPanel"));
            headerPanel.GetComponent<HeaderPanelController>().TitleText.text = "相关药类";
            headerPanel.transform.SetParent(ContentPanel.transform);
            foreach (var r in classList)
            {
                var itemPanel = Instantiate(Resources.Load<GameObject>("itemPanel"));
                itemPanel.GetComponent<ItemPanelController>().TitleText.text = r.Name;
                itemPanel.GetComponent<ItemPanelController>().RawTitleString = r.Name;
                itemPanel.GetComponent<ItemPanelController>().TitleText.text = itemPanel.GetComponent<ItemPanelController>().TitleText.text.Replace(InputField.text, "<color=#108AE3FF>" + InputField.text + "</color>");
                itemPanel.GetComponent<ItemPanelController>().DrugType = DrugItem.Type.Class;
                itemPanel.GetComponent<ItemPanelController>().ContentText.text = r.ContentText;
                itemPanel.transform.SetParent(ContentPanel.transform);
            }
        }
        if (drugList.Count > 0)
        {
            var headerPanel = Instantiate(Resources.Load<GameObject>("headerPanel"));
            headerPanel.GetComponent<HeaderPanelController>().TitleText.text = isUpdateClass ? "包含的药品" : "相关药品";
            headerPanel.transform.SetParent(ContentPanel.transform);

            foreach (var r in drugList)
            {
                var itemPanel = Instantiate(Resources.Load<GameObject>("itemPanel"));
                itemPanel.GetComponent<ItemPanelController>().TitleText.text = r.Name;
                itemPanel.GetComponent<ItemPanelController>().RawTitleString = r.Name;
                itemPanel.GetComponent<ItemPanelController>().TitleText.text = itemPanel.GetComponent<ItemPanelController>().TitleText.text.Replace(InputField.text, "<color=#108AE3FF>" + InputField.text + "</color>");
                itemPanel.GetComponent<ItemPanelController>().DrugType = DrugItem.Type.Drug;
                itemPanel.GetComponent<ItemPanelController>().ContentText.text = r.ContentText;
                itemPanel.transform.SetParent(ContentPanel.transform);
            }
        }
        int max = 70;
        if (classExtractList.Count > 0)
        {
            var headerPanel = Instantiate(Resources.Load<GameObject>("headerPanel"));
            headerPanel.GetComponent<HeaderPanelController>().TitleText.text = "药类信息快照";
            headerPanel.transform.SetParent(ContentPanel.transform);
            foreach (var r in classExtractList)
            {
                var itemPanel = Instantiate(Resources.Load<GameObject>("itemPanel"));
                itemPanel.GetComponent<ItemPanelController>().TitleText.text = r.Name;
                itemPanel.GetComponent<ItemPanelController>().RawTitleString = r.Name;
                itemPanel.GetComponent<ItemPanelController>().DrugType = DrugItem.Type.Class;
                var contentText = itemPanel.GetComponent<ItemPanelController>().ContentText;
                contentText.text = r.ContentText;
                var index = contentText.text.IndexOf(InputField.text);
                if (index > max / 2)
                {
                    if (contentText.text.Length > max)
                    {
                        var pre_len = index;
                        if (pre_len > max / 2)
                            pre_len -= max / 2;
                        if (contentText.text.Length <= pre_len + max)
                            contentText.text = "..." + contentText.text.Substring(pre_len, contentText.text.Length - pre_len) + "...";
                        else
                            contentText.text = "..." + contentText.text.Substring(pre_len, max) + "...";
                    }
                }
                itemPanel.GetComponent<ItemPanelController>().ContentText.text = itemPanel.GetComponent<ItemPanelController>().ContentText.text.Replace(InputField.text, "<color=#108AE3FF>" + InputField.text + "</color>");
                itemPanel.transform.SetParent(ContentPanel.transform);
            }
        }
        if (drugExtracList.Count > 0)
        {
            var headerPanel = Instantiate(Resources.Load<GameObject>("headerPanel"));
            headerPanel.GetComponent<HeaderPanelController>().TitleText.text = "药品信息快照";
            headerPanel.transform.SetParent(ContentPanel.transform);
            foreach (var r in drugExtracList)
            {
                var itemPanel = Instantiate(Resources.Load<GameObject>("itemPanel"));
                itemPanel.GetComponent<ItemPanelController>().TitleText.text = r.Name;
                itemPanel.GetComponent<ItemPanelController>().RawTitleString = r.Name;
                itemPanel.GetComponent<ItemPanelController>().DrugType = DrugItem.Type.Drug;
                var contentText = itemPanel.GetComponent<ItemPanelController>().ContentText;
                contentText.text = r.ContentText;
                var index = contentText.text.IndexOf(InputField.text);
                if (index > max / 2)
                {
                    if (contentText.text.Length > max)
                    {
                        var pre_len = index;
                        if (pre_len > max / 2)
                            pre_len -= max / 2;
                        if (contentText.text.Length <= pre_len + max)
                            contentText.text = "..." + contentText.text.Substring(pre_len, contentText.text.Length - pre_len) + "...";
                        else
                            contentText.text = "..." + contentText.text.Substring(pre_len, max) + "...";
                    }
                }
                itemPanel.GetComponent<ItemPanelController>().ContentText.text = itemPanel.GetComponent<ItemPanelController>().ContentText.text.Replace(InputField.text, "<color=#108AE3FF>" + InputField.text + "</color>");
                itemPanel.transform.SetParent(ContentPanel.transform);
            }
        }
        StartCoroutine(rePosScorllPanel());
        if(ContentPanel.transform.childCount == 0)
        {
            HelloText.text = "无搜索结果！";
            HelloText.gameObject.SetActive(true);
        }
    }
    IEnumerator updataMatchUI(bool isUpdateClass = false)
    {
        yield return new WaitForSeconds(0.1f);
        
    }
    IEnumerator rePosScorllPanel()
    {
        yield return new WaitForEndOfFrame();

        if (ContentPanel.transform.childCount > 0)
            ScorllPanel.SetActive(true);
        ScorllPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, ScorllPanel.GetComponent<RectTransform>().anchoredPosition3D.y, 0);
        AnimationImage.gameObject.SetActive(false);
    }
}