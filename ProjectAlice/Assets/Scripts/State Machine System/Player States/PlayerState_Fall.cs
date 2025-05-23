using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Fall", fileName = "PlayerState_Fall")]
public class PlayerState_Fall : PlayerState
{
    [SerializeField] AnimationCurve speedCurve;

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

            input.SetJumpInputBufferTimer();
        }
    }

    public override void PhysicUpdate()
    {
        player.Move(moveSpeed);
        player.SetVelocityY(speedCurve.Evaluate(StateDuration));
    }
}