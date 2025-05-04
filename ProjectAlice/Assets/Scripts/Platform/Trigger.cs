using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] AudioClip pickSFX;
    [SerializeField] ParticleSystem pickVFX;

    public event System.Action Delegate;

    void OnTriggerEnter(Collider other)// ���������ǽ���
    {
        Delegate?.Invoke();// �����¼�

        SoundEffectPlayer.audioSource.PlayOneShot(pickSFX);
        Instantiate(original: pickVFX, position: transform.position, rotation: Quaternion.identity);
        Destroy(obj: gameObject);

    }
}
