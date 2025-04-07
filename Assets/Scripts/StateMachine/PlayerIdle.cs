using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerBaseState
{

    public PlayerIdle(PlayerState context, PlayerStateFactory factory): base (context, factory){
    }

    public override void EnterState(){
        _context.Animator.SetBool(_context.IsWalkingHash,false);
        _context.Animator.SetBool(_context.IsRunningHash,false);
        _context.WalkX = 0;
        _context.WalkZ = 0;
        
    }
   
    public override void UpdateState(
    ){
        CheckSwitchState();
    }

    public override void ExitState(){}

    public override void CheckSwitchState(){
       if(_context.IsWalkPressed ){
            SwitchState(_factory.WalkState());
        }
        else if(_context.IsRunPressed && _context.IsWalkPressed){
            SwitchState(_factory.RunState());
        }

    }
    
    public override void InitializeSubState(){
        
    
    }

}


