using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemButtonTextController : MonoBehaviour {

    void Start () {
	}

    void Update()
    {
        if (!(transform.parent.GetComponent<Button>().image.sprite.name == "UISprite"))
        {
            gameObject.SetActive(false);
        }
    }
}
