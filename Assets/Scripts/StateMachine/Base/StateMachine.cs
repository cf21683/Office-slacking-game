using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
   protected IState currentState;

   protected Dictionary<System.Type,IState> stateTable;

   void Update(){
    currentState.LogicUpdate();
   }

   void FixedUpdate(){
    currentState.PhysicsUpdate();
   }

   protected void SwitchOn(IState newState){
    currentState = newState;
    currentState.Enter();
   }


   public void SwitchState(IState newState){
    currentState.Exit();
    SwitchOn(newState);
   }

   public void SwitchState(System.Type newStateType){
      SwitchState(stateTable[newStateType]);
   }
}
