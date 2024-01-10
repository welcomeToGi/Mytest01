using HighlightingSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public enum DPIGameobjectEventMSGType
{
    IS_OnMouseDown,
}


public class DPIGameobjectEvent : MonoBehaviour
{
    public ItemRoot itemRoot;

    public int SiblingIndex = -1;

    public bool iSOnClick = true;
    private Vector3 _YSelectScale = Vector3.one;
    private Vector3 _YSelectPos = Vector3.one;
    //private Highlighter highlighter=null;



    void Start()
    {
        _YSelectScale = transform.localScale;
        _YSelectPos = transform.localPosition;
        Highlighter highlighter = GetComponent<Highlighter>();
        if (highlighter==null)
        {
            highlighter = gameObject.AddComponent<Highlighter>();
        }
        highlighter.constant = false;
    }
    //private bool isOnMouseDown = true;


    public void SetPos(Action action)
    {

        transform.localPosition = _YSelectPos;
        if (action!=null)
        {
            action();
        }
    }


    public void SetScale(bool isScale,Vector3 scale)
    {
     
        if (isScale==false)
        {
            transform.DOScale(_YSelectScale, 0.5f);

        }else
        {
            transform.DOScale(scale, 0.5f);
        }

    }
  


    public void OnMouseDown()
    {

        //Debug.Log("当前点击物体00:" + transform.name);
		if (EventSystem.current.IsPointerOverGameObject()) return;
        if (itemRoot != null)
        {
            itemRoot.ObjButtonEvent();
        }
       
    }

  

    public void SetUITreeEvnet(bool isOpen)
    {
        if (itemRoot != null)
            itemRoot.SetTreeEvnet(isOpen);

    }
 
 
}
