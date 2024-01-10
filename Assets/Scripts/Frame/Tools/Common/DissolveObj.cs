using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// …Ë÷√ŒÔÃÂΩ•“˛Ω•œ‘
public class DissolveObj : MonoBehaviour
{
    private MeshRenderer[] meshs;
    private List<Material> materials;
    private void Awake()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        materials = new List<Material>();
        Init();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        for (int i = 0; i < meshs.Length; i++)
        {
            Material[] mats = meshs[i].materials;
            for (int j = 0; j < mats.Length; j++)
            {
                materials.Add(mats[j]);
            }
        }
        if (transform.GetComponent<MeshRenderer>() != null)
        {
            for(int i = 0; i < GetComponent<MeshRenderer>().materials.Length; i++)
            {
                materials.Add(GetComponent<MeshRenderer>().materials[i]);
            }
        }
    }

    public void FadeIn(TweenCallback action = null)
    {
        gameObject.SetActive(true);
        SetMatColor(0, 0);
        SetMatColor(1, 1f, action);
    }

    public void FadeOut(TweenCallback action = null)
    {
        SetMatColor(0, 1f, action);
    }

    private void SetMatColor(float a, float time, TweenCallback action = null)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            Material mat = materials[i];
            Color color = mat.color;
            mat.DOColor(new Color(color.r, color.g, color.b, a), time).OnComplete(action);
        }
    }
}
