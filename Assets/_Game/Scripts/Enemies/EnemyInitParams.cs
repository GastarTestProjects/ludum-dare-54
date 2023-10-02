namespace _Game.Scripts.Enemy
{
    public class EnemyInitParams
    {
        public float Health;
        public float Speed;
        public int Damage = 5;
        // public float ExplosionDistance = 2f;
        // public float AttackExplosionDelay = 0.2f;
        
        public EnemyInitParams(float health, float speed)
        {
            Health = health;
            Speed = speed;
        }
    }
}