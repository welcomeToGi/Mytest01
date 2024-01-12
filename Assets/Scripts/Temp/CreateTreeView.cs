using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 测试读取json生成树形列表
/// 测试读取excel生成右侧表格
/// </summary>
public class CreateTreeView : MonoBehaviour
{
    public TreeView TreeView;
    public CameraLookAt init;
    private void Awake()
	{
	}
	void Start()
    {
        //transform.XuYiFindChild("场站总览_页面").gameObject.SetActive(true);
        // 加载树形结构
        TreeView.GetTreeViewByJson(Application.streamingAssetsPath + "/treeView.json", CreateTreeItemRootCallback);

        //transform.XuYiFindChild("场站总览_页面").gameObject.SetActive(false);
        //加载表格
        //DataTable dataTable = LoadExcelData.ReadExcel(Application.streamingAssetsPath + "/属性面板/生产分离器.xlsx", 0);
        //mCreateTable.CreateTable(dataTable, transform.XuYiFindChild("Info"));
        Camera.main.GetComponent<CameraMotion>().LookAtObj(init);

    }
    private ItemRoot CreateTreeItemRootCallback(ChildDatas info, TreeRoot parent)
	{
		ItemRoot item = parent.AddItem();
        
        item.SetName(info.showStr);

        //// 给item添加点击聚焦的物体等内容
        if (EquipMgr.Instance.transform.XuYiFindChild(info.showStr + "_聚焦") != null)
            item.TextClickEvnetObj = EquipMgr.Instance.transform.XuYiFindChild(info.showStr + "_聚焦").gameObject;
        //if (EquipMgr.Instance.transform.XuYiFindChild("3DUI_" + info.showStr) != null)
        //{
        //    item.TextopenGameObjects.Add(EquipMgr.Instance.transform.XuYiFindChild("3DUI_" + info.showStr).gameObject);
        //}

        //if (transform.XuYiFindChild("场站总览_页面_右侧") != null)
        //{
        //    if (transform.XuYiFindChild("场站总览_页面_右侧").XuYiFindChild(info.showStr) != null)
        //        item.TextopenGameObjects.Add(transform.XuYiFindChild("场站总览_页面_右侧").XuYiFindChild(info.showStr).gameObject);
        //}

        //if (EquipMgr.Instance.transform.XuYiFindChild("FX_" + info.showStr) != null)
        //{
        //    item.TextopenGameObjects.Add(EquipMgr.Instance.transform.XuYiFindChild("FX_" + info.showStr).gameObject);
        //}

        //      item.SetRootActive(false);
        //item.TextopenGameObjects.Add(biaoshi);
        return item;
    }
    public void setimage()
    {

    }
}
