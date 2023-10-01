using System;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Effects
{
    public class EnemyExplosion : MonoBehaviour, IPoolable<EnemyExplosionParams, IMemoryPool>
    {
        [SerializeField] private ParticleSystem _particleSystem;
        
        private IMemoryPool _pool;
        private float _lifeTime;

        private float _despawnTime;
        
        
        [Inject]
        private void Construct(Config config)
        {
            _lifeTime = config.lifeTime;
        }

        private void Update()
        {
            if (Time.realtimeSinceStartup >= _despawnTime)
            {
                _pool.Despawn(this);
            }
        }

        public void OnDespawned()
        {
            _pool = null;
            _particleSystem.Stop();
            _particleSystem.Simulate(0, true, true);
        }

        public void OnSpawned(EnemyExplosionParams initParams, IMemoryPool pool)
        {
            _pool = pool;
            _despawnTime = Time.realtimeSinceStartup + _lifeTime;
            _particleSystem.Play();
        }

        [Serializable]
        public class Config
        {
            public float lifeTime = 1;
        }
        
        public class Factory : PlaceholderFactory<EnemyExplosionParams, EnemyExplosion>
        {
        }
    }
}