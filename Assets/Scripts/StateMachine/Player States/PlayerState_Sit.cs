using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Sit",fileName = "PlayerState_Sit")]
public class PlayerState_Sit : PlayerState
{
    
    public override void Enter(){
        player.computerCam.Priority = 20;
        player.IsSitting = true;

        player._characterController.enabled = false;

   
        Transform sitPoint = player.currentChair.sitPoint;
        Vector3 pos = sitPoint.position;
        pos.y = -0.68f; 

        player.transform.position = pos;
        player.transform.rotation = Quaternion.Euler(0, 90, 0);

    player._characterController.enabled = true;
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
