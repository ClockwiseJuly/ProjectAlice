using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] VoidEventChannel triggerEventChannel;// ���л���ǰ����¼�Ƶ������
    [SerializeField] AudioClip pickSFX;
    [SerializeField] ParticleSystem pickVFX;

    //public event System.Action Delegate;// �¼�ί��

    void OnTriggerEnter(Collider other)// ���������ǽ���
    {
        //Delegate?.Invoke();// �����¼�����ί��
        triggerEventChannel.Broadcast(); // �㲥�¼�
        SoundEffectPlayer.audioSource.PlayOneShot(pickSFX);
        Instantiate(original: pickVFX, position: transform.position, rotation: Quaternion.identity);
        Destroy(obj: gameObject);

    }
}
