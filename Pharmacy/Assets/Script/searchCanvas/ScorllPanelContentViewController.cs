using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorllPanelContentViewController : MonoBehaviour {

    public GameObject Header, More;
    RectTransform rectTransform;
    void Start () {
        _ui();
        rectTransform = gameObject.GetComponent<RectTransform>();
	}
    List<GameObject> headerList;
    void Update () {

        var items_height = 0.0f;
        for (int i = 0; i < transform.childCount; ++i)
            items_height += transform.GetChild(i).GetComponent<RectTransform>().rect.height;
		if(rectTransform.rect.height != items_height)
        {
            items_height = _layout_items();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, items_height);
        }

        if(!More.activeSelf && transform.childCount > 0 && headerList != null)
        {
            if (!Header.activeSelf)
                Header.SetActive(true);
            GameObject header = null;
            foreach (var item in headerList)
                if (Mathf.Abs(item.GetComponent<RectTransform>().anchoredPosition3D.y) <= gameObject.GetComponent<RectTransform>().anchoredPosition3D.y)
                    header = item;
            if (header != null)
                Header.GetComponent<HeaderPanelController>().TitleText.text = header.GetComponent<HeaderPanelController>().TitleText.text;
            if (gameObject.GetComponent<RectTransform>().anchoredPosition3D.y <= 0)
                Header.SetActive(false);
        }
	}
    float _layout_items()
    {
        if (headerList == null)
            headerList = new List<GameObject>();
        else
            headerList.Clear();

        var items_height = 0f;
        for (int i = 0; i < transform.childCount; ++i)
        {
            var item = transform.GetChild(i);
            var item_com = item.GetComponent<HeaderPanelController>();
            if(item_com != null)
            {
                headerList.Add(item.gameObject);
                item_com.Layout(() => {
                    var item_posy = i == 0 ? 0f : transform.GetChild(i - 1).GetComponent<RectTransform>().anchoredPosition3D.y - transform.GetChild(i - 1).GetComponent<RectTransform>().rect.height;
                    item.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, item_posy, 0);
                    item.GetComponent<RectTransform>().sizeDelta = new Vector2(Display.main.systemWidth, item.GetComponent<RectTransform>().sizeDelta.y);
                });
                if(i != 0)
                {
                    var pre_item = transform.GetChild(i - 1).GetComponent<ItemPanelController>();
                    if(pre_item != null)
                    {
                        var line = pre_item.transform.Find("lineImage");
                        if (line != null)
                            line.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
               var item_com_ = item.GetComponent<ItemPanelController>();
                item_com_.Layout(() => {
                    var item_posy = i == 0 ? 0f : transform.GetChild(i - 1).GetComponent<RectTransform>().anchoredPosition3D.y - transform.GetChild(i - 1).GetComponent<RectTransform>().rect.height;
                    item.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, item_posy, 0);
                    item.GetComponent<RectTransform>().sizeDelta = new Vector2(Display.main.systemWidth, item.GetComponent<RectTransform>().sizeDelta.y);

                });
            }
            items_height += item.GetComponent<RectTransform>().rect.height;
        }
        return items_height;
    }
    void _ui()
    {
        Header.GetComponent<RectTransform>().sizeDelta = new Vector2(Display.main.systemWidth, Header.GetComponent<RectTransform>().sizeDelta.y);
        Header.SetActive(false);
    }
}
