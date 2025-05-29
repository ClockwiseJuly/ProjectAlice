using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1.持有所有的状态类，并对他们进行管理和切换
//2.负责进行当前状态的更新
public class StateMachine : MonoBehaviour
{
    IState currentState;

    //将状态的Type作为键，状态实例作为值存入字典
    //键为状态类型System.Type，值为状态类实例IState的字典
    protected Dictionary<System.Type, IState> stateTable;

    //处理逻辑更新
    void Update()
    {
        currentState.LogicUpdate();
    }

    //处理物理更新
    void FixedUpdate()
    {
        currentState.PhysicUpdate();
    }

    //启动状态机
    protected void SwitchOn(IState newState)
    {
        currentState = newState;
        currentState.Enter();
    }

    //状态切换
    public void SwitchState(IState newState)
    {
        currentState.Exit();
        SwitchOn(newState);
    }

    //状态切换重载，调用原状态切换函数，传入参数对应状态字典中的值即可
    public void SwitchState(System.Type newStateType)
    {
        SwitchState(stateTable[newStateType]);
    }
}