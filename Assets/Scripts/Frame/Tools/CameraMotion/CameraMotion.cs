// Partially derived from FlyThrough.js available on the Unify Wiki
// http://wiki.unity3d.com/index.php/FlyThrough

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ViewTool
{
    /// <value>
    /// Camera is not in control of anything
    /// </value>
    None,
    /// <value>
    /// Camera is spherically rotating around target
    /// </value>
    Orbit,
    /// <value>
    /// Camera is moving right or left
    /// </value>
    Pan,
    /// <value>
    /// Camera is moving forward or backwards
    /// </value>
    Dolly,
    /// <value>
    /// Camera is looking and possibly flying
    /// </value>
    Look
}

/**
 * Requires InputSettings to have:
 * 	- "Horizontal", "Vertical", "CameraUp", with Gravity and Sensitivity set to 3.
 */
[RequireComponent(typeof(Camera))]
sealed class CameraMotion : MonoBehaviour
{
    private static CameraMotion instance;
    public static CameraMotion Instance { get { return instance; } }
    public ViewTool cameraState { get; private set; }
    const string k_InputMouseScrollwheel = "Mouse ScrollWheel";
    const string k_InputMouseHorizontal = "Mouse X";
    const string k_InputMouseVertical = "Mouse Y";

    const int k_LeftMouse = 0;
    const int k_RightMouse = 1;
    const int k_MiddleMouse = 2;



#if USE_DELTA_TIME
		public float moveSpeed = 15f;
		public float lookSpeed = 200f;
		public float orbitSpeed = 200f;
		public float scrollModifier = 100f;
		public float zoomSpeed = .05f;
#else
    public bool isMove;

    // How fast the camera position moves.
    [Header("移动速度")]
    public float moveSpeed = 15f;
    public float shiftSpeed = 10;

    // How fast the camera rotation adjusts.
    [Header("旋转速度")]
    public float lookSpeed = 5f;

    // How fast the camera rotation adjusts.
    public float orbitSpeed = 7f;

    // How fast the mouse scroll wheel affects distance from pivot.
    [Header("滚轮移动速度")]
    public float scrollModifier = 50f;

    public float zoomSpeed = .1f;
    [Header("pivot与摄像机的最小距离")]
    public float k_MinCameraDistance = 1f;
    [Header("pivot与摄像机的最大距离")]
    public float k_MaxCameraDistance = 100f;
    public CameraLookAt LookAtTargetObj;
#endif

    //bool m_UseEvent;
    bool iftouch;
    private Touch oldTouch1;
    private Touch oldTouch2;
    Camera m_CameraComponent;
    Transform m_Transform;
    public Vector3 m_ScenePivot = Vector3.zero;
    float m_DistanceToCamera = 1f;

    // Store the mouse position from the last frame. Used in calculating deltas for mouse movement.
    Vector3 m_PreviousMousePosition = Vector3.zero;

    //bool m_CurrentActionValid = true;

    [Header("是否现在摄像机移动范围")]
    public bool IsLimitRange = false;
    public  Vector3 m_MinTransformPosLimit = new Vector3(-7f, 1f, -7f);
    public  Vector3 m_MaxTransforPosLimit = new Vector3(7f, 7f, 7f);
    private GameObject maskbox;
    static bool ifuse3Dmask;

    private bool isFocus;
    void Awake()
    {
        instance = this;
        m_CameraComponent = GetComponent<Camera>();
        Assert.IsNotNull(m_CameraComponent);
        m_Transform = GetComponent<Transform>();
        m_ScenePivot = m_Transform.forward * -m_DistanceToCamera;
        m_DistanceToCamera = Vector3.Distance(m_Transform.position, m_ScenePivot);
        //Debug.Log("m_ScenePivot = " + m_ScenePivot);
        //Debug.Log("m_Transform.forward = " + m_Transform.forward);
        //if(LookAtTargetObj!=null) Focus(LookAtTargetObj.gameObject);
        //setmaskbox();
    }

