using UnityEngine;

public class PlayerState : ScriptableObject, IState
{
    [SerializeField] string stateName;

    [SerializeField, Range(0f, 1f)] float transitionDuration = 0.1f;//交叉淡化持续时间

    float stateStartTime;//状态开始时间

    int stateHash;//状态哈希值

    protected float currentSpeed;

    protected Animator animator;//进行动画切换

    protected PlayerController player;//玩家控制器引用

    protected PlayerInput input;

    protected PlayerStateMachine stateMachine;//执行状态切换

    //动画是否播放完毕，通过判断当前状态持续时间是否大于等于当前动画状态的长度
    protected bool IsAnimationFinished => StateDuration >= animator.GetCurrentAnimatorStateInfo(0).length;

    //获取当前状态持续时间
    protected float StateDuration => Time.time - stateStartTime;


    void OnEnable()
    {
        stateHash = Animator.StringToHash(stateName);
    }

    //初始化组件
    public void Initialize(Animator animator, PlayerController player, PlayerInput input, PlayerStateMachine stateMachine)
    {
        this.animator = animator;
        this.input = input;
        this.player = player;
        this.stateMachine = stateMachine;
    }

    //virtual修饰符，让子类可以重写此方法
    public virtual void Enter()
    {
        animator.CrossFade(stateHash, transitionDuration);//播放动画交叉淡化
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
