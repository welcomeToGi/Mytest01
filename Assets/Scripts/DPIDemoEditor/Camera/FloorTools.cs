using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
public enum FloorType
{
    主楼,
    楼内物体,
}


public enum FloorModel
{
    层级,
    阵列,
}




public class FloorTools : MonoBehaviour
{

    public TreeView treeView;


    public static FloorTools Instance;

    public FloorModel floorModel = FloorModel.层级;
    public List<GameObject> FloorList = new List<GameObject>(); 

    [Header("楼层预设")]
    public GameObject FloorPrefab;

    [Header("楼层名称")]
    public string FloorName="楼";
    [Header("楼层数量")]
    public int FloorCount;
    [Header("每层间距")]
    public float Floorspacing;
    [Header("每层显示大小")]
    public Vector3 SelectScale = Vector3.one;

    public GameObject StartGameObject;
    private DPIGameobjectEvent _DPIGameobjectEvent;


    [Header("UI树分配节点")]
    [SerializeField]
    public TreeRoot treeRoot;

    public GameObject ArrayGameObject;


    [HideInInspector]
    public string FloorButtonName = "创建楼层";
    [Button("$FloorButtonName", ButtonSizes.Large)]
    public void CreateFloor()
    {
        if (FloorList.Count!=0)
        {
            foreach (var item in FloorList)
            {
                DestroyImmediate(item);
            }
            FloorList.Clear();
        }
        for (int i = 1; i < (FloorCount+1); i++)
        {
            var obj = Instantiate(FloorPrefab) as GameObject;
            obj.transform.SetParent(gameObject.transform,false);
            obj.transform.localPosition = new Vector3(0, 0 + Floorspacing* FloorList.Count, 0);
            obj.name = i.ToString()+ FloorName;
            _DPIGameobjectEvent = obj.GetComponent<DPIGameobjectEvent>();
            if (_DPIGameobjectEvent == null)
                _DPIGameobjectEvent = obj.gameObject.AddComponent<DPIGameobjectEvent>();
            MeshRenderer[] meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();
            foreach (var item in meshRenderers)
            {
                if (item.gameObject.GetComponent<MouseColliderEvent>() == null)
                {
                    item.gameObject.AddComponent<MouseColliderEvent>();
                }
            }

            if (obj.GetComponent<Floor>()!=null)
            {
                ItemRoot TmpItemRoot = treeRoot.AddItem();
                TmpItemRoot.treeItem.t.text= i.ToString() + FloorName;
                TmpItemRoot.TextClickEvnetObj = obj;
                SetTreeRoot(TmpItemRoot, obj);
            }
            FloorList.Add(obj);
        }
    }


    public float h;

    public float x = 0;
    public float z = 0;
    public float wh = 0;

    private bool isArray = false;


    [HideInInspector]
    public string arrayButtonName = "阵列展示";
    [Button("$arrayButtonName", ButtonSizes.Large)]
    public  void array()
    {
        z = 0;
        x = 0;
        wh = 0;
        isArray = false;
        foreach (var item in FloorList)
        {
            if (isArray)
                x += h;
            isArray = true;
            item.GetComponent<DPIGameobjectEvent>().SetScale(false, Vector3.zero);
            item.transform.localPosition = new Vector3(x, 0, z);
            wh++;
            if (wh==5)
            {
                x = 0;
                z += h;
                wh = 0;
                isArray = false;
            }
          
        }
        DPICameraManage.Instance.isFocusObjects2 = false;
        floorModel = FloorModel.阵列;
    
        DPICameraManage.Instance.LookAtAppointTarget(ArrayGameObject, 5);
        treeView.GetTreeRoot().itemRoots[0].TextClickEvnetObj = ArrayGameObject;
        treeView.GetTreeRoot().itemRoots[0].treeItem.showGameObject = ArrayGameObject;
    }


