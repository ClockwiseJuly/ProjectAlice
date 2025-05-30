using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SwitchViewManager : MonoBehaviour
{
    public CinemachineVirtualCamera _virtualCamera2D;
    public CinemachineVirtualCamera _virtualCamera3Dleft;
    public CinemachineVirtualCamera _virtualCamera3Dright;
    public CinemachineVirtualCamera _virtualCamera3Dtop;
    private bool is2DViewActive = true;
    public bool Is2DViewActive => is2DViewActive;
    private int current3DView = 0;
    public PlayerController playerController;
    public List<GameObject> _gameObjects;
    public List<BoxCollider> boxColliders = new List<BoxCollider>();

    // 添加按键状态跟踪变量
    private bool qKeyWasPressed = false;
    private bool eKeyWasPressed = false;
    private bool rKeyWasPressed = false;
    [SerializeField] private float keyDebounceTime = 0.5f; // 按键防抖时间（秒）
    private float qKeyTimer = 0f;
    private float eKeyTimer = 0f;
    private float rKeyTimer = 0f;

    private GameObject player;
    private Transform playerTransform;
    
    // 添加场景切换监听
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    // 场景加载完成后重新查找引用
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 延迟一帧执行，确保场景完全加载
        StartCoroutine(RefreshReferencesDelayed());
    }
    
    private IEnumerator RefreshReferencesDelayed()
    {
        yield return null; // 等待一帧
        RefreshReferences();
    }
    
    // 重新查找所有引用
    private void RefreshReferences()
    {
        Debug.Log("SwitchViewManager: 重新查找场景引用...");
        
        // 重新查找PlayerController
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning("SwitchViewManager: 未找到PlayerController!");
        }
        
        // 重新查找Player对象
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("SwitchViewManager: 找到Player对象");
        }
        else
        {
             Debug.LogWarning("SwitchViewManager: 未找到Player对象!");
            playerTransform = null; // 确保设置为null
        }
        
        // 重新查找虚拟相机（如果为空的话）
        RefreshCameraReferences();
        
        // 重新查找2DCollider对象
        RefreshColliderReferences();
    }
    
    private void RefreshCameraReferences()
    {
        // 如果相机引用丢失，尝试通过名称查找
        if (_virtualCamera2D == null)
        {
            _virtualCamera2D = GameObject.Find("Virtual Camera Player Follow 2D")?.GetComponent<CinemachineVirtualCamera>();
        }
        if (_virtualCamera3Dleft == null)
        {
            _virtualCamera3Dleft = GameObject.Find("Virtual Camera Player Follow 3D Left")?.GetComponent<CinemachineVirtualCamera>();
        }
        if (_virtualCamera3Dright == null)
        {
            _virtualCamera3Dright = GameObject.Find("Virtual Camera Player Follow 3D Right")?.GetComponent<CinemachineVirtualCamera>();
        }
        if (_virtualCamera3Dtop == null)
        {
            _virtualCamera3Dtop = GameObject.Find("Virtual Camera Player Follow 3D Top")?.GetComponent<CinemachineVirtualCamera>();
        }
        
        Debug.Log($"相机引用状态: 2D={_virtualCamera2D != null}, 3DLeft={_virtualCamera3Dleft != null}, 3DRight={_virtualCamera3Dright != null}, 3DTop={_virtualCamera3Dtop != null}");
    }
    
    private void RefreshColliderReferences()
    {
        // 清空现有列表
        boxColliders.Clear();
        
        // 重新查找2DCollider对象
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("2DCollider");
        foreach (var obj in gameObjects)
        {
            BoxCollider[] colliders = obj.GetComponents<BoxCollider>();
            if (colliders.Length > 1)
            {
                boxColliders.Add(colliders[1]);
            }
            else
            {
                Debug.LogWarning($"对象 {obj.name} 没有足够的BoxCollider组件!");
            }
        }
        
        Debug.Log($"找到 {boxColliders.Count} 个2DCollider对象");
    }

    private void Start()
    {
        RefreshReferences();
    }

    private void Update()
    {
         // 添加空值检查，防止访问已销毁的Transform
    if (playerTransform == null)
    {
        // 尝试重新查找Player对象
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("SwitchViewManager: 在Update中重新找到Player对象");
        }
        else
        {
            // 如果仍然找不到，跳过这一帧的处理
            return;
        }
    }
        // 再次检查确保Transform有效
    if (playerTransform != null)
    {
        float x = playerTransform.localScale.x;
        UpdateAnimator2DForCurrentView(x);
    }
    else
    {
        // 如果playerTransform仍然为null，跳过处理
        return;
    }

        // 更新计时器
        if (qKeyTimer > 0) qKeyTimer -= Time.deltaTime;
        if (eKeyTimer > 0) eKeyTimer -= Time.deltaTime;
        if (rKeyTimer > 0) rKeyTimer -= Time.deltaTime;

        // Q键处理
        bool qKeyIsPressed = Keyboard.current.qKey.isPressed;
        if (qKeyIsPressed && !qKeyWasPressed && qKeyTimer <= 0)
        {
            qKeyTimer = keyDebounceTime;
            ResetPosition();
            if (is2DViewActive)
            {
                SwitchTo3DLeftView();
            }
            else if (current3DView == 1) // 如果当前是左视角
            {
                SwitchTo2DView(); // 返回2D视角
            }
            else // 如果当前是右视角
            {
                SwitchTo3DLeftView(); // 切换到左视角
            }
        }
        qKeyWasPressed = qKeyIsPressed;

        // E键处理
        bool eKeyIsPressed = Keyboard.current.eKey.isPressed;
        if (eKeyIsPressed && !eKeyWasPressed && eKeyTimer <= 0)
        {
            eKeyTimer = keyDebounceTime;
            ResetPosition();
            if (is2DViewActive)
            {
                SwitchTo3DRightView();
            }
            else if (current3DView == 2) // 如果当前是右视角
            {
                SwitchTo2DView(); // 返回2D视角
            }
            else // 如果当前是左视角
            {
                SwitchTo3DRightView(); // 切换到右视角
            }
        }
        eKeyWasPressed = eKeyIsPressed;

        // R键处理
        bool rKeyIsPressed = Keyboard.current.rKey.isPressed;
        if (rKeyIsPressed && !rKeyWasPressed && rKeyTimer <= 0)
        {
            rKeyTimer = keyDebounceTime;
            ResetPosition();
            if (is2DViewActive)
            {
                SwitchTo3DTopView(); // 切换到俯视角
            }
            else if (current3DView == 3) // 如果当前是俯视角
            {
                SwitchTo2DView(); // 返回2D视角
            }
            else
            {
                SwitchTo3DTopView(); // 切换到视角
            }
        }
        rKeyWasPressed = rKeyIsPressed;
    }

    public void SwitchTo2DView()
    {
        _virtualCamera2D.Priority = 10;
        _virtualCamera3Dleft.Priority = 9;
        _virtualCamera3Dright.Priority = 9;
        _virtualCamera3Dtop.Priority = 9;
        is2DViewActive = true;
        current3DView = 0;

        CollisionEnable();
    }

    public void SwitchTo3DLeftView()
    {
        _virtualCamera3Dleft.Priority = _virtualCamera2D.Priority + 1;
        _virtualCamera3Dright.Priority = _virtualCamera2D.Priority - 1;
        is2DViewActive = false;
        current3DView = 1;

        CollisionDisable();
    }

    public void SwitchTo3DRightView()
    {
        _virtualCamera3Dright.Priority = _virtualCamera2D.Priority + 1;
        _virtualCamera3Dleft.Priority = _virtualCamera2D.Priority - 1;
        is2DViewActive = false;
        current3DView = 2;

        CollisionDisable();
    }

    public void SwitchTo3DTopView()
    {
        _virtualCamera3Dtop.Priority = _virtualCamera2D.Priority + 1;
        _virtualCamera3Dleft.Priority = _virtualCamera2D.Priority - 1;
        _virtualCamera3Dright.Priority = _virtualCamera2D.Priority - 1;
        is2DViewActive = false;
        current3DView = 3;

        CollisionDisable();
    }
    public void ResetPosition()
    {
        GameObject boxCollider = playerController.GetboxCollider();
        playerController.gameObject.transform.position = new Vector3
        (playerController.gameObject.transform.position.x,
        playerController.gameObject.transform.position.y,
        boxCollider.transform.position.z);
    }

    public void CollisionEnable()
    {
        foreach (var boxColl in boxColliders)
        {
            boxColl.enabled = true;
        }
    }
    public void CollisionDisable()
    {
        foreach (var boxColl in boxColliders)
        {
            boxColl.enabled = false;
        }
    }

    private void UpdateAnimator2DForCurrentView(float x)
    {
        GameObject animator2D = GameObject.FindGameObjectsWithTag("Player")[0];

        // 根据当前视角更新动画状态
        
        if (current3DView == 1) // 3D左视角
        {
            if (x > 0f)
            {
                animator2D.transform.rotation = Quaternion.Euler(0, 45, 0);
                animator2D.transform.localScale = new Vector3(0.25f, 0.25f, 1f);

            }
            else if (x < 0f)
            {
                animator2D.transform.rotation = Quaternion.Euler(0, 225, 0);
                animator2D.transform.localScale = new Vector3(-0.25f, 0.25f, 1f);
            }
        }
        else if (current3DView == 2) // 3D右视角
        {
            if (x > 0f)
            {
                animator2D.transform.rotation = Quaternion.Euler(0, -45, 0);
                animator2D.transform.localScale = new Vector3(0.25f, 0.25f, 1f);

            }
            else if (x < 0f)
            {
                animator2D.transform.rotation = Quaternion.Euler(0, -225, 0);
                animator2D.transform.localScale = new Vector3(-0.25f, 0.25f, 1f);
            }
        }
        else if (current3DView == 3) // 3D俯视角
        {
            animator2D.transform.rotation = Quaternion.Euler(0, 0, 0);
            animator2D.transform.localScale = new Vector3(0.25f, 0.25f, 1f);
        }
        else if (is2DViewActive) // 2D视角
        {
            animator2D.transform.rotation = Quaternion.Euler(0, 0, 0);
            animator2D.transform.localScale = new Vector3(0.25f, 0.25f, 1f);
        }

    }

    // 1. 添加了按键状态跟踪变量，记录上一帧的按键状态
    // 2. 添加了防抖计时器，防止短时间内重复触发
    // 3. 使用 isPressed 而不是 wasPressedThisFrame ，配合状态跟踪实现自定义的按键检测
    // 4. 只有当按键从未按下状态变为按下状态，且防抖计时器为0时，才会触发操作
    // 综上可确保：
    // 单次点击只会触发一次操作
    // 连续按住也只会在第一次按下时触发一次操作
    // 松开按键后需要等待防抖时间才能再次触发（可以根据需要调整 keyDebounceTime 的值）
    // 这样无论是单次点击还是连续按住，都会被视为只输入了一帧

    //摄像机切换视角改变，角色贴图朝向也随之改变

}