using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AddMeshCollider : Editor
{




    [MenuItem("Tools/AddMeshCollider")]
    public static void AddCollider()
    {
        Transform tform = Selection.activeTransform;
        Debug.Log(tform.name);
        MeshRenderer[] meshRenderers = tform.GetComponentsInChildren<MeshRenderer>();


        foreach (var item in meshRenderers)
        {
            if (item.gameObject.GetComponent<MeshCollider>() == null)
                item.gameObject.AddComponent<MeshCollider>();
        }

        Debug.Log("AddMeshCollider 完成");

    }




}
