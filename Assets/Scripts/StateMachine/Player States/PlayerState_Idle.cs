using UnityEngine;


[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Idle",fileName = "PlayerState_Idle")]
public class PlayerState_Idle : PlayerState
{
    public override void Enter(){
        player.IsIdle = true;
        animator.CrossFade("Idle", 0.2f);
    }

    public override void LogicUpdate(){
    if (input.isWalkPressed && input.isRunPressed)
    {
        stateMachine.SwitchState(typeof(PlayerState_Run));
    }else if(input.isWalkPressed){
        stateMachine.SwitchState(typeof(PlayerState_Walk));
    }else if(player.SitRequested && !player.IsSitting && player.IsNearChair){
        player.SitRequested = false;
        stateMachine.SwitchState(typeof(PlayerState_Sit));
    }
        
    }

    public override void PhysicsUpdate(){
        player.SetVelocity(0f);
    }
}
