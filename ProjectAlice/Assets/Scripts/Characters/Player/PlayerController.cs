using UnityEngine;
using Cinemachine;

public class PlayerController : TimeControlled
{
    [SerializeField] VoidEventChannel levelclearedEventChannel;// 监听关卡胜利事件频道

    [Header("Camera Settings")]// 引用虚拟相机
    [SerializeField] private CinemachineVirtualCamera playerFollowCamera;

    PlayerGroundDetector groundDetector;

    PlayerInput input;

    Rigidbody rigidBody;

    public AudioSource VoicePlayer { get; private set; } // 语音播放器

    public bool Victory { get; private set; }// 胜利状态
    public bool CanAirJump { get; set; } = true;// 空中跳跃二段跳

    //public bool CanAirJump { get; set; };默认false，可作为获取道具解锁二段跳能力的基础
    //相关代码：Land状态，镜子碎片等拾取物体

    public bool IsGrounded => groundDetector.IsGrounded;

    public bool IsFalling => rigidBody.velocity.y < 0f && !IsGrounded;

    //public float MoveSpeed => Mathf.Abs(rigidBody.velocity.x);

    // 修改移动速度计算，考虑X和Z轴
    public float MoveSpeed => Mathf.Sqrt(rigidBody.velocity.x * rigidBody.velocity.x +
    rigidBody.velocity.z * rigidBody.velocity.z);
    private BoxCollider boxCollider;

    //[SerializeField] private float cameraTiltDuration = 0.5f; // 添加可序列化的过渡时间
    //private bool isCameraTilted = false;// 记录相机是否处于倾斜状态
    //private bool isTransitioning = false; // 添加过渡状态标志
    //private const float TILT_ANGLE = 45f;// 相机倾斜角度
    //private bool is2DMode = false; // 跟踪当前视角模式
    //private int currentViewDirection = 0; // 0=正面，1=右侧，2=背面，3=左侧
    protected override void Awake()
    {
        base.Awake(); // 调用基类的Awake方法
        
        groundDetector = GetComponentInChildren<PlayerGroundDetector>();
        input = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody>();
        VoicePlayer = GetComponentInChildren<AudioSource>();
    }

    void Start()
    {
        input.EnableGameplayInputs();//启用动作表

        // 获取场景中的虚拟相机引用
        if (playerFollowCamera == null)
        {
            playerFollowCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        }
    }

