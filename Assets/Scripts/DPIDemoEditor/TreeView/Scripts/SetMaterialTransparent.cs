using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialTransparent : MonoBehaviour
{

    private Material[] tempMaterials;
    private MeshRenderer thisRenderer;
    public Material TranMaterial;
    private Material[] InitMaterials;
    // Start is called before the first frame update
    void Start()
    {

        TranMaterial = Resources.Load<Material>("透明");
        thisRenderer = gameObject.GetComponent<MeshRenderer>();
        if (thisRenderer==null)
        {
            return;
        }
        InitMaterials = thisRenderer.materials;
        tempMaterials = new Material[InitMaterials.Length];
        for (int i = 0; i < tempMaterials.Length; i++)
        {
            tempMaterials[i] = TranMaterial;
        }
    }
    public  void isTransparent(bool IsTran)
    {
        if (thisRenderer == null)
        {
            return;
        }
        if (IsTran)
        {
            thisRenderer.materials = tempMaterials;
        }
        else
        {
            thisRenderer.materials = InitMaterials;
        }

        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        if (boxCollider != null)
            boxCollider.enabled = !IsTran;
        if (meshCollider != null)
            meshCollider.enabled = !IsTran;
    }

}
