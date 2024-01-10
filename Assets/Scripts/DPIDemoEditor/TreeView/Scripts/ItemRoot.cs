
using DG.Tweening;
using HighlightingSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonType
{
    切换,
    单向,
}

public enum ItemType
{
    空,
    地板,
    地形,
    根节点,
}

[Serializable]
public class ToggleInfo
{
    [HideInInspector]
    public GameObject ToggleGameObject;
    public string name;



    [Header("子是否联动")]
    public bool Islinkage = false;

    [Header("点击显示的物体")]
    public GameObject[] openGameObjects;


    [Header("点击关闭的物体")]
    public GameObject[] offGameObjects;


}


public class ItemRoot : MonoBehaviour
{


    [SerializeField]
    public TreeRoot TreeRoot;



    [Header("Item类型")]
    public ItemType itemType = ItemType.空;

    [Header("文字点击是否聚焦")]
    [SerializeField]
    public bool Isfocusing = true;
    [Header("文字点击聚焦的物体")]
    [SerializeField]
    public GameObject TextClickEvnetObj;
    [Header("点击文字显示的物体")]
    [SerializeField]
    public List<GameObject> TextopenGameObjects;
    [Header("点击文字关闭的物体")]
    [SerializeField]
    public List<GameObject> TextoffGameObjects;
    


    #region 按钮工具
    [ToggleGroup("IsButton", "是否使用点击按钮")]
    public bool IsButton;

    [ToggleGroup("IsButton")]
    [ShowIf("IshowInfo")]
    [Header("按钮类型")]
    public ButtonType buttonType = ButtonType.单向;

    [ToggleGroup("IsButton")]
    [ShowIf("IsButton")]
    [Header("点击按钮预设")]
    public GameObject ButtonPrefab;

    [ToggleGroup("IsButton")]
    [ShowIf("IshowInfo")]
    [Header("点击按钮显示的物体")]
    public GameObject[] openGameObjects;

    [ToggleGroup("IsButton")]
    [ShowIf("IshowInfo")]
    [Header("点击按钮关闭的物体")]
    public GameObject[] offGameObjects;

    [ToggleGroup("IsButton")]
    [ShowIf("IshowButton")]
    [Button("$AddButtonName")]
    public void AddButton()
    {
        if (ButtonPrefab != null)
        {
            IshowButton = false;
            var TreeButtonObj = Instantiate(ButtonPrefab) as GameObject;
            TreeView treeView = GetComponentInParent<TreeView>();
            treeItem.AddButton(TreeButtonObj);
            TreeButton = TreeButtonObj.GetComponent<Button>();
            IshowInfo = true;
        }
    }

    [HideInInspector]
    public Button TreeButton;


    [HideInInspector]
    public bool IshowButton = true;
    [HideInInspector]
    public bool IshowInfo = false;
    [HideInInspector]
    public string AddButtonName = "添加按钮";

    #endregion


    [HideInInspector]
    public bool IsToggleButton = true;
    [HideInInspector]
    public bool IsToggleInfo = false;

    [ToggleGroup("IsToggle", "是否使用点击按钮")]
    public bool IsToggle;
    [ToggleGroup("IsToggle")]
    public List<ToggleInfo> ToggleInfos = new List<ToggleInfo>();

    [ToggleGroup("IsToggle")]
    [Header("预设")]
    public GameObject TogglePrefab;


    [ToggleGroup("IsToggle")]
    // [HorizontalGroup("Split", 0.5f)]
    [Button("$AddToggleName", ButtonSizes.Large)]
    public void AddToggle()
    {

        if (TogglePrefab != null)
        {
            var ToggleObj = Instantiate(TogglePrefab) as GameObject;
            TreeView treeView = GetComponentInParent<TreeView>();
            treeItem.AddToggle(ToggleObj, 99);

            ToggleInfo Info = new ToggleInfo();
            Info.name = "在后面添加==>";
            Info.ToggleGameObject = ToggleObj;
            ToggleInfos.Add(Info);
        }
    }

    public void SetName(string name)
    {
        gameObject.name = name;

        treeItem.SetItemText(name);
    }

    [HideInInspector]
    public string AddToggleName = "在后面添加";

    [ToggleGroup("IsToggle")]
    //[HorizontalGroup("Split", 0.5f)]
    [Button("$AddToggleName2", ButtonSizes.Large)]
    public void AddToggle2()
    {
        if (TogglePrefab != null)
        {
            var ToggleObj = Instantiate(TogglePrefab) as GameObject;
            TreeView treeView = GetComponentInParent<TreeView>();
            treeItem.AddToggle(ToggleObj, 1);

            ToggleInfo Info = new ToggleInfo();
            Info.name = "在前面添加==>";
            Info.ToggleGameObject = ToggleObj;
            ToggleInfos.Add(Info);

        }

    }

