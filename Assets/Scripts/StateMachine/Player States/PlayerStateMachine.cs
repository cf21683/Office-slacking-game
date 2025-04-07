using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    Animator _animator;

    [SerializeField] PlayerState[] states;

    PlayerInputAction input;

    PlayerController player;

    void Awake(){
        _animator = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
        
        stateTable = new Dictionary<System.Type, IState>(states.Length);

        input = GetComponent<PlayerInputAction>();

        foreach (PlayerState state in states){
            state.Initialize(_animator, input, player,this);
            stateTable.Add(state.GetType(),state);
        }

    }

    void Start(){
        SwitchOn(stateTable[typeof(PlayerState_Idle)]);
    }
}
