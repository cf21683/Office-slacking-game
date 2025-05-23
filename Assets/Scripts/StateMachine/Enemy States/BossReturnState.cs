using UnityEngine;
using UnityEngine.AI;
public class BossReturnState : BaseState
{
    private NavMeshAgent Agent;
    private Animator Anim;
    private PlayerDetector PlayerDetector;
    private Vector3 ReturnPoint;
    private AudioSource FootstepSource;
    // private AudioSource HeatBeatingSource;
    private AudioClip walkClip;
    // private AudioClip heatBeatingClip;

    private bool SetDestinationSny;
    private bool SetSoundsSny;

    public override void EnterState(BaseEnemy Enemy)
    {
        // Debug.Log("进入回归状态");
        CurrentEnemy = Enemy;
        Agent = CurrentEnemy.Agent;
        Anim = CurrentEnemy.Anim;
        PlayerDetector = CurrentEnemy.PlayerDetector;
        ReturnPoint = CurrentEnemy.ReturnPointTransform.position;
        FootstepSource = CurrentEnemy.FootstepSource;
        // HeatBeatingSource = CurrentEnemy.HeatBeatingSource;
        walkClip = CurrentEnemy.WalkClip;
        // heatBeatingClip = CurrentEnemy.HeatBeatingClip;
        

        Agent.speed = CurrentEnemy.CurrentSpeed;
        Agent.stoppingDistance = CurrentEnemy.StoppingDistance;

        SetDestinationSny = true; 
        SetSoundsSny = true; // 设置为 true，表示需要播放脚步声
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
                SetSoundsSny = false; // 设置为 false，表示不需要播放脚步声
                CurrentEnemy.TimeCounter(() => ReturnLogicStrategy());
            }
            if(PlayerDetector.CanDetectPlayer())
            {
                // Debug.Log("检测到玩家，切换到追逐状态！");
                // 如果检测到玩家，切换到追逐状态
                CurrentEnemy.IsReturning = false;
                CurrentEnemy.IsChasing = true;
                SetSoundsSny = false; // 设置为 false，表示不需要播放脚步声
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
        SoundsPlay();
    }
    
    public override void ExitState()
    {
        if (FootstepSource.isPlaying)
        {
            FootstepSource.loop = false;
            FootstepSource.Stop();
            // HeatBeatingSource.loop = false;
            // HeatBeatingSource.Stop();
        }

    }

    private void ReturnLogicStrategy()
    {
        CurrentEnemy.IsPatrolling = true;
        CurrentEnemy.IsReturning = false;
        CurrentEnemy.SwitchState(BaseEnemyState.Patrol);
    }

    private void SoundsPlay()
    {
        if (FootstepSource.clip != walkClip)
            FootstepSource.clip = walkClip;
        
        // if (HeatBeatingSource.clip != heatBeatingClip)
        //     HeatBeatingSource.clip = heatBeatingClip;

        if (!FootstepSource.isPlaying && SetSoundsSny)
        {
            float maxTime = walkClip.length - 0.3f; 
            FootstepSource.time = Random.Range(0f, maxTime);
            FootstepSource.pitch = Random.Range(0.95f, 1.05f);
            FootstepSource.loop = true;
            FootstepSource.Play();
        }
        else if (FootstepSource.isPlaying && !SetSoundsSny)
        {
            FootstepSource.loop = false;
            FootstepSource.Stop(); // 停止播放脚步声
        }

        // if (!HeatBeatingSource.isPlaying)
        // {
        //     float maxTime = heatBeatingClip.length - 0.3f; 
        //     HeatBeatingSource.time = Random.Range(0f, maxTime);
        //     HeatBeatingSource.pitch = Random.Range(0.95f, 1.05f);
        //     HeatBeatingSource.loop = true;
        //     HeatBeatingSource.Play();
        // }
    }

}