    [HideInInspector]
    public string AddToggleName2 = "在前面添加";


    //[HideInInspector]
    public TreeItem treeItem;
    [HideInInspector]
    public TreeRoot treeRoot;

    private bool ButtonSwitch = true;

    private DPIGameobjectEvent dPIGameobjectEvent;


    private ItemRoot[] itemRootParents;
    private ItemRoot[] itemRootChildrens;


    public GameObject obj;
    private Color TextColor;
    private bool isEvent = true;

    public bool IsHighlighter = true;
    public void OnEnable()
    {

    }

    public void Awake()
    {
        itemRootParents = gameObject.GetComponentsInParent<ItemRoot>();
        itemRootChildrens = gameObject.GetComponentsInChildren<ItemRoot>();
        //print(treeItem.t);
        if (treeItem.t == null) treeItem.t = treeItem.GetComponentInChildren<TextMeshProUGUI>();
        gameObject.name = treeItem.t.text;
        treeItem.ArrowEventAction = SetRootActive;
        treeItem.showGameObject = TextClickEvnetObj;
        treeItem.openGameObjects = TextopenGameObjects;
        treeItem.offGameObjects = TextoffGameObjects;
        treeItem.TextOnClickAction = TextButtonEvent;
        TextColor = treeItem.t.color;
        InitEventObj();
        SetToggleEvnet();

        if (TreeButton != null)
        {
            TreeButton.onClick.AddListener(() =>
            {
                if (isEvent == false)
                    return;
                if (buttonType == ButtonType.单向)
                {
                    if (offGameObjects != null)
                    {
                        foreach (var item in offGameObjects)
                        {
                            item.SetActive(false);
                        }
                    }
                    if (openGameObjects != null)
                    {
                        foreach (var item in openGameObjects)
                        {
                            item.SetActive(true);

                        }
                    }

                }
                if (buttonType == ButtonType.切换)
                {
                    if (openGameObjects != null)
                    {
                        foreach (var item in openGameObjects)
                        {
                            item.SetActive(ButtonSwitch);
                        }
                    }
                    if (offGameObjects != null)
                    {
                        foreach (var item in offGameObjects)
                        {
                            item.SetActive(!ButtonSwitch);
                        }
                    }
                    ButtonSwitch = !ButtonSwitch;
                }


            });
        }

    }
    public void Start()
    {
        if (TreeRoot == null)
        {
            TreeRoot = transform.Find("Root").GetComponent<TreeRoot>();
        }


        //foreach (var item in itemRootChildrens)
        //{
        //	if (item != this && item.TextClickEvnetObj != null)
        //	{
        //		StartCoroutine(FindTransparent());
        //		return;
        //	}
        //}
        if (itemRootParents.Length >= 2)
        {
            //Debug.Log(itemRootParents[1].name);
            ItemRoot parent = itemRootParents[1];
            if(parent.TextClickEvnetObj != null)
			{
                SetMaterialTransparent[] trans = parent.TextClickEvnetObj.GetComponentsInChildren<SetMaterialTransparent>();
                List<SetMaterialTransparent> tranList = trans.ToList();
                if (TextClickEvnetObj != null && TextClickEvnetObj.GetComponent<SetMaterialTransparent>() != null)
                    tranList.Remove(TextClickEvnetObj.GetComponent<SetMaterialTransparent>());
                foreach (var item in tranList)
                {
                    TransparentGameObjectList.Add(item.gameObject);
                }
            }
        }
    }

    public void Init()
    {
        itemRootParents = gameObject.GetComponentsInParent<ItemRoot>();
        itemRootChildrens = gameObject.GetComponentsInChildren<ItemRoot>();
        //foreach (var item in itemRootChildrens)
        //{
        //	if (item != this && item.TextClickEvnetObj != null)
        //	{
        //		StartCoroutine(FindTransparent());
        //		return;
        //	}
        //}
    }

