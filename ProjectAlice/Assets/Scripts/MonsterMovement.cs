using System.Collections;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private LayerMask groundLayerMask = -1;
    
    [Header("移动模式设置")]
    [SerializeField] private float minMoveTime = 1f;
    [SerializeField] private float maxMoveTime = 2f;
    [SerializeField] private float minWaitTime = 0.5f;
    [SerializeField] private float maxWaitTime = 1.5f;
    
    [Header("调试信息")]
    [SerializeField] private bool showDebugMessages = true;
    [SerializeField] private bool showGroundCheck = true;
    
    private Rigidbody2D rb;
    private Collider2D col;
    private bool isMoving = false;
    private int moveDirection = 1; // 1为右，-1为左
    private bool isGrounded = false;
    
    private void Awake()
    {
        // 检查GameObject是否有效
        if (gameObject == null)
        {
            Debug.LogError("MonsterMovement: GameObject is null in Awake()");
            return;
        }
        
        // 获取组件
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            
            // 验证组件是否成功添加
            if (rb == null)
            {
                Debug.LogError($"MonsterMovement: 无法为 {gameObject.name} 添加Rigidbody2D组件！");
                return;
            }
        }
        
        // 安全检查：确保rb不为null再设置属性
        if (rb != null)
        {
            // 设置Rigidbody2D属性
            rb.freezeRotation = true;
            rb.gravityScale = 1f;
            
            if (showDebugMessages)
                Debug.Log($"MonsterMovement: {gameObject.name} 初始化完成");
        }
        else
        {
            Debug.LogError($"MonsterMovement: {gameObject.name} 的Rigidbody2D组件为null，无法初始化！");
        }
        
        // 检查Collider2D组件
        if (col == null)
        {
            Debug.LogWarning($"MonsterMovement: {gameObject.name} 没有Collider2D组件，地面检测可能无法正常工作！");
        }
    }
    
    private void Start()
    {
        // 验证地面设置
        ValidateGroundSettings();
        
        // 开始移动循环
        StartCoroutine(MovementLoop());
    }
    
    private void Update()
    {
        // 检查是否在地面上
        CheckGrounded();
    }
    
    /// <summary>
    /// 验证地面设置
    /// </summary>
    private void ValidateGroundSettings()
    {
        // 查找附近的地面对象
        GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("2DCollider");
        int validGroundCount = 0;
        
        foreach (GameObject obj in groundObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Ground") && obj.GetComponent<Collider2D>() != null)
            {
                validGroundCount++;
                if (showDebugMessages)
                    Debug.Log($"MonsterMovement: 找到有效地面对象 '{obj.name}'，Layer: {LayerMask.LayerToName(obj.layer)}");
            }
        }
        
        if (validGroundCount == 0)
        {
            Debug.LogWarning($"MonsterMovement: {gameObject.name} 附近没有找到符合条件的地面对象！\n" +
                           "请确保地面对象的Tag为'2DCollider'，Layer为'Ground'，并且有Collider2D组件。");
        }
        else
        {
            if (showDebugMessages)
                Debug.Log($"MonsterMovement: {gameObject.name} 找到 {validGroundCount} 个有效地面对象");
        }
    }
    
    /// <summary>
    /// 检查是否在地面上
    /// </summary>
    private void CheckGrounded()
    {
        // 临时设置为总是在地面上，用于测试移动功能
        isGrounded = true;
        
        // 注释掉原来的检测代码进行测试
        /*
        if (col == null) return;
        
        Vector2 rayOrigin = new Vector2(transform.position.x, col.bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance, groundLayerMask);
        
        isGrounded = hit.collider != null;
        
        if (isGrounded && hit.collider.gameObject.tag != "2DCollider")
        {
            isGrounded = false;
        }
        */
    }
    
    /// <summary>
    /// 移动循环协程
    /// </summary>
    private IEnumerator MovementLoop()
    {
        while (true)
        {
            // 随机选择移动方向和次数
            int moveSteps = Random.Range(2, 5); // 2-4次移动
            moveDirection = Random.Range(0, 2) == 0 ? -1 : 1; // 随机选择左右
            
            if (showDebugMessages)
                Debug.Log($"MonsterMovement: {gameObject.name} 开始移动，方向: {(moveDirection > 0 ? "右" : "左")}，步数: {moveSteps}");
            
            // 执行移动
            for (int i = 0; i < moveSteps; i++)
            {
                float moveTime = Random.Range(minMoveTime, maxMoveTime);
                yield return StartCoroutine(MoveInDirection(moveDirection, moveTime));
                
                // 检查是否还在地面上，如果不在则停止移动
                if (!isGrounded)
                {
                    if (showDebugMessages)
                        Debug.Log($"MonsterMovement: {gameObject.name} 检测到即将离开地面，停止移动");
                    break;
                }
                
                // 短暂停顿
                yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            }
            
            // 等待一段时间后继续下一轮移动
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            if (showDebugMessages)
                Debug.Log($"MonsterMovement: {gameObject.name} 等待 {waitTime:F1} 秒后继续移动");
            
            yield return new WaitForSeconds(waitTime);
        }
    }
    
    /// <summary>
    /// 向指定方向移动
    /// </summary>
    private IEnumerator MoveInDirection(int direction, float duration)
    {
        isMoving = true;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            // 检查前方是否有地面
            if (!CheckGroundAhead(direction))
            {
                if (showDebugMessages)
                    Debug.Log($"MonsterMovement: {gameObject.name} 前方没有地面，停止移动");
                break;
            }
            
            // 移动
            Vector2 movement = new Vector2(direction * moveSpeed * Time.deltaTime, 0);
            transform.Translate(movement);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        isMoving = false;
    }
    
    /// <summary>
    /// 检查前方是否有地面
    /// </summary>
    private bool CheckGroundAhead(int direction)
    {
        if (col == null) return false;
        
        // 检查前方地面
        Vector2 frontRayOrigin = new Vector2(
            transform.position.x + (direction * col.bounds.size.x * 0.6f),
            col.bounds.min.y
        );
        
        RaycastHit2D frontHit = Physics2D.Raycast(frontRayOrigin, Vector2.down, groundCheckDistance * 1.5f, groundLayerMask);
        
        bool hasGroundAhead = frontHit.collider != null && frontHit.collider.gameObject.tag == "2DCollider";
        
        // 调试显示
        if (showGroundCheck)
        {
            Color rayColor = hasGroundAhead ? Color.blue : Color.yellow;
            Debug.DrawRay(frontRayOrigin, Vector2.down * groundCheckDistance * 1.5f, rayColor);
        }
        
        return hasGroundAhead;
    }
    
    /// <summary>
    /// 外部接口：暂停移动
    /// </summary>
    public void PauseMovement()
    {
        StopAllCoroutines();
        isMoving = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
    
    /// <summary>
    /// 外部接口：恢复移动
    /// </summary>
    public void ResumeMovement()
    {
        if (!isMoving)
        {
            StartCoroutine(MovementLoop());
        }
    }
    
    /// <summary>
    /// 外部接口：设置移动速度
    /// </summary>
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = Mathf.Max(0, speed);
    }
}