using DPI.Tools;
using HighlightingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TreeItem : MonoBehaviour
{
    [HideInInspector]
    public bool Isopen = true;
    [HideInInspector]
    public int ItemLayer = 0;

    public Action<bool> ArrowEventAction;

    [HideInInspector]
    public GameObject ArrowGameObject;
    [HideInInspector]
    public GameObject[] ArrowImage;

    public TextMeshProUGUI t;
    public GameObject CheckImg;

    [HideInInspector]
    public GameObject showGameObject;

    public Action TextOnClickAction;

    public Color TextInitColor;
    private ScrollRect scrollRect;

    private GameObject IconObj = null;
    public List<GameObject> StationList;

    private void Awake()
    {
        StationList = new List<GameObject>();
        t = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponentInParent<ScrollRect>();
        //t.gameObject.AddComponent<ScrollRectEvnet>();

        Init();
    }

    public int GetChildCount()
    {
        int childCount = 0;
        foreach (Transform item in transform)
        {
            if (item.gameObject.activeSelf)
            {
                childCount++;
            }

        }
        return childCount;
    }


    public void AddButton(GameObject obj)
    {
        obj.transform.SetParent(transform, false);
        obj.GetComponent<RectTransform>().SetSiblingIndex(99);
        SetItemSizeDelta();
    }

    public void AddToggle(GameObject obj, int index)
    {
        obj.transform.SetParent(transform, false);
        obj.GetComponent<RectTransform>().SetSiblingIndex(index);
        SetItemSizeDelta();
    }
    public Vector2 SetItem(int itemLayer)
    {
        ItemLayer = itemLayer;
        for (int i = 0; i < ItemLayer; i++)
        {
            var Station = Instantiate(Resources.Load("TreeViewPrefab/Station")) as GameObject;
            Station.transform.SetParent(gameObject.transform, false);
            Station.GetComponent<RectTransform>().SetSiblingIndex(0);
            StationList.Add(Station);

        }
        return SetItemSizeDelta();
    }

    [Header("点击文字显示的物体")]
    public List<GameObject> openGameObjects;

    [Header("点击文字关闭的物体")]
    public List<GameObject> offGameObjects;

    //private bool IsIcon = false;

    public void SetIcon(GameObject icon, bool isDel)
    {
        Transform tmp = null;
        foreach (Transform item in transform)
        {
            if (item.gameObject.activeSelf)
            {
                if (item.GetComponent<Text>() != null)
                {
                    break;
                }
                tmp = item;
            }
        }
        if (tmp != null && tmp.name != "Arrow")
        {
            if (isDel)
            {

                if (IconObj != null)
                {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                    DestroyImmediate(IconObj);
                }
                IconObj = null;
            }
            else
            {
                if (tmp.childCount == 0)
                {
                    IconObj = Instantiate(icon) as GameObject;
                    IconObj.transform.SetParent(tmp, false);
                    //Debug.Log(tmp.name);
                    var Station = Instantiate(Resources.Load("TreeViewPrefab/Station")) as GameObject;
                    Station.transform.SetParent(gameObject.transform, false);
                    Station.GetComponent<RectTransform>().SetSiblingIndex(0);
                }
            }
        }
    }

    public bool isEvent = true;


    public void TextOnClick()
    {
        if (isEvent == false) return;

        var treeView = FindObjectOfType<CreateTreeView>().TreeView;

        if (treeView == null)
            return;
        if (treeView.isTextClick == false) return;
        treeView.SetSelectTreeItem();

        foreach (var item in treeView.TreeViewList)
        {
            item.SetSelectTreeItem();
        }
        treeView.SelectTreeItem = this;

        t.color = treeView.TextColor;
        //t.GetComponentInChildren<Image>(true).gameObject.SetActive(true);
        if(CheckImg!=null) CheckImg.SetActive(true);
        t.fontStyle = FontStyles.Bold;


        foreach (var item in openGameObjects)
        {
            if (item != null)
                item.SetActive(true);
        }
        foreach (var item in offGameObjects)
        {
            if (item != null)
                item.SetActive(false);
        }
        if(TextOnClickAction != null)
        TextOnClickAction();

    }

    public void SetItemText(string content)
    {
        t.text = content;

        SetItemSizeDelta();
    }
    public bool isInitopenGameObjectSetActive = true;
    public void Init()
    {
        TreeView treeView = gameObject.GetComponentInParent<TreeView>();

        if (treeView != null)
        {
            t.color = treeView.TextInitColor;
            //t.GetComponentInChildren<Image>(true).gameObject.SetActive(false);
            if(CheckImg!=null)CheckImg.SetActive(false);
            t.fontStyle = FontStyles.Normal;
        }

        if (isInitopenGameObjectSetActive && openGameObjects != null)
        {
            foreach (var item in openGameObjects)
            {
                if (item != null)
                    item.SetActive(false);
            }
        }
        if (offGameObjects != null)
        {
            foreach (var item in offGameObjects)
            {
                if (item != null)
                    item.SetActive(true);
            }
        }
        if (isInitopenGameObjectSetActive == false)
        {
            treeView.SelectTreeItem = this;
        }
        isInitopenGameObjectSetActive = true;



    }
    public Vector2 SetItemSizeDelta()
    {
        Vector2 tmpSizeDelta = Vector2.zero;
        foreach (Transform item in transform)
        {
            if (item.gameObject.activeSelf)
            {
                tmpSizeDelta += item.GetComponent<RectTransform>().sizeDelta;
            }
        }
        tmpSizeDelta.x += 10;
        gameObject.GetComponent<RectTransform>().sizeDelta = tmpSizeDelta;
        return tmpSizeDelta;
    }

    public void ArrowEvent()
    {
        ArrowImage[0].gameObject.SetActive(!Isopen);
        ArrowImage[1].gameObject.SetActive(Isopen);
        Isopen = !Isopen;
        ArrowEventAction(Isopen);
    }
    public void OpenArrow()
    {
        if (!ArrowGameObject.activeInHierarchy)
        {
            ArrowGameObject.SetActive(true);
            ArrowImage[0].gameObject.SetActive(true);
            SetItemSizeDelta();
            StationList.Add(ArrowGameObject);
        }
    }

}
