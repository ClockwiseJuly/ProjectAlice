using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EventChannels/VoidEventChannel", fileName = "VoidEventChannel_")]

public class VoidEventChannel : ScriptableObject
{
    event System.Action Delegate; // �¼�ί��

    public void Broadcast() // �㲥�¼�
    {
        Delegate?.Invoke(); // ����ж����ߣ�������¼�
    }

    public void AddListener(System.Action action) // ��Ӽ���
    {
        Delegate += action; // ����������ӵ��¼�ί����
    }

    public void RemoveListener(System.Action action) // �Ƴ�����
    {
        Delegate -= action; // �������ߴ��¼�ί�����Ƴ�
    }
}
