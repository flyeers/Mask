namespace AI
{
    public interface IStateBehavior
    {
        public void StartBehavior();
        public void StopBehavior();
        public bool Completed();
    }
}
