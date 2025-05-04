using UnityEngine;

public class TriggerGround : MonoBehaviour
{
    [SerializeField] VoidEventChannel triggerEventChannel;
    //[SerializeField] Trigger trigger;

    new Collider collider;
    MeshRenderer meshRenderer;

    void OnEnable()
    {
        //trigger.Delegate += Open;
        triggerEventChannel.AddListener(action: Open); // 订阅事件
    }

    void OnDisable()
    {
        //trigger.Delegate -= Open;
        triggerEventChannel.RemoveListener(action: Open); // 取消订阅事件
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
