
using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Slack",fileName = "PlayerState_Slack")]
public class PlayerState_Slack : PlayerState
{
    public override void Enter(){
        player.IsSlack = true;
        player.IsWork = false;
        player.IsIdle = false;
        animator.Play("Gaming");
    }

    
    public override void LogicUpdate(){
        if(input.isInteractPressed){
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }else if(input.isWalkPressed){
            stateMachine.SwitchState(typeof(PlayerState_Work));
        }
    }

    public override void PhysicsUpdate(){
        player.SetVelocity(0f);
    }
}
