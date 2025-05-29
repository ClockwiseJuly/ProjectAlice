using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundDetector : MonoBehaviour
{
    [SerializeField] float detectionRadius = 0.1f;
    [SerializeField] LayerMask groundLayer;

    Collider[] colliders = new Collider[1];

    //Ͷ����������������ײ�жϣ�NonAlloc���ڴ����·������������
    //����1�����ģ�����2������뾶������3���洢���������ײ�����飬����4�����Ĳ㼶,����5��Ĭ�Ϻ���
    //������ʽ�����
    public bool IsGrounded => Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, colliders, groundLayer) != 0;

    //Ͷ������
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
