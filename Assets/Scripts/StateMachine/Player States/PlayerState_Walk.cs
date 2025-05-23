using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Walk",fileName = "PlayerState_Walk")]
public class PlayerState_Walk : PlayerState
{
    [SerializeField]float walkSpeed = 3f;
    public override void Enter(){

        if (player.footstepSource.clip != player.walkClip)
            player.footstepSource.clip = player.walkClip;

        if (!player.footstepSource.isPlaying)
        {
            float maxTime = player.walkClip.length - 0.3f; 
            player.footstepSource.time = Random.Range(0f, maxTime);

            player.footstepSource.pitch = Random.Range(0.95f, 1.05f);
            player.footstepSource.loop = true;
            player.footstepSource.Play();
        }

        player.computerCam.Priority = 1;
        player.IsIdle = true;
        player.IsWork = false;
        player.IsSlack = false;
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

    public override void Exit()
    {
        player.footstepSource.Stop();
    }

}
