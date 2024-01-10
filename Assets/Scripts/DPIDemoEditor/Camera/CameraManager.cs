using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



// 摄像机控制脚本
public class CameraManager : MonoBehaviour
{
    [Header("缩放后退")]
    public float scaleMax = -200;
    [Header("缩放前进")]
    public float scaleMin = 20;

    public bool IsOpenCameraControl = true;

    public List<GameObject> TestTargetObj = new List<GameObject>();

    [Header("地面的水平面高度")]
    public float planeZeroHeight = 0;

    #region 属性
    public Transform LookAtTarget;                                          //摄像机看向的目标
    public float CameraDistance = 6.0F;                                     //摄像机与看向目标的距离
    public float CameraHeight = 3.0F;                                       //摄像机高度
    public float CmaeraOffset = 1.0F;                                       //摄像机的偏移
    public float MainCameraMoveSpeed = 2F;                                  //主摄像机移动的速度
    public float WaitTime = 0F;                                             //等待摄像机移动到设备附近的时间

    public bool IsLookAtAppointTarget = false;                             //是否看向指定的物体

    public GameObject cameraVirtualFocus;
    public Transform CameraRotAroundPoint;

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
    public float autoScaleSpeed = 5.0f;
    // 旋转速率
    public float rotateSpeed = 5.0f;  // 航向   
    // 移动速率
    public bool isAllowMove = true;
    public float moveSpeed = 2f;

    // 统一的速率缩放倍率
    [Range(0.1f, 5)]
    public float speedModulus = 1.0f;

    public float autoSpeedModulus = 1f;

    // 平移旋转时的延迟系数（阻尼/惯性效果）  
    public float moveDelay = 1.0f;      // 平移  
    public float rotateDelay = 1.0f;    // 旋转

    #endregion

    public bool isRot = false;
    public bool isFocusObjects = false;

    #region 生命周期
    public static CameraManager Instance;

    private Camera mycamera;

    private Tween mycameraTweenDOMove;
    private Tween mycameraTweenDORot;

