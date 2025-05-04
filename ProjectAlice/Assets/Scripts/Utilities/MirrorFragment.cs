using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorFragment : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;
    [SerializeField] ParticleSystem pickupVFX;

    AudioSource audioSource;
    new Collider collider;
    MeshRenderer meshRenderer;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        collider = GetComponent<Collider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(component: out PlayerController player))
        {
            collider.enabled = false; // 禁用碰撞体，避免重复触发
            meshRenderer.enabled = false; // 隐藏物体
            audioSource.PlayOneShot(pickupSFX); // 播放音效
            Instantiate(original: pickupVFX, position: transform.position, rotation: transform.rotation); // 实例化粒子效果

            // 镜子碎片统计UI更新

        }
    }
}
