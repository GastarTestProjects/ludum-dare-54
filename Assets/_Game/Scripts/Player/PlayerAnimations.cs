using System;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerAnimations : MonoBehaviour
    {
        public bool IsInHiddenState { get; private set; }
        [SerializeField] private Transform shotParticlesSpawnPoint;
        [SerializeField] private GameObject shotParticlesPrefab;
        [SerializeField] private AnimancerComponent animancerComponent;
        [SerializeField] private ClipTransition idleAnimation;

        [SerializeField] private List<RigPointsBinding> recoilPoints;
        [SerializeField] private List<RigPointsBinding> hidePoints;
        private bool _isInitDefaultRecoilPoints;

        private void InitDefaultRigPoints()
        {
            recoilPoints.ForEach(
                points =>
                {
                    points.defaultLocalPosition = points.rigPoint.localPosition;
                    points.defaultLocalRotation = points.rigPoint.localRotation;
                });
            hidePoints.ForEach(
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
                InitDefaultRigPoints();

            foreach (var points in recoilPoints)
                points.rigPoint.DOKill();

            const float duration = .13f;
            foreach (var points in recoilPoints)
            {
                // randomly select a recoil point
                var randomIndex = UnityEngine.Random.Range(0, points.customTargetPoints.Count);
                var recoilPoint = points.customTargetPoints[randomIndex];

                points.rigPoint.DOLocalMove(recoilPoint.localPosition, duration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(
                        () => { points.rigPoint.DOLocalMove(points.defaultLocalPosition, duration * 2f); });
                points.rigPoint.DOLocalRotateQuaternion(recoilPoint.localRotation, duration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(
                        () => { points.rigPoint.DOLocalRotateQuaternion(points.defaultLocalRotation, duration * 2f); });
            }

            var spawnedParticles = Instantiate(
                shotParticlesPrefab,
                shotParticlesSpawnPoint.position,
                shotParticlesSpawnPoint.rotation);
            Destroy(spawnedParticles, 3);
        }

        public void PlayIdleAnimation()
        {
            animancerComponent.Play(idleAnimation);
        }

        public void AnimateHide()
        {
            foreach (var points in hidePoints)
                points.rigPoint.DOKill();

            const float duration = .13f;
            IsInHiddenState = true;
            foreach (var points in hidePoints)
            {
                points.rigPoint.DOLocalMove(points.customTargetPoints[0].localPosition, duration)
                    .SetEase(Ease.OutQuad);
                points.rigPoint.DOLocalRotateQuaternion(points.customTargetPoints[0].localRotation, duration);
            }
        }

        public void AnimateUnHide()
        {
            foreach (var points in hidePoints)
                points.rigPoint.DOKill();

            const float duration = .35f;
            foreach (var points in hidePoints)
            {
                points.rigPoint.DOLocalMove(points.defaultLocalPosition, duration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => IsInHiddenState = false);
                points.rigPoint.DOLocalRotateQuaternion(points.defaultLocalRotation, duration);
            }
        }

        public void SetDefaultRigPointsState()
        {
            if (!_isInitDefaultRecoilPoints)
                InitDefaultRigPoints();
            IsInHiddenState = false;
            foreach (var points in recoilPoints)
                points.rigPoint.DOKill();
            foreach (var points in hidePoints)
                points.rigPoint.DOKill();

            foreach (var points in recoilPoints)
            {
                points.rigPoint.localPosition = points.defaultLocalPosition;
                points.rigPoint.localRotation = points.defaultLocalRotation;
            }

            foreach (var points in hidePoints)
            {
                points.rigPoint.localPosition = points.defaultLocalPosition;
                points.rigPoint.localRotation = points.defaultLocalRotation;
            }
        }
    }

    [Serializable]
    public class RigPointsBinding
    {
        [HideInInspector] public Vector3 defaultLocalPosition;
        [HideInInspector] public Quaternion defaultLocalRotation;
        public Transform rigPoint;
        public List<Transform> customTargetPoints;
    }
}