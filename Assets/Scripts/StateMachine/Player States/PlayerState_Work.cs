
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Work",fileName = "PlayerState_Work")]
public class PlayerState_Work: PlayerState
{
    public override void Enter(){
        player.IsSlack = false;
        player.IsWork = true;
        player.IsIdle = false;
        animator.CrossFade("Typing",0.1f);
    }

    public override void LogicUpdate(){
        if(input.isInteractPressed){
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }else if(input.isSlackPressed){
            stateMachine.SwitchState(typeof(PlayerState_Slack));
        }
    }

    public override void PhysicsUpdate(){
        player.SetVelocity(0f);
    }

}
