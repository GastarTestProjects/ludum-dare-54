namespace _Game.Scripts.Enemy
{
    public class EnemyInitParams
    {
        public float Health;
        public float Speed;
        public int damage;
        
        public EnemyInitParams(float health, float speed)
        {
            Health = health;
            Speed = speed;
        }
    }
}