using UnityEngine;
using System.Collections;

public class JianTouMgr : MonoBehaviour 
{
    [HideInInspector]
    public Animator mAnim;
    public void Start() 
    {
        mAnim = this.GetComponent<Animator>();
        this.gameObject.SetActive(false);
    }
    public void Shut() 
    {
        mAnim.SetBool("Open", false);
        this.gameObject.SetActive(false);
    }
    public void Open()
    {
        this.gameObject.SetActive(true);
        mAnim.SetBool("Open", true);
    }
}
