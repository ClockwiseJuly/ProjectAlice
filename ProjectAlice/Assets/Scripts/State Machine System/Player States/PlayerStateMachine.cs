using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [SerializeField] PlayerState[] states;//���л����״̬����

    Animator animator;

    PlayerController player;

    PlayerInput input;

    //�����ÿ��״̬���г�ʼ��
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        input = GetComponent<PlayerInput>();

        player = GetComponent<PlayerController>();

        stateTable = new Dictionary<System.Type, IState>(states.Length);//�ֵ�Ĭ��Ԫ�ظ���Ϊ״̬���鳤��

        foreach (PlayerState state in states)
        {
            state.Initialize(animator, player, input, this);
            stateTable.Add(state.GetType(), state);
            //��ȡ����ֵ����Ϊ״̬���ͣ�ֵΪ״̬ʵ��
            //��Ҫȡ���ֵ���ĳ���ض���ֵ��ֻҪ����״̬��Ӧ��type���Ϳ�ȡ�ô˼���Ӧ��״̬��
        }
    }

    void Start()
    {
        SwitchOn(stateTable[typeof(PlayerState_Idle)]);//��ȡ����״̬��
    }
}
