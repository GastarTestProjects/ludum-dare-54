using System;
using _Game.Scripts.Player;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Enemies
{
    public class BombSpawner : MonoBehaviour
    {
        private Config _config;
        private Bomb.Factory _bombFactory;
        private PlayerController _player;

        private float _nextSpawnTime;

        [Inject]
        public void Construct(Config config, Bomb.Factory bombFactory, PlayerController player)
        {
            _config = config;
            _player = player;
            _bombFactory = bombFactory;
        }

        private void Update()
        {
            if (Time.time > _nextSpawnTime)
            {
                _nextSpawnTime = Time.time + _config.bombSpawnDelay +
                                 Random.Range(-_config.bombSpawnDelayRandomness,
                                     _config.bombSpawnDelayRandomness);
                SpawnBomb();
            }
        }
        
        private void SpawnBomb()
        {
            var bomb = _bombFactory.Create();
            bomb.transform.position = _player.transform.position;
        }

        [Serializable]
        public class Config
        {
            public float bombSpawnDelay = 6f;
            public float bombSpawnDelayRandomness = 1f;
        }
    }
}