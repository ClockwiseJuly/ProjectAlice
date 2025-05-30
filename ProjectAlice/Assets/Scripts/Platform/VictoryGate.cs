using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryGate : MonoBehaviour
{
    [Header("通关设置")]
    [SerializeField] private float delayBeforeSceneChange = 3f;
    [SerializeField] private bool requiresItems = true; // 是否需要道具才能通关
    
    [Header("门控制设置")]
    [SerializeField] private bool enableGateDoor = true; // 是否启用门控制功能
    
    [Header("调试信息")]
    [SerializeField] private bool showDebugMessages = true;
    
    private bool hasTriggered = false;
    private bool playerInZone = false;
    private bool doorOpened = false;
    
    // 门相关组件
    private GameObject gateDoor;
    private GameObject closedDoor;
    private GameObject openedDoor;
    private VictoryGateDoorTrigger doorTriggerScript;
    
    private void Start()
    {
        // 检查子节点是否有触发器
        ValidateTriggerSetup();
        
        // 检查玩家对象的Tag设置
        ValidatePlayerTags();
        
        // 初始化门控制系统
        if (enableGateDoor)
        {
            InitializeGateDoor();
        }
    }
    
    /// <summary>
    /// 初始化门控制系统
    /// </summary>
    private void InitializeGateDoor()
    {
        // 查找GateDoor子节点
        Transform gateDoorTransform = transform.Find("GateDoor");
        if (gateDoorTransform != null)
        {
            gateDoor = gateDoorTransform.gameObject;
            
            // 查找关门和开门子节点
            Transform closedDoorTransform = gateDoorTransform.Find("关门");
            Transform openedDoorTransform = gateDoorTransform.Find("开门");
            
            if (closedDoorTransform != null)
            {
                closedDoor = closedDoorTransform.gameObject;
            }
            else
            {
                Debug.LogWarning("VictoryGate: 未找到名为'关门'的子节点！");
            }
            
            if (openedDoorTransform != null)
            {
                openedDoor = openedDoorTransform.gameObject;
            }
            else
            {
                Debug.LogWarning("VictoryGate: 未找到名为'开门'的子节点！");
            }
            
            // 为GateDoor添加专门的触发器脚本
            doorTriggerScript = gateDoor.GetComponent<VictoryGateDoorTrigger>();
            if (doorTriggerScript == null)
            {
                doorTriggerScript = gateDoor.AddComponent<VictoryGateDoorTrigger>();
            }
            doorTriggerScript.Initialize(this);
            
            // 检查GateDoor的触发器设置
            Collider doorCollider = gateDoor.GetComponent<Collider>();
            if (doorCollider == null || !doorCollider.isTrigger)
            {
                Debug.LogWarning("VictoryGate: GateDoor没有触发器组件或未设置为触发器！");
            }
            
            // 设置初始门状态（关门启用，开门禁用）
            SetDoorState(false);
            
            if (showDebugMessages)
            {
                Debug.Log($"VictoryGate: 门控制系统初始化完成 - GateDoor: {gateDoor != null}, 关门: {closedDoor != null}, 开门: {openedDoor != null}");
            }
        }
        else
        {
            Debug.LogWarning("VictoryGate: 未找到名为'GateDoor'的子节点！请确保Gate Zone下有GateDoor子节点。");
            enableGateDoor = false;
        }
    }
    
    /// <summary>
    /// 当玩家触发GateDoor时调用（由VictoryGateDoorTrigger调用）
    /// </summary>
    public void OnPlayerTriggerGateDoor()
    {
        if (showDebugMessages)
            Debug.Log("玩家触发GateDoor");
        
        // 检查是否满足通关条件
        if (CanPlayerComplete())
        {
            // 满足条件，打开门
            SetDoorState(true);
            
            if (showDebugMessages)
                Debug.Log("满足通关条件，门已打开！");
        }
        else
        {
            if (showDebugMessages)
                Debug.Log("不满足通关条件，门保持关闭状态");
            
            ShowInsufficientItemsMessage();
        }
    }
    
    /// <summary>
    /// 设置门的状态
    /// </summary>
    /// <param name="isOpen">是否打开门</param>
    private void SetDoorState(bool isOpen)
    {
        if (!enableGateDoor) return;
        
        if (closedDoor != null)
        {
            closedDoor.SetActive(!isOpen); // 关门：打开时禁用，关闭时启用
        }
        
        if (openedDoor != null)
        {
            openedDoor.SetActive(isOpen); // 开门：打开时启用，关闭时禁用
        }
        
        doorOpened = isOpen;
        
        if (showDebugMessages)
        {
            string doorState = isOpen ? "打开" : "关闭";
            Debug.Log($"VictoryGate: 门状态设置为 {doorState}");
        }
    }
    
    /// <summary>
    /// 验证当前GameObject的子节点是否有触发器设置
    /// </summary>
    private void ValidateTriggerSetup()
    {
        bool hasTrigger = false;
        
        // 检查当前GameObject的Collider
        Collider currentCollider = GetComponent<Collider>();
        if (currentCollider != null && currentCollider.isTrigger)
        {
            hasTrigger = true;
            if (showDebugMessages)
                Debug.Log($"VictoryGate: 当前GameObject '{gameObject.name}' 的Collider已设置为触发器");
        }
        
        // 检查所有子节点的Collider
        Collider[] childColliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in childColliders)
        {
            if (collider.isTrigger)
            {
                hasTrigger = true;
                if (showDebugMessages)
                    Debug.Log($"VictoryGate: 子节点 '{collider.gameObject.name}' 的Collider已设置为触发器");
            }
        }
        
        if (!hasTrigger)
        {
            Debug.LogWarning($"VictoryGate警告: GameObject '{gameObject.name}' 及其子节点都没有设置为触发器！请确保至少有一个Collider的Is Trigger被勾选。");
        }
    }
    
    /// <summary>
    /// 验证场景中玩家对象的Tag设置
    /// </summary>
    private void ValidatePlayerTags()
    {
        // 查找所有Tag为"Player2D"的GameObject
        GameObject[] player2DObjects = GameObject.FindGameObjectsWithTag("Player2D");
        
        if (player2DObjects.Length > 0)
        {
            foreach (GameObject playerObj in player2DObjects)
            {
                if (showDebugMessages)
                    Debug.Log($"VictoryGate: 找到玩家对象 '{playerObj.name}'，Tag已正确设置为'Player2D'");
            }
        }
        else
        {
            Debug.LogWarning("VictoryGate警告: 场景中未找到Tag为'Player2D'的GameObject！请确保玩家对象的Tag设置为'Player2D'");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // 检查是否是玩家（Tag为Player2D）
        if (other.CompareTag("Player2D"))
        {
            // 这里只处理主要的通关区域触发
            if (!hasTriggered)
            {
                playerInZone = true;
                
                if (showDebugMessages)
                    Debug.Log($"玩家进入通关区域 - 触发对象: {other.gameObject.name}");
                
                // 检查通关条件
                if (CanPlayerComplete())
                {
                    TriggerVictory();
                }
                else
                {
                    ShowInsufficientItemsMessage();
                }
            }
        }
        else if (showDebugMessages)
        {
            Debug.Log($"非玩家对象进入区域: {other.gameObject.name}, Tag: {other.tag}");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player2D"))
        {
            playerInZone = false;
            
            if (showDebugMessages)
                Debug.Log($"玩家离开通关区域 - 触发对象: {other.gameObject.name}");
        }
    }

    /// <summary>
    /// 检查玩家是否满足通关条件
    /// </summary>
    /// <returns>是否可以通关</returns>
    private bool CanPlayerComplete()
    {
        if (!requiresItems)
            return true;
            
        // TODO: 在这里添加道具检查逻辑
        // 示例代码（需要根据你的道具系统修改）：
        /*
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
        if (inventory != null)
        {
            // 检查是否有必需的道具
            bool hasKey = inventory.HasItem("Key");
            bool hasCrystal = inventory.HasItem("Crystal");
            
            return hasKey && hasCrystal;
        }
        */
        
        // 临时返回true，等待道具系统实现
        return true;
    }
    
    /// <summary>
    /// 触发通关
    /// </summary>
    private void TriggerVictory()
    {
        if (hasTriggered)
            return;
            
        hasTriggered = true;
        
        if (showDebugMessages)
            Debug.Log("通关触发！3秒后切换场景...");
        
        // 如果启用了门控制，确保门是打开的
        if (enableGateDoor && !doorOpened)
        {
            SetDoorState(true);
        }
        
        // 可选：播放通关音效
        PlayVictorySound();
        
        // 可选：显示通关UI
        ShowVictoryUI();
        
        // 3秒后切换场景
        StartCoroutine(LoadNextSceneAfterDelay());
    }
    
    /// <summary>
    /// 显示道具不足提示
    /// </summary>
    private void ShowInsufficientItemsMessage()
    {
        if (showDebugMessages)
            Debug.Log("道具不足，无法通关！");
            
        // TODO: 显示UI提示
        /*
        UIManager.Instance.ShowMessage("需要收集所有道具才能通关！");
        */
    }
    
    /// <summary>
    /// 播放通关音效
    /// </summary>
    private void PlayVictorySound()
    {
        // TODO: 添加音效播放
        /*
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && victoryClip != null)
        {
            audioSource.PlayOneShot(victoryClip);
        }
        */
    }
    
    /// <summary>
    /// 显示通关UI
    /// </summary>
    private void ShowVictoryUI()
    {
        // TODO: 显示通关界面
        /*
        VictoryScreen victoryScreen = FindObjectOfType<VictoryScreen>();
        if (victoryScreen != null)
        {
            victoryScreen.Show();
        }
        */
    }
    
    /// <summary>
    /// 延迟后加载下一个场景
    /// </summary>
    private IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeSceneChange);
        
        // 加载下一个场景
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        if (showDebugMessages)
            Debug.Log($"正在加载场景索引: {nextSceneIndex}");
            
        SceneManager.LoadScene(nextSceneIndex);
    }
    
    /// <summary>
    /// 外部接口：强制触发通关（用于测试或特殊情况）
    /// </summary>
    public void ForceVictory()
    {
        TriggerVictory();
    }
    
    /// <summary>
    /// 外部接口：重置通关状态
    /// </summary>
    public void ResetVictoryState()
    {
        hasTriggered = false;
        playerInZone = false;
        
        // 重置门状态
        if (enableGateDoor)
        {
            SetDoorState(false);
        }
    }
    
    /// <summary>
    /// 外部接口：手动控制门的开关
    /// </summary>
    /// <param name="open">是否打开门</param>
    public void SetDoorOpen(bool open)
    {
        if (enableGateDoor)
        {
            SetDoorState(open);
        }
    }
    
    /// <summary>
    /// 外部接口：获取门的当前状态
    /// </summary>
    /// <returns>门是否打开</returns>
    public bool IsDoorOpen()
    {
        return doorOpened;
    }
}

/// <summary>
/// GateDoor专用的触发器脚本
/// </summary>
public class VictoryGateDoorTrigger : MonoBehaviour
{
    private VictoryGate victoryGate;
    
    public void Initialize(VictoryGate gate)
    {
        victoryGate = gate;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player2D") && victoryGate != null)
        {
            victoryGate.OnPlayerTriggerGateDoor();
        }
    }
}
