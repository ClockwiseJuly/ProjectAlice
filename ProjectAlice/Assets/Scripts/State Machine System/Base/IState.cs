//I开头状态接口
public interface IState
{
    void Enter();//状态进入
    void Exit();//状态推出
    void LogicUpdate();//状态逻辑更新
    void PhysicUpdate();//状态物理更新（基于刚体模拟运动）

}
