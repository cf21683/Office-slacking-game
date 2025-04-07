public abstract class PlayerBaseState
{
    protected PlayerState _context;
    protected PlayerStateFactory _factory;
    protected PlayerBaseState _superState;
    protected PlayerBaseState _subState;

    protected PlayerBaseState _currentState;

    public PlayerBaseState(PlayerState context,PlayerStateFactory factory){
        _context = context;
        _factory = factory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchState();

    public abstract void InitializeSubState();

    public void UpdateStates(){
        UpdateState();
        if(_subState != null){
            _subState.UpdateStates();
        }
    }

    protected void SwitchState(PlayerBaseState newState){
        ExitState();

        newState.EnterState();

        _context.CurrentState = newState;

    }

    protected void SetSuperState(PlayerBaseState newState){
        _superState = newState;
    }

    protected void SetSubState(PlayerBaseState newState){
        _subState = newState;
        newState.SetSuperState(this);
    }
}
