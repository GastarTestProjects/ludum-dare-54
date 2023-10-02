using System;
using System.Collections;
using _Game.Scripts.Effects;
using _Game.Scripts.Enemy;
using _Game.Scripts.Player;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace _Game.Scripts.Enemies
{
    public class EnemyController : MonoBehaviour, IPoolable<EnemyInitParams, IMemoryPool>
    {
        [SerializeField] private EnemyAnimations animations;
        [SerializeField] private NavMeshAgent enemyAgent;
        
        private EnemyRegistry _registry;
        private IMemoryPool _pool;
        private PlayerController _player;
        private Transform _playerTransform;
        private EnemyExplosion.Factory _explosionFactory;
        
        private EnemyInitParams _initParams;
        private Config _config;

        private float _health;
        // private float _speed;
        // private int _damage;

        // private EnemyBehaviour _behaviour;
        private bool attacking;

        // private float attackEndTime;
        
        [Inject]
        private SignalBus _signalBus;

        [Inject]
        private void Construct(Config config,
            EnemyRegistry registry,
            PlayerController player,
            EnemyExplosion.Factory explosionFactory)
        {
            _config = config;
            _explosionFactory = explosionFactory;
            _player = player;
            _playerTransform = _player.transform;
            _registry = registry;
            // _behaviour = new EnemyBehaviour(enemyAgent, Attack);
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
            _initParams = initParams;

            enemyAgent.transform.position = new Vector3(0, 1, 0);
            enemyAgent.enabled = true;
            enemyAgent.speed = _initParams.Speed;

            _registry.AddEnemy(this);
        }

        private void Update()
        {
            if (attacking)
                return;
            
            if (Vector3.Distance(_playerTransform.position, enemyAgent.transform.position) <=
                _config.explosionDistance)
            {
                StartCoroutine(Attack());
            }
            else
            {
                if (enemyAgent.isActiveAndEnabled)
                    enemyAgent.SetDestination(_playerTransform.position);
            }
        }

        private IEnumerator Attack()
        {
            animations.PlayAttackAnimation();
            enemyAgent.enabled = false;
            yield return new WaitForSeconds(_config.attackExplosionDelay);
            Die(false);
        }
        
        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
                Die(true);
        }
        
        private void Die(bool killed)
        {
            StopAllCoroutines();
            if (_pool == null)
                return;
            _pool.Despawn(this);
            var explosion = _explosionFactory.Create(new EnemyExplosionParams(killed));
            explosion.transform.position = transform.position;

            if (Vector3.Distance(_playerTransform.position, enemyAgent.transform.position) <=
                _config.explosionDistance * 1.2)
            {
                _player.TakeDamage(killed ? (_initParams.Damage / 2) : _initParams.Damage);
            }
            
            _signalBus.Fire(new EnemyDiedEvent(killed));
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var hitSpeed = _player.GetComponent<Rigidbody>().velocity.magnitude;
                //Debug.Log("HitSpeed: " + hitSpeed);
                TakeDamage((int) (hitSpeed / 5));
            }
        }

        [Serializable]
        public class Config
        {
            public float explosionDistance = 1.5f;
            public float attackExplosionDelay = 0.2f;
        }


        public class Factory : PlaceholderFactory<EnemyInitParams, EnemyController>
        {
        }
    }
}