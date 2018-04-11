using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPanelController : MonoBehaviour, IPointerClickHandler {
    public UnityEngine.UI.Text TitleText, ContentText;
    public string RawTitleString;
    public DrugItem.Type DrugType;
    GameObject searchCanvas, drugInfoCanvas;
    void Start () {
        var all = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
        foreach (var o in all)
            if (o.transform.parent == null)
            {
                if (o.name == "searchCanvas")
                    searchCanvas = o;
                else if (o.name == "drugInfoCanvas")
                    drugInfoCanvas = o;
                  
            }

        StartCoroutine(_init());
        _ui();

        OnPointClick += (ed) => {

            Debug.Log(TitleText.text+" 被点击了！");
            var searchVC = searchCanvas.GetComponent<SearchViewController>();
            if(DrugType == DrugItem.Type.Class)
            {
                searchCanvas.GetComponent<SearchViewController>().IsItemClicked = false;
                searchVC.ok = false;
                searchVC.InputField.text = RawTitleString;
                _updateData();
            }else if(DrugType == DrugItem.Type.Drug)
            {
                DataHelper.Instance.Query(RawTitleString, (classes) => {
                    if(classes != null)
                    {
                        searchCanvas.GetComponent<SearchViewController>().IsItemClicked = true;
                        drugInfoCanvas.GetComponent<DrugInfoViewController>().IsSearch = true;
                        drugInfoCanvas.GetComponent<DrugInfoViewController>().ClassesObj = classes;
                        drugInfoCanvas.SetActive(true);
                    }
                }, true);
            }
        };
	}
    List<DrugItem> resultItems;
    IEnumerator _init()
    {
        if (resultItems == null)
            resultItems = new List<DrugItem>();
        else
            resultItems.Clear();

        while (DrugType == DrugItem.Type.Unknow)
            yield return new WaitForEndOfFrame();

        foreach (var file in MainJsonData.DrugFileList)
            foreach (var item in file.Drug_Class_ItemList)
            {
                if (item.ItemType == DrugItem.Type.Class)
                {
                    if (item.ItemType == DrugType && RawTitleString == item.Name)
                    {
                        resultItems.Add(item);
                        if(item.JsonData!= null && item.JsonData.IsObject)
                        {
                            var drugs = item.JsonData["包含药物"];
                            if (drugs != null && drugs.IsArray)
                            {
                                foreach (var one in drugs)
                                {
                                    var drug = file.Drug_Class_ItemList.Find((d) =>
                                    {
                                        if (d.ItemType == DrugItem.Type.Drug && d.Name == one.ToString())
                                            return true;
                                        return false;
                                    });
                                    if (drug != null)
                                        resultItems.Add(drug);
                                }
                            }
                        }
                        break;
                    }
                    else if(DrugType == DrugItem.Type.Drug)
                    {
                        if (item.JsonData != null && item.JsonData.IsObject)
                        {
                            bool isFinded = false;
                            foreach (var key in item.JsonData.Keys)
                                if (key == RawTitleString)
                                {
                                    isFinded = true;
                                    resultItems.Add(item);
                                    break;
                                }
                            if (isFinded)
                            {
                                var drugs = item.JsonData["包含药物"];
                                if (drugs != null && drugs.IsArray)
                                {
                                    foreach (var one in drugs)
                                    {
                                        var drug = file.Drug_Class_ItemList.Find((d) =>
                                        {
                                            if (d.ItemType == DrugItem.Type.Drug && d.Name == one.ToString())
                                                return true;
                                            return false;
                                        });
                                        if (drug != null)
                                            resultItems.Add(drug);
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

    }
    void _updateData()
    {
        MainJsonData.MatchResultList.Clear();
        foreach(var item in resultItems)
            if(item.ItemType == DrugItem.Type.Drug)
            {
                MatchResult mr = new MatchResult();
                mr.Name = item.Name;
                foreach (var info in item.InfoTextList)
                    mr.ContentText += info;
                MainJsonData.MatchResultList.Add(mr);
            }
        searchCanvas.GetComponent<SearchViewController>().UpdateMatchUI(true);
    }

    public event tabfun.Action_1_param<PointerEventData> OnPointClick;
    float constHeight = 0;
    void Update () {
        if(TitleText.rectTransform.rect.height + ContentText.rectTransform.rect.height + constHeight != gameObject.GetComponent<RectTransform>().rect.height)
        {
            var height = TitleText.rectTransform.rect.height + constHeight;
            if(ContentText.text != null && ContentText.text != string.Empty)
                height += ContentText.rectTransform.rect.height;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, height);
        }
	}
    void _ui()
    {
        constHeight = (float)(53.5 / 667.0) * Display.main.systemHeight;
        // title text
        float title_text_height = (float)(16 / 667.0) * Display.main.systemHeight,
            title_text_posx = (float)(24 / 375.0) * Display.main.systemWidth,
            title_text_posy = -(float)(22.5 / 667.0) * Display.main.systemHeight,
            title_text_width = (float)(transform.parent.GetComponent<RectTransform>().rect.width - title_text_posx - (float)(27/375.0)*Display.main.systemWidth);
        TitleText.rectTransform.anchoredPosition3D = new Vector3(title_text_posx, title_text_posy, 0);
        TitleText.rectTransform.sizeDelta = new Vector2(title_text_width, title_text_height);
        // content_Text
        int content_text_font = (int)((13 / 375.0) * Display.main.systemWidth);
        float content_text_posy = (float)(22.5 / 667.0) * Display.main.systemHeight;
        float content_text_width = (float)transform.GetComponent<RectTransform>().rect.width - (float)(51.5 / 375.0) * Display.main.systemWidth;
        ContentText.fontSize = content_text_font;
        ContentText.rectTransform.anchoredPosition3D = new Vector3(title_text_posx, content_text_posy, 0);
        ContentText.rectTransform.sizeDelta = new Vector2(content_text_width, ContentText.rectTransform.rect.height);
        // 底部线
        var lineWidth = (float)(327 / 375.0) * Display.main.systemWidth;
        var lineHeight = Display.main.systemHeight <= 667.0 ? 1f : (float)(1 / 667.0) * Display.main.systemHeight;
        var lineImage = transform.Find("lineImage").GetComponent<UnityEngine.UI.Image>();
        lineImage.rectTransform.sizeDelta = new Vector2(lineWidth, lineHeight);

    }
    public void Layout(System.Action action)
    {
        if (action != null)
            action();
        _ui();
        var maxHeight = (float)(51.5/667.0) * Display.main.systemHeight;
        if(ContentText.text != null && ContentText.text != string.Empty && ContentText.preferredHeight > maxHeight)
        {
            Destroy(ContentText.GetComponent<UnityEngine.UI.ContentSizeFitter>());
            ContentText.rectTransform.sizeDelta = new Vector2(ContentText.rectTransform.sizeDelta.x, maxHeight);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnPointClick != null)
            OnPointClick(eventData);
    }
}
