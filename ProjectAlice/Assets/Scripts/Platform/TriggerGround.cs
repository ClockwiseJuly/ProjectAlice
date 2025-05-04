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
        triggerEventChannel.AddListener(action: Open); // �����¼�
    }

    void OnDisable()
    {
        //trigger.Delegate -= Open;
        triggerEventChannel.RemoveListener(action: Open); // ȡ�������¼�
    }

    void Awake()
    {
        collider = GetComponent<Collider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        collider.enabled = false; // ������ײ��
        meshRenderer.enabled = false; // ��������
    }
    void Open()
    {
        collider.enabled = true;
        meshRenderer.enabled = true;
    }
}
