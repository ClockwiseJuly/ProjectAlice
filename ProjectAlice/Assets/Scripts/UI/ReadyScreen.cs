using UnityEngine;

public class ReadyScreen : MonoBehaviour
{
    [SerializeField] AudioClip startVoice; // ��ʼ����
    [SerializeField] VoidEventChannel levelStartedEventChannel; // �¼�ͨ��������֪ͨ�ؿ���ʼ

    //�����¼�����
    void LevelStart()
    {
        levelStartedEventChannel.Broadcast(); // �㲥�ؿ���ʼ�¼�
        GetComponent<Canvas>().enabled = false; // ���õ�ǰ����
        GetComponent<Animator>().enabled = false; // ���õ�ǰ����������
    }

    void PlayDtartVoice()
    {
        SoundEffectPlayer.audioSource.PlayOneShot(clip: startVoice); // ���ſ�ʼ����
    }
}
