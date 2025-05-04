using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/CoyoteTime", fileName = "PlayerState_CoyoteTime")]
public class PlayerState_CoyoteTime : PlayerState
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float CoyoteTime = 0.1f;
    public override void Enter()
    {
        base.Enter();
        player.SetUseGravity(value: false);
    }

    public override void Exit()
    {
        player.SetUseGravity(value: true);
    }


    public override void LogicUpdate()
    {

        if (input.Jump)
        {
            stateMachine.SwitchState(typeof(PlayerState_JumpUp));
        }

        if (StateDuration > CoyoteTime || !input.Move)
        {
            stateMachine.SwitchState(typeof(PlayerState_Fall));
        }

    }

    public override void PhysicUpdate()
    {
        player.Move(speed: runSpeed);
    }
}
