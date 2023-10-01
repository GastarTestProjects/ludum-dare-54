using System;
using Animancer;
using UnityEngine;

namespace _Game.Scripts.Game.Player
{
    public class PlayerAnimations : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent animancerComponent;
        [SerializeField] private ClipTransition shootAnimation;
        [SerializeField] private ClipTransition idleAnimation;
        [SerializeField] private ClipTransition deathAnimation;

        private AnimancerState _shootState;

        public void PlayShootAnimation()
        {
            if (_shootState is {IsPlaying: true})
                _shootState.Stop();
            _shootState = animancerComponent.Play(shootAnimation);
        }

        public void PlayIdleAnimation()
        {
            if (_shootState is {IsPlaying: true})
                return;
            animancerComponent.Play(idleAnimation);
        }

        public void PlayDeathAnimation()
        {
            if (_shootState is {IsPlaying: true})
                _shootState.Stop();
            animancerComponent.Play(deathAnimation);
        }
    }
}