using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollRectEvnet : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{


    public void OnBeginDrag(PointerEventData data)
    {
        GetComponentInParent<ScrollRect>().OnBeginDrag(data);
    }

    public void OnDrag(PointerEventData data)
    {
        GetComponentInParent<ScrollRect>().OnDrag(data);
    }

    public void OnEndDrag(PointerEventData data)
    {
        GetComponentInParent<ScrollRect>().OnEndDrag(data);
    }

}
