using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorFragment : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;// 拾取音效
    [SerializeField] ParticleSystem pickupVFX;// 拾取粒子效果

    //[SerializeField] float resetTime = 3.0f; // 重置时间

    WaitForSeconds waitResetTime;
    new Collider collider;
    MeshRenderer meshRenderer;

    void Awake()
    {
        collider = GetComponent<Collider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        //waitResetTime = new WaitForSeconds(seconds:resetTime); // 设置重置时间
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(component: out PlayerController player))//检测进入触发器对象是否是玩家
        {
            collider.enabled = false; // 禁用碰撞体，避免重复触发
            meshRenderer.enabled = false; // 关闭渲染器，隐藏物体
            SoundEffectPlayer.audioSource.PlayOneShot(clip: pickupSFX); // 播放音效
            Instantiate(original: pickupVFX, position: transform.position, rotation: transform.rotation); // 实例化粒子效果

            //player.CanAirJump = true; // 允许玩家空中跳跃

            // 镜子碎片统计UI更新

            //Invoke(methodName: nameof(Reset), time :resetTime); //Invoke只适合调用一次，多次则用协程
            //StartCoroutine(routine : ResetCoroutine());

        }
    }

    //拾取物体重生
    // void Reset()
    // {
    //     collider.enabled = true; // 确保在重置时启用碰撞体
    //     meshRenderer.enabled = true; // 确保在重置时启用渲染器
    // }

    // IEnumerator ResetCoroutine()
    // {
    //     yield return waitResetTime; // 等待指定的重置时间
    //     Reset():
    // }
}
