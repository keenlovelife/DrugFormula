using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SearchImageEvents : MonoBehaviour, IPointerEnterHandler,IPointerDownHandler,IPointerExitHandler, IPointerUpHandler {
    UnityEngine.UI.Image image;
    void Start () {
        image = transform.Find("Image").GetComponent<UnityEngine.UI.Image>();
    }
    void Update () {
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(1f, 1f, 1f, 0.8f);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        image.color = new Color(1f, 1f, 1.0f, 0.5f);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        image.color = new Color(1f, 1f, 1.0f, 0.8f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}
