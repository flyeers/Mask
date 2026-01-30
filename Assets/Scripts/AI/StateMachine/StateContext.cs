namespace AI.StateMachine
{
    public class StateContext
    {
        public IStateMachine StateMachine { get; set; }
        public float StateTime { get; set; }

        public void Reset()
        {
            StateTime = 0;
        }
    }
}
