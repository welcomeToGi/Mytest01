using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPosToScreen : MonoBehaviour
{

    public Transform Obj;

    Vector3 objPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        objPos = Obj.position;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(objPos);
        transform.position = screenPos; 
    }
}
