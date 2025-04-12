
using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Slack",fileName = "PlayerState_Slack")]
public class PlayerState_Slack : PlayerState
{
    public override void Enter(){
        player.computerCam.Priority = 20;
        player.IsSlack = true;
        player.IsWork = false;
        player.IsIdle = false;
        player.UnlockCursor();
        animator.CrossFade("Gaming",0.1f);
    }

    
    public override void LogicUpdate(){
        if(!player.IsBusy){
            stateMachine.SwitchState(typeof(PlayerState_Sit));
        }else if(input.isWorkPressed){
            stateMachine.SwitchState(typeof(PlayerState_Work));
        }
    }

    public override void PhysicsUpdate(){
        player.SetVelocity(0f);
    }
}
