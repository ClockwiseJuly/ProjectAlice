using UnityEngine;

public class TriggerGround : MonoBehaviour
{
    [SerializeField] VoidEventChannel triggerEventChannel;// 序列化当前类的事件频道变量
    //[SerializeField] Trigger trigger;// 获取trigger的引用实例

    new Collider collider;
    MeshRenderer meshRenderer;

    void OnEnable()
    {
        //trigger.Delegate += Open;
        triggerEventChannel.AddListener(action: Open); // 添加监听
    }

    void OnDisable()
    {
        //trigger.Delegate -= Open;
        triggerEventChannel.RemoveListener(action: Open); // 取消监听
    }

    void Awake()
    {
        collider = GetComponent<Collider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        collider.enabled = false; // 禁用碰撞体
        meshRenderer.enabled = false; // 隐藏物体
    }
    void Open()
    {
        collider.enabled = true;
        meshRenderer.enabled = true;
    }
}
