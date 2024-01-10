using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.ProBuilder;
using Debug = UnityEngine.Debug;


// 鼠标所处状态
public enum DPICursorState
{
    Viewing,
    UIOperate,
}

// 摄像机控制脚本
public class DPICameraManage : MonoBehaviour
{
    [Header("缩放后退")]
    public float scaleMax = -200;
    [Header("缩放前进")]
    public float scaleMin = 20;
    public bool IsOpenCameraControl = true;


    #region 属性
    public Transform LookAtTarget;                                          //摄像机看向的目标

    public GameObject cameraVirtualFocus;

    #endregion

    #region ------鼠标状态------
    // 当前鼠标状态
    public DPICursorState curCursorState = DPICursorState.Viewing;
    #endregion

    #region ------引用信息------
    // 实际摄像机视野
    private float curField_of_view = 40;
    #endregion

    #region ------旋转/平移/缩放参数------
    // 缩放倍率
    public float scaleSpeed = 5.0f;
    // 旋转速率
    public float rotateSpeed = 5.0f;  // 航向   
    // 移动速率
    public bool isAllowMove = true;
    public float moveSpeed = 2f;

    // 统一的速率缩放倍率
    [Range(0.1f, 5)]
    public float speedModulus = 1.0f;

    // 平移旋转时的延迟系数（阻尼/惯性效果）  
    public float moveDelay = 1.0f;      // 平移  
    public float rotateDelay = 1.0f;    // 旋转




    public float autoSpeedModulus = 1f;
    #endregion

    public bool isRot = false;
    public bool isFocusObjects = false;

    public GameObject Plane;

    #region 生命周期
    public static DPICameraManage Instance;




    private Camera mycamera;

    private Tween mycameraTweenDOMove;
    private Tween mycameraTweenDORot;

    private Tween cameraVirtualFocusTweenDOMove;
    private Tween cameraVirtualFocusTweenDOMove1;
    private Tween cameraVirtualFocusTweenDOMove2;
    private float scaleMax2 = 200;
    private CameraLookAt cameraLookAt;
    private float scaleMin2 = 20;

    bool isCameraMoving = false;
    bool IsCameraMoving
    {
        get
        {
            return isCameraMoving;
        }
        set
        {
            if (isCameraMoving == value)
                return;
            isCameraMoving = value;
            IsCurCameraControling = value;
        }
    }
    bool isCameraRotating = false;
    bool IsCameraRotating
    {
        get
        {
            return isCameraRotating;
        }
        set
        {
            if (isCameraRotating == value)
                return;
            isCameraRotating = value;
            IsCurCameraControling = value;
        }
    }
    bool isCameraScaling = false;
    bool IsCameraScaling
    {
        get
        {
            return isCameraScaling;
        }
        set
        {
            if (isCameraScaling == value)
                return;
            isCameraScaling = value;
            IsCurCameraControling = value;
        }
    }
    bool isFocusing = false;
    bool IsFocusing
    {
        get
        {
            return isFocusing;
        }
        set
        {
            if (isFocusing == value)
                return;
            isFocusing = value;
            IsCurCameraControling = value;
        }
    }
    //摄像机是否在被控制 
    bool isCurCameraControling = false;
    bool IsCurCameraControling
    {
        get
        {
            return isCurCameraControling;
        }
        set
        {
            if (isCurCameraControling == value)
                return;
            isCurCameraControling = IsCameraMoving || IsCameraRotating || IsCameraScaling || IsFocusing;
        }
    }
    public void Start()
    {

    }


    public void OnEnable()
    {
        Instance = this;
        mycamera = transform.GetComponentInChildren<Camera>();
        curField_of_view = mycamera.fieldOfView;
        scaleMax2 = this.scaleMax;
        scaleMin2 = this.scaleMin;
        Init();

    }
    public void Awake()
    {
        mycamera = transform.GetComponentInChildren<Camera>();
        curField_of_view = mycamera.fieldOfView;
        Instance = this;
        scaleMax2 = this.scaleMax;
        scaleMin2 = this.scaleMin;
    }

