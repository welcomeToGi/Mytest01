using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAtObj : MonoBehaviour
{


    public bool isLooAk = false;
    public Vector3 pos;
    public Quaternion Rot;

    [Header("缩放后退")]
    public float scaleMax = 200;
    [Header("缩放前进")]
    public float scaleMin = 20;
    // Start is called before the first frame update
    void Start()
    {

    }
  
// Update is called once per frame
    void Update()
    {
       
    }
}
