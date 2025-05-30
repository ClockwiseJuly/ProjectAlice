using System.Collections;
using UnityEngine;

public class SimpleMonsterController : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private LayerMask groundLayerMask = -1;
    
    [Header("移动模式设置")]
    [SerializeField] private int minMoveSteps = 2;
    [SerializeField] private int maxMoveSteps = 4;
    [SerializeField] private float stepDuration = 0.5f;
    [SerializeField] private float waitTime = 1f;
    
    [Header("调试信息")]
    [SerializeField] private bool showDebugMessages = true;
    [SerializeField] private bool showGroundRays = true;
    
    private Rigidbody rb;
    private Collider col;
    private bool isMoving = false;
    private bool hasValidGround = false;
    
    private void Start()
    {
        // 获取3D组件
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        
        if (rb == null)
        {
            Debug.LogError($"SimpleMonsterController: {gameObject.name} 没有找到Rigidbody组件！");
            return;
        }
        
        if (col == null)
        {
            Debug.LogError($"SimpleMonsterController: {gameObject.name} 没有找到Collider组件！");
            return;
        }
        
        // 设置刚体属性
        rb.freezeRotation = true;
        
        // 验证地面设置
        ValidateGroundObjects();
        
        // 开始移动
        if (hasValidGround)
        {
            StartCoroutine(MovementRoutine());
        }
        else
        {
            Debug.LogWarning($"SimpleMonsterController: {gameObject.name} 没有找到有效地面，不会开始移动");
        }
        
        if (showDebugMessages)
            Debug.Log($"SimpleMonsterController: {gameObject.name} 初始化完成");
    }
    
    /// <summary>
    /// 验证地面对象设置
    /// </summary>
    private void ValidateGroundObjects()
    {
        GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("2DCollider");
        int validCount = 0;
        
        foreach (GameObject obj in groundObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Ground") && obj.GetComponent<Collider>() != null)
            {
                validCount++;
                if (showDebugMessages)
                    Debug.Log($"SimpleMonsterController: 找到有效地面 '{obj.name}'");
            }
        }
        
        hasValidGround = validCount > 0;
        
        if (!hasValidGround)
        {
            Debug.LogWarning($"SimpleMonsterController: {gameObject.name} 没有找到符合条件的地面对象！\n" +
                           "请确保地面对象的Tag为'2DCollider'，Layer为'Ground'，并且有Collider组件。");
        }
        else
        {
            if (showDebugMessages)
                Debug.Log($"SimpleMonsterController: {gameObject.name} 找到 {validCount} 个有效地面对象");
        }
    }
    
    /// <summary>
    /// 移动循环
    /// </summary>
    private IEnumerator MovementRoutine()
    {
        while (true)
        {
            // 随机选择移动方向和步数
            int direction = Random.Range(0, 2) == 0 ? -1 : 1; // -1左，1右
            int steps = Random.Range(minMoveSteps, maxMoveSteps + 1);
            
            if (showDebugMessages)
                Debug.Log($"SimpleMonsterController: {gameObject.name} 开始移动 - 方向: {(direction > 0 ? "右" : "左")}，步数: {steps}");
            
            // 执行移动步骤
            for (int i = 0; i < steps; i++)
            {
                // 检查是否可以安全移动
                if (CanMoveInDirection(direction))
                {
                    yield return StartCoroutine(MoveOneStep(direction));
                }
                else
                {
                    if (showDebugMessages)
                        Debug.Log($"SimpleMonsterController: {gameObject.name} 检测到前方不安全，停止移动");
                    break;
                }
            }
            
            // 等待一段时间
            if (showDebugMessages)
                Debug.Log($"SimpleMonsterController: {gameObject.name} 等待 {waitTime} 秒");
            
            yield return new WaitForSeconds(waitTime);
        }
    }
    
    /// <summary>
    /// 执行一步移动
    /// </summary>
    private IEnumerator MoveOneStep(int direction)
    {
        isMoving = true;
        float elapsedTime = 0f;
        
        while (elapsedTime < stepDuration)
        {
            // 使用Transform移动（适用于3D对象）
            Vector3 movement = new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);
            transform.Translate(movement);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        isMoving = false;
    }
    
    /// <summary>
    /// 检查是否可以向指定方向移动
    /// </summary>
    private bool CanMoveInDirection(int direction)
    {
        if (col == null) return false;
        
        // 检查脚下是否有地面
        bool hasGroundBelow = CheckGroundBelow();
        
        // 检查前方是否有地面
        bool hasGroundAhead = CheckGroundAhead(direction);
        
        return hasGroundBelow && hasGroundAhead;
    }
    
    /// <summary>
    /// 检查脚下是否有地面
    /// </summary>
    private bool CheckGroundBelow()
    {
        Vector3 rayOrigin = new Vector3(transform.position.x, col.bounds.min.y, transform.position.z);
        Ray ray = new Ray(rayOrigin, Vector3.down);
        
        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, groundLayerMask))
        {
            bool isValidGround = hit.collider.gameObject.CompareTag("2DCollider");
            
            if (showGroundRays)
            {
                Color rayColor = isValidGround ? Color.green : Color.red;
                Debug.DrawRay(rayOrigin, Vector3.down * groundCheckDistance, rayColor, 0.1f);
            }
            
            return isValidGround;
        }
        
        if (showGroundRays)
        {
            Debug.DrawRay(rayOrigin, Vector3.down * groundCheckDistance, Color.red, 0.1f);
        }
        
        return false;
    }
    
    /// <summary>
    /// 检查前方是否有地面
    /// </summary>
    private bool CheckGroundAhead(int direction)
    {
        // 计算前方检测点
        float checkDistance = col.bounds.size.x * 0.6f;
        Vector3 frontPosition = transform.position + new Vector3(direction * checkDistance, 0, 0);
        Vector3 rayOrigin = new Vector3(frontPosition.x, col.bounds.min.y, frontPosition.z);
        
        Ray ray = new Ray(rayOrigin, Vector3.down);
        
        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance * 1.5f, groundLayerMask))
        {
            bool isValidGround = hit.collider.gameObject.CompareTag("2DCollider");
            
            if (showGroundRays)
            {
                Color rayColor = isValidGround ? Color.blue : Color.yellow;
                Debug.DrawRay(rayOrigin, Vector3.down * groundCheckDistance * 1.5f, rayColor, 0.1f);
            }
            
            return isValidGround;
        }
        
        if (showGroundRays)
        {
            Debug.DrawRay(rayOrigin, Vector3.down * groundCheckDistance * 1.5f, Color.yellow, 0.1f);
        }
        
        return false;
    }
    
    /// <summary>
    /// 外部接口：停止移动
    /// </summary>
    public void StopMovement()
    {
        StopAllCoroutines();
        isMoving = false;
        if (rb != null)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }
    
    /// <summary>
    /// 外部接口：重新开始移动
    /// </summary>
    public void StartMovement()
    {
        if (!isMoving && hasValidGround)
        {
            StartCoroutine(MovementRoutine());
        }
    }
    
    /// <summary>
    /// 外部接口：设置移动速度
    /// </summary>
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = Mathf.Max(0, speed);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (col != null)
        {
            // 绘制地面检测范围
            Gizmos.color = Color.green;
            Vector3 center = new Vector3(transform.position.x, col.bounds.min.y - groundCheckDistance * 0.5f, transform.position.z);
            Gizmos.DrawWireCube(center, new Vector3(col.bounds.size.x, groundCheckDistance, col.bounds.size.z));
        }
    }
}