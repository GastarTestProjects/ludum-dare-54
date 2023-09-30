namespace _Game.Scripts.Game.Events
{
    public class PauseEvent
    {
        public bool Paused;
        public bool PlayerDied;
        
        public PauseEvent(bool paused, bool playerDied)
        {
            Paused = paused;
            PlayerDied = playerDied;
        }
    }
}