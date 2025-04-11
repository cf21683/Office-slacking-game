using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Run",fileName = "PlayerState_Run")]
public class PlayerState_Run : PlayerState
{
    [SerializeField] float runSpeed = 6f;
     public override void Enter(){
        if (player.footstepSource.clip != player.walkClip)
            player.footstepSource.clip = player.walkClip;

        if (!player.footstepSource.isPlaying)
        {
            float maxTime = player.walkClip.length - 0.3f; 
            player.footstepSource.time = Random.Range(0f, maxTime); 
            player.footstepSource.pitch = Random.Range(1.2f, 1.4f);
            player.footstepSource.loop = true;
            player.footstepSource.Play();
        }
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
    public override void Exit()
    {
        player.footstepSource.Stop();
    }

    
}
