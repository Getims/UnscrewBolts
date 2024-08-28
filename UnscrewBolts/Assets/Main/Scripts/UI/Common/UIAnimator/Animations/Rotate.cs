using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.UI.Common.UIAnimator.Animations
{
    [Serializable]
    public class Rotate : Animation
    {
        [SerializeField]
        private Transform _transform;

        [SerializeField]
        [HideIf(nameof(_instant))]
        private bool _useStartRotation = false;

        [SerializeField]
        [HideIf(nameof(_instant)), ShowIf(nameof(_useStartRotation))]
        private Vector3 _startRotation = Vector3.one;

        [SerializeField]
        private Vector3 _targetRotation;

        private Tweener _rotateTW;

        public override void Play()
        {
            if (_instant)
            {
                _transform.localEulerAngles = _targetRotation;
                return;
            }

            _rotateTW?.Kill();
            if (_useStartRotation)
            {
                _transform.localEulerAngles = _startRotation;
            }

            _rotateTW = _transform.DOLocalRotate(_targetRotation, _animationTime)
                .SetEase(_animationEase)
                .SetDelay(_startDelay);
        }

        public override void Stop() =>
            _rotateTW?.Kill();
    }
}