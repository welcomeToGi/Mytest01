using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


//[CustomEditor(typeof(TreeRoot))]
public class TreeRootEditor : Editor
{
	
	private TreeRoot _TreeRoot;
	void OnEnable()
	{
		_TreeRoot = target as TreeRoot;
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("添加Item"))
		{
			var itemRoot =  Instantiate(Resources.Load("TreeViewPrefab/ItemRoot")) as GameObject;
			itemRoot.transform.SetParent(_TreeRoot.transform,false);
			TreeRoot[] TreeRoots = _TreeRoot.GetComponentsInParent<TreeRoot>();
			itemRoot.GetComponent<ItemRoot>().treeItem.SetItem(TreeRoots.Length - 1);

			TreeView treeView = itemRoot.GetComponentInParent<TreeView>();
            if (treeView.IconGameObject!=null)
            {
				itemRoot.GetComponent<ItemRoot>().treeItem.SetIcon(treeView.IconGameObject,false);
			}

			ItemRoot iroot =_TreeRoot.GetComponentInParent<ItemRoot>();
            if (iroot!=null)
            {
				iroot.treeItem.SetIcon(null,true);
				iroot.treeItem.OpenArrow();
			}
		}
	}
}
