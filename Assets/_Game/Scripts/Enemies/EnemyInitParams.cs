namespace _Game.Scripts.Enemy
{
    public class EnemyInitParams
    {
        public float Health;
        public float Speed;
        public int Damage = 5;
        public float ExplosionDistance = 1.5f;
        public float AttackExplosionDelay = 0.1f;
        
        public EnemyInitParams(float health, float speed)
        {
            Health = health;
            Speed = speed;
        }
    }
}