using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Idle",fileName = "PlayerState_Idle")]
public class PlayerState_Idle : PlayerState
{
    public override void Enter(){
        animator.CrossFade("Idle", 0.2f);
    }

    public override void LogicUpdate(){
    if (input.isWalkPressed)
    {
        stateMachine.SwitchState(typeof(PlayerState_Walk));
    }else if(input.isWalkPressed && input.isRunPressed){
        stateMachine.SwitchState(typeof(PlayerState_Run));
    }
        
    }

    public override void PhysicsUpdate(){
        player.SetVelocity(0f);
    }
}
