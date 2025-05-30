using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }
    
    public static float gravity = -9.81f;

    // 在TimeController类中添加帧数控制变量
    [SerializeField] public int stepForwardFrames = 30; // 可在Inspector中调整
    
    public struct RecordedData
    {
        public Vector3 pos;
        public Vector3 vel;
        public Quaternion rot;
        public Vector3 scale;
        public float animationTime;
        
        // 添加玩家状态相关数据
        public bool isGrounded;
        public bool canAirJump;
        public bool victory;
        
        // 添加输入状态（可选）
        public bool hasJumpInputBuffer;
    }

    RecordedData[,] recordedData;
    int recordMax = 100000;
    int recordCount;
    int recordIndex;

    bool wasSteppingBack = false;
    
    // 添加时间控制状态
    public bool isPaused = false;
    public bool isRewinding = false;

    TimeControlled[] timeObjects;

    private void OnEnable()
    {
        // 场景切换时重新查找TimeControlled对象
        RefreshTimeObjects();
    }

    public void RefreshTimeObjects()
    {
        timeObjects = GameObject.FindObjectsOfType<TimeControlled>();
        // 重新初始化记录数组
        recordedData = new RecordedData[timeObjects.Length, recordMax];
        recordCount = 0;
        recordIndex = 0;
        wasSteppingBack = false;
        isPaused = false;
        isRewinding = false;
    }

    void Update()
    {
        bool pause = Input.GetKeyDown(KeyCode.X);
        bool stepBack = Input.GetKey(KeyCode.Z); // 改为持续按住
        bool stepForward = Input.GetKey(KeyCode.C); // 改为持续按住
        
        // 更新暂停状态
        if (pause)
        {
            isPaused = !isPaused;
        }
        
        isRewinding = stepBack;

        if (stepBack && !isPaused)
        {
            // 连续倒流
            if (recordIndex > 0)
            {
                recordIndex--;
                ApplyRecordedData();
            }
        }
        // 修改原来的单帧前进逻辑
        else if (isPaused && stepForward)
        {
            // 暂停状态下的多帧前进
            wasSteppingBack = true;
            
            // 一次前进多帧
            for (int i = 0; i < stepForwardFrames; i++)
            {
                if (recordIndex < recordCount - 1)
                {
                    recordIndex++;
                }
                else
                {
                    break; // 到达末尾时停止
                }
            }
            
            ApplyRecordedData();
        }
        else if (!isPaused && !stepBack)
        {
            // 正常时间流逝
            if (wasSteppingBack)
            {
                recordCount = recordIndex;
                wasSteppingBack = false;
            }

            RecordCurrentData();
            
            // 更新所有时间控制对象
            foreach (TimeControlled timeObject in timeObjects)
            {
                timeObject.TimeUpdate();
                timeObject.UpdateAnimation();
            }
        }
    }
    
    void RecordCurrentData()
    {
        for (int objectIndex = 0; objectIndex < timeObjects.Length; objectIndex++)
        {
            TimeControlled timeObject = timeObjects[objectIndex];
            
            // 添加空引用检查
            if (timeObject == null || timeObject.gameObject == null)
            {
                continue; // 跳过已销毁的对象
            }
            
            timeObject.UpdateVelocity(); // 更新velocity
            
            RecordedData data = new RecordedData();
            data.pos = timeObject.transform.position;
            data.rot = timeObject.transform.rotation;
            data.scale = timeObject.transform.localScale;
            data.vel = timeObject.velocity;
            data.animationTime = timeObject.animationTime;
            
            // 如果是玩家，记录额外状态
            if (timeObject is PlayerController player && player != null)
            {
                data.isGrounded = player.IsGrounded;
                data.canAirJump = player.CanAirJump;
                data.victory = player.Victory;
                
                // 记录输入缓冲状态
                PlayerInput input = player.GetComponent<PlayerInput>();
                if (input != null)
                {
                    data.hasJumpInputBuffer = input.HasJumpInputBuffer;
                }
            }
            
            recordedData[objectIndex, recordCount] = data;
        }
        recordCount++;
        recordIndex = recordCount;
    }
    
    void ApplyRecordedData()
    {
        for (int objectIndex = 0; objectIndex < timeObjects.Length; objectIndex++)
        {
            TimeControlled timeObject = timeObjects[objectIndex];
            RecordedData data = recordedData[objectIndex, recordIndex];
            
            timeObject.transform.position = data.pos;
            timeObject.transform.rotation = data.rot;
            timeObject.transform.localScale = data.scale;
            timeObject.velocity = data.vel;
            timeObject.animationTime = data.animationTime;
            timeObject.ApplyVelocity(); // 应用velocity到Rigidbody
            timeObject.UpdateAnimation();
            
            // 如果是玩家，恢复额外状态
            if (timeObject is PlayerController player)
            {
                // 注意：某些状态可能需要特殊处理
                player.CanAirJump = data.canAirJump;
                
                // 恢复输入缓冲状态
                PlayerInput input = player.GetComponent<PlayerInput>();
                if (input != null)
                {
                    input.HasJumpInputBuffer = data.hasJumpInputBuffer;
                }
            }
        }
    }
    
    private void Awake()
    {
        // 单例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            RefreshTimeObjects();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // 订阅场景加载事件
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDestroy()
    {
        // 取消订阅
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // 场景加载完成后重新初始化
        RefreshTimeObjects();
    }
}
