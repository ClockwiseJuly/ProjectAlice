using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] float jumpInputBufferTime = 0.5f; // 跳跃输入缓冲时间
    WaitForSeconds waitJumpInputBufferTime;
    PlayerInputActions playerInputActions;
    Vector2 axes => playerInputActions.Gameplay.Axes.ReadValue<Vector2>();

    public bool HasJumpInputBuffer { get; set; }
    public bool Jump => playerInputActions.Gameplay.Jump.WasPressedThisFrame();
    public bool StopJump => playerInputActions.Gameplay.Jump.WasReleasedThisFrame();

    //public bool Move => AxisX != 0f;
    public bool Move => AxisX != 0f || AxisY != 0f; // 修改移动判断，包含X轴或Z轴有输入
    public float AxisX => axes.x;
    public float AxisY => axes.y; // 添加Y轴输入值访问器，用于Z轴移动

    //交互键
    public bool Interact => playerInputActions.Gameplay.Interaction.WasPressedThisFrame();

    void Awake()
    {
        playerInputActions = new PlayerInputActions();

        waitJumpInputBufferTime = new WaitForSeconds(seconds: jumpInputBufferTime);
    }

    void OnEnable()
    {
        playerInputActions.Gameplay.Jump.canceled += delegate
        {
            HasJumpInputBuffer = false;
        };
    }

    //void OnGUI()
    //{
    //Rect rect = new Rect(x: 200, y: 200, width: 200, height: 200);
    //string message = "Has Jump Input Buffer: " + HasJumpInputBuffer;
    //GUIStyle style = new GUIStyle();
    //style.fontSize = 20;
    //style.fontStyle = FontStyle.Bold;
    //GUI.Label(position: rect, text: message, style: style);
    //}

    public void EnableGameplayInputs()
    {
        playerInputActions.Gameplay.Enable();
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetJumpInputBufferTimer()
    {
        StopCoroutine(methodName: nameof(JumpInputBufferCoroutine));
        StartCoroutine(methodName: nameof(JumpInputBufferCoroutine));
    }
    IEnumerator JumpInputBufferCoroutine()
    {
        HasJumpInputBuffer = true;
        yield return waitJumpInputBufferTime;
        HasJumpInputBuffer = false;
    }
}