    public void InitEventObj()
    {
        treeItem.showGameObject = TextClickEvnetObj;
        if (TextClickEvnetObj != null)
        {
            dPIGameobjectEvent = TextClickEvnetObj.GetComponent<DPIGameobjectEvent>();
            if (dPIGameobjectEvent == null)
            {
                dPIGameobjectEvent = TextClickEvnetObj.gameObject.AddComponent<DPIGameobjectEvent>();
            }
            MeshRenderer[] meshRenderers = TextClickEvnetObj.GetComponentsInChildren<MeshRenderer>();
            foreach (var item in meshRenderers)
            {
                if (item.gameObject.GetComponent<MouseColliderEvent>() == null)
                {
                    item.gameObject.AddComponent<MouseColliderEvent>();
                }
            }
            dPIGameobjectEvent.itemRoot = this;
        }
        Init();
    }
    private void SetToggleEvnet()
    {
        if (ToggleInfos != null)
        {
            for (int i = 0; i < ToggleInfos.Count; i++)
            {
                ToggleComponent toggleComponent = ToggleInfos[i].ToggleGameObject.GetComponent<ToggleComponent>();
                if (toggleComponent == null)
                {
                    toggleComponent = ToggleInfos[i].ToggleGameObject.AddComponent<ToggleComponent>();
                }
                toggleComponent.toggleinfo = ToggleInfos[i];
                toggleComponent.itemRoot = this;
            }
        }
    }


    public void ObjButtonEvent()
    {
        //获取层级结构列表
        var treeView= FindObjectOfType<CreateTreeView>().TreeView;
        if (treeView == null)
            return;
        SetScrollViewNevigation();

        treeItem.TextOnClick();
        if (treeView.treeViewType == TreeViewType.建筑层级)
        {
            FloorTools.Instance.OnMouseDown(TextClickEvnetObj);
        }
    }
    //设置ScrollviewNavigation
    private void SetScrollViewNevigation()
    {
        var treeView = FindObjectOfType<CreateTreeView>().TreeView;
        ScrollRect scrollRect = treeView.transform.XuYiFindChild("Scroll View").GetComponent<ScrollRect>();
        if (itemRootParents.Length == 1)
        {
            //DOTween.To(() => scrollRect.verticalScrollbar.value, x => scrollRect.verticalScrollbar.value = x, 1, 0.8f);
        }
        else
        {
            if (scrollRect == null)
                return;
            ScrollViewNevigation scrollViewNevigation = scrollRect.GetComponent<ScrollViewNevigation>();
            if (scrollViewNevigation == null)
            {
                scrollViewNevigation = scrollRect.gameObject.AddComponent<ScrollViewNevigation>();
            }
            scrollViewNevigation.Init();
			scrollViewNevigation.Nevigate(treeItem.GetComponent<RectTransform>());
		}
    }


    public bool isSetMaterialTransparent = true;
    public void TextButtonEvent()
    {
        TreeView treeView = FindObjectOfType<CreateTreeView>().TreeView;

        SetScrollViewNevigation();
        treeView.InitMaterialTransparent();
        if (gameObject.GetComponent<DpiMessengerBase>() != null)
        {
            gameObject.GetComponent<DpiMessengerBase>().SendInfo();
        }
        if (isSetMaterialTransparent)
        {
            if (TransparentGameObjectList.Count > 0)
            {
                foreach (var item in TransparentGameObjectList)
                {
                    item.GetComponent<SetMaterialTransparent>().isTransparent(true);
                }
            }
            //如果只有一个头节点
            //if (itemRootParents.Length == 2)
            //{
            //             foreach(var item in itemRootParents[1].TransparentGameObjectList)
            //	{
            //                     item.GetComponent<SetMaterialTransparent>().isTransparent(true);
            //             }
            //         }
            //else
            //{
            //	if (itemType != ItemType.根节点)
            //	{
            //		//如果只有子节点 把自己变透明
            //		//if (itemRootChildrens.Length > 1)
            //		//{
            //		//	for (int i = 0; i < TransparentGameObjectList.Count; i++)
            //		//	{
            //		//		TransparentGameObjectList[i].GetComponent<SetMaterialTransparent>().isTransparent(true);
            //		//	}
            //		//}
            //	}
            //	//循环头节点
            //	foreach (var item in itemRootParents)
            //	{
            //		if (item.itemType != ItemType.根节点)
            //		{
            //			if (item != this)
            //			{
            //				for (int i = 0; i < item.TransparentGameObjectList.Count; i++)
            //				{
            //					item.TransparentGameObjectList[i].GetComponent<SetMaterialTransparent>().isTransparent(true);
            //				}
            //			}
            //		}

            //	}
            //}
        }
        //设置导航
        treeView.SetNavigation(itemRootParents);




        if (TextClickEvnetObj == null) return;

        if (Isfocusing == false) return;


        if (IsHighlighter && TextClickEvnetObj.GetComponent<Highlighter>() != null)
        {
            //TextClickEvnetObj.GetComponent<Highlighter>().constantColor = SceneMgr.Instance.HighlighterColor;
            TextClickEvnetObj.GetComponent<Highlighter>().constantColor = treeView.HighlightColor;
            TextClickEvnetObj.GetComponent<Highlighter>().overlay = true;

            TextClickEvnetObj.GetComponent<Highlighter>().enabled = true;
            TextClickEvnetObj.GetComponent<Highlighter>().constant = true;
            Highlighter[] childHighlighters = TextClickEvnetObj.GetComponentsInChildren<Highlighter>();
            for(int i = 0; i < childHighlighters.Length; i++)
			{
                childHighlighters[i].constantColor = treeView.HighlightColor;
                childHighlighters[i].enabled = true;
                childHighlighters[i].constant = true;
			}
        }






        if (treeView.treeViewType == TreeViewType.建筑层级)
        {
            if (FloorTools.Instance.floorModel != FloorModel.阵列)
            {
                if (itemRootParents.Length > 1)
                {
                    FloorTools.Instance.OnMouseDown(TextClickEvnetObj);
                }
                else
                {

                    FloorTools.Instance.OnMouseDown(TextClickEvnetObj, true);
                }
            }
            else
            {
                if (itemRootParents.Length == 1)
                {
                    //DPICameraManage.Instance.LookAtAppointTarget(TextClickEvnetObj, 5);
                    //SimpleCameraController.Instance.LookAtObj(TextClickEvnetObj.transform);
                    CameraMotion.Instance.LookAtObj(TextClickEvnetObj.GetComponent<CameraLookAt>());
                }
                else
                    FloorTools.Instance.OnMouseDown(TextClickEvnetObj);

            }


        }
        else
        {
            //DPICameraManage.Instance.LookAtAppointTarget(TextClickEvnetObj, 5);
                    CameraMotion.Instance.LookAtObj(TextClickEvnetObj.GetComponent<CameraLookAt>());
            //SimpleCameraController.Instance.LookAtObj(TextClickEvnetObj.transform);
        }


    }
    /// <summary>
    /// 被改变透明的物体
    /// </summary>
   // [HideInInspector]
    public List<GameObject> TransparentGameObjectList = new List<GameObject>();

