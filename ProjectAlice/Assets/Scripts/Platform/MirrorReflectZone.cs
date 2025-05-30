using System.Collections.Generic;
using UnityEngine;

public class MirrorReflectZone : MonoBehaviour
{
    [Header("镜子映射设置")]
    [SerializeField] private KeyCode activationKey = KeyCode.F;
    [SerializeField] private bool requirePlayerInZone = true;
    
    [Header("调试信息")]
    [SerializeField] private bool showDebugMessages = true;
    
    private bool playerInZone = false;
    private bool mirrorActivated = false;
    private List<GameObject> mirrorGrounds = new List<GameObject>();
    
    private void Start()
    {
        // 查找所有符合条件的Mirror ground对象
        FindMirrorGrounds();
        
        // 确保Mirror ground初始状态为禁用
        SetMirrorGroundsActive(false);
    }
    
    private void Update()
    {
        // 检查玩家输入
        if (Input.GetKeyDown(activationKey))
        {
            if (!requirePlayerInZone || playerInZone)
            {
                ToggleMirrorReflection();
            }
            else if (showDebugMessages)
            {
                Debug.Log("玩家不在镜子范围内，无法激活镜子映射");
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player2D"))
        {
            playerInZone = true;
            
            if (showDebugMessages)
                Debug.Log($"玩家进入镜子区域 - 按{activationKey}键激活镜子映射");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player2D"))
        {
            playerInZone = false;
            
            if (showDebugMessages)
                Debug.Log("玩家离开镜子区域");
        }
    }
    
    /// <summary>
    /// 查找所有符合条件的Mirror ground对象
    /// 条件：Tag是2DCollider，layer是Ground，名称包含"Mirror ground"
    /// </summary>
    private void FindMirrorGrounds()
    {
        mirrorGrounds.Clear();
        
        // 获取Ground层的LayerMask
        int groundLayer = LayerMask.NameToLayer("Ground");
        
        if (groundLayer == -1)
        {
            Debug.LogWarning("MirrorReflectZone: 未找到名为'Ground'的Layer！请确保已创建Ground层。");
            return;
        }
        
        // 查找所有Tag为"2DCollider"的GameObject
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("2DCollider");
        
        foreach (GameObject obj in taggedObjects)
        {
            // 检查Layer是否为Ground
            if (obj.layer == groundLayer)
            {
                // 检查名称是否包含"Mirror ground"（不区分大小写）
                if (obj.name.ToLower().Contains("mirror ground"))
                {
                    mirrorGrounds.Add(obj);
                    
                    if (showDebugMessages)
                        Debug.Log($"找到Mirror ground对象: {obj.name} (Tag: {obj.tag}, Layer: {LayerMask.LayerToName(obj.layer)})");
                }
            }
        }
        
        if (showDebugMessages)
            Debug.Log($"总共找到 {mirrorGrounds.Count} 个Mirror ground对象");
            
        if (mirrorGrounds.Count == 0)
        {
            Debug.LogWarning("MirrorReflectZone: 未找到符合条件的Mirror ground对象！请检查对象的Tag、Layer和名称设置。");
        }
    }
    
    /// <summary>
    /// 切换镜子映射状态
    /// </summary>
    private void ToggleMirrorReflection()
    {
        mirrorActivated = !mirrorActivated;
        SetMirrorGroundsActive(mirrorActivated);
        
        if (showDebugMessages)
        {
            string status = mirrorActivated ? "激活" : "关闭";
            Debug.Log($"镜子映射已{status} - 影响了 {mirrorGrounds.Count} 个Mirror ground对象");
        }
        
        // 可选：播放音效
        PlayMirrorSound();
        
        // 可选：显示视觉效果
        ShowMirrorEffect();
    }
    
    /// <summary>
    /// 设置所有Mirror ground对象的激活状态
    /// </summary>
    /// <param name="active">是否激活</param>
    private void SetMirrorGroundsActive(bool active)
    {
        foreach (GameObject mirrorGround in mirrorGrounds)
        {
            if (mirrorGround != null)
            {
                mirrorGround.SetActive(active);
                
                if (showDebugMessages)
                    Debug.Log($"Mirror ground '{mirrorGround.name}' 设置为: {(active ? "激活" : "禁用")}");
            }
        }
    }
    
    /// <summary>
    /// 播放镜子激活音效
    /// </summary>
    private void PlayMirrorSound()
    {
        // TODO: 添加音效播放
        /*
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && mirrorActivationClip != null)
        {
            audioSource.PlayOneShot(mirrorActivationClip);
        }
        */
    }
    
    /// <summary>
    /// 显示镜子激活视觉效果
    /// </summary>
    private void ShowMirrorEffect()
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
    /// 重新扫描Mirror ground对象（用于运行时动态添加对象的情况）
    /// </summary>
    public void RefreshMirrorGrounds()
    {
        FindMirrorGrounds();
        
        if (showDebugMessages)
            Debug.Log("已重新扫描Mirror ground对象");
    }
    
    /// <summary>
    /// 外部接口：强制激活镜子映射
    /// </summary>
    public void ForceMirrorActivation(bool activate)
    {
        mirrorActivated = activate;
        SetMirrorGroundsActive(mirrorActivated);
        
        if (showDebugMessages)
            Debug.Log($"强制设置镜子映射状态为: {(activate ? "激活" : "关闭")}");
    }
    
    /// <summary>
    /// 外部接口：获取当前镜子激活状态
    /// </summary>
    public bool IsMirrorActivated()
    {
        return mirrorActivated;
    }
    
    /// <summary>
    /// 外部接口：获取找到的Mirror ground对象数量
    /// </summary>
    public int GetMirrorGroundCount()
    {
        return mirrorGrounds.Count;
    }
}