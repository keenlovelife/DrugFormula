using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderPanelController : MonoBehaviour {

    public UnityEngine.UI.Text TitleText;
    
    void Start () {
        _ui();
	}
	
	void Update () {
		
	}
    void _ui()
    {
        float height = (float)(39 / 667.0) * Display.main.systemHeight;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, height);

        float text_height = (float)(15 / 667.0) * Display.main.systemHeight,
            text_posx = (float)(24 / 375.0) * Display.main.systemWidth;
        TitleText.rectTransform.sizeDelta = new Vector2(TitleText.rectTransform.sizeDelta.x, text_height);
        TitleText.rectTransform.offsetMin = new Vector3(text_posx, TitleText.rectTransform.offsetMin.y);
        
    }
    public void Layout(System.Action action)
    {
        if (action != null)
            action();
        _ui();
        
    }
}