    //根据玩家x轴方向输入来移动并改变朝向
    public void Move(float speed)
    {
        UpdatePlayerRotation();// 检测按键组合并调整角色朝向

        //if (input.Move)
        //if (input.AxisX != 0f)// 只在X轴有输入时才改变朝向

        // 保持localScale控制左右朝向，但只在没有前进输入时才应用
        if (input.AxisX != 0f && input.AxisY <= 0)
        {
            transform.localScale = new Vector3(input.AxisX, 1f, 1f);//镜像旋转
            transform.rotation = Quaternion.Euler(0, 0, 0); //重置旋转
        }

        // 添加平滑处理
        Vector3 targetVelocity = new Vector3(speed * input.AxisX, rigidBody.velocity.y, speed * input.AxisY);

        // 如果没有输入，应用更强的减速
        if (Mathf.Approximately(input.AxisX, 0f) && Mathf.Approximately(input.AxisY, 0f))
        {
            // 水平方向平滑减速到零
            rigidBody.velocity = new Vector3(
                Mathf.Lerp(rigidBody.velocity.x, 0, Time.deltaTime * 10), // 增加减速系数
                rigidBody.velocity.y,
                Mathf.Lerp(rigidBody.velocity.z, 0, Time.deltaTime * 10)
            );
        }
        else
        {
            // 有输入时正常设置速度
            SetVelocityXZ(targetVelocity.x, targetVelocity.z);
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        rigidBody.velocity = velocity;
    }

    // 添加设置X轴速度的方法，处理玩家左右移动
    public void SetVelocityX(float velocityX)
    {
        rigidBody.velocity = new Vector3(velocityX, rigidBody.velocity.y);
    }

    // 添加设置Y轴速度的方法，处理玩家上下移动
    public void SetVelocityY(float velocityY)
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, velocityY);
    }

    // 添加设置Z轴速度的方法    
    public void SetVelocityZ(float velocityZ)
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, velocityZ);
    }

    // 添加同时设置X和Z轴速度的方法
    public void SetVelocityXZ(float velocityX, float velocityZ)
    {
        rigidBody.velocity = new Vector3(velocityX, rigidBody.velocity.y, velocityZ);
    }

    // 根据按键组合更新角色旋转
    private void UpdatePlayerRotation()
    {
        // 获取当前输入值
        float xInput = input.AxisX;
        float yInput = input.AxisY;

        // 只在有前进输入时才应用旋转
        if (yInput > 0)
        {
            // 检测W+A组合（左上方移动）
            if (xInput < 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f); // 重置缩放
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            // 检测W+D组合（右上方移动）
            else if (xInput > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                transform.rotation = Quaternion.Euler(0, 270, 0);
            }
            // 仅向前移动(W)
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                transform.rotation = Quaternion.Euler(0, 225, 0);
            }
        }
    }

    // 设置刚体的重力使用状态
    public void SetUseGravity(bool value)
    {
        rigidBody.useGravity = value; 
    }

    void OnEnable()
    {
        levelclearedEventChannel.AddListener(action: OnLevelCleared); // 添加监听
    }

    void OnDisable()
    {
        levelclearedEventChannel.RemoveListener(action: OnLevelCleared); // 取消监听
    }

    void OnLevelCleared()
    {
        Victory = true; // 设置胜利状态为true
    }

    private void OnCollisionEnter(Collision collision)
    {
        boxCollider = collision.gameObject.GetComponent<BoxCollider>();
        Debug.Log(boxCollider);
    }

    public GameObject GetboxCollider()
    {
        return boxCollider.gameObject;
    }

    // void Update()
    // {
    //     if (input.Interact && !isTransitioning) // 确保在过渡期间不会触发新的切换
    //     {
    //         StartCoroutine(ToggleCameraAngleSmooth());// 按下F交互触发切换相机角度
    //     }
    // }

    // private IEnumerator ToggleCameraAngleSmooth()
    // {
    //     if (playerFollowCamera != null)
    //     {
    //         isTransitioning = true;  // 设置过渡状态标志，防止在动画过程中重复触发

    //         var currentRotation = playerFollowCamera.transform.eulerAngles;  // 获取相机当前的欧拉角旋转值
    //         float startAngleX = currentRotation.x;  // 记录开始时的X轴角度
    //         float targetAngleX = !isCameraTilted ? TILT_ANGLE : 0f;  // 根据当前状态决定目标角度：未倾斜时设为45度，已倾斜时设为0度

    //         // Unity的欧拉角范围是0-360度，当角度大于180度时需要进行修正
    //         // 这样可以确保相机始终沿最短路径旋转
    //         if (startAngleX > 180f)
    //         {
    //             startAngleX -= 360f;
    //         }

    //         float elapsedTime = 0f;  // 记录已经过去的时间

    //         // 在设定的过渡时间内持续更新相机角度
    //         while (elapsedTime < cameraTiltDuration)
    //         {
    //             elapsedTime += Time.deltaTime;  // 累加已过去的时间
    //             float t = elapsedTime / cameraTiltDuration;  // 计算插值系数（0到1之间）

    //             // 使用线性插值计算当前帧的角度
    //             // Mathf.Lerp在两个值之间进行平滑过渡
    //             float newAngleX = Mathf.Lerp(startAngleX, targetAngleX, t);
    //             currentRotation.x = newAngleX;
    //             playerFollowCamera.transform.eulerAngles = currentRotation;  // 应用新的旋转角度

    //             yield return null;  // 等待下一帧继续执行
    //         }

    //         // 确保最终角度精确匹配目标角度
    //         currentRotation.x = targetAngleX;
    //         playerFollowCamera.transform.eulerAngles = currentRotation;

    //         isCameraTilted = !isCameraTilted;  // 切换相机倾斜状态
    //         isTransitioning = false;  // 重置过渡状态标志
    //     }
    // }


    //处理视角模式变化
    // public void OnViewModeChanged(bool is2DMode, int viewDirection)
    // {
    //     // 存储当前视角模式
    //     this.is2DMode = is2DMode;
    //     this.currentViewDirection = viewDirection;
    //     // 可以在这里添加更多逻辑，例如：
    //     // 1. 调整移动速度
    //     // 2. 修改碰撞体大小或形状
    //     // 3. 切换动画状态
    //     // 4. 调整重力设置
    //     // 重置速度，避免视角切换时的异常移动
    //     rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
    // }

    // 重写TimeUpdate方法
    public override void TimeUpdate()
    {
        base.TimeUpdate();
        
        // 在时间倒流时，禁用某些更新
        TimeController timeController = FindObjectOfType<TimeController>();
        if (timeController != null && timeController.isRewinding)
        {
            return; // 时间倒流时不执行正常更新
        }
        
        // 正常时间流逝时的更新逻辑可以放在这里
    }
    
    public override void UpdateVelocity()
    {
        base.UpdateVelocity();
        // PlayerController特有的velocity更新逻辑
    }
    
    public override void ApplyVelocity()
    {
        base.ApplyVelocity();
        // PlayerController特有的velocity应用逻辑
    }
    
    // 添加时间控制相关的方法
    public void OnTimeRewind()
    {
        // 时间倒流时的特殊处理
        // 例如：禁用输入、停止音效等
    }
    
    public void OnTimeResume()
    {
        // 时间恢复时的特殊处理
    }
}
