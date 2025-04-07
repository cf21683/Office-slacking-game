public class PlayerStateFactory
{
    PlayerState _context;
    public PlayerStateFactory(PlayerState context){
        _context = context;
    }
  
    public PlayerBaseState IdleState(){
        return new PlayerIdle(_context,this);
    }

    public PlayerBaseState WalkState(){
        return new PlayerWalk(_context,this);
    }

    public PlayerBaseState RunState(){
        return new PlayerRun(_context,this);
    }

    public PlayerBaseState GroundedState(){
        return new PlayerGrounded(_context,this);
    }
    
}
