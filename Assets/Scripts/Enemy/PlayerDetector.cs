using Unity.VisualScripting;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private float detectionAngle; // 检测角度
    private float detectionRadius; // 检测半径
    private float innerDetectionRadius; // 内部检测半径
    private float hateDuration; // 检测仇恨持续时间
    private float inChaseTime; // 进入追逐状态的时间
    private float enemyHight; // 敌人高度
    private float coolDownTime; // 警告后探测冷却时间
    private bool IsPlayerWork = false;
    private LayerMask obstacleLayer; // 障碍物的 Layer
    private BaseEnemy enemy; // 敌人对象 
    private float HateTimer = 0f;
    private float CoolDownTimer = 0f; // 冷却计时器
    
    internal Transform PlayerTransform; // 玩家对象的 Transform

    PlayerDetectionStrategy DetectionStrategy; // 检测策略接口
    public PlayerController PlayerWorkDetector; // 玩家对象的引用

    internal bool IsPlayerDetected = false; // 是否检测到玩家
    internal bool IsPlayerAdmonished = false;  // 是否处于警告后状态
    

    void Awake()
    {
        enemy = GetComponent<BaseEnemy>(); // 获取敌人对象
        detectionAngle = enemy.detectionAngle; // 获取敌人检测角度
        detectionRadius = enemy.detectionRadius; // 获取敌人检测半径
        innerDetectionRadius = enemy.innerDetectionRadius; // 获取敌人内部检测半径
        hateDuration = enemy.HateDuration; // 获取敌人仇恨持续时间
        inChaseTime = enemy.InChaseTime; // 获取敌人进入追逐状态的时间
        obstacleLayer = enemy.ObstacleLayer; // 获取敌人障碍物的 Layer
        enemyHight = enemy.EnemyHeight; // 获取敌人高度
        coolDownTime = enemy.CoolDownTime; // 获取敌人警告后探测冷却时间
    }

    void Start()
    {

        PlayerTransform = GameObject.FindGameObjectWithTag(enemy.AttackTag).transform; // 获取玩家对象的 Transform
        DetectionStrategy = new ConDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius, inChaseTime, enemyHight, PlayerWorkDetector, obstacleLayer); // 创建检测策略实例
    }

    void Update()
    {
        IsPlayerWork = PlayerWorkDetector.IsWork; // 获取玩家的工作状态
        if(IsPlayerAdmonished)
        {
            // Debug.Log($"玩家处于警告后状态，屏蔽探测{IsPlayerAdmonished}"); // 输出玩家处于警告后状态的日志
            enemy.IsAfterAdmonish = true; // 设置敌人处于警告后状态
            HateTimer = 0f; // 重置仇恨计时器
            IsPlayerDetected = false; // 如果玩家处于警告后状态，屏蔽探测
            // Debug.Log($"不能探测玩家:{IsPlayerDetected}"); // 输出不能探测玩家的日志
            CoolDownTimer += Time.deltaTime; // 如果处于警告后状态，增加冷却计时器
            // Debug.Log($"冷却计时器:{CoolDownTimer}"); // 输出冷却计时器的日志
            if (CoolDownTimer >= coolDownTime) // 如果冷却时间到达
            {
                // Debug.Log($"冷却时间到达，重新探测玩家:{IsPlayerAdmonished}"); // 输出冷却时间到达的日志
                IsPlayerAdmonished = false; // 重置警告后状态
                CoolDownTimer = 0f; // 重置冷却计时器
                enemy.IsAfterAdmonish = false; // 设置敌人不处于警告后状态
                // Debug.Log($"警告后状态结束，重新探测玩家:{IsPlayerDetected}"); // 输出警告后状态结束的日志
            }
        }

        if (IsPlayerDetected)
        {
            if(IsPlayerWork)
            {
                IsPlayerDetected = false; // 如果玩家在工作状态，重置检测状态
                // Debug.Log($"玩家正在工作，重置仇恨计时器:{IsPlayerDetected}"); // 输出玩家正在工作的日志
                HateTimer = 0f; // 如果玩家在工作状态，重置仇恨计时器
            }
            // Debug.Log($"检测到玩家:{IsPlayerDetected}"); // 输出检测到玩家的日志
            // 如果玩家在检测范围内，重置仇恨计时器
            if (DetectionStrategy.Execute(PlayerTransform, transform, IsPlayerDetected))
            {
                // Debug.Log($"玩家在范围内，重置仇恨计时器:{IsPlayerDetected}"); // 输出玩家在范围内的日志
                HateTimer = 0f;
            }
            else
            {
                // 玩家脱离检测范围，开始仇恨计时
                HateTimer += Time.deltaTime;
                // Debug.Log($"玩家脱离检测范围，开始仇恨计时:{CoolDownTimer}"); // 输出玩家脱离范围的日志
                if (HateTimer >= hateDuration)
                {   
                    IsPlayerDetected = false; // 仇恨时间结束，判定玩家脱离
                    // Debug.Log($"仇恨时间结束，玩家脱离:{IsPlayerDetected}"); // 输出仇恨时间结束的日志
                }
            }
        }
        else
        {
            // Debug.Log($"尝试重新探测玩家：{IsPlayerDetected}"); // 输出尝试重新探测玩家的日志
            // 如果未检测到玩家，尝试检测
            if (DetectionStrategy.Execute(PlayerTransform, transform, IsPlayerDetected) && !IsPlayerAdmonished)
            {
                IsPlayerDetected = true; // 玩家进入检测范围
                // Debug.Log($"重新探测到玩家:{IsPlayerDetected}"); // 输出检测到玩家的日志
                HateTimer = 0f; // 重置仇恨计时器
            }
        }
    }

    public bool CanDetectPlayer()
    {
        return IsPlayerDetected; //|| DetectionStrategy.Execute(PlayerTransform, transform, IsPlayerDetected);
    }

    private void OnDrawGizmos()
    {
        if (enemy == null) return;

        // 设置 Gizmos 的颜色
        Gizmos.color = new Color(1, 0, 0, 0.3f); // 红色，半透明
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // 绘制外部检测半径

        Gizmos.color = new Color(0, 1, 0, 0.3f); // 绿色，半透明
        Gizmos.DrawWireSphere(transform.position, innerDetectionRadius); // 绘制内部检测半径

        Gizmos.color = Color.yellow; // 黄色，用于绘制检测角度
        Vector3 forward = transform.forward * detectionRadius;
        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle / 2, 0) * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary); // 左边界
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary); // 右边界

        // 绘制射线
        if (PlayerTransform != null)
        {
            Vector3 adjustedDetectorPosition = new Vector3(transform.position.x, transform.position.y + enemyHight, transform.position.z);
            Vector3 adjustedPlayerPosition = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y + enemyHight, PlayerTransform.position.z);
            // 绘制射线到玩家
            Gizmos.color = Color.blue; // 蓝色表示射线
            Gizmos.DrawLine(adjustedDetectorPosition, adjustedPlayerPosition); // 绘制射线到玩家
        }
    }
}
