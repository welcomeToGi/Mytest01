using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMessenge : DpiMessengerBase
{
    [SerializeField]
    public TextInfo ChildrenList;

    public override void SendInfo()
    {
        Debug.LogError("SendInfo");
        if (messgerBase != null)
        {
            List<Info> infos = new List<Info>();
            infos.Add(ChildrenList);
            messgerBase.ReceInfo(infos);
        }
    }
    public void Start()
    {
        
    }
}
[System.Serializable]
public class TextInfo : Info
{
    [SerializeField]
    public int ID;

    [SerializeField]
    public string Name;

    [SerializeField]
    public int Level;

    [SerializeField]
    public int NodeType;

    [SerializeField]
    public Vector2 Point;
}


