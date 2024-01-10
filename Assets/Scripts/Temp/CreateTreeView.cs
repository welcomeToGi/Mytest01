using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���Զ�ȡjson���������б�
/// ���Զ�ȡexcel�����Ҳ���
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
        //transform.XuYiFindChild("��վ����_ҳ��").gameObject.SetActive(true);
        // �������νṹ
        TreeView.GetTreeViewByJson(Application.streamingAssetsPath + "/treeView.json", CreateTreeItemRootCallback);

        //transform.XuYiFindChild("��վ����_ҳ��").gameObject.SetActive(false);
        //���ر��
        //DataTable dataTable = LoadExcelData.ReadExcel(Application.streamingAssetsPath + "/�������/����������.xlsx", 0);
        //mCreateTable.CreateTable(dataTable, transform.XuYiFindChild("Info"));
        //Camera.main.GetComponent<CameraMotion>().LookAtObj(init);

    }
    private ItemRoot CreateTreeItemRootCallback(ChildDatas info, TreeRoot parent)
	{
		ItemRoot item = parent.AddItem();
        
        item.SetName(info.showStr);

        //// ��item��ӵ���۽������������
        if (EquipMgr.Instance.transform.XuYiFindChild(info.showStr + "_�۽�") != null)
            item.TextClickEvnetObj = EquipMgr.Instance.transform.XuYiFindChild(info.showStr + "_�۽�").gameObject;
        //if (EquipMgr.Instance.transform.XuYiFindChild("3DUI_" + info.showStr) != null)
        //{
        //    item.TextopenGameObjects.Add(EquipMgr.Instance.transform.XuYiFindChild("3DUI_" + info.showStr).gameObject);
        //}

        //var myimage = item.transform.XuYiFindChild("Image").GetComponent<Image>();
        //if (myimage != null)
        //{
        //    string targetStr = "Prefabs/UI/icon_" + info.showStr;
        //    if (info.showStr.Contains("���������"))
        //    {
        //        targetStr = "Prefabs/UI/icon_���׮���";
        //    }
        //    else if(info.showStr== "�ƺ���羰��" || info.showStr == "��ί��" 
        //        || info.showStr == "��ί�ܱ߽���" || info.showStr == "��ѵѧУ"
        //        || info.showStr == "������ͼ")
        //    {
        //        targetStr = "Prefabs/UI/icon_��ׯ";
        //    }
        //    else if (info.showStr.Contains("�׼����վ"))
        //    {
        //        targetStr = "Prefabs/UI/icon_���ܼ��";
        //    }
        //    var myspir= Resources.Load<Sprite>(targetStr);
        //    if (myspir!=null)
        //    {
        //        myimage.sprite = myspir;
        //        myimage.SetNativeSize();
        //    }
        //}

        //if (transform.XuYiFindChild("��վ����_ҳ��_�Ҳ�") != null)
        //{
        //    if (transform.XuYiFindChild("��վ����_ҳ��_�Ҳ�").XuYiFindChild(info.showStr) != null)
        //        item.TextopenGameObjects.Add(transform.XuYiFindChild("��վ����_ҳ��_�Ҳ�").XuYiFindChild(info.showStr).gameObject);
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
