using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [SerializeField] PlayerState[] states;//序列化玩家状态数组

    Animator animator;

    PlayerController player;

    PlayerInput input;

    //对玩家每个状态进行初始化
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        input = GetComponent<PlayerInput>();

        player = GetComponent<PlayerController>();

        stateTable = new Dictionary<System.Type, IState>(states.Length);//字典默认元素个数为状态数组长度

        foreach (PlayerState state in states)
        {
            state.Initialize(animator, player, input, this);
            stateTable.Add(state.GetType(), state);
            //获取键和值，键为状态类型，值为状态实例
            //当要取得字典中某个特定的值，只要传入状态对应的type，就可取得此键对应的状态类
        }
    }

    void Start()
    {
        SwitchOn(stateTable[typeof(PlayerState_Idle)]);//获取空闲状态类
    }
}
