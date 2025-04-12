using UnityEngine;
using UnityEngine.AI;

public class BossPatrolState : BaseState
{
    private NavMeshAgent Agent;
    private Animator Anim;
    private PlayerDetector PlayerDetector;
    private Transform PlayerTransform;
    private float PatrolRadius;
    private float MinPatrolDistance;
    private int PatrolCount = 0; // 用于记录巡逻次数
    private bool SetDestinationSny;
    private Vector3 Destination;

    public override void EnterState(BaseEnemy Enemy)
    {
        CurrentEnemy = Enemy;
        Agent = CurrentEnemy.Agent;
        Anim = CurrentEnemy.Anim;
        PlayerDetector = CurrentEnemy.PlayerDetector;
        PatrolRadius = CurrentEnemy.PatrolRadius;
        MinPatrolDistance = CurrentEnemy.MinPatrolDistance;
        PlayerTransform = CurrentEnemy.PlayerTransform;
        
        if (CurrentEnemy.footstepSource.clip != CurrentEnemy.walkClip)
            CurrentEnemy.footstepSource.clip = CurrentEnemy.walkClip;

        if (!CurrentEnemy.footstepSource.isPlaying )
        {
            float maxTime = CurrentEnemy.walkClip.length - 0.3f; 
            CurrentEnemy.footstepSource.time = Random.Range(0f, maxTime);

            CurrentEnemy.footstepSource.pitch = Random.Range(0.95f, 1.05f);
            CurrentEnemy.footstepSource.loop = true;
            CurrentEnemy.footstepSource.Play();
        }

        // 设置巡逻参数
        Agent.speed = CurrentEnemy.CurrentSpeed;
        Agent.stoppingDistance = CurrentEnemy.StoppingDistance;

        // 初始化巡逻状态
        PatrolCount = 0;
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
                CurrentEnemy.IsIdle = true; // 标记为正在等待
                Anim.CrossFade("Idle", 0.1f); // 播放等待动画
                CurrentEnemy.TimeCounter(() => PatrolLogicStrategy());
            }

            if(PlayerDetector.CanDetectPlayer())
            {
                // Debug.Log("检测到玩家，切换到追逐状态！");
                // 如果检测到玩家，切换到追逐状态
                CurrentEnemy.IsPatrolling = false;
                CurrentEnemy.IsChasing = true;
                CurrentEnemy.SwitchState(BaseEnemyState.Chase);
            }
        }
    }

    public override void PhysicsUpdateState()
    {
        if (SetDestinationSny)
        {
            SetNewDestination();
        }
    }

    public override void ExitState()
    {
        
    }

    // 随机生成新目标，确保生成点距离 Boss 不低于 MinPatrolDistance
    private void SetNewDestination()
    {
        float randomDistance = Random.Range(MinPatrolDistance, PatrolRadius);
        Vector3 randomDirection = Random.insideUnitSphere.normalized * randomDistance;
        randomDirection += PlayerTransform.position;
        
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, PatrolRadius, NavMesh.AllAreas))
        {
            Destination = hit.position;
            Anim.CrossFade("Patrol", 0.1f); // 播放巡逻动画
            Agent.SetDestination(Destination);
            SetDestinationSny = false; // 设置为 false，表示已经设置了目标点
        }
    }

    private void PatrolLogicStrategy()
    {
        if(CurrentEnemy.IsPatrolling)
        {
            PatrolCount++; // 增加巡逻次数
            // 如果完成两次巡逻，切换到返回状态
            if (PatrolCount >= CurrentEnemy.PatrolCount)
            {
                CurrentEnemy.IsPatrolling = false;
                CurrentEnemy.IsReturning = true;
                CurrentEnemy.SwitchState(BaseEnemyState.Return);

            }
            else
            {
                // 继续设置下一个巡逻目标
                SetDestinationSny = true; // 设置为 true，表示需要设置新的目标点
            }
        } 
        
    }
    


}


