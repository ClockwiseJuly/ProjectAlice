using UnityEngine;

public class VictoryGate : MonoBehaviour
{
    [SerializeField] VoidEventChannel levelclearedEventChannel;// 序列化当前类的事件频道变量，只要在需要监听此频道的类里声明同样的变量即可
    [SerializeField] AudioClip pickSFX;
    //[SerializeField] ParticleSystem pickVFX;

    void OnTriggerEnter(Collider other)// 触发器而非交互
    {
        levelclearedEventChannel.Broadcast(); // 广播事件
        SoundEffectPlayer.audioSource.PlayOneShot(pickSFX);
        //Instantiate(original: pickVFX, position: transform.position, rotation: Quaternion.identity);
        //Destroy(obj: gameObject);
    }
}
