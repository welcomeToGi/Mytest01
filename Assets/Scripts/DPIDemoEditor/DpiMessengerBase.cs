using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public abstract class DpiMessengerBase : MonoBehaviour
{

    public DpiMessengerBase messgerBase;

    /// <summary>
    /// ∑¢ÀÕ
    /// </summary>
    /// <param name="infos"></param>
    public virtual void SendInfo()
    {

    }

    /// <summary>
    /// Ω” ’
    /// </summary>
    /// <param name="infos"></param>
    public virtual void ReceInfo(List<Info> infos)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class Info
{

}
