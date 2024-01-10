/****************************************************
    文件：ScriptsName.cs
    作者：XXX
    日期：#20230329#
    功能：XXX
*****************************************************/
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Xiaoditu : MonoBehaviour, IScrollHandler
{
    public Transform cam_xiaoditu;
    public Transform target;
    private Transform jiantou;
    private float scrollSpeed = 15;
    private float hight_min = 30;
    private float hight_max = 550;
    private float hight_now = 75;
    private Image myimage;
    // Start is called before the first frame update
    void Start()
    {
        jiantou = transform.XuYiFindChild("玩家位置_小地图");

        myimage = GetComponent<Image>();
        myimage.alphaHitTestMinimumThreshold = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        cam_xiaoditu.position = new Vector3(target.position.x,hight_now,target.position.z);
        jiantou.eulerAngles = new Vector3(0, 0, -target.eulerAngles.y);
    }
    /// <summary>
    /// 设置小地图缩放
    /// </summary>
    public void SetPoint()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll>=0)
        {
            hight_now -= Mathf.Abs(scroll* scrollSpeed);
        }
        else if (scroll < 0)
        {
            hight_now += Mathf.Abs(scroll* scrollSpeed);
        }
        if (hight_now<hight_min)
        {
            hight_now = hight_min;
        }
        else if (hight_now > hight_max)
        {
            hight_now=hight_max;
        }
        cam_xiaoditu.position = new Vector3(target.position.x, hight_now, target.position.z);
    }

    public void OnScroll(PointerEventData eventData)
    {
        SetPoint();
    }
}
