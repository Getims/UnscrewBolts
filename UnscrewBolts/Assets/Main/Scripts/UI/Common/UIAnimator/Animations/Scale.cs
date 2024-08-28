using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.UI.Common.UIAnimator.Animations
{
    [Serializable]
    public class Scale : Animation
    {
        [SerializeField]
        private Transform _transform;

        [SerializeField]
        [HideIf(nameof(_instant))]
        private bool _useStartScale = false;

        [SerializeField]
        [HideIf(nameof(_instant)), ShowIf(nameof(_useStartScale))]
        private Vector3 _startScale = Vector3.one;

        [SerializeField]
        private Vector3 _targetScale;

        private Tweener _scaleTW;

        public override void Play()
        {
            if (_instant)
            {
                _transform.localScale = _targetScale;
                return;
            }

            _scaleTW?.Kill();
            if (_useStartScale)
            {
                _transform.localScale = _startScale;
            }

            _scaleTW = _transform.DOScale(_targetScale, _animationTime)
                .SetEase(_animationEase)
                .SetDelay(_startDelay);
        }

        public override void Stop() =>
            _scaleTW?.Kill();
    }
}