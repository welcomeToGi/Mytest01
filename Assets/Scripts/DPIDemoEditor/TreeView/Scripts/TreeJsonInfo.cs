using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeItemInfo
{
    public string uid;                          //唯一标签
    public string showStr;                      //展示名称                                        
    public string objectCode;                   //位号(模型名称)
    public bool isControl = true;               //是否可控制
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
