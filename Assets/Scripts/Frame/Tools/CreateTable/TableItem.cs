using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableItem : MonoBehaviour
{
    public Color DarkColor;
    public Color LightColor;
    public void InitItem(string text, float width, int index)
    {
        transform.GetComponentInChildren<TextMeshProUGUI>().text = text;
        transform.GetComponent<Image>().color = index % 2 == 0 ? DarkColor : LightColor;
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, rectTransform.rect.height);

    }
}