    public void LateUpdate()
    {
        if(isMove)
            DoLateUpdate();
    }
    Vector3 velocity = Vector3.zero;
    Vector3 pos;
    public void setmaskbox()
    {
        maskbox = new GameObject("maskbox");
        maskbox.transform.parent = transform;
        maskbox.transform.localPosition = new Vector3(0, 0, 1);
        maskbox.transform.localRotation = transform.localRotation;
        var cld = maskbox.AddComponent<BoxCollider>();
        maskbox.transform.localScale = new Vector3(3, 3, 0.2f);
        maskbox.gameObject.SetActive(false);

    }
    public void DoLateUpdate()
    {

		#region 触控操作代码
		//        if (Input.touchCount > 0) { iftouch = true; } else { iftouch = false; }

		//        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		//        {
		//            Touch newTouch1 = Input.GetTouch(0);

		//            Vector2 deltaPos1 = newTouch1.deltaPosition;//位置增量

		//            if (Mathf.Abs(deltaPos1.x) > 2.5f || Mathf.Abs(deltaPos1.y) > 2.5f) { maskbox.gameObject.SetActive(true); }
		//        }

		//        if (Input.touchCount > 0)
		//        {
		//            if (Input.GetTouch(0).phase == TouchPhase.Ended && maskbox.gameObject.activeSelf)
		//                this.SetDelay(0.3f, () => { maskbox.gameObject.SetActive(false); });
		//        }

		//        //移动
		//        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
		//        {
		//            Touch newTouch1 = Input.GetTouch(0);

		//            float hor = newTouch1.deltaPosition.x;//获取X轴移动
		//            float ver = newTouch1.deltaPosition.y;//获取Y轴移动
		//            Vector2 delta = new Vector2(hor, ver);
		//            delta.x = ScreenToWorldDistance(delta.x, m_DistanceToCamera);
		//            delta.y = ScreenToWorldDistance(delta.y, m_DistanceToCamera);

		//            if (!IsLimitRange)
		//            {
		//                m_Transform.position -= m_Transform.right * delta.x;
		//                m_Transform.position -= m_Transform.up * delta.y;
		//            }
		//            else
		//            {
		//                m_Transform.position = LimitPos(m_Transform.position - m_Transform.right * delta.x);
		//                m_Transform.position = LimitPos(m_Transform.position - m_Transform.up * delta.y);
		//            }

		//            m_ScenePivot = m_Transform.position + m_Transform.forward * m_DistanceToCamera;

		//        }
		//        //旋转
		//        else if (Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Moved)
		//        {


		//            Touch newTouch2 = Input.GetTouch(1);

		//            Vector2 deltaPos2 = newTouch2.deltaPosition;//位置增量
		//            float rotX = deltaPos2.x / 50;
		//            float rotY = -deltaPos2.y / 50;
		//            if (rotX != 0 || rotY != 0)
		//            {
		//                cameraState = ViewTool.Orbit;
		//                Debug.Log("出发拖拽");
		//                m_UseEvent = true;
		//                Vector3 eulerRotation = transform.localRotation.eulerAngles;

		//                if ((Mathf.Approximately(eulerRotation.x, 90f) && rotY > 0f) ||
		//                    (Mathf.Approximately(eulerRotation.x, 270f) && rotY < 0f))
		//                    rotY = 0f;

		//#if USE_DELTA_TIME
		//				eulerRotation.x += rot_y * orbitSpeed * Time.deltaTime;
		//				eulerRotation.y += rot_x * orbitSpeed * Time.deltaTime;
		//#else
		//                eulerRotation.x += rotY * orbitSpeed;
		//                eulerRotation.y += rotX * orbitSpeed;
		//#endif

		//                eulerRotation.z = 0f;
		//                eulerRotation.x = Mathf.Clamp(eulerRotation.x, 10, 90);
		//                transform.localRotation = Quaternion.Euler(eulerRotation);
		//                transform.position = CalculateCameraPosition(m_ScenePivot);
		//            }
		//        }
		//        //缩放
		//       if (Input.touchCount > 1 )
		//        {

		//            Touch newTouch1 = Input.GetTouch(0);
		//            Touch newTouch2 = Input.GetTouch(1);


		//            //第2点刚开始接触屏幕, 只记录，不做处理  
		//            if (newTouch2.phase == TouchPhase.Began)
		//            {
		//                oldTouch2 = newTouch2;
		//                oldTouch1 = newTouch1;
		//                return;
		//            }

		//            //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
		//            float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
		//            float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

		//            if (Input.GetTouch(1).phase == TouchPhase.Moved && Input.GetTouch(0).phase == TouchPhase.Moved)
		//            {

		//                //两个距离之差，为正表示放大手势， 为负表示缩小手势  
		//                float offset = newDistance - oldDistance;
		//                //	Debug.Log("两只距离"+ offset);
		//                //	Vector2 deltaPos = newTouch2.deltaPosition;//位置增量
		//                //放大因子， 一个像素按 0.01倍来算(100可调整)  
		//                float scaleFactor = 0;
		//                if (ConfigMgr.useCacheData)
		//				{
		//                    scaleFactor = offset * Mathf.Abs(k_MaxCameraDistance - k_MinCameraDistance)/ ConfigMgr.ZoomFactor;
		//                }
		//                else
		//				{
		//                    scaleFactor = (offset) * Mathf.Abs(k_MaxCameraDistance - k_MinCameraDistance) * 3840 / 260000;
		//                }

		//                //超过多少系数开始缩放

		//                m_ScenePivot = m_Transform.position + m_Transform.forward * m_DistanceToCamera;

		//                if (scaleFactor < 0)
		//                {
		//                    m_DistanceToCamera += Mathf.Abs(scaleFactor);
		//                    m_DistanceToCamera = Mathf.Clamp(m_DistanceToCamera, k_MinCameraDistance, k_MaxCameraDistance);
		//                    m_Transform.position = m_Transform.localRotation * (Vector3.forward * -m_DistanceToCamera) + m_ScenePivot;
		//                    Debug.Log("suof" + scaleFactor);
		//                }
		//                if (scaleFactor > 0)
		//                {
		//                    Debug.Log("suof" + scaleFactor);
		//                    m_DistanceToCamera -= Mathf.Abs(scaleFactor);
		//                    m_DistanceToCamera = Mathf.Clamp(m_DistanceToCamera, k_MinCameraDistance, k_MaxCameraDistance);
		//                    m_Transform.position = m_Transform.localRotation * (Vector3.forward * -m_DistanceToCamera) + m_ScenePivot;
		//                }

		//                //记住最新的触摸点，下次使用  
		//                oldTouch1 = newTouch1;
		//                oldTouch2 = newTouch2;
		//            }
		//        }

		//        //如果检测有触控则和鼠标操作互斥
		//        if (iftouch) return;

		#endregion

		if ( Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
            {
                //m_UseEvent = false; 
            }

         

            cameraState = ViewTool.None;

        // Camera is flying itself to a target
        if (m_Zooming)
        {
			transform.rotation = Quaternion.Lerp(m_PreviousRotation, m_TargetRotation, (m_ZoomProgress += Time.deltaTime) / zoomSpeed);
            transform.position = Vector3.Lerp(m_PreviousPosition, m_TargetPosition, (m_ZoomProgress += Time.deltaTime) / zoomSpeed);
			if (Vector3.Distance(transform.position, m_TargetPosition) < .1f) m_Zooming = false;
            m_DistanceToCamera = Vector3.Distance(transform.position, m_ScenePivot);
            return;
        }
        if (IsOnUIElement())
            return;
        if ((Input.GetAxis(k_InputMouseScrollwheel) != 0f || (Input.GetMouseButton(k_RightMouse) && Input.GetKey(KeyCode.LeftAlt)))/* && CheckMouseOverGUI()*/)
        {
            float delta = Input.GetAxis(k_InputMouseScrollwheel);
            
            //if (Mathf.Approximately(delta, 0f))
            //{
            //    cameraState = ViewTool.Dolly;
            //    delta = CalcSignedMouseDelta(Input.mousePosition, m_PreviousMousePosition);
            //}
            //Debug.Log("delta = " + delta);
            m_DistanceToCamera -= delta * (m_DistanceToCamera / k_MaxCameraDistance) * scrollModifier;
            m_DistanceToCamera = Mathf.Clamp(m_DistanceToCamera, k_MinCameraDistance, k_MaxCameraDistance);
            if ((m_DistanceToCamera < k_MaxCameraDistance && delta > 0) || (m_DistanceToCamera > k_MinCameraDistance && delta < 0))
			{
                m_Transform.position = m_Transform.localRotation * (Vector3.forward * -m_DistanceToCamera) + m_ScenePivot;
            }


            //Debug.Log("m_DistanceToCamera = " + m_DistanceToCamera);
        }

        //bool viewTool = true;

		// If the current tool isn't View, or no mouse button is pressed, record the mouse position then early exit.
//		if (!m_CurrentActionValid || (viewTool
//#if !CONTROLLER
//										&& !Input.GetMouseButton(k_LeftMouse)
//									&& !Input.GetMouseButton(k_RightMouse)
//									&& !Input.GetMouseButton(k_MiddleMouse)
//									&& !Input.GetKey(KeyCode.LeftAlt)
//#endif
//				))
//		{
//			Rect screen = new Rect(0, 0, Screen.width, Screen.height);

//			if (screen.Contains(Input.mousePosition))
//				m_PreviousMousePosition = Input.mousePosition;

//			return;
//		}

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
		{
            isFocus = false;
		}

            // FPS view camera
            if((Input.GetMouseButton(k_RightMouse) || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && !isFocus /*&& !Input.GetKey(KeyCode.LeftAlt)*/) //|| Input.GetKey(KeyCode.LeftShift) )
																					  //if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
		{
			//Debug.Log(Input.GetAxis("Vertical"));
			cameraState = ViewTool.Look;

            //m_UseEvent = true;

            if(Input.GetMouseButton(k_RightMouse))
			{
                // Rotation
                float rotX = Input.GetAxis(k_InputMouseHorizontal);
                float rotY = Input.GetAxis(k_InputMouseVertical);

                Vector3 eulerRotation = m_Transform.localRotation.eulerAngles;

#if USE_DELTA_TIME
				eulerRotation.x -= rot_y * lookSpeed * Time.deltaTime; 	// Invert Y axis
				eulerRotation.y += rot_x * lookSpeed * Time.deltaTime;
#else
                eulerRotation.x -= rotY * lookSpeed;
                eulerRotation.y += rotX * lookSpeed;
#endif
                eulerRotation.z = 0f;
                m_Transform.localRotation = Quaternion.Euler(eulerRotation);
            }
            // PositionHandle-- Always use delta time when flying
            float speed = moveSpeed * Time.deltaTime;
            if(Input.GetKey(KeyCode.LeftShift))
            {             
                speed *= shiftSpeed;
            }

            if(!IsLimitRange)
			{
				m_Transform.position += m_Transform.forward * speed * Input.GetAxis("Vertical");
				m_Transform.position += m_Transform.right * speed * Input.GetAxis("Horizontal");
			}
			else
			{
                m_Transform.position = LimitPos(m_Transform.position + m_Transform.forward * speed * Input.GetAxis("Vertical"));
                m_Transform.position = LimitPos(m_Transform.position + m_Transform.right * speed * Input.GetAxis("Horizontal"));
            }

            try
            {
                m_Transform.position += m_Transform.up * speed * Input.GetAxis("CameraUp");
            }
            catch
            {
                Debug.LogWarning(
                    "CameraUp input is not configured.  Open \"Edit/Project Settings/Input\" and add an input named \"CameraUp\", mapping q and e to Negative and Positive buttons.");
            }
            Vector3 pivot = transform.position + transform.forward * m_DistanceToCamera;
            m_ScenePivot = pivot;
        }
        // Orbit
        else if (/*Input.GetKey(KeyCode.LeftAlt) && */Input.GetMouseButton(k_RightMouse) && isFocus)
        {
            float rotX = Input.GetAxis(k_InputMouseHorizontal);
            float rotY = -Input.GetAxis(k_InputMouseVertical);
            if(rotX != 0 || rotY != 0)
			{
                cameraState = ViewTool.Orbit;
                //Debug.Log("出发拖拽");
                //m_UseEvent = true;



                Vector3 eulerRotation = transform.localRotation.eulerAngles;

                if ((Mathf.Approximately(eulerRotation.x, 90f) && rotY > 0f) ||
                    (Mathf.Approximately(eulerRotation.x, 270f) && rotY < 0f))
                    rotY = 0f;

#if USE_DELTA_TIME
				eulerRotation.x += rot_y * orbitSpeed * Time.deltaTime;
				eulerRotation.y += rot_x * orbitSpeed * Time.deltaTime;
#else
                eulerRotation.x += rotY * orbitSpeed;
                eulerRotation.y += rotX * orbitSpeed;
#endif

                eulerRotation.z = 0f;
                eulerRotation.x = Mathf.Clamp(eulerRotation.x, 10, 90);
                transform.localRotation = Quaternion.Euler(eulerRotation);
                transform.position = CalculateCameraPosition(m_ScenePivot);

            }

        }
        // Pan
        else if (Input.GetMouseButton(k_MiddleMouse) /*|| (Input.GetMouseButton(k_LeftMouse) && viewTool)*/)
        {
            isFocus = false;
            cameraState = ViewTool.Pan;

            Vector2 delta = Input.mousePosition - m_PreviousMousePosition;

            delta.x = ScreenToWorldDistance(delta.x, m_DistanceToCamera);
            delta.y = ScreenToWorldDistance(delta.y, m_DistanceToCamera);



            if(!IsLimitRange)
			{
				m_Transform.position -= m_Transform.right * delta.x;
				m_Transform.position -= m_Transform.up * delta.y;

            }
            else
			{
                m_Transform.position = LimitPos(m_Transform.position - m_Transform.right * delta.x);
                m_Transform.position = LimitPos(m_Transform.position - m_Transform.up * delta.y);
                Vector3 pivot = m_Transform.position + m_Transform.forward * m_DistanceToCamera;
    //            if(pivot.y < 0)
				//{
    //                pivot = GetIntersectWithLineAndPlane(m_Transform.position, m_Transform.forward, Vector3.up, Vector3.zero); 
    //            }
    //            m_DistanceToCamera = Vector3.Distance(m_Transform.position, pivot);
    //            m_ScenePivot = pivot;

            }
            m_ScenePivot = m_Transform.position + m_Transform.forward * m_DistanceToCamera;


        }

        m_PreviousMousePosition = Input.mousePosition;

        
    }

	public void LookAtObj(CameraLookAt obj)
	{
        if (obj != null)
		{
            //isMove = false;
            LookAtTargetObj = obj;
            obj.isLooAk = true;
            isFocus = true;
            Focus(LookAtTargetObj.gameObject);
		}
	}
    /// <summary>
    /// 计算直线与平面的交点
    /// </summary>
    /// <param name="point">直线上某一点</param>
    /// <param name="direct">直线的方向</param>
    /// <param name="planeNormal">垂直于平面的的向量</param>
    /// <param name="planePoint">平面上的任意一点</param>
    /// <returns></returns>
    private Vector3 GetIntersectWithLineAndPlane(Vector3 point, Vector3 direct, Vector3 planeNormal, Vector3 planePoint)
    {
        float d = Vector3.Dot(planePoint - point, planeNormal) / Vector3.Dot(direct.normalized, planeNormal);

        return d * direct.normalized + point;
    }

    Vector3 CalculateCameraPosition(Vector3 target)
    {
        return transform.localRotation * (Vector3.forward * -m_DistanceToCamera) + target;
    }

    bool m_Zooming = false;
    float m_ZoomProgress = 0f;
    Vector3 m_PreviousPosition = Vector3.zero;
    Quaternion m_PreviousRotation;
    Vector3 m_TargetPosition = Vector3.zero;
    Quaternion m_TargetRotation;

    /// <summary>
    /// Lerp the camera to the current selection
    /// </summary>
    /// <param name="target"></param>
    public void Focus(GameObject target)
    {
		//Vector3 center = ;
        //Renderer renderer = target.GetComponent<Renderer>();
        //float distance = Vector3.Distance(transform.position, center);
        //Bounds bounds = renderer != null ? renderer.bounds : new Bounds(center, Vector3.one * distance);
        Vector3 center = FocusToTarget(new List<GameObject> { target });
        //Debug.Log("摄像机中心点 = " + center);
		Focus(center, m_DistanceToCamera);
    }

    public void Focus(Vector3 target, float distance)
    {
        m_Zooming = true;

        m_ScenePivot = target;
		//m_DistanceToCamera = distance;
		m_PreviousPosition = transform.position;
        m_PreviousRotation = transform.rotation;
        m_TargetPosition = LookAtTargetObj.pos;
        m_TargetRotation = LookAtTargetObj.Rot;
        m_ZoomProgress = 0f;
    }
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
        }


        Bounds centerBounds = meshRenderers[0].bounds;

        for (int i = 1; i < meshRenderers.Count; i++)
        {
            centerBounds.Encapsulate(meshRenderers[i].bounds);
        }

        Vector3 centerPoint = centerBounds.center;
        return centerPoint;
    }

