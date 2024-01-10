using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>    /// 物体跟随鼠标运动    /// </summary>
public class ObjMouseMove : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public enum axialEnum
    {
        x,
        y,
        z,
    }
    /// <summary>        /// 最大值        /// </summary>
    [SerializeField]
    float Max;
    /// <summary>        /// 最小值        /// </summary>
    [SerializeField]
    float Min;
    /// <summary>        /// 是否开启        /// </summary>
    bool isOpen;
    /// <summary>        /// 鼠标精灵        /// </summary>
    [SerializeField]
    Texture2D[] mCursors;
    /// <summary>        /// 移动的轴        /// </summary>
    public axialEnum axial;

    Ray ray;
    RaycastHit hit;
    [SerializeField]
    LayerMask layer;

    float X, Y, Z;
    private void Update()
    {
        if (isOpen)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 500, layer))
            {
                X = hit.point.x;
                Y = hit.point.y;
                Z = hit.point.z;
                if (!isJinRuOneSelf)
                    Cursor.SetCursor(mCursors[0], new Vector2(16, 0), CursorMode.Auto);
                switch (axial)
                {
                    case axialEnum.x:
                        transform.position = new Vector3(Mathf.Clamp(X, Min, Max), transform.position.y, transform.position.z);
                        break;
                    case axialEnum.y:
                        transform.position = new Vector3(transform.position.x, Mathf.Clamp(Y, Min, Max), transform.position.z);
                        break;
                    case axialEnum.z:
                        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(Z, Min, Max));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                isOpen = false;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                //FirstViewControl.instance.isCanRotate = Save_Fins;
            }

        }
    }
    bool isJinRuOneSelf;
    bool Save_Fins;
    public void OnPointerEnter(PointerEventData eventData)
    {
        isJinRuOneSelf = true;

        Cursor.SetCursor(mCursors[1], new Vector2(16, 0), CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isJinRuOneSelf = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Save_Fins = FirstViewControl.instance.isCanRotate;
        //FirstViewControl.instance.isCanRotate = false;
        isOpen = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isOpen = false;
        //FirstViewControl.instance.isCanRotate = Save_Fins;
    }
}
