using _Game.Scripts.Enemy;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Enemies
{
    public class EnemyController : MonoBehaviour, IPoolable<EnemyInitParams, IMemoryPool>
    {
        public void OnDespawned()
        {
            throw new System.NotImplementedException();
        }

        public void OnSpawned(EnemyInitParams initParams, IMemoryPool pool)
        {
            throw new System.NotImplementedException();
        }


        public class Factory : PlaceholderFactory<EnemyInitParams, EnemyController>
        {
        }
    }
}