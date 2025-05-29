using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorFragment : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;// ʰȡ��Ч
    [SerializeField] ParticleSystem pickupVFX;// ʰȡ����Ч��

    //[SerializeField] float resetTime = 3.0f; // ����ʱ��

    WaitForSeconds waitResetTime;
    new Collider collider;
    MeshRenderer meshRenderer;

    void Awake()
    {
        collider = GetComponent<Collider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        //waitResetTime = new WaitForSeconds(seconds:resetTime); // ��������ʱ��
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(component: out PlayerController player))//�����봥���������Ƿ������
        {
            collider.enabled = false; // ������ײ�壬�����ظ�����
            meshRenderer.enabled = false; // �ر���Ⱦ������������
            SoundEffectPlayer.audioSource.PlayOneShot(clip: pickupSFX); // ������Ч
            Instantiate(original: pickupVFX, position: transform.position, rotation: transform.rotation); // ʵ��������Ч��

            //player.CanAirJump = true; // ������ҿ�����Ծ

            // ������Ƭͳ��UI����

            //Invoke(methodName: nameof(Reset), time :resetTime); //Invokeֻ�ʺϵ���һ�Σ��������Э��
            //StartCoroutine(routine : ResetCoroutine());

        }
    }

    //ʰȡ��������
    // void Reset()
    // {
    //     collider.enabled = true; // ȷ��������ʱ������ײ��
    //     meshRenderer.enabled = true; // ȷ��������ʱ������Ⱦ��
    // }

    // IEnumerator ResetCoroutine()
    // {
    //     yield return waitResetTime; // �ȴ�ָ��������ʱ��
    //     Reset():
    // }
}
