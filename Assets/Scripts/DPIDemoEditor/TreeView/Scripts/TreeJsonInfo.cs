using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeItemInfo
{
    public string uid;                          //Ψһ��ǩ
    public string showStr;                      //չʾ����                                        
    public string objectCode;                   //λ��(ģ������)
    public bool isControl = true;               //�Ƿ�ɿ���
}

[System.Serializable]
public class ChildDatas : TreeItemInfo
{
    public List<ChildDatas> childDatas = new List<ChildDatas>();
}

[System.Serializable]
public class TreeInfos
{
    public List<ChildDatas> treeInfos = new List<ChildDatas>();
}
