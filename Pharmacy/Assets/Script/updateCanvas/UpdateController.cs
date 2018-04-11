using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateController : MonoBehaviour {

    public GameObject BgPanel;
    public GameObject BombPanel;
    public UnityEngine.UI.Text TitleText, ContentText;
    public UnityEngine.UI.Button UpdateButton;

    void Start () {
        _ui();
	}
	
	void Update () {
		if(ContentText.preferredWidth > BombPanel.GetComponent<RectTransform>().rect.width-10)
        {
            Destroy(ContentText.gameObject.GetComponent<UnityEngine.UI.ContentSizeFitter>());
            ContentText.rectTransform.sizeDelta = new Vector2(BombPanel.GetComponent<RectTransform>().rect.width - 10, ContentText.rectTransform.rect.height);
        }
	}
    void _ui()
    {
        // bombPanel
        float bombPanel_width = (float)(300 / 375.0) * Display.main.systemWidth,
            bombPanel_height = (float)(200.0/300)*bombPanel_width;
        BombPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(bombPanel_width, bombPanel_height);
        // title_text
        float title_text_posy = -(float)(20 / 667.0) * Display.main.systemHeight,
            title_text_width = (float)(240 / 375.0) * Display.main.systemWidth,
            titile_text_height = (float)(25 / 667.0) * Display.main.systemHeight;
        TitleText.rectTransform.sizeDelta = new Vector2(title_text_width, titile_text_height);
        TitleText.rectTransform.anchoredPosition3D = new Vector3(0, title_text_posy, 0);
        // content_text
        float content_text_width = (float)(280 / 375.0) * Display.main.systemWidth,
            content_text_height = (float)(80 / 667.0) * Display.main.systemHeight;
        int fontsize = (int)((18 / 375.0) * Display.main.systemWidth);
        ContentText.fontSize = fontsize;
        ContentText.rectTransform.sizeDelta = new Vector2(ContentText.preferredWidth, content_text_height);
        // update_button
        float update_button_width = (float)(240 / 375.0) * Display.main.systemWidth,
            update_button_height = (float)(35 / 667.0) * Display.main.systemHeight;
        UpdateButton.GetComponent<RectTransform>().sizeDelta = new Vector2(update_button_width, update_button_height);
        UpdateButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -title_text_posy, 0);

        // updata_button_text
        UpdateButton.transform.Find("Text").GetComponent<RectTransform>().sizeDelta = new Vector2(update_button_width - 10, update_button_height/2.0f);

    }
}
