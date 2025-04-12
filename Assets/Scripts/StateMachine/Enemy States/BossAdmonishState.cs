using UnityEngine;
using UnityEngine.AI;

public class BossAdmonishState : BaseState
{
    private Camera AdmonishCamera; // 跳杀摄像机的引用
    private AudioListener AdmonishAudioListener; // 跳杀摄像机的音频监听器引用
    private Animator Anim;
    private NavMeshAgent Agent;
    private PlayerDetector PlayerDetector;
    private float AdmonishTime; // 跳杀时间
    private float AdmonishTimer = 0f; // 跳杀计时器
    private bool IsAdmonishCamera = false; // 是否使用跳杀摄像机
    

    public override void EnterState(BaseEnemy Enemy)
    {
        AdmonishCamera = GameObject.FindGameObjectWithTag("AdmonishCamera").GetComponent<Camera>(); // 获取跳杀摄像机
        AdmonishAudioListener = GameObject.FindGameObjectWithTag("AdmonishCamera").GetComponent<AudioListener>(); // 获取跳杀摄像机的音频监听器
        CurrentEnemy = Enemy; // 获取当前敌人
        Anim = CurrentEnemy.Anim; // 获取敌人的动画组件
        Agent = CurrentEnemy.Agent; // 获取敌人的导航代理组件
        AdmonishTime = CurrentEnemy.AdmonishTime; // 获取跳杀时间
        PlayerDetector = CurrentEnemy.PlayerDetector; // 获取玩家检测器

        Anim.CrossFade("Idle", 0.1f); // 播放Idle动画
    }

    public override void LogicUpdateState()
    {
        IsAdmonishCamera = true; // 设置为使用跳杀摄像机
        PlayerDetector.IsPlayerDetected = false; // 重置玩家检测器的状态
        PlayerDetector.enabled = false; // 禁用玩家检测器
        
    }

    public override void PhysicsUpdateState()
    {
        if(IsAdmonishCamera)
        {
            AdmonishCamera.enabled = true; // 启用跳杀摄像机
            AdmonishAudioListener.enabled = true; // 启用跳杀摄像机的音频监听器
            AdmonishTimer += Time.deltaTime;
            // Debug.Log($"跳杀计时器:{AdmonishTimer}"); // 输出跳杀计时器的值
            if (AdmonishTimer >= AdmonishTime) // 如果跳杀时间到达
            {   
                // Debug.Log("开始跳杀"); // 输出跳杀开始的调试信息
                AdmonishTimer = 0f; 
                IsAdmonishCamera = false; // 设置为不使用跳杀摄像机
                AdmonishCamera.enabled = false; // 禁用跳杀摄像机
                AdmonishAudioListener.enabled = false; // 禁用跳杀摄像机的音频监听器
                CurrentEnemy.IsReturning = true; // 标记为正在回归状态
                CurrentEnemy.IsAdmonishing = false; // 标记为不在警告状态
                PlayerDetector.enabled = true; // 启用玩家检测器
                PlayerDetector.IsPlayerAdmonished = true; // 设置玩家检测器为警告后状态
                // Debug.Log($"跳杀结束，恢复主摄像机，探测隔绝状态:{PlayerDetector.IsPlayerAdmonished}"); // 输出跳杀结束的调试信息
                Agent.isStopped = false; // 允许导航代理移动
                // Debug.Log("跳杀结束"); // 输出跳杀结束的调试信息
                CurrentEnemy.SwitchState(BaseEnemyState.Return); // 切换回巡逻状态
            }
        }
    }
    
    public override void ExitState()
    {
        
    }
}