    [HideInInspector]
    public string arrayButtonName2 = "返回层级";
    [Button("$arrayButtonName2", ButtonSizes.Large)]
    public void arrayBack()
    {

        for (int i = 0; i < FloorList.Count; i++)
        {
            Debug.LogError(Floorspacing * FloorList.Count);
            FloorList[i].transform.localPosition = new Vector3(0,Floorspacing *i, 0);
        }
        floorModel = FloorModel.层级;
        // StartGameObject.GetComponent<DPIGameobjectEvent>().OnMouseDown();
        treeView.GetTreeRoot().itemRoots[0].TextClickEvnetObj = StartGameObject;
        treeView.GetTreeRoot().itemRoots[0].treeItem.showGameObject = StartGameObject;
        OnMouseDown(StartGameObject,true); 
    }


    private void SetTreeRoot(ItemRoot itemRoot, GameObject obj)
    {
        foreach (Transform item in obj.transform)
        {
            if (item.GetComponent<Floor>() != null)
            {
                ItemRoot TmpItemRoot = itemRoot.GetComponentInChildren<TreeRoot>().AddItem();
                TmpItemRoot.TextClickEvnetObj = item.gameObject;
                TmpItemRoot.treeItem.t.text = item.name;
                SetTreeRoot(TmpItemRoot, item.gameObject);
            }
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
  
    }
    public void OnEnable()
    {
        Instance = this;
        if (floorModel == FloorModel.层级)
            DPICameraManage.Instance.LookAtAppointTarget(StartGameObject, 5);
        if (floorModel == FloorModel.阵列)
            DPICameraManage.Instance.LookAtAppointTarget(ArrayGameObject, 5);
    }

    private GameObject SelectGameObject = null;

    private int Index = 0;
    public void OnMouseDown(GameObject obj,bool isInit=false)
    {

        Floor[] floors= obj.GetComponentsInParent<Floor>();

        foreach (var item in floors)
        {
            if (item.floorType== FloorType.主楼)
            {
                obj = item.gameObject;
            }
        }
        if (floorModel == FloorModel.层级)
        {
            if (SelectGameObject != null)
            {
                if (SelectGameObject.GetComponent<DPIGameobjectEvent>()!=null)
                {
                    SelectGameObject.GetComponent<DPIGameobjectEvent>().SetScale(false, Vector3.zero);
                    SelectGameObject.GetComponent<DPIGameobjectEvent>().SetPos(null);
                }
              
            }
            obj.GetComponent<DPIGameobjectEvent>().SetScale(true, SelectScale);

            foreach (var item in FloorList)
            {
                item.GetComponent<DPIGameobjectEvent>().SetPos(null);
            }
            if (isInit)
            {
                SelectGameObject = null;
               
                DPICameraManage.Instance.isFocusObjects2 = false;
                DPICameraManage.Instance.LookAtAppointTarget(StartGameObject, 5);
                return;
            }

        }

        SelectGameObject = obj;
        bool isPos = true;


        for (int i = 0; i < FloorList.Count; i++)
        {
            if (FloorList[i] == obj)
            {
                isPos = false;
                Index = i;
                continue;
            }
            if (floorModel == FloorModel.层级)
            {
                if (isPos)
                {
                    var v = FloorList[i].transform.localPosition;
                    FloorList[i].transform.localPosition = new Vector3(v.x, v.y - 10, v.z);

                }
                else
                {
                    var v = FloorList[i].transform.localPosition;
                    FloorList[i].transform.localPosition = new Vector3(v.x, v.y + 10, v.z);
                }
            }
        }

        if (floorModel == FloorModel.层级)
        {
            DPICameraManage.Instance.isFocusObjects2 = true;
        }
        if (floorModel == FloorModel.阵列)
        {
            DPICameraManage.Instance.isFocusObjects2 = false;
        }
        DPICameraManage.Instance.LookAtAppointTarget(obj, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (floorModel == FloorModel.层级&& SelectGameObject!=null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                int n = Index - 1;
                if (n < 0)
                {
                    n = 0;
                }
                FloorList[n].GetComponent<DPIGameobjectEvent>().OnMouseDown();
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                int n = Index + 1;
                if (n > FloorList.Count - 1)
                {
                    n = FloorList.Count - 1;
                }
                FloorList[n].GetComponent<DPIGameobjectEvent>().OnMouseDown();
            }
        }
    }
}