    float ScreenToWorldDistance(float screenDistance, float distanceFromCamera)
    {
        Vector3 start = m_CameraComponent.ScreenToWorldPoint(Vector3.forward * distanceFromCamera);
        Vector3 end = m_CameraComponent.ScreenToWorldPoint(new Vector3(screenDistance, 0f, distanceFromCamera));
        return CopySign(Vector3.Distance(start, end), screenDistance);
    }

    static float CalcSignedMouseDelta(Vector2 lhs, Vector2 rhs)
    {
        float delta = Vector2.Distance(lhs, rhs);
        float scale = 1f / Mathf.Min(Screen.width, Screen.height);

        // If horizontal movement is greater than vertical movement, use the X axis for sign.
        if (Mathf.Abs(lhs.x - rhs.x) > Mathf.Abs(lhs.y - rhs.y))
            return delta * scale * ((lhs.x - rhs.x) > 0f ? 1f : -1f);

        return delta * scale * ((lhs.y - rhs.y) > 0f ? 1f : -1f);
    }

    static float CalcMinDistanceToBounds(Camera cam, Bounds bounds)
    {
        float frustumHeight = Mathf.Max(Mathf.Max(bounds.size.x, bounds.size.y), bounds.size.z);
        float distance = frustumHeight * .5f / Mathf.Tan(cam.fieldOfView * .5f * Mathf.Deg2Rad);

        return distance;
    }

