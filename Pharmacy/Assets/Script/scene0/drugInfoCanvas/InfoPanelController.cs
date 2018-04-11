using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    public GameObject BoxPanel, InfoDataPanel, K1Panel, K2Panel;
    public Image DrugBoxImage;

    bool isSeceted1, isSeceted2;
    SpriteState ss, ss2;
    private void OnEnable()
    {

        ui_layout();

        // 药品图像适配到相应位置
        var dbw = (float)((375 - (112 - 25) * 2) / 375.0) * Display.main.systemWidth;
        var to_top = (float)((86 - 25) / 667.0) * Display.main.systemHeight;
        BoxPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(dbw, dbw);
        BoxPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -to_top - dbw / 2.0f, 0);
        DrugBoxImage.gameObject.GetComponent<MainStruetureController>().SetPngCount(DrugBoxImage.gameObject.GetComponent<MainStruetureController>().Num, false);

        // 按钮设置和事件
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button1.image.sprite = Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().KnowledgePoint_1_nor;
        var ss = new SpriteState();
        ss.pressedSprite = DrugInfoViewController.Instance.KnowledgePoint_1_seceted;
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button1.spriteState = ss;
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button2.image.sprite = Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().KnowledgePoint_2_nor;
        var ss2 = new SpriteState();
        ss2.pressedSprite = Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().KnowledgePoint_2_seceted;
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button2.spriteState = ss2;
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().BackButton.onClick.RemoveAllListeners();
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().BackButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().BoxPanel.transform.rotation = Quaternion.identity;
        });
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button1.onClick.RemoveAllListeners();
        UnityEngine.Events.UnityAction b1 = () =>
        {
            if (isSeceted1)
                return;
            Debug.Log("InfoPanelController 点击了知识1按钮");
            isSeceted1 = true;
            isSeceted2 = false;

            // 显示知识点1
            Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button1.image.sprite = Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().KnowledgePoint_1_seceted;
            Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button2.image.sprite = Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().KnowledgePoint_2_nor;
            K1Panel.SetActive(true);
            K2Panel.SetActive(false);
            K1Panel.GetComponent<RectTransform>().sizeDelta = new Vector2(InfoDataPanel.GetComponent<RectTransform>().sizeDelta.x, height_all_1);
            K1Panel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            GetComponent<ScrollRect>().content = K1Panel.GetComponent<RectTransform>();
        };
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button1.onClick.AddListener(b1);
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button2.onClick.RemoveAllListeners();
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button2.onClick.AddListener(() =>
        {
            if (isSeceted2)
                return;
            Debug.Log("InfoPanelController 点击了知识2按钮");
            isSeceted1 = false;
            isSeceted2 = true;

            // 显示知识点2
            Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button2.image.sprite = Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().KnowledgePoint_2_seceted;
            Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button1.image.sprite = Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().KnowledgePoint_1_nor;
            K1Panel.SetActive(false);
            K2Panel.SetActive(true);
            K2Panel.GetComponent<RectTransform>().sizeDelta = new Vector2(InfoDataPanel.GetComponent<RectTransform>().sizeDelta.x, height_all_2);
            K2Panel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            GetComponent<ScrollRect>().content = K2Panel.GetComponent<RectTransform>();

        });

        // 加载药品信息数据
        loadDrugInfoData();

        // 执行按钮1的事件
        StartCoroutine(selectedK1(() => { b1(); }));
    }
    private void OnDisable()
    {
        // 药品图像适配到相应位置
        //var dbw = (float)((375 - 50) / 375.0) * Display.main.systemWidth;
        //var to_top = (float)(170 / 667.0) * Display.main.systemHeight;
        //BoxPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(dbw, dbw);
        //BoxPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -to_top, 0);
        DrugBoxImage.gameObject.GetComponent<MainStruetureController>().SetPngCount(DrugBoxImage.gameObject.GetComponent<MainStruetureController>().Num, false);

        // 按钮设置
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button1.image.sprite = DrugInfoViewController.Instance.MainStructureSprite_nor;
        ss.pressedSprite = Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().MainStructureSprite_seceted;
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button1.spriteState = ss;
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button2.image.sprite = Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().InfoSprite_nor;
        ss2.pressedSprite = Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().InfoSprite_seceted;
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Button2.spriteState = ss2;
        isSeceted1 = false;
        isSeceted2 = false;

        // 药品信息数据清除
        unloadDrugInfoData();
        Resources.UnloadUnusedAssets();
        StopAllCoroutines();
        K1Panel.SetActive(false);
        K2Panel.SetActive(false);

        // 界面刷新
        Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Endable();
    }
    void loadDrugInfoData()
    {
        // 加载动画
        transform.Find("animationImage").gameObject.SetActive(true);

        // 加载逻辑
        if (Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().isMainStructureButtonClick)
        {
            StartCoroutine(loadInfoData(Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().ClassesObj.Info));
            Debug.Log(" #" + Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().ClassesObj.Info.ToString_1() + " \n #" + Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().ClassesObj.Info.ToString_2());
        }
        else
        {
            StartCoroutine(loadInfoData(Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Drug.Info));
            Debug.Log(" #" + Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Drug.Info.ToString_1() + " \n #" + Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().Drug.Info.ToString_2());

        }
    }
    float height_all_1 = 0, height_all_2 = 0;
    IEnumerator loadInfoData(Information info)
    {
        // 生成
        K1Panel.SetActive(true);
        K2Panel.SetActive(true);

        var k2text_SYZ = Instantiate<Text>(Resources.Load<Text>("konwText"));
        var k2text_ZYSX = Instantiate<Text>(Resources.Load<Text>("konwText"));
        var k2text_other2 = Instantiate<Text>(Resources.Load<Text>("konwText"));
        k2text_SYZ.transform.SetParent(K2Panel.transform);
        k2text_ZYSX.transform.SetParent(K2Panel.transform);
        k2text_other2.transform.SetParent(K2Panel.transform);

        var k1text_JG = Instantiate<Text>(Resources.Load<Text>("konwText"));
        var k1text_other1 = Instantiate<Text>(Resources.Load<Text>("konwText"));
        k1text_JG.transform.SetParent(K1Panel.transform);
        k1text_other1.transform.SetParent(K1Panel.transform);

        //var font = (float)(40 / 750.0) * Display.main.systemWidth;
        var font_ = (float)(28 / 750.0) * Display.main.systemWidth;
        //k1text_JG.fontSize = (int)font;
        //k1text_other1.fontSize = (int)font;
        //k2text_other2.fontSize = (int)font;
        //k2text_SYZ.fontSize = (int)font;
        //k2text_ZYSX.fontSize = (int)font;
        k1text_JG.transform.GetChild(0).GetComponent<Text>().fontSize = (int)font_;
        k1text_other1.transform.GetChild(0).GetComponent<Text>().fontSize = (int)font_;
        k2text_other2.transform.GetChild(0).GetComponent<Text>().fontSize = (int)font_;
        k2text_SYZ.transform.GetChild(0).GetComponent<Text>().fontSize = (int)font_;
        k2text_ZYSX.transform.GetChild(0).GetComponent<Text>().fontSize = (int)font_;
        var h = (float)(44 / 1334.0) * Display.main.systemHeight;
        k1text_JG.rectTransform.sizeDelta = new Vector2(k1text_JG.rectTransform.rect.width, h);
        k1text_other1.rectTransform.sizeDelta = new Vector2(k1text_other1.rectTransform.rect.width, h);
        k2text_other2.rectTransform.sizeDelta = new Vector2(k2text_other2.rectTransform.rect.width, h);
        k2text_SYZ.rectTransform.sizeDelta = new Vector2(k2text_SYZ.rectTransform.rect.width, h);
        k2text_ZYSX.rectTransform.sizeDelta = new Vector2(k2text_ZYSX.rectTransform.rect.width, h);
        // 赋值
        k1text_JG.text = "结构：";
        k1text_JG.transform.GetChild(0).GetComponent<Text>().text = "";
        int c = 1;
        foreach (var o in info.StructureList)
            k1text_JG.transform.GetChild(0).GetComponent<Text>().text += c++ + "、" + o + "\n";
        if (k1text_JG.transform.GetChild(0).GetComponent<Text>().text == string.Empty)
            k1text_JG.transform.GetChild(0).GetComponent<Text>().text = "暂无！\n";
        k1text_other1.text = "其他：";
        k1text_other1.transform.GetChild(0).GetComponent<Text>().text = info.Other1;
        c = 1;
        if (k1text_other1.transform.GetChild(0).GetComponent<Text>().text == string.Empty)
        {
            k1text_other1.transform.GetChild(0).GetComponent<Text>().text = "暂无！\n";
            DestroyImmediate(k1text_other1.gameObject, true);
        }
        k2text_SYZ.text = "适应症：";
        k2text_SYZ.transform.GetChild(0).GetComponent<Text>().text = "";
        foreach (var o in info.IndicationsList)
        {
            k2text_SYZ.transform.GetChild(0).GetComponent<Text>().text += c++ + "、" + o + "\n";
        }
        c = 1;
        if (k2text_SYZ.transform.GetChild(0).GetComponent<Text>().text == string.Empty)
            k2text_SYZ.transform.GetChild(0).GetComponent<Text>().text = "暂无！\n";
        k2text_ZYSX.text = "注意事项：";
        k2text_ZYSX.transform.GetChild(0).GetComponent<Text>().text = "";
        foreach (var o in info.PrecautionsList)
            k2text_ZYSX.transform.GetChild(0).GetComponent<Text>().text += c++ + "、" + o + "\n";
        c = 1;
        if (k2text_ZYSX.transform.GetChild(0).GetComponent<Text>().text == string.Empty)
            k2text_ZYSX.transform.GetChild(0).GetComponent<Text>().text = "暂无！\n";
        k2text_other2.text = "其他：";
        k2text_other2.transform.GetChild(0).GetComponent<Text>().text = info.Other2;
        if (k2text_other2.transform.GetChild(0).GetComponent<Text>().text == string.Empty)
        {
            k2text_other2.transform.GetChild(0).GetComponent<Text>().text = "暂无！\n";
            DestroyImmediate(k2text_other2.gameObject, true);
        }
        
        // 布局 
        var left = (float)(16 / 375.0) * Display.main.systemWidth;
        var to_top = (14 / 667.0) * Display.main.systemHeight;
        var w = InfoDataPanel.GetComponent<RectTransform>().rect.width - left * 2;

        for (int i = 0; i < K1Panel.transform.childCount; ++i)
        {
            K1Panel.transform.GetChild(i).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            K1Panel.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(w, K1Panel.transform.GetChild(i).GetComponent<RectTransform>().rect.height);
            K1Panel.transform.GetChild(i).GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -K1Panel.transform.GetChild(i).GetComponent<RectTransform>().rect.height- (float)to_top * 0.5f,0);
            K1Panel.transform.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(w, K1Panel.transform.GetChild(i).GetChild(0).GetComponent<RectTransform>().rect.height);
        }
        for (int i = 0; i < K2Panel.transform.childCount; ++i)
        {
            K2Panel.transform.GetChild(i).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            K2Panel.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(w, K2Panel.transform.GetChild(i).GetComponent<RectTransform>().rect.height);
            K2Panel.transform.GetChild(i).GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -K2Panel.transform.GetChild(i).GetComponent<RectTransform>().rect.height- (float)to_top * 0.5f, 0);
            K2Panel.transform.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(w, K2Panel.transform.GetChild(i).GetChild(0).GetComponent<RectTransform>().rect.height);
        }
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < K1Panel.transform.childCount; ++i)
        {
            float top = 0;
            if (i == 0)
            {
                top = -(float)(7 / 375.0) * Display.main.systemHeight;
            }
            else
            {
                top = (float)(K1Panel.transform.GetChild(i - 1).GetComponent<RectTransform>().anchoredPosition3D.y -
                    K1Panel.transform.GetChild(i - 1).GetComponent<RectTransform>().rect.height -
                    K1Panel.transform.GetChild(i - 1).GetChild(0).GetComponent<RectTransform>().rect.height + (K1Panel.transform.GetChild(i - 1).GetComponent<RectTransform>().rect.height - to_top - (float)to_top * 0.5f));
            }
            K1Panel.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, top, 0);
        }
        height_all_1 = -(float)(K1Panel.transform.GetChild(K1Panel.transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition3D.y -
                    K1Panel.transform.GetChild(K1Panel.transform.childCount - 1).GetComponent<RectTransform>().rect.height -
                    K1Panel.transform.GetChild(K1Panel.transform.childCount - 1).GetChild(0).GetComponent<RectTransform>().rect.height +
                    (K1Panel.transform.GetChild(K1Panel.transform.childCount - 1).GetComponent<RectTransform>().rect.height - 2 * to_top));
        for (int i = 0; i < K2Panel.transform.childCount; ++i)
        {
            float top = 0;
            if (i == 0)
            {
                top = -(float)(7 / 375.0) * Display.main.systemHeight;
            }
            else
            {
                top = (float)(K2Panel.transform.GetChild(i - 1).GetComponent<RectTransform>().anchoredPosition3D.y -
                    K2Panel.transform.GetChild(i - 1).GetComponent<RectTransform>().rect.height -
                    K2Panel.transform.GetChild(i - 1).GetChild(0).GetComponent<RectTransform>().rect.height + (K1Panel.transform.GetChild(i - 1).GetComponent<RectTransform>().rect.height - to_top - (float)to_top * 0.5f));
            }
            K2Panel.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, top, 0);
        }
        height_all_2 = -(float)(K2Panel.transform.GetChild(K2Panel.transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition3D.y -
                    K2Panel.transform.GetChild(K2Panel.transform.childCount - 1).GetComponent<RectTransform>().rect.height -
                    K2Panel.transform.GetChild(K2Panel.transform.childCount - 1).GetChild(0).GetComponent<RectTransform>().rect.height +
                    (K2Panel.transform.GetChild(K2Panel.transform.childCount - 1).GetComponent<RectTransform>().rect.height - 2 * to_top));

        K1Panel.SetActive(false);
        K2Panel.SetActive(false);

        // 隐藏加载动画
        transform.Find("animationImage").gameObject.SetActive(false);
    }

    IEnumerator selectedK1(System.Action action)
    {
        yield return new WaitForSeconds(0.1f);
        action();
    }

    void unloadDrugInfoData()
    {
        for (int i = 0; i < K1Panel.transform.childCount; ++i)
            Destroy(K1Panel.transform.GetChild(i).gameObject);
        K1Panel.transform.DetachChildren();
        for (int i = 0; i < K2Panel.transform.childCount; ++i)
            Destroy(K2Panel.transform.GetChild(i).gameObject);
        K2Panel.transform.DetachChildren();
        height_all_1 = 0;
        height_all_2 = 0;
    }

    void Start()
    {
        ui_layout();
        ss = new SpriteState();
        ss2 = new SpriteState();
        Debug.Log("InfoPanelController 启动");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            Helper.DrugInfocanvas.GetComponent<DrugInfoViewController>().BoxPanel.transform.rotation = Quaternion.identity;
        }
    }
    private void OnApplicationQuit()
    {
        List<GameObject> l = new List<GameObject>();
        for (int i = 0; i < K1Panel.transform.childCount; ++i)
            l.Add(K1Panel.transform.GetChild(i).gameObject);
        K1Panel.transform.DetachChildren();
        for (int i = 0; i < K2Panel.transform.childCount; ++i)
            l.Add(K2Panel.transform.GetChild(i).gameObject);
        K2Panel.transform.DetachChildren();
        foreach (var o in l)
            DestroyImmediate(o, true);
    }
    int _i = 0;
    void ui_layout()
    {
        if (_i > 0)
            return;
        else
            ++_i;
        var info_list_to_buttom = (float)(125 / 776.0) * Display.main.systemHeight;
        var info_list_w = (float)((375 - 50) / 375.0) * Display.main.systemWidth;
        var info_list_h = (float)(578 / 652.0) * info_list_w;

        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(info_list_w, info_list_h);
        gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, info_list_to_buttom, 0);
    }
}
