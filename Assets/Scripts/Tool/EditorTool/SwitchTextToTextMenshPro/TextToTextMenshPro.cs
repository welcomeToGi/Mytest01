/****************************************************
    文件：ScriptsName.cs
    作者：XXX
    日期：#20230329#
    功能：XXX
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class TextToTextMenshPro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        TextToTMp();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void TextToTMp()
    {
        var mytext = GetComponent<Text>();
        if (mytext != null)
        {
            string t_text = mytext.text;
            DestroyImmediate(mytext);
            var newText = GetComponent<TextMeshProUGUI>();
            if (newText==null)
                newText=gameObject.AddComponent<TextMeshProUGUI>();
            newText.text = t_text;
        }
    }
}
