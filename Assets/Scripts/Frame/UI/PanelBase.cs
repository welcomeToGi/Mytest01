using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour
{

    public void OpenUI()
    {
        gameObject.SetActive(true);
        Transform left = transform.XuYiFindChild("Left");
        Vector3 normalLeftPos = left.localPosition;
        Transform right = transform.XuYiFindChild("Right");
        Vector3 normalRightPis = right.localPosition;
        left.localPosition = new Vector3(-2400, normalLeftPos.y, normalLeftPos.z);
        right.localPosition = new Vector3(2400, normalRightPis.y, normalRightPis.z);
        transform.XuYiFindChild("Left").DOLocalMoveX(-1460, 0.3f);
        transform.XuYiFindChild("Right").DOLocalMoveX(1460, 0.3f);
    }
    public void CloseUI()
    {
        //transform.XuYiFindChild("Left").DOLocalMoveX(-2400, 0.3f);
        //transform.XuYiFindChild("Right").DOLocalMoveX(2400, 0.3f).OnComplete(() => { gameObject.SetActive(false); });
        gameObject.SetActive(false);
    }
}
