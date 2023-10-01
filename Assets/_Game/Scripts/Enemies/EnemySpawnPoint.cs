using System;
using _Game.Scripts.Enemy;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Enemies
{
    public class EnemySpawnPoint : MonoBehaviour, IPoolable<IMemoryPool>
    {
        [SerializeField] ParticleSystem _particleSystem;
        
        private float _spawnTime;
        private IMemoryPool _pool;
        private Action<EnemySpawnPoint> _onFinish;

        private bool _finished;
        private Config _config;

        [Inject]
        private void Construct(Config config)
        {
            _config = config;
        }

        private void Update()
        {
            if (!_finished && Time.time >= _spawnTime)
            {
                _onFinish?.Invoke(this);
                _finished = true;
                _pool.Despawn(this);
            }
        }
        
        public void SetSpawnFinishListener(Action<EnemySpawnPoint> onFinish)
        {
            _onFinish = onFinish;
        }

        public void OnDespawned()
        {
            _pool = null;
            _particleSystem.Stop();
            _particleSystem.Simulate(0, true, true);
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            _particleSystem.Play();
            _finished = false;
            _spawnTime = Time.time + _config.spawnDelay + Random.Range(
                -_config.spawnDelayRandomness,
                _config.spawnDelayRandomness);
        }
        
        [Serializable]
        public class Config
        {
            public float spawnDelay = 1;
            public float spawnDelayRandomness = 0.1f;
        }
        
        public class Factory : PlaceholderFactory<EnemySpawnPoint>
        {
        }
    }
}