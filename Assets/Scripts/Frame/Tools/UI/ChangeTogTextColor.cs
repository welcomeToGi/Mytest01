using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTogTextColor : MonoBehaviour
{
    public Color NormalColor;
    public Color HightLightColor;
    private Toggle toggle;
    private TextMeshProUGUI text;
    private void Awake()
    {
        toggle = transform.GetComponent<Toggle>();
        text = transform.GetComponentInChildren<TextMeshProUGUI>(true);
    }
    void Start()
    {
        text.color = toggle.isOn ? HightLightColor : NormalColor;
        toggle.onValueChanged.AddListener((a) => {
            text.color = a ? HightLightColor : NormalColor;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
