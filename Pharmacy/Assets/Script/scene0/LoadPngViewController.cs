using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadPngViewController : MonoBehaviour {

    private void OnEnable()
    {
        canAnimation = true;
    }
    private void OnDisable()
    {
        canAnimation = false;
    }
    bool canAnimation = false;
    void Start () {
        var w = (float)(67 / 750.0) * Display.main.systemWidth;
        var h = (float)(71 / 67.0) * w;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
	}
    void Update () {
		if(canAnimation)
        {
            transform.Rotate(new Vector3(0, 0, -200 * Time.deltaTime));
        }
	}
}
