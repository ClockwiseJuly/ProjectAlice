using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;

public class SwitchViewManager : MonoBehaviour
{
    public CinemachineVirtualCamera _virtualCamera2D;
    public CinemachineVirtualCamera _virtualCamera3Dleft;
    public CinemachineVirtualCamera _virtualCamera3Dright;
    public CinemachineVirtualCamera _virtualCamera3Dtop;
    private bool is2DViewActive = true; // 默认为2D视角
    private int current3DView = 0; // 0=无，1=左视角，2=右视角, 3=俯视角;
    public PlayerController playerController; // 玩家控制器引用
    public List<GameObject> _gameObjects;
    public List<BoxCollider> boxColliders = new List<BoxCollider>();
    
    // 添加按键状态跟踪变量
    private bool qKeyWasPressed = false;
    private bool eKeyWasPressed = false;
    private bool fKeyWasPressed = false;
    private float keyDebounceTime = 0.5f; // 按键防抖时间（秒）
    private float qKeyTimer = 0f;
    private float eKeyTimer = 0f;
    private float fKeyTimer = 0f;

    private void Start()
    {   
        playerController = FindObjectOfType<PlayerController>();
        //打标签+双碰撞体
       GameObject[] _gameObjects = GameObject.FindGameObjectsWithTag("2DCollider");
       foreach (var obj in _gameObjects)
       {
            BoxCollider boxCollider = obj.GetComponents<BoxCollider>()[1];
            boxColliders.Add(boxCollider);
       }
    }
    
    private void Update()
    {
        // 更新计时器
        if (qKeyTimer > 0) qKeyTimer -= Time.deltaTime;
        if (eKeyTimer > 0) eKeyTimer -= Time.deltaTime;
        if (fKeyTimer > 0) fKeyTimer -= Time.deltaTime;
        
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
        
        // F键处理
        bool fKeyIsPressed = Keyboard.current.fKey.isPressed;
        if (fKeyIsPressed && !fKeyWasPressed && fKeyTimer <= 0)
        {
            fKeyTimer = keyDebounceTime;
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
        fKeyWasPressed = fKeyIsPressed;
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

// 1. 添加了按键状态跟踪变量，记录上一帧的按键状态
// 2. 添加了防抖计时器，防止短时间内重复触发
// 3. 使用 isPressed 而不是 wasPressedThisFrame ，配合状态跟踪实现自定义的按键检测
// 4. 只有当按键从未按下状态变为按下状态，且防抖计时器为0时，才会触发操作
// 综上可确保：
// 单次点击只会触发一次操作
// 连续按住也只会在第一次按下时触发一次操作
// 松开按键后需要等待防抖时间才能再次触发（可以根据需要调整 keyDebounceTime 的值）
// 这样无论是单次点击还是连续按住，都会被视为只输入了一帧

}