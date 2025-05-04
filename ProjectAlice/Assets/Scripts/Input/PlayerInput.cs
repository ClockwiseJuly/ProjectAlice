using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    PlayerInputActions playerInputActions;
    Vector2 axes => playerInputActions.Gameplay.Axes.ReadValue<Vector2>();
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
    }

    public void EnableGameplayInputs()
    {
        playerInputActions.Gameplay.Enable();
        //Cursor.lockState = CursorLockMode.Locked;
    }


}
