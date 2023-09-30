namespace _Game.Scripts.Game.Events
{
    public class PauseEvent
    {
        public bool Paused;
        public bool PlayerDied;
        public float PlayerTime;

        public PauseEvent(bool paused, bool playerDied, float playerTime = 0f)
        {
            Paused = paused;
            PlayerDied = playerDied;
            PlayerTime = playerTime;
        }
    }
}