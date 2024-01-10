/****************************************************
    文件：Daditu.cs
    作者：XX
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Daditu : MonoBehaviour,IBeginDragHandler,IDragHandler,IScrollHandler
{
    public Transform player;
    public Camera cam_daditu;
    private Transform jiantou;
    private Vector2 dragStartPos;
    private Vector3 dragStartPos_rect;
    private RectTransform m_rect;
    private float dragpos_min_x;
    private float dragpos_max_x;
    private float dragpos_min_y;
    private float dragpos_max_y;
    private float scall_speed = 0.1f;

    private float scall_min_value = 1f;
    private float scall_max_value = 2f;
    private float scal = 1;


    private Vector2 oldPosition1;
    private Vector2 oldPosition2;
    // Start is called before the first frame update
    void Start()
    {
        //jiantou = transform.XuYiFindChild("玩家位置_大地图");
        m_rect=GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(m_rect.localPosition);
        }
        #region 安卓版
#if UNITY_ANDROID
        //该方法可以判断触摸点是否在UI上
        //if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        //    return;
        if (Input.touchCount > 1)
        {
            //当从单指触摸进入多指触摸的时候,记录一下触摸的位置
            //保证计算缩放都是从两指手指触碰开始的
            ScaleCamera();
            oldPosition1 = Input.GetTouch(0).position;
            oldPosition2 = Input.GetTouch(1).position;
        }
#endif
        #endregion

    }
    private void OnEnable()
    {
        //if(jiantou==null) jiantou = transform.XuYiFindChild("玩家位置_大地图");
        //jiantou.position = cam_daditu.WorldToScreenPoint(player.position);
        //jiantou.eulerAngles = new Vector3(0, 0, -player.eulerAngles.y);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPos=eventData.position;
        dragStartPos_rect = m_rect.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 offset = eventData.position - dragStartPos;
        m_rect.position = dragStartPos_rect + new Vector3(offset.x,offset.y,0);

        DragLimitSet();
    }
    public void OnScroll(PointerEventData eventData)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(m_rect, Input.mousePosition)) return;
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue == 0) return;

        Suofang(scrollValue);
        DragLimitSet();
    }
    void DragLimitSet()
    {
        dragpos_min_x = m_rect.rect.width * scal / 2;
        dragpos_max_x = Screen.width - m_rect.rect.width * scal / 2;
        dragpos_min_y = m_rect.rect.height * scal / 2;
        dragpos_max_y = Screen.height - m_rect.rect.height * scal / 2;

        if (scal>1)
        {
            Image_L_Limit();
        }
        else
        {
            Image_s_Limit();
        }
    }
    /// <summary>
    /// 大于屏幕的图片，移动限制
    /// 移动范围限制四个角不能在屏幕内
    /// 这里指缩放值大于1的
    /// </summary>
    void Image_L_Limit()
    {
        if (m_rect.position.x >= dragpos_min_x)
        {
            m_rect.position = new Vector3(dragpos_min_x, m_rect.position.y, 0);
        }
        if (m_rect.position.x <= dragpos_max_x)
        {
            m_rect.position = new Vector3(dragpos_max_x, m_rect.position.y, 0);
        }
        if (m_rect.position.y >= dragpos_min_y)
        {
            m_rect.position = new Vector3(m_rect.position.x, dragpos_min_y, 0);
        }
        if (m_rect.position.y <= dragpos_max_y)
        {
            m_rect.position = new Vector3(m_rect.position.x, dragpos_max_y, 0);
        }
    }
    /// <summary>
    /// 小于屏幕的图片，移动限制
    /// 移动范围限制在屏幕内
    /// 这里指缩放值小于1的
    /// </summary>
    void Image_s_Limit()
    {
        if (m_rect.position.x >= dragpos_max_x)
        {
            m_rect.position = new Vector3(dragpos_max_x, m_rect.position.y, 0);
        }
        if (m_rect.position.x <= dragpos_min_x)
        {
            m_rect.position = new Vector3(dragpos_min_x, m_rect.position.y, 0);
        }
        if (m_rect.position.y >= dragpos_max_y)
        {
            m_rect.position = new Vector3(m_rect.position.x, dragpos_max_y, 0);
        }
        if (m_rect.position.y <= dragpos_min_y)
        {
            m_rect.position = new Vector3(m_rect.position.x, dragpos_min_y, 0);
        }
    }

    void Suofang(float c_scrollValue)
    {
        scal = m_rect.transform.localScale.x;
        if (c_scrollValue > 0)
        {
            scal += scall_speed;
        }
        else
        {
            scal -= scall_speed;
        }
        if (scal < scall_min_value)
        {
            scal = scall_min_value;
        }
        if (scal > scall_max_value)
        {
            scal = scall_max_value;
        }
        m_rect.transform.localScale = new Vector3(scal, scal, scal);
    }

    /// <summary>
    /// 触摸缩放摄像头
    /// </summary>
    private void ScaleCamera()
    {
        //计算出当前两点触摸点的位置
        var tempPosition1 = Input.GetTouch(0).position;
        var tempPosition2 = Input.GetTouch(1).position;

        float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
        float lastTouchDistance = Vector3.Distance(oldPosition1, oldPosition2);

        float scrollValue = (currentTouchDistance - lastTouchDistance);

        Suofang(scrollValue);
        //备份上一次触摸点的位置，用于对比
        oldPosition1 = tempPosition1;
        oldPosition2 = tempPosition2;
    }
}