    private void Update()
    {
        // 判断是否处在UI上面，如果处在UI上面，则鼠标的操作不能对场景产生影响
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    curCursorState = DPICursorState.UIOperate;
        //}
        //else
        //{
        //    curCursorState = DPICursorState.Viewing;
        //}
    }


    private void LateUpdate()
    {
        TranslationKeyCode();
        DPI_MouseScale();
        DPI_MouseRotation();
        if (isFocusObjects == false && isAllowMove == true)
        {
            DPI_MouseMove();
        }
    }

    #endregion

    private GameObject LookAtobj;
    #region Other
    /// <summary>
    /// 摄像机看向指定物体的方法
    /// </summary>
    public void LookAtAppointTarget(GameObject lookAtobj, float Cameraheight, float moveTime = 1.0f, bool isRot = true)
    {


        if (lookAtobj == null) return;
        Vector3 centerPoint = Vector3.zero;
        centerPoint = lookAtobj.transform.position;
        //if (lookAtobj.transform.name == "InitView")
        //{
        //    centerPoint = lookAtobj.transform.position;
        //}
        //else
        //{
        //    centerPoint = FocusToTarget(new List<GameObject>() { lookAtobj });
        //}

        LookAtobj = lookAtobj;
        isFocusObjects = true;

        LookAtTarget = LookAtobj.transform;
        if (mycameraTweenDOMove != null)
        {
            mycameraTweenDOMove.Kill();
            mycameraTweenDORot.Kill();
            cameraVirtualFocusTweenDOMove.Kill();
            cameraVirtualFocusTweenDOMove1.Kill();
            cameraVirtualFocusTweenDOMove2.Kill();
        }
        cameraLookAt = LookAtobj.GetComponent<CameraLookAt>();

        ////让摄像机 看 物体 
        if (isRot)
            cameraVirtualFocusTweenDOMove2 = transform.DORotate(cameraVirtualFocus.transform.rotation.eulerAngles, 1f);
        ////让摄像机 移动到 物体 
        cameraVirtualFocusTweenDOMove = transform.DOMove(cameraVirtualFocus.transform.position, 0).OnComplete(() =>
        {
            mycamera.transform.SetParent(this.transform);
            //  mycamera.transform.localPosition = new Vector3(0, mycamera.transform.localPosition.y, mycamera.transform.localPosition.z); 
            //cameraZ = mycamera.transform.localPosition.z;
            //Debug.Log(cameraZ);
            isFocusObjects = false;
        });
        if (cameraLookAt != null)
        {
            cameraLookAt.isLooAk = true;
            //mycamera.transform.SetParent(null);
            cameraVirtualFocus.transform.position = centerPoint;
            if (isRot)
                cameraVirtualFocus.transform.rotation = cameraLookAt.Rot;
            mycameraTweenDORot = transform.DORotate(cameraLookAt.Rot.eulerAngles, moveTime);
            mycameraTweenDOMove = mycamera.transform.DOMove(cameraLookAt.pos, moveTime).OnComplete(() =>
            {
                cameraZ = mycamera.transform.localPosition.z;

                Debug.Log(cameraZ);
            });
            scaleMax2 = cameraLookAt.scaleMax;
            scaleMin2 = cameraLookAt.scaleMin;
        }
        else
        {
            scaleMax2 = this.scaleMax;
            scaleMin2 = this.scaleMin;
            cameraVirtualFocus.transform.position = centerPoint;
            if (isRot)
                cameraVirtualFocus.transform.rotation = LookAtobj.transform.rotation;
        }
        if (isRot == false)
        {
            cameraVirtualFocusTweenDOMove1 = mycamera.transform.DOLocalMove(new Vector3(0, 0, -60), moveTime).OnComplete(() => { });
        }
    }

    private float cameraZ = 0;

