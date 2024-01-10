using UnityEngine;
using System.Collections.Generic;

public static class TransformHelper //变换【】
{
    
    struct TransformClild 
    {
        public Transform parent;
        public string goName;
    }
    static Dictionary<TransformClild, Transform> TranChild = new Dictionary<TransformClild, Transform>(); 
    //方法1：在所有子级中查找子物体。
    public static Transform XuYiFindChild(this Transform parent, string goName)
    {
        TransformClild ClildItem = new TransformClild();
        ClildItem.parent = parent;
        ClildItem.goName = goName;

        if (TranChild.ContainsKey(ClildItem) && TranChild[ClildItem])
            return TranChild[ClildItem];
        var go=_XuYiFingChild(parent, goName);
        if (go)
        {
            TranChild.Add(ClildItem, go);
        }
        return go;
    }
    public static Transform XuYiNotFiadChild(this Transform parent, string goName) 
    {
        return _XuYiFingChild(parent, goName);
    }
    private static Transform _XuYiFingChild(this Transform parent, string goName) 
    {
        //超找自身的孩子
        var child = parent.Find(goName);
        if (child != null) return child;
        //如果没有在查找后代中哥有没有这个对象
        for (int i = 0; i < parent.childCount; i++)
        {
            child = parent.GetChild(i);
            var go = _XuYiFingChild(child, goName);
            if (go != null)
            {
                return go;
            }
        }
        return null;     
    }
    /// <summary>
    /// 转向
    /// </summary>
    public static void LookAtTarget(Vector3 targetDir, Transform transform,
        float rotationSpeed)
    {
        if (targetDir != Vector3.zero)
        {
            Quaternion dir = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Lerp
                (transform.rotation, dir, rotationSpeed);
        }
    }
    /// <summary>    /// 更改Position中X的值    /// </summary>
    /// <param name="target"></param>
    /// <param name="x"></param>
    /// <param name="isWord"></param>
    public static Transform SetPositionX(this Transform target, float x, bool isWord = false)
    {
        if (isWord) 
        {
            target.position = new Vector3(x, target.position.y, target.position.z);
        }
        else
        {
            target.localPosition = new Vector3(x, target.localPosition.y, target.localPosition.z);
        }
        return target;
    }
    /// <summary>    /// 更改Position中Y的值    /// </summary>
    /// <param name="target"></param>
    /// <param name="x"></param>
    /// <param name="isWord"></param>
    public static Transform SetPositionY(this Transform target, float y, bool isWord = false)
    {
        if (isWord)
        {
            target.position = new Vector3(target.position.x, y, target.position.z);
        }
        else
        {
            target.localPosition = new Vector3(target.localPosition.x, y, target.localPosition.z);
        }
        return target;
    }
    /// <summary>    /// 更改Position中Z的值    /// </summary>
    /// <param name="target"></param>
    /// <param name="x"></param>
    /// <param name="isWord"></param>
    public static Transform SetPositionZ(this Transform target, float z, bool isWord = false)
    {
        if (isWord)
        {
            target.position = new Vector3(target.position.x, target.position.y, z);
        }
        else
        {
            target.localPosition = new Vector3(target.localPosition.x, target.localPosition.y,z);
        }
        return target;
    }
    /// <summary>    /// 更改EulerAngles中Z的值    /// </summary>
    /// <param name="target"></param>
    /// <param name="x"></param>
    /// <param name="isWord"></param>
    public static Transform SetEulerAnglesZ(this Transform target, float z, bool isWord = false)
    {
        if (isWord)
        {
            target.eulerAngles = new Vector3(target.eulerAngles.x, target.eulerAngles.y, z);
        }
        else
        {
            target.localEulerAngles = new Vector3(target.localEulerAngles.x, target.localEulerAngles.y, z);
        }
        return target;
    }
    /// <summary>    /// 更改EulerAngles中X的值    /// </summary>
    /// <param name="target"></param>
    /// <param name="x"></param>
    /// <param name="isWord"></param>
    public static Transform SetEulerAnglesX(this Transform target, float x, bool isWord = false)
    {
        if (isWord)
        {
            target.eulerAngles = new Vector3(x, target.eulerAngles.y, target.eulerAngles.z);
        }
        else
        {
            target.localEulerAngles = new Vector3(x, target.localEulerAngles.y, target.localEulerAngles.z);
        }
        return target;
    }
    /// <summary>    /// 更改Position中Y的值    /// </summary>
    /// <param name="target"></param>
    /// <param name="x"></param>
    /// <param name="isWord"></param>
    public static Transform SetEulerAnglesY(this Transform target, float y, bool isWord = false)
    {
        if (isWord)
        {
            target.eulerAngles = new Vector3(target.position.x, y, target.position.z);
        }
        else
        {
            target.localEulerAngles = new Vector3(target.localEulerAngles.x, y, target.localEulerAngles.z);
        }
        return target;
    }
}
