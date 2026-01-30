namespace AI.StateMachine
{
    public interface IState
    {
        public void Enter(StateContext context);
        public void Execute(StateContext context, float deltaTime);
        public void Exit(StateContext context);
    }
}
