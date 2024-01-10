using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class MouseColliderEvent : MonoBehaviour
{

    public DPIGameobjectEvent dPIGameobjectEvent;
    // Start is called before the first frame update
    void Start()
    {
          
    }
	private void Update()
	{
		
	}


	public void OnMouseDown()
    {
        DPIGameobjectEvent[] dPIGameobjectEvents = gameObject.GetComponentsInParent<DPIGameobjectEvent>();
        if (dPIGameobjectEvents.Length != 0)
        {

            dPIGameobjectEvent = dPIGameobjectEvents[0];

            dPIGameobjectEvent.OnMouseDown();
        }
        else
        {
         
        }
    }
}
