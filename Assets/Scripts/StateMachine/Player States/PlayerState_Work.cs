
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Work",fileName = "PlayerState_Work")]
public class PlayerState_Work: PlayerState
{
    public override void Enter(){
        player.computerCam.Priority = 20;
        player.IsSlack = false;
        player.IsWork = true;
        player.IsIdle = false;
        player.UnlockCursor();
        animator.CrossFade("Typing",0.1f);
    }

    public override void LogicUpdate(){
        if(!player.IsBusy){
            stateMachine.SwitchState(typeof(PlayerState_Sit));
        }else if(input.isSlackPressed){
            stateMachine.SwitchState(typeof(PlayerState_Slack));
        }
    }

    public override void PhysicsUpdate(){
        player.SetVelocity(0f);
    }

}
