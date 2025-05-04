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
            collider.enabled = false; // ������ײ�壬�����ظ�����
            meshRenderer.enabled = false; // ��������
            audioSource.PlayOneShot(pickupSFX); // ������Ч
            Instantiate(original: pickupVFX, position: transform.position, rotation: transform.rotation); // ʵ��������Ч��

            // ������Ƭͳ��UI����

        }
    }
}
