using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EventChannels/VoidEventChannel", fileName = "VoidEventChannel_")]

public class VoidEventChannel : ScriptableObject
{
    event System.Action Delegate; // 事件委托

    public void Broadcast() // 广播事件
    {
        Delegate?.Invoke(); // 如果有订阅者，则调用事件
    }

    public void AddListener(System.Action action) // 添加监听
    {
        Delegate += action; // 将订阅者添加到事件委托中
    }

    public void RemoveListener(System.Action action) // 移除监听
    {
        Delegate -= action; // 将订阅者从事件委托中移除
    }
}
