using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : PlayerBaseState
{   

    public PlayerWalk(PlayerState context,PlayerStateFactory factory):base(context,factory){
    }




    public override void EnterState(){
        _context.Animator.SetBool(_context.IsWalkingHash,true);
        _context.Animator.SetBool(_context.IsRunningHash,false);

    }
   
    public override void UpdateState(){
        CheckSwitchState();
        _context.WalkX = _context.WalkInput.x ;
        _context.WalkZ = _context.WalkInput.y ;
    
    }
    public override void ExitState(){

    }
    public override void CheckSwitchState(){
        if(!_context.IsWalkPressed){
            SwitchState(_factory.IdleState());
        }
        else if(_context.IsRunPressed){
            Debug.Log("Switching to Run State");
            SwitchState(_factory.RunState());

        }
    }
    
    public override void InitializeSubState(){

    }
}
