using UnityEngine;
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Sit",fileName = "PlayerState_Sit")]
public class PlayerState_Sit : PlayerState
{
    
    public override void Enter(){
        player.computerCam.Priority = 20;
        player.IsSitting = true;
        Vector3 position = player.currentChair.sitPoint.position;
        position.y = 0.192f;
        player.transform.position = position;

        player.transform.rotation = Quaternion.Euler(0, 90, 0);

        
        animator.Play("Sit");
        
    }

    public override void LogicUpdate(){
        if(input.isInteractPressed){
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