    public IEnumerator FindTransparent()
    {
        //把所有带网格的物体找出来
        //所有在Item上索引的物体 都有 DPIGameobjectEvent 组件

        //使用 查找网格的物体父有没有 DPIGameobjectEvent 组件 

        if (TextClickEvnetObj != null && itemRootChildrens.Length > 1)
        {
            TreeView treeView = gameObject.GetComponentInParent<TreeView>();
            Transform[] Transforms = TextClickEvnetObj.GetComponentsInChildren<Transform>();
            List<Transform> tmpTransformList = new List<Transform>(Transforms);
            tmpTransformList.Remove(TextClickEvnetObj.transform);
            List<Transform> tmpList = new List<Transform>();
            foreach (var item in tmpTransformList)
            {
                if (treeView.FindTreeIndexesGameObject(item.gameObject))
                {
                    foreach (var item3 in tmpTransformList)
                    {
                        Transform[] RootTransforms = item3.GetComponentsInParent<Transform>();
                        foreach (var item2 in RootTransforms)
                        {
                            if (item2.gameObject == item.gameObject)
                            {
                                tmpList.Add(item3);
                            }
                        }
                    }
                }
            }
            foreach (var item in tmpTransformList)
            {
                if (tmpList.Find(t => t == item) == false)
                    TransparentGameObjectList.Add(item.gameObject);
            }

            if (itemRootChildrens.Length > 1)
            {
                TransparentGameObjectList.Add(TextClickEvnetObj);
            }
            foreach (GameObject itemobj in TransparentGameObjectList)
            {
                if (itemobj.GetComponent<SetMaterialTransparent>() == null)
                {
                    itemobj.AddComponent<SetMaterialTransparent>();
                }

            }
        }

        yield return null;
    }

    //是否正在搜索
    public bool isSearching = false;
    public void SetRootActive(bool Active)
    {
        //if (!isSearching)
        //Debug.Log(Active);
        treeRoot.gameObject.SetActive(Active);
        TreeView treeView = gameObject.GetComponentInParent<TreeView>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(treeView.treeRoot.transform.GetComponent<RectTransform>());
        foreach (var parentRoot in itemRootParents)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentRoot.GetComponent<RectTransform>());
        }

    }



    public void SetTreeEvnet(bool isOpen)
    {
        if (isOpen)
        {
            treeItem.t.color = TextColor;

        }
        else
        {
            treeItem.t.color = Color.red;
        }
        isEvent = isOpen;
        treeItem.isEvent = isOpen;
        if (TreeButton != null)
            TreeButton.enabled = isOpen;
    }
}