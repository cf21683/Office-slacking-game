using UnityEngine;

public abstract class BaseState
{
    protected BaseEnemy CurrentEnemy;
    public abstract void EnterState(BaseEnemy Enemy);
    public abstract void PhysicsUpdateState();
    public abstract void LogicUpdateState();
    public abstract void ExitState();

}
