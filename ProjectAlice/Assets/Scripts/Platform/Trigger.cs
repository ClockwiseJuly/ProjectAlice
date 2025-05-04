using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] VoidEventChannel triggerEventChannel;// 序列化当前类的事件频道变量
    [SerializeField] AudioClip pickSFX;
    [SerializeField] ParticleSystem pickVFX;

    //public event System.Action Delegate;// 事件委托

    void OnTriggerEnter(Collider other)// 触发器而非交互
    {
        //Delegate?.Invoke();// 触发事件调用委托
        triggerEventChannel.Broadcast(); // 广播事件
        SoundEffectPlayer.audioSource.PlayOneShot(pickSFX);
        Instantiate(original: pickVFX, position: transform.position, rotation: Quaternion.identity);
        Destroy(obj: gameObject);

    }
}
