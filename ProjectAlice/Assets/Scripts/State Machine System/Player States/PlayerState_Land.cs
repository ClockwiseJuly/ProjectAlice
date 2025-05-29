using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Land", fileName = "PlayerState_Land")]
public class PlayerState_Land : PlayerState
{
    [SerializeField] float stiffTime = 0.2f;//��ؽ�ֱʱ��
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();

        player.SetVelocity(Vector3.zero);

        player.CanAirJump = true;//Ҫ��Ϊ��ȡ���߽�����������ɾ���˾�
    }
    public override void LogicUpdate()
    {
        if (input.HasJumpInputBuffer || input.Jump)//�������뻺��
        {
            stateMachine.SwitchState(typeof(PlayerState_JumpUp));
        }

        //״̬Ӳֱ
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
