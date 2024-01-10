using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchItem : MonoBehaviour
{
    //[HideInInspector]
    public string Info;
    private Text[] mTexts;
    private void Awake()
    {
        mTexts = transform.GetComponentsInChildren<Text>();
    }
    void Start()
    {
        for(int i = 0; i < mTexts.Length; i++)
        {
            Info += mTexts[i].text + "_";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
