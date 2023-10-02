using System;
using UnityEngine;

namespace _Game.Scripts.Enemies
{
    public class BombObject : MonoBehaviour
    {
        private Action onCollision;
        
        public void SetOnCollisionListener(Action onCollision)
        {
            this.onCollision = onCollision;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            onCollision?.Invoke();
        }
    }
}