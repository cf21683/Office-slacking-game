
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Work",fileName = "PlayerState_Work")]
public class PlayerState_Work: PlayerState
{
    float speed = 0f;
    public override void Enter(){
        animator.CrossFade("Typing", 0.05f);
    }

    public override void LogicUpdate(){
        
    }

    public override void PhysicsUpdate(){
        player.SetVelocity(speed);
    }

}
