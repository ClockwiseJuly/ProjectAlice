using UnityEngine;

public class VictoryGate : MonoBehaviour
{
    [SerializeField] VoidEventChannel levelclearedEventChannel;// ���л���ǰ����¼�Ƶ��������ֻҪ����Ҫ������Ƶ������������ͬ���ı�������
    [SerializeField] AudioClip pickSFX;
    //[SerializeField] ParticleSystem pickVFX;

    void OnTriggerEnter(Collider other)// ���������ǽ���
    {
        levelclearedEventChannel.Broadcast(); // �㲥�¼�
        SoundEffectPlayer.audioSource.PlayOneShot(pickSFX);
        //Instantiate(original: pickVFX, position: transform.position, rotation: Quaternion.identity);
        //Destroy(obj: gameObject);
    }
}