    public Vector3 FocusToTarget(List<GameObject> _targetObjs)
    {

        if (_targetObjs == null || _targetObjs.Count == 0)
            return Vector3.zero;
        List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

        for (int i = 0; i < _targetObjs.Count; i++)
        {
            if (_targetObjs[i] == null)
                continue;
            MeshRenderer[] _mrs = _targetObjs[i].GetComponentsInChildren<MeshRenderer>(true);
            if (_mrs != null)
            {
                for (int j = 0; j < _mrs.Length; j++)
                {
                    meshRenderers.Add(_mrs[j]);
                }
            }
        }

        if (meshRenderers.Count == 0)
            return Vector3.zero;
        if (meshRenderers.Count == 1)
        {
            BoxCollider boxCollider = meshRenderers[0].gameObject.GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                boxCollider = meshRenderers[0].gameObject.AddComponent<BoxCollider>();
            }
            //if (meshRenderers[0].gameObject.GetComponent<PolyShape>()!=null)
            //{
            //    boxCollider.center = Vector3.one;
            //    return GetBoxcolliderCenter(boxCollider);
            //}
        }


        //  SetBoxCollider(_targetObjs[0].transform);


        Bounds centerBounds = meshRenderers[0].bounds;

        for (int i = 1; i < meshRenderers.Count; i++)
        {
            centerBounds.Encapsulate(meshRenderers[i].bounds);
        }

