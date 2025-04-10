using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Run",fileName = "PlayerState_Run")]
public class PlayerState_Run : PlayerState
{
    [SerializeField] float runSpeed = 6f;
     public override void Enter(){
        player.computerCam.Priority = 1;
        player.IsIdle = true;
        player.IsWork = false;
        player.IsSlack = false;
        animator.CrossFade("Run", 0.2f);
    }

    public override void LogicUpdate(){
        if(!input.isWalkPressed){
        stateMachine.SwitchState(typeof(PlayerState_Idle));
        }else if(!input.isRunPressed && input.isWalkPressed){
            stateMachine.SwitchState(typeof(PlayerState_Walk));
        }
    }

    public override void PhysicsUpdate(){
        player.SetVelocity(runSpeed);
    }
}
