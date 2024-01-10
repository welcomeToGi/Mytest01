using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using HighlightingSystem;
using System.IO;
using System.Linq;

public delegate void TextClickAction(string name);
/// <summary>
/// 解析完成回调
/// </summary>
/// <param name="info">解析的当前元素信息</param>
/// <param name="parent">当前元素的父物体</param>
/// <returns></returns>
public delegate ItemRoot CreateTreeItemRootCallback(ChildDatas info, TreeRoot parent);
public enum TreeViewType
{
    建筑层级,
    普通场景

}


[Serializable]
public class TreeView : MonoBehaviour
{
    public static TreeView Instance;
    public TreeItem SelectTreeItem;
    public TreeViewType treeViewType = TreeViewType.普通场景;
    public GameObject Content;
    [SerializeField]
    public Vector2 SizeDelta = Vector2.zero;


    public List<TreeView> TreeViewList = new List<TreeView>();

    [SerializeField]
    public TreeRoot treeRoot;

    public GameObject imgAddressSub;
    public GameObject NavigationRoot;


    public List<GameObject> imgAddressSubList = new List<GameObject>();

    public ItemRoot trenItemRoot;

    [Header("文字是否能点击")]
    public bool isTextClick = true;

    public TextClickAction TextClickAction;
    [Header("物体高亮颜色")]
    public Color HighlightColor;

    [SerializeField]
    [Header("文字点击颜色")]
    public Color TextColor;




    [SerializeField]
    [Header("文字初始化颜色")]
    public Color TextInitColor;
    [SerializeField]
    [Header("Icon预设")]
    public GameObject IconGameObject;
    [SerializeField]
    [Header("是否删除图标")]
    public bool isDleIcon = false;


    /// <summary>
    /// 被UI树索引的物体
    /// </summary>

    public List<GameObject> TreeIndexesGameObjectList = new List<GameObject>();

    /// <summary>
    /// 被改变透明的物体
    /// </summary>
    [HideInInspector]
    public List<GameObject> TransparentGameObjectList = new List<GameObject>();
    private List<ItemRoot> itemRootChildrens = new List<ItemRoot>();

    [Header("分层建筑")]
    [SerializeField]
    public GameObject FloorGameObject;

    [Header("分层建筑UI树预设")]
    [SerializeField]
    public GameObject FloorTreeItmePrefab;

    [Header("搜索框组件Comp")]
    [SerializeField]
    InputFieldComp inputFieldComp;
    public void Awake()
    {
        Instance = this;
        ItemRoot[] childrens = GetComponentsInChildren<ItemRoot>();
        itemRootChildrens = childrens.ToList();
		//itemRootChildrens = GetComponentsInChildren<ItemRoot>();
		foreach (ItemRoot item in itemRootChildrens)
		{
			TreeIndexesGameObjectList.Add(item.TextClickEvnetObj);
		}
		if (inputFieldComp != null)
        {
            inputFieldComp.OnClickSearchAction += OnClickSearchCallBack;
        }
    }
    public TreeRoot GetTreeRoot()
    {
        return treeRoot;
    }
    public bool FindTreeIndexesGameObject(GameObject obj)
    {
        return TreeIndexesGameObjectList.Find(g => g == obj.gameObject);
    }

    //是否正搜索状态
    bool isSearching = false;
    public bool IsSearching
    {
        get
        {
            return isSearching;
        }
        set
        {
            if (isSearching == value)
                return;
            isSearching = value;
            RecursionSetSearching(treeRoot, isSearching);
        }
    }

