using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Fall", fileName = "PlayerState_Fall")]
public class PlayerState_Fall : PlayerState
{
    [SerializeField] AnimationCurve speedCurve;//动画速度曲线

    [SerializeField] float moveSpeed = 5f;
    public override void LogicUpdate()
    {
        if (player.IsGrounded)
        {
            stateMachine.SwitchState(typeof(PlayerState_Land));
        }

        if (input.Jump)
        {
            if (player.CanAirJump)
            {
                stateMachine.SwitchState(newStateType: typeof(PlayerState_AirJump));
            }

            //跳跃未触发二段跳，一段时间后从true变为false
            input.SetJumpInputBufferTimer();
        }
    }

    public override void PhysicUpdate()
    {
        player.Move(moveSpeed);//空中移动
        player.SetVelocityY(speedCurve.Evaluate(StateDuration));//控制掉落速度
    }
}