    private Vector3 LimitPos(Vector3 target)
	{
        Vector3 pos = target;
        pos.x = Mathf.Clamp(pos.x, m_MinTransformPosLimit.x, m_MaxTransforPosLimit.x);
        pos.y = Mathf.Clamp(pos.y, m_MinTransformPosLimit.y, m_MaxTransforPosLimit.y);
        pos.z = Mathf.Clamp(pos.z, m_MinTransformPosLimit.z, m_MaxTransforPosLimit.z);
        return pos;
	}

    /// <summary>
    /// Return the magnitude of X with the sign of Y.
    /// </summary>
    float CopySign(float x, float y)
    {
        if (x < 0f && y < 0f || x > 0f && y > 0f || x == 0f || y == 0f)
            return x;

        return -x;
    }
    #region 判断是否在ui上

    [Header("监测的Canvas")]
    public GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;
    private PointerEventData eventData;
    private bool init = false;
    void RaycasterInit()
    {
        if (!init)
        {
            //graphicRaycaster = GameObject.Find("EquipUIMgr").GetComponent<GraphicRaycaster>();
            //                                  ↑这里要根据你自己的项目自己找canvas上的GraphicRaycaster
            eventSystem = EventSystem.current;
            eventData = new PointerEventData(eventSystem);
            init = true;
        }
    }

    /// <summary>
    /// 是否在UI上
    /// </summary>
    /// <returns></returns>
    public bool IsOnUIElement()
    {
        RaycasterInit();
        if (graphicRaycaster == null)
        {
            Debug.Log("无GraphicRaycaster,有问题");
            return false;
        }
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, list);
        foreach (var temp in list)
        {
            if (temp.gameObject.layer.Equals(5)) return true;
        }
        return false;
    }

    #endregion
}