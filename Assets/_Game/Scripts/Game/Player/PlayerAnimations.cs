using System;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Game.Player
{
    public class PlayerAnimations : MonoBehaviour
    {
        [SerializeField] private ParticleSystem shotParticles;
        [SerializeField] private AnimancerComponent animancerComponent;
        [SerializeField] private ClipTransition idleAnimation;

        [SerializeField] private List<RigPointsBinding> recoilPoints;
        private bool _isInitDefaultRecoilPoints;

        private void InitDefaultRecoilPoints()
        {
            recoilPoints.ForEach(
                points =>
                {
                    points.defaultLocalPosition = points.rigPoint.localPosition;
                    points.defaultLocalRotation = points.rigPoint.localRotation;
                });
            _isInitDefaultRecoilPoints = true;
        }

        public void PlayShootAnimation()
        {
            if (!_isInitDefaultRecoilPoints)
                InitDefaultRecoilPoints();
            const float duration = .13f;
            foreach (var points in recoilPoints)
            {
                // randomly select a recoil point
                var randomIndex = UnityEngine.Random.Range(0, points.recoilPoints.Count);
                var recoilPoint = points.recoilPoints[randomIndex];

                points.rigPoint.DOLocalMove(recoilPoint.localPosition, duration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(
                        () => { points.rigPoint.DOLocalMove(points.defaultLocalPosition, duration * 2f); });
                points.rigPoint.DOLocalRotateQuaternion(recoilPoint.localRotation, duration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(
                        () => { points.rigPoint.DOLocalRotateQuaternion(points.defaultLocalRotation, duration * 2f); });
            }
        }

        public void PlayIdleAnimation()
        {
            animancerComponent.Play(idleAnimation);
        }
    }

    [Serializable]
    public class RigPointsBinding
    {
        public Transform rigPoint;
        public Vector3 defaultLocalPosition;
        public Quaternion defaultLocalRotation;
        public List<Transform> recoilPoints;
    }
}