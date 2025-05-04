using UnityEngine;

public class VictoryGate : MonoBehaviour
{
    [SerializeField] VoidEventChannel levelclearedEventChannel;
    [SerializeField] AudioClip pickSFX;
    [SerializeField] ParticleSystem pickVFX;

    void OnTriggerEnter(Collider other)// 触发器而非交互
    {
        levelclearedEventChannel.Broadcast(); // 广播事件
        SoundEffectPlayer.audioSource.PlayOneShot(pickSFX);
        Instantiate(original: pickVFX, position: transform.position, rotation: Quaternion.identity);
        //Destroy(obj: gameObject);
    }
}
