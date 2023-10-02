namespace _Game.Scripts.Enemies
{
    public class EnemyDiedEvent
    {
        private bool _killed;
        public bool Killed => _killed;
        
        public EnemyDiedEvent(bool killed)
        {
            _killed = killed;
        }
    }
}