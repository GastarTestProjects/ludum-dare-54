using System;
using Animancer;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject;

namespace _Game.Scripts.Enemies
{
    public class EnemyAnimations : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private AnimancerComponent animancerComponent;
        [SerializeField] private ClipTransition idleAnimation;
        [SerializeField] private ClipTransition walkAnimation;
        [SerializeField] private ClipTransition attackAnimation;

        private bool _attacking;
        private Config _config;

        [Inject]
        private void Construct(Config config)
        {
            _config = config;
        }
        
        private void OnEnable()
        {
            _attacking = false;
        }

        private void Update()
        {
            if (_attacking)
                return;
            if (agent.velocity.magnitude > 0.01f)
            {
                walkAnimation.Speed = agent.velocity.magnitude / _config.animationSpeedDivider;
                animancerComponent.Play(walkAnimation);
            }
            else
            {
                animancerComponent.Play(idleAnimation);
            }
        }
        
        public void PlayAttackAnimation()
        {
            _attacking = true;
            animancerComponent.Play(attackAnimation);
        }

        [Serializable]
        public class Config
        {
            public float animationSpeedDivider = 2f;
        }
    }
}