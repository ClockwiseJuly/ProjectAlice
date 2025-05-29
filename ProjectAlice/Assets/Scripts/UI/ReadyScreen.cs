using UnityEngine;

public class ReadyScreen : MonoBehaviour
{
    [SerializeField] AudioClip startVoice; // 开始语音
    [SerializeField] VoidEventChannel levelStartedEventChannel; // 事件通道，用于通知关卡开始

    //动画事件函数
    void LevelStart()
    {
        levelStartedEventChannel.Broadcast(); // 广播关卡开始事件
        GetComponent<Canvas>().enabled = false; // 禁用当前画布
        GetComponent<Animator>().enabled = false; // 禁用当前动画控制器
    }

    void PlayDtartVoice()
    {
        SoundEffectPlayer.audioSource.PlayOneShot(clip: startVoice); // 播放开始语音
    }
}
