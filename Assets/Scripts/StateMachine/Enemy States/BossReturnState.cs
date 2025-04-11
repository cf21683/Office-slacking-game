using UnityEngine;
using UnityEngine.AI;
public class BossReturnState : BaseState
{
    private NavMeshAgent Agent;
    private Animator Anim;
    private PlayerDetector PlayerDetector;
    private Vector3 ReturnPoint;
    private bool SetDestinationSny;

    public override void EnterState(BaseEnemy Enemy)
    {
        // Debug.Log("进入回归状态");
        CurrentEnemy = Enemy;
        Agent = CurrentEnemy.Agent;
        Anim = CurrentEnemy.Anim;
        PlayerDetector = CurrentEnemy.PlayerDetector;
        ReturnPoint = CurrentEnemy.ReturnPointTransform.position;

        Agent.speed = CurrentEnemy.CurrentSpeed;
        Agent.stoppingDistance = CurrentEnemy.StoppingDistance;

        SetDestinationSny = true; 
        Anim.SetBool("IsPatrol", true); // 设置返回动画
    }
    
    public override void LogicUpdateState()
    {
        
        if (Agent != null && !Agent.pathPending)
        {
            // 如果到达目标点
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                CurrentEnemy.IsIdle = true;
                Anim.SetBool("IsPatrol", false); // 设置返回动画为 false
                Anim.SetBool("IsIdle", true); // 设置等待动画为 true
                CurrentEnemy.TimeCounter(() => ReturnLogicStrategy());
            }
            if(PlayerDetector.CanDetectPlayer())
            {
                // Debug.Log("检测到玩家，切换到追逐状态！");
                // 如果检测到玩家，切换到追逐状态
                CurrentEnemy.IsReturning = false;
                CurrentEnemy.IsChasing = true;
                Anim.SetBool("IsPatrol", false); // 设置巡逻动画为 false
                Anim.SetBool("IsIdle", false); // 设置等待动画为 false
                CurrentEnemy.SwitchState(BaseEnemyState.Chase);
            }
        }
    }

    public override void PhysicsUpdateState()
    {
        if (SetDestinationSny)
        {
            Anim.SetBool("IsPatrol", true); // 设置返回动画为 true
            Agent.SetDestination(ReturnPoint); // 设置返回点为目标点
            SetDestinationSny = false; // 设置为 false，表示已经设置了目标点
        }
    }
    
    public override void ExitState()
    {
        
    }

    private void ReturnLogicStrategy()
    {
        CurrentEnemy.IsPatrolling = true;
        CurrentEnemy.IsReturning = false;
        Anim.SetBool("IsIdle", false); // 设置等待动画为 false
        CurrentEnemy.SwitchState(BaseEnemyState.Patrol);
    }

}
