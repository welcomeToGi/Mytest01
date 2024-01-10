using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleComponent : MonoBehaviour
{
    public ItemRoot itemRoot;
    public ToggleInfo toggleinfo;
    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener((b) =>
        {
            SetToggle(b);
        });

    }

    public void SetToggle(bool b)
    {
        if (toggleinfo != null)
        {
            if (toggleinfo.openGameObjects.Length != 0)
            {
                foreach (var item in toggleinfo.openGameObjects)
                {
                    DPIGameobjectEvent[] dPIGameobjectEvents= item.GetComponentsInChildren<DPIGameobjectEvent>();
                    foreach (var GameobjectEvent in dPIGameobjectEvents)
                    {
                        GameobjectEvent.GetComponent<DPIGameobjectEvent>().SetUITreeEvnet(!b);
                    }
                 
                    item.SetActive(!b);
                }
            }
            if (toggleinfo.offGameObjects.Length != 0)
            {
                foreach (var item in toggleinfo.offGameObjects)
                {
                    DPIGameobjectEvent[] dPIGameobjectEvents = item.GetComponentsInChildren<DPIGameobjectEvent>();
                    foreach (var GameobjectEvent in dPIGameobjectEvents)
                    {
                        GameobjectEvent.GetComponent<DPIGameobjectEvent>().SetUITreeEvnet(b);
                    }

                    item.SetActive(b);
                }
            }
        }
        if (toggleinfo.Islinkage)
        {
            ItemRoot[] ItemRoots = itemRoot.GetComponentsInChildren<ItemRoot>();
            foreach (var item in ItemRoots)
            {
                if (item != itemRoot)
                {
                    ToggleComponent[] toggles = item.treeItem.GetComponentsInChildren<ToggleComponent>();

                    foreach (var item2 in toggles)
                    {
                        if(item2.toggleinfo.Islinkage)
                            item2.GetComponent<Toggle>().isOn = b;
                    }

                }
            }
        }
    }
}
