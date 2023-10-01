using System;
using UnityEngine;

namespace _Game.Scripts.Enemies
{
    public class EnemySpawner: MonoBehaviour
    {
        [SerializeField] private BoxCollider spawnArea;


        [Serializable]
        public class Config
        {
            public float minEnemiesToSpawn;
        }
    }
}