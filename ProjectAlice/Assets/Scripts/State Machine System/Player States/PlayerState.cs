using UnityEngine;

public class PlayerState : ScriptableObject, IState
{
    [SerializeField] string stateName;

    [SerializeField, Range(0f, 1f)] float transitionDuration = 0.1f;//���浭������ʱ��

    float stateStartTime;//״̬��ʼʱ��

    int stateHash;//״̬��ϣֵ

    protected float currentSpeed;

    protected Animator animator;//���ж����л�

    protected PlayerController player;//��ҿ���������

    protected PlayerInput input;

    protected PlayerStateMachine stateMachine;//ִ��״̬�л�

    //�����Ƿ񲥷���ϣ�ͨ���жϵ�ǰ״̬����ʱ���Ƿ���ڵ��ڵ�ǰ����״̬�ĳ���
    protected bool IsAnimationFinished => StateDuration >= animator.GetCurrentAnimatorStateInfo(0).length;

    //��ȡ��ǰ״̬����ʱ��
    protected float StateDuration => Time.time - stateStartTime;


    void OnEnable()
    {
        stateHash = Animator.StringToHash(stateName);
    }

    //��ʼ�����
    public void Initialize(Animator animator, PlayerController player, PlayerInput input, PlayerStateMachine stateMachine)
    {
        this.animator = animator;
        this.input = input;
        this.player = player;
        this.stateMachine = stateMachine;
    }

    //virtual���η��������������д�˷���
    public virtual void Enter()
    {
        animator.CrossFade(stateHash, transitionDuration);//���Ŷ������浭��
        stateStartTime = Time.time;
    }

    public virtual void Exit()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicUpdate()
    {

    }
}
