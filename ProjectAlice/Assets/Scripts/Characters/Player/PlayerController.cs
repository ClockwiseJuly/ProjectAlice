using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{

    [Header("Camera Settings")]// 引用虚拟相机
    [SerializeField] private CinemachineVirtualCamera playerFollowCamera;
    [SerializeField] private float cameraTiltDuration = 0.5f; // 添加可序列化的过渡时间
    private bool isCameraTilted = false;// 记录相机是否处于倾斜状态
    private bool isTransitioning = false; // 添加过渡状态标志
    private const float TILT_ANGLE = 45f;// 相机倾斜角度

    PlayerGroundDetector groundDetector;

    PlayerInput input;

    Rigidbody rigidBody;

    //public bool CanAirJump { get; set; } = true;

    public bool IsGrounded => groundDetector.IsGrounded;

    public bool IsFalling => rigidBody.velocity.y < 0f && !IsGrounded;

    public float MoveSpeed => Mathf.Abs(rigidBody.velocity.x);

    void Awake()
    {
        groundDetector = GetComponentInChildren<PlayerGroundDetector>();
        input = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        input.EnableGameplayInputs();

        // 获取场景中的虚拟相机引用
        if (playerFollowCamera == null)
        {
            playerFollowCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        }
    }

    public void Move(float speed)
    {
        if (input.Move)
        {
            transform.localScale = new Vector3(input.AxisX, 1f, 1f);
        }

        SetVelocityX(speed * input.AxisX);
    }
    public void SetVelocity(Vector3 velocity)
    {
        rigidBody.velocity = velocity;
    }

    public void SetVelocityX(float velocityX)
    {
        rigidBody.velocity = new Vector3(velocityX, rigidBody.velocity.y);
    }

    public void SetVelocityY(float velocityY)
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, velocityY);
    }

    void Update()
    {
        if (input.Interact && !isTransitioning) // 确保在过渡期间不会触发新的切换
        {
            StartCoroutine(ToggleCameraAngleSmooth());// 按下F交互触发切换相机角度
        }
    }

    private IEnumerator ToggleCameraAngleSmooth()
    {
        if (playerFollowCamera != null)
        {
            isTransitioning = true;  // 设置过渡状态标志，防止在动画过程中重复触发
            
            var currentRotation = playerFollowCamera.transform.eulerAngles;  // 获取相机当前的欧拉角旋转值
            float startAngleX = currentRotation.x;  // 记录开始时的X轴角度
            float targetAngleX = !isCameraTilted ? TILT_ANGLE : 0f;  // 根据当前状态决定目标角度：未倾斜时设为45度，已倾斜时设为0度
            
            // Unity的欧拉角范围是0-360度，当角度大于180度时需要进行修正
            // 这样可以确保相机始终沿最短路径旋转
            if (startAngleX > 180f)
            {
                startAngleX -= 360f;
            }
            
            float elapsedTime = 0f;  // 记录已经过去的时间
            
            // 在设定的过渡时间内持续更新相机角度
            while (elapsedTime < cameraTiltDuration)
            {
                elapsedTime += Time.deltaTime;  // 累加已过去的时间
                float t = elapsedTime / cameraTiltDuration;  // 计算插值系数（0到1之间）
                
                // 使用线性插值计算当前帧的角度
                // Mathf.Lerp在两个值之间进行平滑过渡
                float newAngleX = Mathf.Lerp(startAngleX, targetAngleX, t);
                currentRotation.x = newAngleX;
                playerFollowCamera.transform.eulerAngles = currentRotation;  // 应用新的旋转角度
                
                yield return null;  // 等待下一帧继续执行
            }
            
            // 确保最终角度精确匹配目标角度
            currentRotation.x = targetAngleX;
            playerFollowCamera.transform.eulerAngles = currentRotation;
            
            isCameraTilted = !isCameraTilted;  // 切换相机倾斜状态
            isTransitioning = false;  // 重置过渡状态标志
        }
    }
}
