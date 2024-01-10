using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRoot : MonoBehaviour
{
    //[SerializeField]
    public List<ItemRoot> itemRoots = new List<ItemRoot>();
    [HideInInspector]
    public string FloorButtonName = "添加Item";


    public void ClearItem()
    {
        for (int i = 0; i < itemRoots.Count; i++)
            if (itemRoots[i] != null)
                DestroyImmediate(itemRoots[i].gameObject);

        itemRoots.Clear();
    }
    [Button("$FloorButtonName", ButtonSizes.Large)]
    public ItemRoot AddItem()
    {
        var itemRoot = Instantiate(Resources.Load("TreeViewPrefab/ItemRoot")) as GameObject;
        itemRoot.transform.SetParent(transform, false);
        TreeRoot[] TreeRoots = GetComponentsInParent<TreeRoot>();
        ItemRoot itemRootScript = itemRoot.GetComponent<ItemRoot>();
        //itemRootScript.treeItem.SetItem(TreeRoots.Length);

        TreeView treeView = FindObjectOfType<CreateTreeView>().TreeView;

        if (treeView.IconGameObject != null)
        {
            itemRootScript.treeItem.SetIcon(treeView.IconGameObject, false);
        }
        ItemRoot iroot = GetComponentInParent<ItemRoot>();
        if (iroot != null)
        {
            //iroot.treeItem.SetIcon(null, true);
            iroot.treeItem.OpenArrow();
            itemRootScript.treeItem.SetItem(iroot.treeItem.StationList.Count + 1);
        }
        itemRoots.Add(itemRootScript);

        return itemRootScript;
    }
    public ItemRoot ZhedieItem()
    {
        var itemRoot = Instantiate(Resources.Load("TreeViewPrefab/ItemRoot")) as GameObject;
        itemRoot.transform.SetParent(transform, false);
        TreeRoot[] TreeRoots = GetComponentsInParent<TreeRoot>();
        ItemRoot itemRootScript = itemRoot.GetComponent<ItemRoot>();
        //itemRootScript.treeItem.SetItem(TreeRoots.Length);

        TreeView treeView = itemRoot.GetComponentInParent<TreeView>();
        if (treeView.IconGameObject != null)
        {
            itemRootScript.treeItem.SetIcon(treeView.IconGameObject, false);
        }
        ItemRoot iroot = GetComponentInParent<ItemRoot>();
        if (iroot != null)
        {
            //iroot.treeItem.SetIcon(null, true);
            iroot.treeItem.OpenArrow();
            itemRootScript.treeItem.SetItem(iroot.treeItem.StationList.Count + 1);
        }
        itemRoots.Add(itemRootScript);

        return itemRootScript;
    }

    // Start is called before the first frame update
    public void Start()
    {
        if (itemRoots.Count == 0)
        {
            foreach (Transform item in transform)
            {
                if (item.GetComponent<ItemRoot>() != null)
                    itemRoots.Add(item.GetComponent<ItemRoot>());
            }
        }


    }

    public void Reset()
    {
        Debug.Log("------Reset--------------");
    }
}
