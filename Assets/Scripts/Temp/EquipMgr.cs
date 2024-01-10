using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipMgr : MonoBehaviour
{
    private static  EquipMgr instance;
    public static EquipMgr Instance => instance;
	private void Awake()
	{
		instance = this;
	}
	void Start()
    {
        //Vector3 center = GetCenter(transform.XuYiFindChild("GameObject"));
        //Debug.Log("计算物体的中心点" + center);
        //transform.XuYiFindChild("Sphere").position = center;
        InitView();
    }
    public void InitView()
    { 
        //Camera.main.GetComponent<CameraMotion>().LookAtObj(transform.XuYiFindChild("场站总览_聚焦").GetComponent<CameraLookAt>());
    }
    public static Vector3 GetCenter(Transform tt)
    {

        Transform parent = tt;

        Vector3 postion = parent.position;

        Quaternion rotation = parent.rotation;

        Vector3 scale = parent.localScale;

        parent.position = Vector3.zero;

        parent.rotation = Quaternion.Euler(Vector3.zero);

        parent.localScale = Vector3.one;

        Vector3 center = Vector3.zero;

        Renderer[] renders = parent.GetComponentsInChildren<Renderer>();

        foreach (Renderer child in renders)
        {
            center += child.bounds.center;
        }

        center /= parent.GetComponentsInChildren<Renderer>().Length;

        Bounds bounds = new Bounds(center, Vector3.zero);

        foreach (Renderer child in renders)
        {

            bounds.Encapsulate(child.bounds);

        }

        parent.position = postion;

        parent.rotation = rotation;

        parent.localScale = scale;

        foreach (Transform t in parent)
        {

            t.position = t.position - bounds.center;

        }
        parent.transform.position = bounds.center + parent.position;
        return parent.transform.position;
    }
}
