using UnityEngine;

public class TriggerGround : MonoBehaviour
{
    [SerializeField] VoidEventChannel triggerEventChannel;// ���л���ǰ����¼�Ƶ������
    //[SerializeField] Trigger trigger;// ��ȡtrigger������ʵ��

    new Collider collider;
    MeshRenderer meshRenderer;

    void OnEnable()
    {
        //trigger.Delegate += Open;
        triggerEventChannel.AddListener(action: Open); // ��Ӽ���
    }

    void OnDisable()
    {
        //trigger.Delegate -= Open;
        triggerEventChannel.RemoveListener(action: Open); // ȡ������
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