        Vector3 centerPoint = centerBounds.center;
        return centerPoint;
    }
    private Vector3 center = Vector3.zero;
    private Vector3 GetBoxcolliderCenter(BoxCollider boxcollider)
    {

        Vector3[] veces = GetBoxColliderVertexPositions(boxcollider);

        for (int i = 0; i < veces.Length; i++)
        {
            center += veces[i];
        }
        center /= 8;
        Bounds bounds = new Bounds(center, Vector3.zero);
        for (int i = 0; i < veces.Length; i++)
        {
            bounds.Encapsulate(veces[i]);
        }
        return bounds.center;
    }




    private Vector3[] GetBoxColliderVertexPositions(BoxCollider boxcollider)
    {
        var vertices = new Vector3[8];
        //下面4个点
        vertices[0] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, -boxcollider.size.y, boxcollider.size.z) * 0.5f);
        vertices[1] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(-boxcollider.size.x, -boxcollider.size.y, boxcollider.size.z) * 0.5f);
        vertices[2] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(-boxcollider.size.x, -boxcollider.size.y, -boxcollider.size.z) * 0.5f);
        vertices[3] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, -boxcollider.size.y, -boxcollider.size.z) * 0.5f);
        //上面4个点
        vertices[4] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, boxcollider.size.y, boxcollider.size.z) * 0.5f);
        vertices[5] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(-boxcollider.size.x, boxcollider.size.y, boxcollider.size.z) * 0.5f);
        vertices[6] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(-boxcollider.size.x, boxcollider.size.y, -boxcollider.size.z) * 0.5f);
        vertices[7] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, boxcollider.size.y, -boxcollider.size.z) * 0.5f);

        return vertices;


    }


    private void SetBoxCollider(Transform f)
    {
        Transform parent = f;
        Vector3 postion = parent.position;
        Quaternion rotation = parent.rotation;
        Vector3 scale = parent.localScale;
        parent.position = Vector3.zero;
        parent.rotation = Quaternion.Euler(Vector3.zero);
        parent.localScale = Vector3.one;

        Collider[] colliders = parent.GetComponentsInChildren<Collider>();
        foreach (Collider child in colliders)
        {
            DestroyImmediate(child);
        }
        Vector3 center = Vector3.zero;
        Renderer[] renders = parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer child in renders)
        {
            center += child.bounds.center;
        }
        center /= parent.childCount;
        Bounds bounds = new Bounds(center, Vector3.zero);
        foreach (Renderer child in renders)
        {
            bounds.Encapsulate(child.bounds);
        }
        BoxCollider boxCollider = parent.gameObject.AddComponent<BoxCollider>();
        boxCollider.center = bounds.center - parent.position;
        boxCollider.size = bounds.size;

        parent.position = postion;
        parent.rotation = rotation;
        parent.localScale = scale;
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////////


    public bool isFocusObjects2 = false;
    private Touch oldTouch1;
    private Touch oldTouch2;

    #region 鼠标缩放
    public void DPI_MouseScale()
    {
        if (isFocusObjects2) return;
        if (curCursorState == DPICursorState.Viewing && isFocusObjects == false)
        {

            if (Input.touchCount == 2)
            {
                //多点触摸, 放大缩小  
                Touch newTouch1 = Input.GetTouch(0);
                Touch newTouch2 = Input.GetTouch(1);

                //第2点刚开始接触屏幕, 只记录，不做处理  
                if (newTouch2.phase == TouchPhase.Began)
                {
                    oldTouch2 = newTouch2;
                    oldTouch1 = newTouch1;
                    return;
                }

                //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

                //两个距离之差，为正表示放大手势， 为负表示缩小手势  
                float offset = newDistance - oldDistance;

                //放大因子， 一个像素按 0.01倍来算(100可调整)  
                float scaleFactor = offset / 100f;

                if (scaleFactor < 0)
                {
                    float z = mycamera.transform.localPosition.z;
                    Vector3 pos = mycamera.transform.localPosition;
                    z -= scaleSpeed;
                    pos.z = z;
                    if (z < cameraZ + (scaleMax2 * -1))
                    {
                        pos.z = cameraZ + (scaleMax2 * -1);
                    }
                    mycamera.transform.localPosition = pos;
                }
                if (scaleFactor > 0)
                {
                    float z = mycamera.transform.localPosition.z;

                    Vector3 pos = mycamera.transform.localPosition;
                    z += scaleSpeed;
                    pos.z = z;
                    if (z > cameraZ + scaleMin2)
                    {
                        pos.z = cameraZ + scaleMin2;
                    }
                    mycamera.transform.localPosition = pos;
                }
                //记住最新的触摸点，下次使用  
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;

            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                float z = mycamera.transform.localPosition.z;
                Vector3 pos = mycamera.transform.localPosition;
                z -= scaleSpeed;
                pos.z = z;
                if (z < cameraZ + (scaleMax2 * -1))
                {
                    pos.z = cameraZ + (scaleMax2 * -1);
                }
                mycamera.transform.localPosition = pos;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                float z = mycamera.transform.localPosition.z;

                Vector3 pos = mycamera.transform.localPosition;
                z += scaleSpeed;
                pos.z = z;
                if (z > cameraZ + scaleMin2)
                {
                    pos.z = cameraZ + scaleMin2;
                }
                mycamera.transform.localPosition = pos;
            }
        }
    }
    #endregion

    #region 鼠标旋转
    public void DPI_MouseRotation()
    {
        if (isFocusObjects == false && curCursorState == DPICursorState.Viewing)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                var mouse_x = Input.GetAxis("Mouse X") * rotateSpeed;//获取鼠标X轴移动
                var mouse_y = Input.GetAxis("Mouse Y") * rotateSpeed;//获取鼠标Y轴移动

                IsCameraRotating = !(mouse_y == 0 && mouse_x == 0);

                Vector3 thirdPCamEuler = transform.localEulerAngles;
                float minAngle = getAngle();
                thirdPCamEuler.x = RotateClampX(gameObject.transform, mouse_y, -90, minAngle);
                thirdPCamEuler.y += mouse_x;
                thirdPCamEuler.z = 0; //

                transform.DOLocalRotate(thirdPCamEuler, 0.001f);
            }

            if (Input.GetKeyUp(KeyCode.Mouse1))
                IsCameraRotating = false;
        }
    }
    public float RotateClampX(Transform t, float degree, float min, float max)
    {
        degree = (t.localEulerAngles.x - degree);
        if (degree > 180f)
        {
            degree -= 360f;
        }
        degree = Mathf.Clamp(degree, min, max);
        return degree;
    }


    private float getAngle()
    {
        float angle = 0;
        Vector3 pos = transform.position - Plane.transform.position;
        Vector3 axis = Plane.transform.up;
        float dis = Vector3.Dot(pos, axis);
        //Debug.Log(dis);
        if (dis < Mathf.Abs(mycamera.transform.localPosition.z))
            angle = 90 - Mathf.Acos(dis / Mathf.Abs(mycamera.transform.localPosition.z)) * Mathf.Rad2Deg -5;
        else
            angle = 80;
        Debug.Log("--------------- angle = " + angle);
        return angle;
    }
    #endregion

    #region 鼠标移动
    float hor = 0;
    float ver = 0;
    public void DPI_MouseMove()
    {
        if (curCursorState == DPICursorState.Viewing)
        {
            if (IsOpenCameraControl)
            {
                if(Input.GetMouseButtonDown(2))
                {
                    
                }
                //if (Input.GetKey(KeyCode.Mouse0))
                if (Input.GetMouseButton(2))
                {
                    // 获得鼠标当前位置的X和Y
                    hor = Input.GetAxis("Mouse X");//获取鼠标X轴移动
                    ver = Input.GetAxis("Mouse Y");//获取鼠标Y轴移动
                    float y = ver * moveSpeed;
                    float x = hor * moveSpeed;
                    // 平移
                    IsCameraMoving = !(y == 0 && x == 0);
                    autoSpeedModulus = Mathf.Clamp(Mathf.Abs(mycamera.transform.localPosition.z) * 0.01f, 0.1f, 1f);
                    Vector3 translation = new Vector3(-x, -y, 0) * autoSpeedModulus;
                    cameraVirtualFocus.transform.Translate(translation, mycamera.transform);
                    // 根据地面位置, 限制摄像机移动, 防止摄像机移动到地面下
                    // 当摄像机世界坐标y值小于地面世界坐标y值时, 摄像机穿帮, 
                    float dis = Plane.transform.position.y - mycamera.transform.localPosition.y + 1;
                    float virtualY = Mathf.Clamp(cameraVirtualFocus.transform.position.y, dis, 1000);
                    cameraVirtualFocus.transform.position = new Vector3(cameraVirtualFocus.transform.position.x, virtualY, cameraVirtualFocus.transform.position.z);
                    //Debug.Log(GetVirtualFocusDis());
                }
                if (Input.GetMouseButtonUp(2))
                {
                    IsCameraMoving = false;
                }
            }
        }
        // 实际平移差值
        transform.position = Vector3.Lerp(transform.position, cameraVirtualFocus.transform.position, 0.1f * moveDelay);
    }

    private float GetVirtualFocusDis()
    {
        Vector3 relativePos = mycamera.transform.position - Plane.transform.position;
        
        Vector3 planeUp = Plane.transform.up;  
        // dis > 0 表示在地上
        float dis = Vector3.Dot(planeUp, relativePos);

        //dis -= mycamera.transform.localPosition.y;
        return dis;
    }


    private float CamAtPlane()
    {
        Vector3 camPos = new Vector3(mycamera.transform.position.x, Plane.transform.position.y, mycamera.transform.position.z);
        //Vector3 virtualPos = new Vector3(cameraVirtualFocus.transform.position.x, camPos.y - mycamera.transform.localPosition.y, cameraVirtualFocus.transform.position.z);

        Vector3 pos = camPos - transform.position;
        float angle = Vector3.Angle(pos, Plane.transform.position);
        Debug.Log(angle);
        return angle;
    }
    #endregion

    #region wasd移动
    private float yaw;
    private float pitch;
    private float roll;

    private void Init()
    {
        pitch = transform.eulerAngles.x;
        yaw = transform.eulerAngles.y;
        roll = transform.eulerAngles.z;
    }
    private void TranslationKeyCode()
    {
        var h = Input.GetAxis("Horizontal") * 0.1f;
        var v = Input.GetAxis("Vertical") * 0.1f;
        var u = Input.GetAxis("UPDown") * 0.1f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //原速度*10为按下Shift后的速度
            h *= 5.0f;
            v *= 5.0f;
            u *= 5.0f;
        }
        cameraVirtualFocus.transform.Translate(new Vector3(h, u, v), mycamera.transform);

        if (Input.GetKey(KeyCode.I))
        {
            pitch -= 10 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.K))
        {
            pitch += 10 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.J))
        {
            yaw -= 10 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.L))
        {
            yaw += 10 * Time.deltaTime;
        }
        pitch = Mathf.Lerp(pitch, transform.eulerAngles.x, 1);
        yaw = Mathf.Lerp(yaw, transform.eulerAngles.y, 1);
        mycamera.transform.eulerAngles = new Vector3(pitch, yaw, 0);
    }
    #endregion

}