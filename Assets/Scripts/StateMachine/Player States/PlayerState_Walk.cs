using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Walk",fileName = "PlayerState_Walk")]
public class PlayerState_Walk : PlayerState
{
    [SerializeField]float walkSpeed = 3f;
    public override void Enter(){
        animator.CrossFade("Walk", 0.05f);
    }

    public override void LogicUpdate(){
        if(!input.isWalkPressed){
        stateMachine.SwitchState(typeof(PlayerState_Idle));
        }else if(input.isWalkPressed && input.isRunPressed){
            stateMachine.SwitchState(typeof(PlayerState_Run));
        }
    }

    public override void PhysicsUpdate(){
        player.SetVelocity(walkSpeed);
    }
}
