using UnityEngine;
using UnityEngine.AI;
public class BossChaseState : BaseState
{
    private NavMeshAgent Agent;
    private Animator Anim;
    private PlayerDetector PlayerDetector;
    private Transform PlayerTransform;
    private bool SetDestinationSny;
    private Vector3 Destination;
    public override void EnterState(BaseEnemy Enemy)
    {
        CurrentEnemy = Enemy;
        Agent = CurrentEnemy.Agent; 
        Anim = CurrentEnemy.Anim;
        PlayerDetector = CurrentEnemy.PlayerDetector; // 获取 PlayerDetector 组件
        PlayerTransform = PlayerDetector.PlayerTransform; // 获取玩家的 Transform
        Agent.speed = CurrentEnemy.CurrentSpeed;
        Agent.stoppingDistance = CurrentEnemy.StoppingDistance;
        Destination = PlayerTransform.position; // 获取玩家的位置
        SetDestinationSny = true; 

        if (!CurrentEnemy.footstepSource.isPlaying)
        {
            float maxTime = CurrentEnemy.walkClip.length - 0.3f; 
            CurrentEnemy.footstepSource.time = Random.Range(0f, maxTime);

            CurrentEnemy.footstepSource.pitch = Random.Range(1.2f, 1.4f);
            CurrentEnemy.footstepSource.loop = true;
            CurrentEnemy.footstepSource.Play();
        }
        Anim.CrossFade("Chase", 0.3f); // 播放 Chase 动画
    }

    
    public override void LogicUpdateState()
    {
         if (Agent != null && !Agent.pathPending)
        {
            // 如果到达目标点
            if (PlayerDetector.CanDetectPlayer() && Agent.remainingDistance <= Agent.stoppingDistance && !CurrentEnemy.IsAfterAdmonish )
            {
                CurrentEnemy.IsChasing = false;
                CurrentEnemy.IsAdmonishing = true; // 标记为正在警告
                Agent.isStopped = true; // 停止移动
                CurrentEnemy.SwitchState(BaseEnemyState.Admonish); // 切换到警告状态
            }
            if (PlayerDetector.CanDetectPlayer() && Agent.remainingDistance > Agent.stoppingDistance)
            {
                PlayerTransform = PlayerDetector.PlayerTransform; // 更新玩家的 Transform 
                Destination = PlayerTransform.position; // 获取玩家的新位置
                SetDestinationSny = true; // 设置为 true，表示需要设置目标点
            }
            if(!PlayerDetector.CanDetectPlayer())
            {
                // 如果未检测到玩家，切换到回归状态
                CurrentEnemy.IsPatrolling = true;
                CurrentEnemy.IsChasing = false;
                CurrentEnemy.SwitchState(BaseEnemyState.Return); // 切换到回归状态
            }
        }
    }

    public override void PhysicsUpdateState()
    {
        if (SetDestinationSny)
        {
            Agent.SetDestination(Destination); // 设置返回点为目标点
            SetDestinationSny = false; // 设置为 false，表示已经设置了目标点
        }
        
    }
    
    public override void ExitState()
    {
        CurrentEnemy.footstepSource.Stop();
    }
}
