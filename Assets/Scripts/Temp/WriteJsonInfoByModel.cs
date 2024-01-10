/****************************************************
    文件：WriteJsonInfoByModel.cs
    作者：hsm
    日期：#20230322#
    功能：通过业务模型的名称层级 生成json
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
public class WriteJsonInfoByModel : MonoBehaviour
{
    public Transform Model_YW;
    // Start is called before the first frame update

    private TreeInfos treeInfos = new TreeInfos();
    void Start()
    {
        ReadChildName(Model_YW,"插座",null,false);
        string str = JsonUtility.ToJson(treeInfos);
        string path = Application.streamingAssetsPath + "/" + "IntelligentSocket" + ".json";
        File.WriteAllText(path, str);
    }
    private void ReadChildName(Transform RootTransform,string ContainsName,string exclusionName, bool isUseClusion) 
    {
        for (int i = 0; i < RootTransform.childCount; i++)//1层
        {
            //print(RootTransform.transform.GetChild(i).name);
            ChildDatas treeInfo = new ChildDatas();
            treeInfo.uid = "a" + i.ToString();
            treeInfo.showStr = RootTransform.transform.GetChild(i).name.ToString();
            //treeInfo.isUseSwitch = true;
            //treeInfo.switchType = 2;
            treeInfos.treeInfos.Add(treeInfo);
            if (RootTransform.transform.GetChild(i).childCount > 0)
            {
                for (int j = 0; j < RootTransform.transform.GetChild(i).childCount; j++)//2层
                {
                    //print(RootTransform.transform.GetChild(i).GetChild(j).name);

                    ChildDatas childDatas = new ChildDatas();
                    childDatas.uid = "b"+j.ToString();
                    childDatas.showStr = RootTransform.transform.GetChild(i).GetChild(j).name.ToString();
                    treeInfos.treeInfos[i].childDatas.Add(childDatas);

                    for (int n = 0; n < RootTransform.transform.GetChild(i).GetChild(j).childCount; n++)//3层
                    {


                        ChildDatas childDatas2 = new ChildDatas();
                        childDatas2.uid = "c" + n.ToString();
                        childDatas2.showStr = RootTransform.transform.GetChild(i).GetChild(j).GetChild(n).name.ToString();
                        //childDatas2.isUseSwitch = true;
                        //childDatas2.switchType = 2;
                        treeInfos.treeInfos[i].childDatas[j].childDatas.Add(childDatas2);

                        print(RootTransform.transform.GetChild(i).GetChild(j).GetChild(n).name);
                        for (int m = 0; m < RootTransform.transform.GetChild(i).GetChild(j).GetChild(n).childCount; m++)//4层
                        {

                            ChildDatas childDatas3 = new ChildDatas();
                            childDatas3.uid = "d" + m.ToString();
                            childDatas3.showStr = RootTransform.transform.GetChild(i).GetChild(j).GetChild(n).GetChild(m).name.ToString();
                            treeInfos.treeInfos[i].childDatas[j].childDatas[n].childDatas.Add(childDatas3);

                            for (int k = 0; k < RootTransform.transform.GetChild(i).GetChild(j).GetChild(n).GetChild(m).childCount; k++)//5层
                            {

                                ChildDatas childDatas4 = new ChildDatas();
                                childDatas4.uid = "e" + k.ToString();
                                childDatas4.showStr = RootTransform.transform.GetChild(i).GetChild(j).GetChild(n).GetChild(m).GetChild(k).name.ToString();
                                treeInfos.treeInfos[i].childDatas[j].childDatas[n].childDatas[m].childDatas.Add(childDatas4);


                            }
                        }
                    }
                }
            }
        }


        //foreach (var item in treeInfos.treeInfos)
        //{
        //    foreach (var item1 in item.childDatas)
        //    {
        //        foreach (var item2 in item1.childDatas)
        //        {
        //            print(item2.showStr);
        //        }
        //    }
        //}


    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
