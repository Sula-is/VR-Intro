public abstract class AIState {
    public enum State {
        Idle,
        Move,
        Interact
    }
    public enum EventState {
        Enter, Update, Exit
    }

    public State _state;
    protected EventState _event;

    public AIState() {
        _event = EventState.Enter;
    }

    public virtual void Enter() {
        _event = EventState.Enter;
    }
    public virtual void Update() {
        _event = EventState.Update;
    }
    public virtual void Exit() {
        _event = EventState.Exit;
    }

    //public AIState Process() {
    //    if (_event == EventState.Enter) {
    //        Enter();
    //    }
    //    if (_event == EventState.Update) {
    //        Update();
    //    }
    //    if (_event == EventState.Enter) {
    //        Exit();
    //        return nextState;
    //    }
    //    return this;
    //}
}

