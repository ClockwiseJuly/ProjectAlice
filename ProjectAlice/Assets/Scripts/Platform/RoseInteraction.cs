using UnityEngine;

public class RoseInteraction : MonoBehaviour
{
    [Header("玫瑰交互设置")]
    [SerializeField] private KeyCode interactionKey = KeyCode.F;
    [SerializeField] private Sprite[] roseSprites = new Sprite[2]; // 两张不同的sprite贴图
    
    [Header("调试信息")]
    [SerializeField] private bool showDebugMessages = true;
    
    private bool playerInRange = false;
    private int currentSpriteIndex = 0;
    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        // 获取SpriteRenderer组件
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null)
        {
            Debug.LogError($"RoseInteraction: {gameObject.name} 没有找到SpriteRenderer组件！");
            return;
        }
        
        // 验证sprite数组设置
        ValidateSprites();
        
        // 设置初始sprite（如果有的话）
        if (roseSprites.Length > 0 && roseSprites[0] != null)
        {
            spriteRenderer.sprite = roseSprites[0];
            currentSpriteIndex = 0;
        }
        
        // 验证触发器设置
        ValidateTriggerSetup();
    }
    
    private void Update()
    {
        // 检查玩家输入
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            ToggleSprite();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player2D"))
        {
            playerInRange = true;
            
            if (showDebugMessages)
                Debug.Log($"玩家靠近 {gameObject.name} - 按{interactionKey}键切换玫瑰状态");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player2D"))
        {
            playerInRange = false;
            
            if (showDebugMessages)
                Debug.Log($"玩家离开 {gameObject.name}");
        }
    }
    
    /// <summary>
    /// 切换sprite贴图
    /// </summary>
    private void ToggleSprite()
    {
        if (roseSprites.Length < 2)
        {
            if (showDebugMessages)
                Debug.LogWarning($"{gameObject.name}: 需要至少2张sprite才能切换！");
            return;
        }
        
        if (spriteRenderer == null)
        {
            Debug.LogError($"{gameObject.name}: SpriteRenderer组件丢失！");
            return;
        }
        
        // 切换到下一个sprite
        currentSpriteIndex = (currentSpriteIndex + 1) % roseSprites.Length;
        
        if (roseSprites[currentSpriteIndex] != null)
        {
            spriteRenderer.sprite = roseSprites[currentSpriteIndex];
            
            if (showDebugMessages)
                Debug.Log($"{gameObject.name}: 切换到sprite {currentSpriteIndex + 1}");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: Sprite {currentSpriteIndex + 1} 为空！");
        }
        
        // 可选：播放切换音效
        PlaySwitchSound();
        
        // 可选：显示切换效果
        ShowSwitchEffect();
    }
    
    /// <summary>
    /// 验证sprite数组设置
    /// </summary>
    private void ValidateSprites()
    {
        if (roseSprites.Length == 0)
        {
            Debug.LogWarning($"{gameObject.name}: Rose Sprites数组为空！请在Inspector中添加sprite。");
            return;
        }
        
        if (roseSprites.Length < 2)
        {
            Debug.LogWarning($"{gameObject.name}: Rose Sprites数组只有{roseSprites.Length}个元素，建议至少添加2个sprite用于切换。");
        }
        
        // 检查是否有空的sprite引用
        for (int i = 0; i < roseSprites.Length; i++)
        {
            if (roseSprites[i] == null)
            {
                Debug.LogWarning($"{gameObject.name}: Rose Sprites[{i}] 为空！请在Inspector中设置sprite。");
            }
        }
        
        if (showDebugMessages)
            Debug.Log($"{gameObject.name}: 已设置 {roseSprites.Length} 个sprite用于切换");
    }
    
    /// <summary>
    /// 验证触发器设置
    /// </summary>
    private void ValidateTriggerSetup()
    {
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogWarning($"{gameObject.name}: 没有找到Collider组件！请添加Collider并设置为触发器。");
        }
        else if (!col.isTrigger)
        {
            Debug.LogWarning($"{gameObject.name}: Collider的Is Trigger未勾选！请在Inspector中勾选Is Trigger。");
        }
        else if (showDebugMessages)
        {
            Debug.Log($"{gameObject.name}: 触发器设置正确");
        }
    }
    
    /// <summary>
    /// 播放切换音效
    /// </summary>
    private void PlaySwitchSound()
    {
        // TODO: 添加音效播放
        /*
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && switchClip != null)
        {
            audioSource.PlayOneShot(switchClip);
        }
        */
    }
    
    /// <summary>
    /// 显示切换视觉效果
    /// </summary>
    private void ShowSwitchEffect()
    {
        // TODO: 添加视觉效果
        /*
        ParticleSystem particles = GetComponent<ParticleSystem>();
        if (particles != null)
        {
            particles.Play();
        }
        */
    }
    
    /// <summary>
    /// 外部接口：设置特定的sprite
    /// </summary>
    /// <param name="index">sprite索引</param>
    public void SetSprite(int index)
    {
        if (index >= 0 && index < roseSprites.Length && roseSprites[index] != null)
        {
            currentSpriteIndex = index;
            spriteRenderer.sprite = roseSprites[index];
            
            if (showDebugMessages)
                Debug.Log($"{gameObject.name}: 手动设置为sprite {index + 1}");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: 无效的sprite索引: {index}");
        }
    }
    
    /// <summary>
    /// 外部接口：获取当前sprite索引
    /// </summary>
    /// <returns>当前sprite索引</returns>
    public int GetCurrentSpriteIndex()
    {
        return currentSpriteIndex;
    }
    
    /// <summary>
    /// 外部接口：重置到第一个sprite
    /// </summary>
    public void ResetToFirstSprite()
    {
        SetSprite(0);
    }
}