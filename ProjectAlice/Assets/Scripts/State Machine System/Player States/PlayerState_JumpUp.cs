using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/JumpUp", fileName = "PlayerState_JumpUp")]

public class PlayerState_JumpUp : PlayerState
{
    [SerializeField] float jumpForce = 7f;

    [SerializeField] float moveSpeed = 5f;

    [SerializeField] ParticleSystem jumpVFX;

    [SerializeField] AudioClip jumpSFX;
    public override void Enter()
    {
        base.Enter();
        input.HasJumpInputBuffer = false;
        player.SetVelocityY(jumpForce);
        player.VoicePlayer.PlayOneShot(clip: jumpSFX);
        Instantiate(original: jumpVFX, position: player.transform.position, rotation: Quaternion.identity);
    }

    public override void LogicUpdate()
    {
        if (input.StopJump || player.IsFalling)
        {
            stateMachine.SwitchState(typeof(PlayerState_Fall));
        }
    }

    public override void PhysicUpdate()
    {
        player.Move(moveSpeed);
    }
}
