using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Sit",fileName = "PlayerState_Sit")]
public class PlayerState_Sit : PlayerState
{
    
    public override void Enter(){
        player.computerCam.Priority = 20;
        player.IsSitting = true;
        player.UnlockCursor();
        
        animator.Play("Sit");
        
    }

    public override void LogicUpdate(){
        if(input.isInteractPressed && !player.IsBusy){
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }else if(input.isWorkPressed){
            stateMachine.SwitchState(typeof(PlayerState_Work));
        }else if(input.isSlackPressed){
            stateMachine.SwitchState(typeof(PlayerState_Slack));
        }
    }

    public override void PhysicsUpdate(){
        player.SetVelocity(0f);
    }
    
}
