namespace _Game.Scripts.Enemies
{
    public class EnemyStateManager
    {
        public enum State
        {
            Idle,
            Chasing,
            Attacking,
            Dead
        }
        
        public State CurrentState { get; private set; }

        public void SetState(State state)
        {
            CurrentState = state;
        }
        
        public void Tick()
        {
            switch (CurrentState)
            {
                case State.Idle:
                    break;
                case State.Chasing:
                    break;
                case State.Attacking:
                    break;
                case State.Dead:
                    break;
            }
        }
        
        
    }
}