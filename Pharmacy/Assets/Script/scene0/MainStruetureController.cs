using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStruetureController : MonoBehaviour {

    public int Num;
    bool isWait;
    // 设置png的数量
    public void SetPngCount(int count, bool isWaitPngs = true)
    {
        Num = count;
        isWait = isWaitPngs;
        if (isWaitPngs)
        {
            for (int i = 2; i < transform.childCount; ++i)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().sprite = DrugInfoViewController.Instance.ShowDrugDefaultSprite;
            }
        }
        canStopAnimation = isWaitPngs ? true : false;
        switch(Num)
        {
            case 0:
            case 1:
                {
                    transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().rect.width - 10.0f, transform.GetComponent<RectTransform>().rect.height - 10);
                    transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
                    for (int i = 3; i < transform.childCount; ++i)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                        transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().sprite = DrugInfoViewController.Instance.ShowDrugDefaultSprite;
                    }
                }
                break;
            case 2:
                {
                    var d = (float)(20 / 750.0) * Display.main.systemWidth;
                    var w = (float)(transform.GetComponent<RectTransform>().rect.width - 3 * d) / 2.0f;
                    for (int i = 2; i < 2 + 2; ++i)
                    {
                        transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(isWaitPngs ? w : transform.GetComponent<RectTransform>().rect.width-10, isWaitPngs ? w : transform.GetComponent<RectTransform>().rect.width - 10);
                        var dd = new Vector3((isWaitPngs ? (-w / 2.0f - d / 2.0f) : 0), 0, 0);

                        if (i == 2)
                            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D = new Vector3((isWaitPngs? (-w/2.0f - d / 2.0f) : 0), 0, 0);
                        else
                            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D = new Vector3((isWaitPngs ? ( w / 2.0f + d / 2.0f) :0), 0, 0);
                    }
                }
                break;
            case 3:
                {
                    var d = (float)(20 / 750.0) * Display.main.systemWidth;
                    var w = (float)(transform.GetComponent<RectTransform>().rect.width - 3 * d) / 2.0f;
                    for (int i = 2; i < 2 + 3; ++i)
                    {
                        transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(isWaitPngs ? w : transform.GetComponent<RectTransform>().rect.width - 10, isWaitPngs ? w : transform.GetComponent<RectTransform>().rect.width - 10);
                        if (i < 2 + 2)
                            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(isWaitPngs ? (i == 2 ? -(w / 2.0f + d / 2.0f): (w / 2.0f + d / 2.0f)) : 0, isWaitPngs? (w / 2.0f + d / 2.0f):0, 0);
                        else
                            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, isWaitPngs ? -(w / 2.0f + d / 2.0f) : 0, 0);
                    }
                }
                break;
            case 4:
                {
                    var d = (float)(20 / 750.0) * Display.main.systemWidth;
                    var w = (float)(transform.GetComponent<RectTransform>().rect.width - 3 * d) / 2.0f;
                    for (int i = 2; i < 2 + 4; ++i)
                    {
                        transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(isWaitPngs ? w : transform.GetComponent<RectTransform>().rect.width - 10, isWaitPngs ? w : transform.GetComponent<RectTransform>().rect.width - 10);
                        if (i < 2 + 2)
                            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(isWaitPngs ? (i == 2 ? -(w / 2.0f + d / 2.0f): (w / 2.0f + d / 2.0f)) : 0, isWaitPngs ? (w / 2.0f + d / 2.0f): 0, 0);
                        else
                            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(isWaitPngs ? (i == 4 ? -(w / 2.0f + d / 2.0f) : (w / 2.0f + d / 2.0f)) : 0, isWaitPngs ? (-w / 2.0f - d / 2.0f ): 0, 0);
                    }
                }
                break;
            default:
                break;
        }
    }

    void Start () {
        startSize = Vector2.zero;
	}
    bool canStopAnimation = false;
    Vector2 startSize;
	void Update () {
        if (canStopAnimation && Num >= 0)
        {
            int count = 0;
            for (int i = 2; i < transform.childCount; ++i)
            {
                if (transform.GetChild(i).gameObject.activeSelf)
                    ++count;
            }
            if (count == Num)
            {
                transform.Find("animationImage").gameObject.SetActive(false);
                canStopAnimation = false;
            }
        }
        if(!isWait && Num > 1)
        {
            var d = (float)(20 / 750.0) * Display.main.systemWidth;
            var off_all = (float)((Num - 0.5) * transform.GetChild(2).GetComponent<RectTransform>().rect.width + (Num - 1) * d+ GetComponent<RectTransform>().rect.width /2.0f);
            if (transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition3D.x <= -off_all)
                transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(GetComponent<RectTransform>().rect.width / 2.0f+ transform.GetChild(2).GetComponent<RectTransform>().rect.width / 2.0f, 0, 0);
            else
                transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition3D.x - 50 * Time.deltaTime, 0, 0);
            for(int i = 3; i < 2+Num; ++i)
            {
                transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(transform.GetChild(i - 1).GetComponent<RectTransform>().anchoredPosition3D.x + transform.GetChild(2).GetComponent<RectTransform>().rect.width + d, 0, 0);
            }
        }

        if (DrugInfoViewController.Instance.canZoom)
        {
            if(startSize == Vector2.zero)
            {
                startSize = transform.GetChild(2).GetComponent<RectTransform>().sizeDelta;
            }
            var size = startSize * DrugInfoViewController.Instance.sizesize;
            for (int i = 2; i < 2 + 4; ++i)
            {
                transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = size;
            }
        }
        else
        {
            startSize = transform.GetChild(2).GetComponent<RectTransform>().sizeDelta;
        }
    }
}
