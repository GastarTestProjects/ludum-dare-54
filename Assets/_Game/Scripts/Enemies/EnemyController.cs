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

        private float _health;
        private float _speed;

        [Inject]
        private void Construct(EnemyRegistry registry, PlayerController player)
        {
            _player = player;
            _registry = registry;
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

            enemyAgent.transform.position = new Vector3(0, 1, 0);
            enemyAgent.enabled = true;
            enemyAgent.speed = 3;

            _registry.AddEnemy(this);
        }

        private void Update()
        {
            enemyAgent.SetDestination(_player.transform.position);
        }


        public class Factory : PlaceholderFactory<EnemyInitParams, EnemyController>
        {
        }
    }
}