using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Land", fileName = "PlayerState_Land")]
public class PlayerState_Land : PlayerState
{
    [SerializeField] float stiffTime = 0.2f;//落地僵直时间
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();

        player.SetVelocity(Vector3.zero);

        player.CanAirJump = true;//要改为获取道具解锁二段跳则删除此句
    }
    public override void LogicUpdate()
    {
        if (input.HasJumpInputBuffer || input.Jump)//处理输入缓冲
        {
            stateMachine.SwitchState(typeof(PlayerState_JumpUp));
        }

        //状态硬直
        if (StateDuration < stiffTime)
        {
            return;
        }

        if (input.Move)
        {
            stateMachine.SwitchState(typeof(PlayerState_Run));
        }

        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }

    }
}
