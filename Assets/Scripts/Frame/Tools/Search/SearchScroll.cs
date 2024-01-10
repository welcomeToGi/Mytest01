using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchScroll : MonoBehaviour
{
    public InputField InputField;
    public Button BtnSure;
    public Button BtnClear;
    public ScrollRect ScrollRect;

    private SearchItem[] mSearchItems;
    private Text[] mTextArr;
    private List<Transform> mItemList;
    private Transform mCurItemParent;
    private Transform mContent;
    private void Awake()
    {
        mContent = ScrollRect.transform.GetComponent<ScrollRect>().content;
        mTextArr = transform.GetComponentsInChildren<Text>(true);
        mSearchItems = mContent.GetComponentsInChildren<SearchItem>();
        mItemList = new List<Transform>();
        for(int i = 0; i < mContent.childCount; i++)
        {
            mItemList.Add(mContent.GetChild(i));
        }

    }
    void Start()
    {
        Debug.Log("mtextArr.length = " + mTextArr.Length);
        BtnSure.onClick.AddListener(OnClickSearchBtn);
        BtnClear.onClick.AddListener(OnClickClearBtn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnClickSearchBtn()
    {
        if(InputField.text != null && InputField.text  != "")
        {
            foreach(var item in mSearchItems)
            {
                bool active = item.Info.Contains(InputField.text);
                item.gameObject.SetActive(active); 
            }
        }
    }

    private void OnClickClearBtn()
    {
        InputField.text = "";
        foreach(var item in mSearchItems)
        {
            item.gameObject.SetActive(true);
        }
    }
}
