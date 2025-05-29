using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1.�������е�״̬�࣬�������ǽ��й�����л�
//2.������е�ǰ״̬�ĸ���
public class StateMachine : MonoBehaviour
{
    IState currentState;

    //��״̬��Type��Ϊ����״̬ʵ����Ϊֵ�����ֵ�
    //��Ϊ״̬����System.Type��ֵΪ״̬��ʵ��IState���ֵ�
    protected Dictionary<System.Type, IState> stateTable;

    //�����߼�����
    void Update()
    {
        currentState.LogicUpdate();
    }

    //�����������
    void FixedUpdate()
    {
        currentState.PhysicUpdate();
    }

    //����״̬��
    protected void SwitchOn(IState newState)
    {
        currentState = newState;
        currentState.Enter();
    }

    //״̬�л�
    public void SwitchState(IState newState)
    {
        currentState.Exit();
        SwitchOn(newState);
    }

    //״̬�л����أ�����ԭ״̬�л����������������Ӧ״̬�ֵ��е�ֵ����
    public void SwitchState(System.Type newStateType)
    {
        SwitchState(stateTable[newStateType]);
    }
}