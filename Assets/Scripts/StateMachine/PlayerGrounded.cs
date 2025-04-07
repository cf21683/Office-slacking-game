using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : PlayerBaseState
{
    public PlayerGrounded(PlayerState context, PlayerStateFactory factory): base (context, factory){
        InitializeSubState();
    }

    public override void EnterState(){
        
    }
   
    public override void UpdateState(
    ){
        CheckSwitchState();
    }

    public override void ExitState(){}

    public override void CheckSwitchState(){

    }
    
    public override void InitializeSubState(){
        if(!_context.IsWalkPressed && !_context.IsRunPressed){
            SetSubState(_factory.IdleState());
            
        }
        else if(_context.IsWalkPressed && !_context.IsRunPressed){
            SetSubState(_factory.WalkState());
        }
        else{
            SetSubState(_factory.RunState());
        }
    }
}
