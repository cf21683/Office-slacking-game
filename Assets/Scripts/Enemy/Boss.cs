using UnityEngine;

public class Boss : BaseEnemy
{
    protected override void Awake()
    {
        base.Awake();
        PatrolState = new BossPatrolState();
        ReturnState = new BossReturnState();
        ChaseState = new BossChaseState();
        AdmonishState = new BossAdmonishState();
        
    }
   
}
