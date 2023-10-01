using System;
using _Game.Scripts.Enemy;
using _Game.Scripts.Game.Player;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace _Game.Scripts.Enemies
{
    public class EnemyController : MonoBehaviour, IPoolable<EnemyInitParams, IMemoryPool>
    {
        [SerializeField] private NavMeshAgent enemyAgent;
        
        private EnemyRegistry _registry;
        private IMemoryPool _pool;
        private PlayerController _player;
        private Transform _playerTransform;

        private float _health;
        private float _speed;
        private int _damage;
        
        private EnemyBehaviour _behaviour;
        private bool attacking;
        private float attackEndTime;

        [Inject]
        private void Construct(EnemyRegistry registry, PlayerController player)
        {
            _player = player;
            _playerTransform = _player.transform;
            _registry = registry;
            _behaviour = new EnemyBehaviour(enemyAgent, Attack);
        }
        
        public void OnDespawned()
        {
            _registry.RemoveEnemy(this);
            _pool = null;
            enemyAgent.enabled = false;
        }

        public void OnSpawned(EnemyInitParams initParams, IMemoryPool pool)
        {
            _pool = pool;
            _health = initParams.Health;
            _speed = initParams.Speed;
            _damage = initParams.damage;

            enemyAgent.transform.position = new Vector3(0, 1, 0);
            enemyAgent.enabled = true;
            enemyAgent.speed = 3;

            _registry.AddEnemy(this);
        }

        private void Update()
        {
            // enemyAgent.SetDestination(_player.transform.position);
            if (!attacking)
            {
                _behaviour.Tick(_playerTransform.position);
            }
            else
            {
                Attack();
            }
        }

        private void Attack()
        {
            if (Time.time > attackEndTime)
            {
                attackEndTime = Time.time + 1;
                _player.TakeDamage(_damage);
            }
        }
        
        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Die();
            }
        }
        
        public void Die()
        {
            _pool.Despawn(this);
        }


        public class Factory : PlaceholderFactory<EnemyInitParams, EnemyController>
        {
        }
    }
}