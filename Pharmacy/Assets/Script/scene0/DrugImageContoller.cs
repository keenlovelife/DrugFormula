using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrugImageContoller : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}
    string pngName;
    public void StartLoadPng(string pngname)
    {
        pngName = pngname;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
        {
            var filepath = "/sdcard/" + Application.productName + "/" + pngName;
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
                        GetComponent<UnityEngine.UI.Image>().sprite = sprite;
                        gameObject.SetActive(true);
                    }
                    else
                    {
                        if (DrugInfoViewController.Instance)
                        {
                            DrugInfoViewController.Instance.LogText.gameObject.SetActive(true);
                            DrugInfoViewController.Instance.LogText.text = "加载失败:" + www.error;
                        }
                    }
                });
            }
            else
            {
                var path = "http://www.baitongshiji.com/images/" + pngName + ".png";
                Debug.Log(" PngName:" + pngName + " path:" + path);
                tabfun.helper.Coroutine.Instance.StartWWW(new WWW(path), (www) =>
                {
                    if (www.error == null)
                    {
                        var savefilepath = "/sdcard/" + Application.productName;
                        if (!System.IO.Directory.Exists(savefilepath))
                            System.IO.Directory.CreateDirectory(savefilepath);
                        savefilepath += "/" + pngName;
                        System.IO.File.WriteAllBytes(savefilepath, www.bytes);

                        var sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(.5f, .5f));
                        sprite.name = www.url.Substring(www.url.LastIndexOf('/') + 1);
                        if (!sprite.name.EndsWith(".png"))
                            sprite.name += ".png";
                        GetComponent<UnityEngine.UI.Image>().sprite = sprite;
                        gameObject.SetActive(true);
                    }
                    else
                    {
                        if (DrugInfoViewController.Instance)
                        {
                            DrugInfoViewController.Instance.LogText.gameObject.SetActive(true);
                            DrugInfoViewController.Instance.LogText.text = "加载失败:" + www.error;
                        }
                    }
                });
            }
        }else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var path = "http://www.baitongshiji.com/images/" + pngName + ".png";
            Debug.Log(" PngName:" + pngName + " path:" + path);
            tabfun.helper.Coroutine.Instance.StartWWW(new WWW(path), (www) =>
            {
                if (www.error == null)
                {
                    //var savefilepath = "/sdcard/" + Application.productName;
                    //if (!System.IO.Directory.Exists(savefilepath))
                    //    System.IO.Directory.CreateDirectory(savefilepath);
                    //savefilepath += "/" + pngName;
                    //if (Application.platform != RuntimePlatform.IPhonePlayer)
                    //    System.IO.File.WriteAllBytes(savefilepath, www.bytes);
                    var sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(.5f, .5f));
                    sprite.name = www.url.Substring(www.url.LastIndexOf('/') + 1);
                    if (!sprite.name.EndsWith(".png"))
                        sprite.name += ".png";
                    GetComponent<UnityEngine.UI.Image>().sprite = sprite;
                    gameObject.SetActive(true);
                }
                else
                {
                    if (DrugInfoViewController.Instance)
                    {
                        DrugInfoViewController.Instance.LogText.gameObject.SetActive(true);
                        DrugInfoViewController.Instance.LogText.text = "加载失败:" + www.error;
                    }
                }
            });
        }
    }
}
