using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundDetector : MonoBehaviour
{
    [SerializeField] float detectionRadius = 0.1f;
    [SerializeField] LayerMask groundLayer;

    Collider[] colliders = new Collider[1];

    //投射虚拟球体用于碰撞判断，NonAlloc无内存重新分配和垃圾回收
    //参数1：球心，参数2：球体半径，参数3：存储检测结果的碰撞体数组，参数4：检测的层级,参数5：默认忽略
    //主体表达式更简洁
    public bool IsGrounded => Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, colliders, groundLayer) != 0;

    //投射球体
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
