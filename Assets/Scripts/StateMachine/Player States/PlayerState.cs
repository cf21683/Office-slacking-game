using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : ScriptableObject,IState
{
    protected Animator animator;
    protected PlayerInputAction input;
    protected PlayerStateMachine stateMachine;

    protected PlayerController player;


    public void Initialize(Animator animator,PlayerInputAction input,PlayerController player,PlayerStateMachine stateMachine){
        this.animator = animator;
        this.input = input;
        this.player = player;
        this.stateMachine = stateMachine;
    }
    public virtual void Enter(){

    }

    public virtual void Exit(){

    }

    public virtual void LogicUpdate(){

    }

    public virtual void PhysicsUpdate(){
    }

}