    void OnClickSearchCallBack(string _str)
    {
        if (treeRoot == null)
            return;
        if (string.IsNullOrEmpty(_str))
        {
            IsSearching = false;
            RecursionSetRootShow(treeRoot, true);
        }
        else
        {
            if (IsSearching)
            {
                //已经搜索 先恢复
                RecursionSetRootShow(treeRoot, true);
            }
            IsSearching = true;
            RecursionCheckIsContainStr(treeRoot, _str);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(treeRoot.GetComponent<RectTransform>());
    }

    bool RecursionCheckIsContainStr(TreeRoot _treeRoot, string _str)
    {
        if (_treeRoot == null || string.IsNullOrEmpty(_str))
            return false;
        bool _isCotain = false;
        foreach (var item in _treeRoot.itemRoots)
        {
            if (item == null || item.treeItem == null)
                continue;
            bool tempIsContain = false;
            if (item.treeItem.t.text.Contains(_str))
            {
                tempIsContain = true;
            }
            else
            {
                tempIsContain = RecursionCheckIsContainStr(item.treeRoot, _str);

            }
            item.gameObject.SetActive(tempIsContain);
            if (tempIsContain)
                _isCotain = true;
        }
        _treeRoot.gameObject.SetActive(_isCotain);
        return _isCotain;
    }

    void RecursionSetRootShow(TreeRoot _treeRoot, bool _show)
    {
        if (_treeRoot == null)
            return;
        foreach (var item in _treeRoot.itemRoots)
        {
            if (item != null)
            {
                RecursionSetRootShow(item.treeRoot, _show);
                item.gameObject.SetActive(_show);
            }
        }
        _treeRoot.gameObject.SetActive(_show);
    }

    void RecursionSetSearching(TreeRoot _treeRoot, bool _isSearching)
    {
        if (_treeRoot == null)
            return;
        foreach (var item in _treeRoot.itemRoots)
        {
            if (item != null)
            {
                RecursionSetSearching(item.treeRoot, _isSearching);
                item.isSearching = _isSearching;
            }
        }
    }

    public void InitMaterialTransparent()
    {
        if (itemRootChildrens == null)
            return;
        foreach (ItemRoot item in itemRootChildrens)
        {
            for (int i = 0; i < item.TransparentGameObjectList.Count; i++)
            {
                if (item.TransparentGameObjectList[i] == null)
                    Debug.Log(item.name);
                if (item != null && item.TransparentGameObjectList[i] != null)
                    item.TransparentGameObjectList[i].GetComponent<SetMaterialTransparent>().isTransparent(false);
            }
        }
    }


    public void SetSelectTreeItem()
    {
        InitMaterialTransparent();
        if (this.SelectTreeItem != null)
        {
            this.SelectTreeItem.Init();
            if (this.SelectTreeItem.showGameObject != null &&
                this.SelectTreeItem.showGameObject.GetComponent<HighlightingSystem.Highlighter>() != null)
            {
                this.SelectTreeItem.showGameObject.GetComponent<HighlightingSystem.Highlighter>().constant = false;
                HighlightingSystem.Highlighter[] childHighlighters = SelectTreeItem.showGameObject.GetComponentsInChildren<HighlightingSystem.Highlighter>();
                for (int i = 0; i < childHighlighters.Length; i++)
                {
                    childHighlighters[i].constant = false;
                }
            }

        }
    }



    //设置导航
    public void SetNavigation(ItemRoot[] itemRootParents)
    {
        //设置导航
        if (NavigationRoot != null)
        {
            for (int i = 1; i < imgAddressSubList.Count; i++)
            {
                imgAddressSubList[i].SetActive(false);
            }
            List<ItemRoot> ItemRootList = new List<ItemRoot>(itemRootParents);
            ItemRootList.Reverse();
            for (int i = 0; i < ItemRootList.Count; i++)
            {
                if (imgAddressSubList.Count >= ItemRootList.Count)
                {
                    imgAddressSubList[i].SetActive(true);
                    //imgAddressSubList[i].GetComponent<NavigationItem>().itemRoot = ItemRootList[i];
                    imgAddressSubList[i].transform.Find("Text").GetComponent<Text>().text = ItemRootList[i].treeItem.t.text + "  ";
                    LayoutRebuilder.ForceRebuildLayoutImmediate(imgAddressSubList[i].GetComponent<RectTransform>());
                }
                else
                {
                    GameObject tmpObj = Instantiate(imgAddressSub) as GameObject;
                    //tmpObj.GetComponent<NavigationItem>().itemRoot = ItemRootList[i];
                    tmpObj.GetComponent<RectTransform>().SetSiblingIndex(10);
                    tmpObj.transform.Find("Text").GetComponent<Text>().text = ItemRootList[i].treeItem.t.text + "  ";
                    tmpObj.transform.SetParent(NavigationRoot.transform, false);
                    imgAddressSubList.Add(tmpObj);

                    LayoutRebuilder.ForceRebuildLayoutImmediate(tmpObj.GetComponent<RectTransform>());
                }
                LayoutRebuilder.ForceRebuildLayoutImmediate(NavigationRoot.GetComponent<RectTransform>());
            }
        }
    }
    /// <summary>
    /// 通过json加载树形结构
    /// </summary>
    public void GetTreeViewByJson(string jsonPath, CreateTreeItemRootCallback callback)
    {
        //Debug.Log(jsonPath);
        TreeInfos mInfo = JsonUtility.FromJson<TreeInfos>(File.ReadAllText(jsonPath));
        //Debug.Log(mInfo.treeInfos.Count);
        for (int i = 0; i < mInfo.treeInfos.Count; i++)
        {
            ChildDatas info = mInfo.treeInfos[i];
            //Debug.Log(info.showStr);
            ItemRoot item = callback(info, treeRoot);
            itemRootChildrens.Add(item);
            TreeIndexesGameObjectList.Add(item.TextClickEvnetObj);
            IterationTreeItem(info, item, callback);
            item.InitEventObj();
        }
    }

    /// <summary>
    /// 递归遍历json结构
    /// </summary>
    /// <param name="info"></param>
    /// <param name="itemParent"></param>
    private void IterationTreeItem(ChildDatas info, ItemRoot itemParent, CreateTreeItemRootCallback callback)
    {
        if (info.childDatas.Count > 0)
        {
            for (int j = 0; j < info.childDatas.Count; j++)
            {
                //Debug.Log(info.childDatas[j].showStr);
                ItemRoot item = callback(info.childDatas[j], itemParent.treeRoot);
                itemRootChildrens.Add(item);
                TreeIndexesGameObjectList.Add(item.TextClickEvnetObj);

                if (info.childDatas[j].childDatas.Count > 0)
                    IterationTreeItem(info.childDatas[j], item, callback);
                item.InitEventObj();
            }
        }
    }
}
