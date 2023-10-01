using System;
using System.Reflection.Emit;
using _Game.Scripts.Enemy;
using _Game.Scripts.Player;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Enemies
{
    public class EnemySpawner: MonoBehaviour
    {
        [SerializeField] private BoxCollider spawnArea;
        
        [Inject]
        private Config _config;
        [Inject]
        private PlayerController _player;
        [Inject]
        private EnemyRegistry _enemyRegistry;
        [Inject]
        private EnemyController.Factory _enemyFactory;
        [Inject]
        private EnemySpawnPoint.Factory _spawnPointFactory;

        private float nextSpawnTime;

        private void Start()
        {
            nextSpawnTime = Time.time + _config.firstSpawnDelay;
        }

        private void Update()
        {
            if (Time.time > nextSpawnTime)
            {
                nextSpawnTime = Time.time + _config.startSpawnDelay;
                if (_enemyRegistry.Enemies.Count < _config.startMaxEnemiesOnScreen)
                {
                    SpawnEnemies();
                }
            }
        }
        
        public void SpawnEnemies()
        {
            for (int i = 0; i < _config.startSpawnAmount; i++)
            {
                var spawnPosition = GetRandomSpawnPosition();
                // var enemy = _enemyFactory.Create(
                //     new EnemyInitParams(_config.baseHealth, _config.baseSpeed));
                var spawnPoint = _spawnPointFactory.Create();
                spawnPoint.transform.position = spawnPosition;
                spawnPoint.SetSpawnFinishListener(OnSpawnPointFinish);
                // enemy.transform.position = spawnPosition;
            }
        }
        
        // TODO: avoid spawning enemies close to each other
        private Vector3 GetRandomSpawnPosition()
        {
            var spawnPosition = new Vector3(
                UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                spawnArea.bounds.min.y,
                UnityEngine.Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
            );
            return spawnPosition;
        }

        private void OnSpawnPointFinish(EnemySpawnPoint spawnPoint)
        {
            if (Vector3.Distance(_player.transform.position, spawnPoint.transform.position) > 1f)
            {
                var enemy = _enemyFactory.Create(
                    new EnemyInitParams(_config.baseHealth, _config.baseSpeed));
                enemy.transform.position = spawnPoint.transform.position;
            }
        }
        

        [Serializable]
        public class Config
        {
            public float firstSpawnDelay = 1f;
            public float startMaxEnemiesOnScreen = 8;
            public float startSpawnDelay = 2;
            public float startSpawnAmount = 3;

            [Header("Base enemy stats")]
            public float baseHealth = 1;
            public float baseSpeed = 1;
        }
    }
}