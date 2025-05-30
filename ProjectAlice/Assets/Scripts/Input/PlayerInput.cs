using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] float jumpInputBufferTime = 0.5f; // 跳跃输入缓冲时间
    WaitForSeconds waitJumpInputBufferTime;
    PlayerInputActions playerInputActions;
    private SwitchViewManager switchViewManager;
    
    Vector2 axes => playerInputActions.Gameplay.Axes.ReadValue<Vector2>();//

    //public bool Move => AxisX != 0f;
    public bool Move => AxisX != 0f || AxisY != 0f; // 修改移动判断，包含X轴或Z轴有输入
    public float AxisX => axes.x;
    //public float AxisY => axes.y; // 添加Y轴输入值访问器，用于Z轴移动
    
    // 修改AxisY属性，在2D视角下禁用W和S键位
    public float AxisY 
    {
        get
        {
            // 如果当前处于2D视角，则禁用Y轴输入（W和S键）
            if (switchViewManager != null && switchViewManager.Is2DViewActive)
            {
                return 0f;
            }
            return axes.y;
        }
    }

    public bool HasJumpInputBuffer { get; set; }// 跳跃输入缓冲标志
    public bool Jump => playerInputActions.Gameplay.Jump.WasPressedThisFrame();
    public bool StopJump => playerInputActions.Gameplay.Jump.WasReleasedThisFrame();
    //交互键
    public bool Interact => playerInputActions.Gameplay.Interaction.WasPressedThisFrame();
    
    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        waitJumpInputBufferTime = new WaitForSeconds(seconds: jumpInputBufferTime);
        
        switchViewManager = FindObjectOfType<SwitchViewManager>();
    }

    void OnEnable()
    {
        playerInputActions.Gameplay.Jump.canceled += delegate
        {
            HasJumpInputBuffer = false;//玩家松开跳跃键，输入缓冲设置关闭
        };
    }

    //void OnGUI()
    //{

    //前两个参数是四边形在屏幕上位置，后两个为宽高
    //Rect rect = new Rect(x: 200, y: 200, width: 200, height: 200);

    //string message = "Has Jump Input Buffer: " + HasJumpInputBuffer;

    //GUI样式
    //GUIStyle style = new GUIStyle();
    //style.fontSize = 20;
    //style.fontStyle = FontStyle.Bold;

    //参数1：信息显示区域
    //GUI.Label(position: rect, text: message, style: style);
    //}

    //启用动作表
    public void EnableGameplayInputs()
    {
        playerInputActions.Gameplay.Enable();

        //禁用鼠标点击
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetJumpInputBufferTimer()
    {
        StopCoroutine(methodName: nameof(JumpInputBufferCoroutine));
        StartCoroutine(methodName: nameof(JumpInputBufferCoroutine));
        //防止同一协程反复开启
    }

    //跳跃输入缓冲协程
    IEnumerator JumpInputBufferCoroutine()
    {
        HasJumpInputBuffer = true;
        yield return waitJumpInputBufferTime;
        HasJumpInputBuffer = false;
    }
    
    // 移除这些不存在的输入引用
    // public bool TimeRewind => playerInputActions.Gameplay.TimeRewind.IsPressed();
    // public bool TimePause => playerInputActions.Gameplay.TimePause.WasPressedThisFrame();
    // public bool TimeForward => playerInputActions.Gameplay.TimeForward.IsPressed();
    
    void Update()
    {
        // 移除时间控制相关的检查，因为TimeController直接使用Input.GetKey
        // TimeController timeController = FindObjectOfType<TimeController>();
        // if (timeController != null && timeController.isRewinding)
        // {
        //     return;
        // }
    }
}
