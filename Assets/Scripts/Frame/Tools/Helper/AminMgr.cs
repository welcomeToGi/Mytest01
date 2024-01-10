using UnityEngine;
using System.Collections;
using UnityEngine.Events;


public class AminMgr : MonoBehaviour 
{
    Animator anim;
    UnityAction ExitAction;

    public GameObject KongZhiMoXing;
    public void Start()
    {
        this.gameObject.SetActive(true);
        anim = this.GetComponent<Animator>();
    }
    public void Open(string AnimString,bool AnimBool,UnityAction action) 
    {
        if (anim == null)
            Start();
        print(action);
        ExitAction = action;
        anim.SetBool(AnimString, AnimBool);
    }
    public void Shut()
    {
        print(ExitAction);
        if (ExitAction != null)
        {
            ExitAction.Invoke();
        }
    }
}
