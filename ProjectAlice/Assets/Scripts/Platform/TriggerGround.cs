using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGround : MonoBehaviour
{
    [SerializeField] Trigger trigger;

    new Collider collider;
    MeshRenderer meshRenderer;

    void OnEnable()
    {
        trigger.Delegate += Open;
    }

    void OnDisable()
    {
        trigger.Delegate -= Open;
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
