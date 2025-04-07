using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : PlayerBaseState
{
    public PlayerRun(PlayerState context, PlayerStateFactory factory): base (context, factory){

    }

    public override void EnterState(){
        _context.Animator.SetBool(_context.IsRunningHash,true);
        _context.Animator.SetBool(_context.IsWalkingHash,true);
    }

   
    public override void UpdateState(){
        CheckSwitchState();
        _context.WalkX = _context.WalkInput.x * _context.RunSpeed;
        _context.WalkZ = _context.WalkInput.y * _context.RunSpeed;
    }

    public override void ExitState(){}

    public override void CheckSwitchState(){
        if(!_context.IsWalkPressed){
            SwitchState(_factory.IdleState());
        }
        else if(!_context.IsRunPressed && _context.IsWalkPressed){
            SwitchState(_factory.WalkState());

        }

    }
    
    public override void InitializeSubState(){}

}