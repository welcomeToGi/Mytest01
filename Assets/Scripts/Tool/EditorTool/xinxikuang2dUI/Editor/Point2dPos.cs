/****************************************************
    文件：Mytest.cs
    作者：MINITAO
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Point2dPos : MonoBehaviour
{
    public Transform target;
    private Transform kuang;
    private RectTransform line01;
    private RectTransform line02;
    private Vector3 startPos;
    private void Start()
    {
        kuang = transform.XuYiFindChild("框");
        line01 = transform.XuYiFindChild("line01").GetComponent<RectTransform>();
        line02 = transform.XuYiFindChild("line02").GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetTargetPos(target.position);
        }
    }
    public void SetTargetPos(Vector3 targetPos)
    {
        var targetTemp = Camera.main.WorldToScreenPoint(targetPos);
        transform.position=new Vector3(targetTemp.x,targetTemp.y,0);
        startPos = new Vector3(transform.position.x, kuang.position.y, 0);
        var length01 = Vector3.Distance(line01.transform.position, startPos);
        var length02 = Vector3.Distance(line02.transform.position, startPos);
        line01.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, length01+1);
        line02.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, length02+1);
    }
    public void SetKuangPos()
    {
        if (kuang.transform.localPosition.y>=0)
        {
            line01.localEulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            line01.localEulerAngles = new Vector3(0, 0, 270);
        }
        if (kuang.transform.localPosition.x >= 0)
        {
            line02.anchorMin = new Vector2(0, 0.5f);
            line02.anchorMax = new Vector2(0, 0.5f);
            line02.pivot = new Vector2(1, 0.5f);
        }
        else
        {
            line02.anchorMin = new Vector2(1, 0.5f);
            line02.anchorMax = new Vector2(1, 0.5f);
            line02.pivot = new Vector2(0, 0.5f);
        }
        //line02.localPosition = Vector3.zero;
        line02.anchoredPosition = Vector2.zero;
        SetTargetPos(target.position);
    }
    /// <summary>
    /// 重绘线条
    /// </summary>
    public void DrawLine()
    {
        if(kuang==null) kuang = transform.XuYiFindChild("框");
        if(line01==null)line01 = transform.XuYiFindChild("line01").GetComponent<RectTransform>();
        if(line02==null)line02 = transform.XuYiFindChild("line02").GetComponent<RectTransform>();

        SetKuangPos();
    }
}
