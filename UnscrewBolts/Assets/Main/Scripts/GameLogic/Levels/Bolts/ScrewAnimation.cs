using System;
using DG.Tweening;
using UnityEngine;

namespace Scripts.GameLogic.Levels.Bolts
{
    [Serializable]
    internal class ScrewAnimation
    {
        [SerializeField]
        private float _screwTime;

        [SerializeField]
        private float _screwHeight = 2f;

        [SerializeField, Range(0, 360)]
        private float _screwAngle = 40f;

        private Sequence _screwSequence;

        public void Destroy()
        {
            _screwSequence?.Kill();
        }

        public void Unscrew(Transform visualContainer, Transform topContainer, Transform modelContainer)
        {
            _screwSequence?.Complete();
            _screwSequence = DOTween.Sequence();

            _screwSequence.Append(visualContainer
                .DOLocalMoveY(_screwHeight, _screwTime)
                .SetEase(Ease.InSine));
            _screwSequence.Join(visualContainer
                .DOLocalRotate(new Vector3(_screwAngle, 0, 0), _screwTime, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutSine));

            _screwSequence.Join(topContainer
                .DOLocalRotate(new Vector3(0, 0, -360), _screwTime, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InSine));
            _screwSequence.Join(modelContainer
                .DOLocalRotate(new Vector3(0, 0, -360), _screwTime, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InSine));

            _screwSequence.Play();
        }

        public void Screw(Transform visualContainer, Transform topContainer, Transform modelContainer)
        {
            _screwSequence?.Complete();
            _screwSequence = DOTween.Sequence();

            _screwSequence.Append(visualContainer
                .DOLocalMoveY(0, _screwTime)
                .SetEase(Ease.OutSine));
            _screwSequence.Join(visualContainer
                .DOLocalRotate(new Vector3(-_screwAngle, 0, 0), _screwTime, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutSine));

            _screwSequence.Join(topContainer
                .DOLocalRotate(new Vector3(0, 0, 360), _screwTime, RotateMode.LocalAxisAdd)
                .SetEase(Ease.OutSine));
            _screwSequence.Join(modelContainer
                .DOLocalRotate(new Vector3(0, 0, 360), _screwTime, RotateMode.LocalAxisAdd)
                .SetEase(Ease.OutSine));

            _screwSequence.Play();
        }
    }
}