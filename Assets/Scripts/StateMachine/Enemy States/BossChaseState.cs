using UnityEngine;
using UnityEngine.AI;
public class BossChaseState : BaseState
{
    private NavMeshAgent Agent;
    private Animator Anim;
    private PlayerDetector PlayerDetector;
    private PlayerController PlayerController;
    private Transform PlayerTransform;
    private AudioSource FootstepSource;
    private AudioSource HeatBeatingSource;
    private AudioSource ChaseSource; 
    private AudioClip walkClip;
    private AudioClip heatBeatingClip;
    private AudioClip chaseClip;
    private Vector3 Destination;
    private bool SetDestinationSny;
    private bool SetSoundsSny; // 是否播放脚步声
    public override void EnterState(BaseEnemy Enemy)
    {
        CurrentEnemy = Enemy;
        Agent = CurrentEnemy.Agent; 
        Anim = CurrentEnemy.Anim;
        PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); // 获取玩家控制器
        PlayerDetector = CurrentEnemy.PlayerDetector; // 获取 PlayerDetector 组件
        PlayerTransform = PlayerDetector.PlayerTransform; // 获取玩家的 Transform
        Agent.speed = CurrentEnemy.CurrentSpeed;
        Agent.stoppingDistance = CurrentEnemy.StoppingDistance;
        FootstepSource = CurrentEnemy.FootstepSource;
        HeatBeatingSource = CurrentEnemy.HeatBeatingSource;
        ChaseSource = PlayerController.chaseSource; 
        walkClip = CurrentEnemy.WalkClip;
        heatBeatingClip = CurrentEnemy.HeatBeatingClip;
        chaseClip = PlayerController.chaseClip;
        Destination = PlayerTransform.position; // 获取玩家的位置
        SetDestinationSny = true; 
        SetSoundsSny = true; // 设置为 true，表示需要播放脚步声
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
                SetSoundsSny = false; // 设置为 false，表示不需要播放脚步声
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
                SetSoundsSny = false; // 设置为 false，表示不需要播放脚步声
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
        SoundsPlay(); // 播放音效
        
    }
    
    public override void ExitState()
    {
        if (FootstepSource.isPlaying)
        {
            FootstepSource.loop = false;
            FootstepSource.Stop();
            HeatBeatingSource.loop = false;
            HeatBeatingSource.Stop();
            ChaseSource.loop = false;
            ChaseSource.Stop(); // 停止播放追逐声
        }
    }

    private void SoundsPlay()
    {
        if (FootstepSource.clip != walkClip)
            FootstepSource.clip = walkClip;
        
        if (HeatBeatingSource.clip != heatBeatingClip)
            HeatBeatingSource.clip = heatBeatingClip;
        
        if (ChaseSource.clip != chaseClip)
            ChaseSource.clip = chaseClip;

        if (!FootstepSource.isPlaying && SetSoundsSny)
        {
            float maxTime = walkClip.length - 0.3f; 
            FootstepSource.time = Random.Range(0f, maxTime);
            FootstepSource.pitch = Random.Range(1.3f, 1.5f);
            FootstepSource.loop = true;
            FootstepSource.Play();
        }
        else if (FootstepSource.isPlaying && !SetSoundsSny)
        {
            FootstepSource.loop = false;
            FootstepSource.Stop(); // 停止播放脚步声
        }

        if (!HeatBeatingSource.isPlaying && SetSoundsSny)
        {
            float maxTime = heatBeatingClip.length - 0.3f; 
            HeatBeatingSource.time = Random.Range(0f, maxTime);
            HeatBeatingSource.pitch = Random.Range(1.5f, 1.7f);
            HeatBeatingSource.loop = true;
            HeatBeatingSource.Play();
        }
        else if (HeatBeatingSource.isPlaying && !SetSoundsSny)
        {
            HeatBeatingSource.loop = false;
            HeatBeatingSource.Stop(); // 停止播放心跳声
        }

        if (!ChaseSource.isPlaying && SetSoundsSny)
        {
            ChaseSource.time = 3f;
            ChaseSource.loop = true;
            ChaseSource.Play();
        }
        else if (ChaseSource.isPlaying && !SetSoundsSny)
        {
            ChaseSource.loop = false;
            ChaseSource.Stop(); // 停止播放追逐声
        }

    }


}
