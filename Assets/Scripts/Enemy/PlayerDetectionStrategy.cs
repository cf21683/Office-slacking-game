using UnityEngine;

public interface PlayerDetectionStrategy
{
    bool Execute(Transform player, Transform detector, bool isPlayerDetected);
}

public class ConDetectionStrategy : PlayerDetectionStrategy
{
    readonly float detectionAngle;
    readonly float detectionRadius;
    readonly float innerDetectionRadius;
    readonly float inChaseTime; // 仇恨注意时间
    readonly float enemyHight; // 敌人高度
    readonly PlayerController playerWorkDetector; // 玩家工作状态
    readonly LayerMask obstacleLayer; // 障碍物的 Layer
    private float DetectionTimer = 0f; // 玩家引起注意的仇恨注意计时器
    private bool isWork = false; // 玩家工作状态
    
    

    
    
    public ConDetectionStrategy(float detectionAngle, float detectionRadius, float innerDetectionRadius, float inChaseTime, float enemyHight,PlayerController playerWorkDetector, LayerMask obstacleLayer)
    {
        this.detectionAngle = detectionAngle;
        this.detectionRadius = detectionRadius;
        this.innerDetectionRadius = innerDetectionRadius;
        this.inChaseTime = inChaseTime;
        this.enemyHight = enemyHight;
        this.obstacleLayer = obstacleLayer;
        this.playerWorkDetector = playerWorkDetector; 
    }
   

    public bool Execute(Transform player, Transform detector, bool isPlayerDetected)
    { 
        // Debug.Log("Execute探测接入isPlayerDetected:" + isPlayerDetected); // 输出检测到玩家的日志

        // 计算玩家与探测器之间的方向和角度
        var DirectionToPlayer = player.position - detector.position;
        var AngleToPlayer = Vector3.Angle(DirectionToPlayer, detector.forward);

        // 检查玩家是否在检测范围内
        bool isInDetectionRange = (AngleToPlayer < detectionAngle / 2 && DirectionToPlayer.magnitude < detectionRadius) || (DirectionToPlayer.magnitude < innerDetectionRadius);
        bool isObstructed = IsObstructed(detector.position, player.position); // 检查是否被障碍物阻挡

        isWork = playerWorkDetector.IsWork; // 获取玩家的工作状态
        // Debug.Log("isObstructed:" + isObstructed); // 输出是否被障碍物阻挡的日志
        // 障碍物将阻断注意
        if (isObstructed)
        {
            // Debug.Log("玩家被障碍物阻挡，重置仇恨注意计时器"); // 输出玩家被障碍物阻挡的日志
            DetectionTimer = 0f; // 如果被阻挡，重置计时器
            return false;
        }

        if(isWork)
        {
            // Debug.Log("玩家正在工作，重置仇恨注意计时器"); // 输出玩家正在工作的日志
            DetectionTimer = 0f; // 如果玩家正在工作，重置计时器
            return false;
        }

        if (isPlayerDetected && isInDetectionRange)
        {
            // Debug.Log("玩家已经被注意,不进行注意判断");
            return true; // 如果已经检测到玩家，直接返回 true
        }

        DetectionTimer += Time.deltaTime;

        if(!isInDetectionRange) 
        {
            DetectionTimer = 0f;
            // Debug.Log($"玩家不在或脱离范围，重置仇恨注意计时器: {DetectionTimer}"); 
            // 玩家不在范围内，重置仇恨注意计时器
            return false;
            
        }
        else
        {
            // Debug.Log($"玩家在范围内, AngleToPlayer: {AngleToPlayer}, DirectionToPlayer: {DirectionToPlayer.magnitude}"); 
            // 玩家在范围内，增加计时器
            
            //Debug.Log($"DetectionTimer: {DetectionTimer}");
            if (DetectionTimer >= inChaseTime)
            {   
                // Debug.Log($"玩家在范围内超过注意阈值，判定探测到玩家: {DetectionTimer}"); 
                // 玩家在范围内的时间超过阈值，判定为检测到玩家
                return true;
            }
            else
            {
                // Debug.Log($"玩家在范围内但未超过注意阈值，继续仇恨注意计时: {DetectionTimer}"); 
                // 玩家在范围内但未超过注意阈值，继续仇恨注意计时
                return false;
            }
        }
    }

    private bool IsObstructed(Vector3 detectorPosition, Vector3 playerPosition)
    {
        // 调整射线起点，沿 y 轴偏移enemyHight
        Vector3 adjustedDetectorPosition = new Vector3(detectorPosition.x, detectorPosition.y + enemyHight, detectorPosition.z);
        Vector3 adjustedPlayerPosition = new Vector3(playerPosition.x, playerPosition.y + enemyHight, playerPosition.z);

        Vector3 directionToPlayer = adjustedPlayerPosition  - adjustedDetectorPosition;
        float distanceToPlayer = directionToPlayer.magnitude;

        // 使用射线检测障碍物
        if (Physics.Raycast(adjustedDetectorPosition, directionToPlayer.normalized, out RaycastHit hit, distanceToPlayer, obstacleLayer))
        {
            // 如果射线击中了障碍物，返回 true
            return true;
        }

        // 没有障碍物，返回 false
        return false;
    }

        
}