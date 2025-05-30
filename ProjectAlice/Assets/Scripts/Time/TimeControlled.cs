using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControlled : MonoBehaviour
{
    public Vector3 velocity; // 改为Vector3支持3D
    public AnimationClip currentAnimation;
    public float animationTime;
    
    // 添加Rigidbody引用
    protected Rigidbody rb;
    
    // 添加状态机引用（如果有的话）
    protected PlayerStateMachine stateMachine;
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stateMachine = GetComponent<PlayerStateMachine>();
    }

    public virtual void TimeUpdate()
    {
        if (currentAnimation != null)
        {
            animationTime += Time.deltaTime;
            if(animationTime > currentAnimation.length)
            {
                animationTime = animationTime - currentAnimation.length;
            }
        }
    }
    
    public void UpdateAnimation()
    {
        if (currentAnimation != null)
        {
            currentAnimation.SampleAnimation(gameObject, animationTime);
        }
    }
    
    // 添加获取和设置velocity的方法
    public virtual void UpdateVelocity()
    {
        if (rb != null)
        {
            velocity = rb.velocity;
        }
    }
    
    public virtual void ApplyVelocity()
    {
        if (rb != null)
        {
            rb.velocity = velocity;
        }
    }
}
  
 