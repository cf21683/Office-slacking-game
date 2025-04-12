using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    Rigidbody Rb;
    private AudioSource[] AudioSources; 
    internal AudioSource FootstepSource;
    internal AudioSource HeatBeatingSource;
    internal Animator Anim;
    internal NavMeshAgent Agent;
    internal PlayerDetector PlayerDetector;
    

    [Header("Enemy Settings")]
    [SerializeField] private float PatrolSpeed = 2.5f;
    [SerializeField] private float ReturnSpeed = 2.5f;
    [SerializeField] private float ChaseSpeed = 6.0f;
    [SerializeField] internal float CurrentSpeed;
    [SerializeField] internal float PatrolRadius = 10f;
    [SerializeField] internal float MinPatrolDistance = 5f;
    [SerializeField] internal float StoppingDistance = 1.5f;
    [SerializeField] internal int PatrolCount = 4; // 用于记录巡逻次数
    [SerializeField] internal float detectionAngle = 60f; // 检测角度
    [SerializeField] internal float detectionRadius = 5f; // 检测半径
    [SerializeField] internal float innerDetectionRadius = 3f; // 内部检测半径
    [SerializeField] internal float EnemyHeight = 1.5f; // 敌人高度
    [SerializeField] internal float audioMaxDistance;  // 音频最大距离
    [SerializeField] internal float audioMinDistance;  // 音频最小距离
    [SerializeField] internal float SpatialBlend = 1.0f;  // 空间混合度(1.0f表示3D音频，0.0f表示2D音频)
    [SerializeField] internal float WalkVolume; // 脚步音效音量
    [SerializeField] internal float HeatBeatingVolume; // 心跳音效音量
    [SerializeField] internal AudioClip WalkClip; // 脚步音效
    [SerializeField] internal AudioClip HeatBeatingClip; // 心跳音效



    [Header("Enemy NavigationPoints")]
    [SerializeField] internal Transform ReturnPointTransform; // 返回点的 Transform
    [SerializeField] internal Transform PlayerTransform;
    [SerializeField] internal LayerMask ObstacleLayer; // 障碍物的 Layer
    [SerializeField] internal string AttackTag ; // 攻击目标的 Tag
   

    [Header("Enemy TimeCounters")]
    [SerializeField] internal float PatrolIdleTime = 2f; // 巡逻时的等待时间
    [SerializeField] internal float ReturnIdleTime = 5f; // 返回时的等待时间
    [SerializeField] private float IdleTimeCounter = 0f; // 巡逻时的等待时间计数器
    [SerializeField] internal float HateDuration = 1f; // 仇恨延续时间
    [SerializeField] internal float InChaseTime = 0.5f; // 仇恨注意时间
    [SerializeField] internal float AdmonishTime = 1.5f; // 警告时间
    [SerializeField] internal float CoolDownTime = 10.0f; // 警告后探测冷却时间


    [Header("Enemy States")]
    
    public bool IsIdle = false;
    public bool IsPatrolling = false;
    public bool IsReturning = false;
    public bool IsChasing = false;
    public bool IsAdmonishing = false;
    public bool IsAfterAdmonish = false;



    private BaseState CurrentState;

    protected BaseState PatrolState;

    protected BaseState ReturnState;

    protected BaseState ChaseState;

    protected BaseState AdmonishState;


    protected virtual void Awake()
    {
        
        Rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        PlayerDetector = GetComponent<PlayerDetector>();
        AudioSources = GetComponents<AudioSource>();
        FootstepSource = AudioSources[0]; // 脚步音效
        HeatBeatingSource = AudioSources[1]; // 心跳音效
        CurrentSpeed = PatrolSpeed;
        FootstepSource.spatialBlend = SpatialBlend; 
        FootstepSource.rolloffMode = AudioRolloffMode.Linear; // 设置音频衰减模式为线性
        FootstepSource.maxDistance = audioMaxDistance;
        FootstepSource.minDistance = audioMinDistance;
        FootstepSource.volume = WalkVolume; // 设置音量
        HeatBeatingSource.spatialBlend = SpatialBlend; 
        HeatBeatingSource.rolloffMode = AudioRolloffMode.Linear; // 设置音频衰减模式为线性
        HeatBeatingSource.maxDistance = audioMaxDistance;
        HeatBeatingSource.minDistance = audioMinDistance;
        HeatBeatingSource.volume = HeatBeatingVolume; // 设置音量


    }

    private void OnEnable()
    {
        IsPatrolling = true;
        if (IsPatrolling)
        {
            CurrentState = PatrolState;
            CurrentState.EnterState(this);
        }    
        
    }

    private void Update()
    {
        CurrentState.LogicUpdateState();
    }

    private void FixedUpdate()
    {
        CurrentState.PhysicsUpdateState();
    }

    private void OnDisable()
    {
        CurrentState.ExitState();
    }

    public void SwitchState(BaseEnemyState State)
    {
        var NewState = State switch
        {
            BaseEnemyState.Patrol => PatrolState,
            BaseEnemyState.Return => ReturnState,
            BaseEnemyState.Chase => ChaseState,
            BaseEnemyState.Admonish => AdmonishState,
            _ => null            
        };

        CurrentSpeed = State switch
        {
            BaseEnemyState.Patrol => PatrolSpeed,
            BaseEnemyState.Return => ReturnSpeed,
            BaseEnemyState.Chase => ChaseSpeed,
            BaseEnemyState.Admonish => 0f, 
            _ => CurrentSpeed            
        };  

        CurrentState.ExitState();
        // Debug.Log($"Switching to {State} state.");      
        CurrentState = NewState;
        CurrentState.EnterState(this);

    }

    public void TimeCounter(Action onIdleComplete)
    {
        if(IsIdle)
        {   
            IdleTimeCounter += Time.deltaTime;
            if (IdleTimeCounter >= PatrolIdleTime && IsPatrolling)
            {   
                IsIdle = false;
                IdleTimeCounter = 0f; 
                onIdleComplete?.Invoke();
            }
            if (IdleTimeCounter >= ReturnIdleTime && IsReturning)
            {   
                IsIdle = false;
                IdleTimeCounter = 0f; 
                onIdleComplete?.Invoke();
            } 
        } 
        
    }

}
