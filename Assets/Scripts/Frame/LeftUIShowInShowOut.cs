using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LeftUIShowInShowOut : MonoBehaviour
{
    public GameObject LeftUI;
    public GameObject LeftUIShowButton;
    public GameObject btn_ShowIn;
    public GameObject btn_ShowOut;
    public bool ShowState;

    public void BtnBackClicked()
    {

    }
    public void Start()
    {
        btn_ShowIn.GetComponent<Button>().onClick.AddListener(delegate()
        {
            Debug.Log("收起按钮被点击了");
            btn_ShowInClicked(btn_ShowIn,btn_ShowOut);
        });
        btn_ShowOut.GetComponent<Button>().onClick.AddListener(delegate()
        {
            Debug.Log("打开按钮被点击了");
            btn_ShowOutClicked(btn_ShowIn,btn_ShowOut);
        });

        ShowState = true;
    }

    private void btn_ShowInClicked(GameObject btnIn,GameObject btnOut)
    {
        LeftUI.transform.DOLocalMove(new Vector3(-1118, -19, 0), 0.5f);
        //LeftUIShowButton.transform.DOLocalMove(new Vector3(-314, 0, 0), 0.5f);
        ShowState = false;
        btnIn.SetActive(false);
        btnOut.SetActive(true);

    }

    private void btn_ShowOutClicked(GameObject btnIn,GameObject btnOut)
    {
        LeftUI.transform.DOLocalMove(new Vector3(-800, -19, 0), 0.5f);
        //LeftUIShowButton.transform.DOLocalMove(new Vector3(0, 0, 0), 0.5f);
        ShowState = true;
        btnIn.SetActive(true);
        btnOut.SetActive(false);
    }
}