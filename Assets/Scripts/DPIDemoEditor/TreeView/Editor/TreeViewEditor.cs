using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TreeView))]
public class TreeViewEditor : Editor
{
	private TreeView _TreeView;
	void OnEnable()
	{
		_TreeView = target as TreeView;
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("补齐"))
		{
			ItemRoot[] itemRoots= _TreeView.gameObject.GetComponentsInChildren<ItemRoot>();
            foreach (var item in itemRoots)
            {
                if (item.treeItem.GetComponent<RectTransform>().sizeDelta.x > _TreeView.SizeDelta.x)
                {
					_TreeView.SizeDelta = item.treeItem.GetComponent<RectTransform>().sizeDelta;
				}
				item.name = item.treeItem.t.text;

			}
			foreach (var item in itemRoots)
			{
				item.treeItem.GetComponent<RectTransform>().sizeDelta= _TreeView.SizeDelta;
			}
		}

		if (GUILayout.Button("没有子节点的Item添加图标"))
        {

			ItemRoot[] itemRoots = _TreeView.gameObject.GetComponentsInChildren<ItemRoot>();
			foreach (var item in itemRoots)
			{
				ItemRoot[] itemRoots2 = item.gameObject.GetComponentsInChildren<ItemRoot>();
                if (itemRoots2.Length==1)
                {
					item.treeItem.SetIcon(_TreeView.IconGameObject, _TreeView.isDleIcon);

				}

			}
		}


		if (GUILayout.Button("装填Root"))
		{
			TreeRoot[] TreeRoots = _TreeView.GetComponentsInChildren<TreeRoot>();
			foreach (var item in TreeRoots)
			{
				item.Start();

			}
		}
	}
}
