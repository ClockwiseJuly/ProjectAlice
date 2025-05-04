using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purification : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;
    [SerializeField] ParticleSystem pickupVFX;
    new Collider collider;
    MeshRenderer meshRenderer;

    void Awake()
    {
        collider = GetComponent<Collider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(component: out PlayerController player))
        {
            collider.enabled = false; // ������ײ�壬�����ظ�����
            meshRenderer.enabled = false; // ��������
            SoundEffectPlayer.audioSource.PlayOneShot(clip: pickupSFX); // ������Ч
            Instantiate(original: pickupVFX, position: transform.position, rotation: transform.rotation); // ʵ��������Ч��

            // ������ͳ��UI����

        }
    }
}
