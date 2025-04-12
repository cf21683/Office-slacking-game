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

        if (!CurrentEnemy.footstepSource.isPlaying)
        {
            
            float maxTime = CurrentEnemy.walkClip.length - 0.3f; 
            CurrentEnemy.footstepSource.time = Random.Range(0f, maxTime);

            CurrentEnemy.footstepSource.pitch = Random.Range(0.95f, 1.05f);
            CurrentEnemy.footstepSource.loop = true;
            CurrentEnemy.footstepSource.Play();
        }

        SetDestinationSny = true; 
        Anim.CrossFade("Patrol", 0.1f); // 播放巡逻动画
    }
    
    public override void LogicUpdateState()
    {
        
        if (Agent != null && !Agent.pathPending)
        {
            // 如果到达目标点
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                CurrentEnemy.IsIdle = true;
                Anim.CrossFade("Idle", 0.1f); // 播放等待动画
                CurrentEnemy.TimeCounter(() => ReturnLogicStrategy());
            }
            if(PlayerDetector.CanDetectPlayer())
            {
                // Debug.Log("检测到玩家，切换到追逐状态！");
                // 如果检测到玩家，切换到追逐状态
                CurrentEnemy.IsReturning = false;
                CurrentEnemy.IsChasing = true;
                CurrentEnemy.SwitchState(BaseEnemyState.Chase);
            }
        }
    }

    public override void PhysicsUpdateState()
    {
        if (SetDestinationSny)
        {
            Anim.CrossFade("Patrol", 0.1f); // 播放巡逻动画
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
        CurrentEnemy.SwitchState(BaseEnemyState.Patrol);
    }

}
