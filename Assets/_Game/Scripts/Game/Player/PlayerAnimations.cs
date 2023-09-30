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

        public void PlayShootAnimation()
        {
            animancerComponent.Play(shootAnimation);
        }

        public void PlayIdleAnimation()
        {
            animancerComponent.Play(idleAnimation);
        }
    }
}