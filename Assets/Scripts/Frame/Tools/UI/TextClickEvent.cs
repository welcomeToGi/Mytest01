using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextClickEvent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Color NormalColor;
    public Color HighlightedColor;
    public UnityAction OnClick;
    private GameObject UnderLine;
    private void Awake()
    {
        UnderLine = transform.GetComponentInChildren<Image>(true).gameObject;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("文本进入");
        transform.GetComponent<Text>().color = HighlightedColor;
        UnderLine.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("文本退出");
        UnderLine.SetActive(false);
        transform.GetComponent<Text>().color = NormalColor; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