    private Tween cameraVirtualFocusTweenDOMove;
    private Tween cameraVirtualFocusTweenDOMove1;
    private Tween cameraVirtualFocusTweenDOMove2;
    private float scaleMax2 = 200;

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
            //if (isCurCameraControling)
            //{
            //    GlobalLODGroupManager.Instance.Check(false);
            //    CustomAssetDownloader.Instance.StopLoading();
            //}
            //else
            //{
            //    GlobalLODGroupManager.Instance.Check(true);
            //    CustomAssetDownloader.Instance.StartLoading();
            //}
        }
    }
    public void Start()
    {
        // StartCoroutine(LaUpdate());
    }


    public void OnEnable()
    {
        Instance = this;
        mycamera = transform.GetComponentInChildren<Camera>();
        curField_of_view = mycamera.fieldOfView;
        scaleMax2 = this.scaleMax;
        scaleMin2 = this.scaleMin;
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
        //// 判断是否处在UI上面，如果处在UI上面，则鼠标的操作不能对场景产生影响
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
        if (IsOpenCameraControl &&!IsFocusing)
        {
            DPI_MouseScale();
            DPI_MouseRotation();
        }

        if (isFocusObjects == false && isAllowMove == true)
        {
            DPI_MouseMove();
            DPI_KeyboardMove();
        }
    }

    WaitForSeconds seconds = new WaitForSeconds(0.1f);
    public IEnumerator LaUpdate()
    {
        while (true)
        { 
            DPI_MouseScale();
            DPI_MouseRotation();
            if (isFocusObjects == false && isAllowMove == true)
            {
                DPI_MouseMove();
            }

            yield return seconds;
        }
    }


    #endregion


    private GameObject _LookAtobj;
    #region Other


    private List<Action<bool>> Actions = new List<Action<bool>>(); 
   

    /// <summary>
    /// 摄像机看向指定物体的方法
    /// </summary>
    public void LookAtAppointTarget(GameObject lookAtobj, float Cmaerheight,Action<bool> FucAction, bool isRot = true)
    {
        if (lookAtobj == null) return;
        if (_LookAtobj!=null && _LookAtobj !=lookAtobj)
        {

            //for (int i = 0; i < Actions.Count; i++)
            //{
            //   // Actions[i].Invoke(false);
            //}
          //  Actions.Clear();
          //  if (_LookAtobj.GetComponent<Highlighter>()!=null)
            //    _LookAtobj.GetComponent<Highlighter>().constant = false;
        }
       // Actions.Add(FucAction);
        _LookAtobj = lookAtobj;
        isFocusObjects = true;
        LookAtTarget = _LookAtobj.transform;
        if (mycameraTweenDOMove != null)
        {
            mycameraTweenDOMove.Kill();
            mycameraTweenDORot.Kill();
            cameraVirtualFocusTweenDOMove.Kill();
            cameraVirtualFocusTweenDOMove1.Kill();
            cameraVirtualFocusTweenDOMove2.Kill();
        }


     
            scaleMax2 = this.scaleMax;
            scaleMin2 = this.scaleMin;
            cameraVirtualFocus.transform.position = _LookAtobj.transform.position;
            if (isRot)
                cameraVirtualFocus.transform.rotation = _LookAtobj.transform.rotation;
        

        if (isRot == false)
        {
            cameraVirtualFocusTweenDOMove1 = mycamera.transform.DOLocalMove(new Vector3(0, 0, -60), 1f).OnComplete(() =>
            {

            });
        }
        ////让摄像机 移动到 物体 
        cameraVirtualFocusTweenDOMove = transform.DOMove(cameraVirtualFocus.transform.position, 1f).OnComplete(() =>
        {
            mycamera.transform.SetParent(gameObject.transform);
            cameraZ = mycamera.transform.localPosition.z;
            isFocusObjects = false;
            FucAction?.Invoke(true);
        });

        ////让摄像机 看 物体 
        if (isRot)
            cameraVirtualFocusTweenDOMove2 = transform.DORotate(cameraVirtualFocus.transform.rotation.eulerAngles, 1f);

    }

    Tweener focusTweener;
    Tweener mycameraTweener;
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
    //摄像机聚焦到目标点
    public void FocusToTarget(List<GameObject> _targetObjs,Action<Bounds> _callBack)
    {
        if (_targetObjs == null || _targetObjs.Count == 0)
            return;
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
            return;
        Bounds centerBounds = meshRenderers[0].bounds;
        bool isUnderPlane = centerBounds.center.y < planeZeroHeight;
        for (int i = 1; i < meshRenderers.Count; i++)
        {
            Bounds _temp = meshRenderers[i].bounds;
            if (_temp.center.y < planeZeroHeight)
                isUnderPlane = true;
            centerBounds.Encapsulate(_temp);
        }

        Vector3 centerPoint = centerBounds.center;
        float dis = Vector3.Distance(centerBounds.center, centerBounds.max);
        Vector3 cameraPoint = centerPoint + transform.rotation.eulerAngles.normalized * dis;

        _callBack?.Invoke(centerBounds);

        IsFocusing = true;
        if (focusTweener != null)
            focusTweener.Kill();
        focusTweener = cameraVirtualFocus.transform.DOMove(centerPoint, 1)/*.SetEase(Ease.InOutQuad)*/;
        if (mycameraTweener != null)
            mycameraTweener.Kill();
        mycameraTweener = mycamera.transform.DOLocalMoveZ(-dis * 2f, 1)/*.SetEase(Ease.InOutQuad)*/.OnComplete(()=> { IsFocusing = false;
        });
    }

    public void SetCameraPosRotScl(Vector3 _pos,Vector3 _rot,float _zoom)
    {
        cameraVirtualFocus.transform.localPosition = _pos;
        //cameraVirtualFocus.transform.DOLocalMove(_pos,1f);
        transform.DOLocalRotate(_rot,1f);
        mycamera.transform.DOLocalMoveZ(_zoom,1f);
    }

    private float cameraZ = 0;


    #endregion

    /////////////////////////////////////////////////////////////////////////////////////


    public bool isFocusObjects2 = false;

    #region 鼠标缩放
    //public void DPI_MouseScale()
    //{
    //    if (isFocusObjects2) return;
    //    if (curCursorState == DPICursorState.Viewing && isFocusObjects == false)
    //    {

    //        autoScaleSpeed = Mathf.Clamp(Mathf.Abs(mycamera.transform.localPosition.z) * 0.1f, 0.1f, 5f);

    //        float _scrollWheel = Input.GetAxis("Mouse ScrollWheel");
    //        if (_scrollWheel < 0)
    //        {
    //            float z = mycamera.transform.localPosition.z;
    //            Vector3 pos = mycamera.transform.localPosition;
    //            z -= autoScaleSpeed;
    //            pos.z = z;
    //            if (z < cameraZ + (scaleMax2 * -1))
    //            {
    //                pos.z = cameraZ + (scaleMax2 * -1);
    //            }
    //            mycamera.transform.localPosition = pos;
    //        }
    //        if (_scrollWheel > 0)
    //        {
    //            float z = mycamera.transform.localPosition.z;

    //            Vector3 pos = mycamera.transform.localPosition;
    //            z += autoScaleSpeed;
    //            pos.z = z;
    //            if (z > cameraZ + scaleMin2)
    //            {
    //                pos.z = cameraZ + scaleMin2;
    //            }
    //            mycamera.transform.localPosition = pos;
    //        }

    //        IsCameraScaling = _scrollWheel != 0;
    //    }
    //}

    public void DPI_MouseScale()
    {
        if(curCursorState == DPICursorState.Viewing && isFocusObjects == false)
        {
            autoScaleSpeed = Mathf.Clamp(Mathf.Abs(mycamera.transform.localPosition.z) * 0.1f, 0.1f, 5f);
            float _scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            //Debug.Log(_scrollWheel);
            float dis = (CameraRotAroundPoint.transform.position - cameraVirtualFocus.transform.position).sqrMagnitude;
            if ((dis <= (scaleMax * scaleMax) && _scrollWheel < 0) || (dis >= (scaleMin * scaleMin) && _scrollWheel > 0))
            {
                cameraVirtualFocus.transform.Translate(new Vector3(0, 0, _scrollWheel));
            }
            IsCameraScaling = _scrollWheel != 0;
        }
    }
    #endregion

    #region 鼠标旋转

    private float angleX;
    private float angleY;
    private float radius;
    public void DPI_MouseRotation()
    {
        if (isFocusObjects == false && curCursorState == DPICursorState.Viewing)
        {
            if (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.LeftAlt))
            {
                radius = (CameraRotAroundPoint.transform.position - transform.position).magnitude;
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                var mouse_x = Input.GetAxis("Mouse X") * rotateSpeed;//获取鼠标X轴移动
                var mouse_y = Input.GetAxis("Mouse Y") * rotateSpeed;//获取鼠标Y轴移动
                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    angleX += mouse_x * Time.deltaTime * 10;
                    angleY -= mouse_y * Time.deltaTime * 10;
                    //float x = Mathf.Lerp(0.05f, mouse_x, Time.deltaTime * rotateSpeed * 10);
                    //this.cameraVirtualFocus.transform.RotateAround(CameraRotAroundPoint.position, Vector3.up, x);
                    //transform.LookAt(CameraRotAroundPoint);
                    Quaternion quaternion = Quaternion.Euler(new Vector3(angleY, angleX, 0));
                    Vector3 position = quaternion * new Vector3(0, 0, -radius) + CameraRotAroundPoint.position;
                    cameraVirtualFocus.transform.position = position;
                    transform.LookAt(CameraRotAroundPoint);
                }
                else
                {
                    IsCameraRotating = !(mouse_y == 0 && mouse_x == 0);

                    Vector3 thirdPCamEuler = transform.localEulerAngles;
                    thirdPCamEuler.x = RotateClampX(this.transform, mouse_y, -90, 80);
                    thirdPCamEuler.y += mouse_x;
                    thirdPCamEuler.z = 0; //

                    transform.DOLocalRotate(thirdPCamEuler, 0.001f);
                    CameraRotAroundPoint.transform.position = transform.position + transform.forward * radius;
                }
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
                    cameraVirtualFocus.transform.Translate(new Vector3(-x, -y, 0) * autoSpeedModulus, mycamera.transform);
                    //cameraVirtualFocus.transform.Translate(new Vector3(-x, -y, 0) * speedModulus, mycamera.transform);
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
    #endregion

    #region WASD键盘控制
    public void DPI_KeyboardMove()
    {
        if (curCursorState == DPICursorState.Viewing)
        {
            if(IsOpenCameraControl)
            {
                hor = Input.GetAxis("Horizontal");
                ver = Input.GetAxis("Vertical");
                float y = ver * 0.2f;
                float x = hor * 0.2f;
                // 平移
                cameraVirtualFocus.transform.eulerAngles = transform.eulerAngles;
                IsCameraMoving = !(y == 0 && x == 0);
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    x *= 5f;
                    y *= 5f;
                }
                cameraVirtualFocus.transform.Translate(new Vector3(x, 0, y), mycamera.transform);

            }
        }
    }
    #endregion

}







