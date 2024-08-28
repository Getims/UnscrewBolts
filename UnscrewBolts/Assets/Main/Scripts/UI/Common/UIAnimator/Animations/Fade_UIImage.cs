using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Common.UIAnimator.Animations
{
    [Serializable]
    public class Fade_UIImage : Animation
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        [HideIf(nameof(_instant))]
        private Fade _fadeType;

        [SerializeField]
        [ShowIf(nameof(ShowAlpha))]
        private float _alphaValue;

        private Tweener _fadeTW;

        private bool ShowAlpha => _instant || _fadeType == Fade.FadeTo;

        public override void Play()
        {
            var color = _image.color;
            if (_instant)
            {
                color = new Color(color.r, color.g, color.b, _alphaValue);
                _image.color = color;
                return;
            }

            _fadeTW?.Kill();
            float endValue = _alphaValue;

            if (_fadeType != Fade.FadeTo)
            {
                endValue = _fadeType == Fade.FadeIn ? 1 : 0;
                _image.color = new Color(color.r, color.g, color.b, _fadeType == Fade.FadeIn ? 0 : 1);
            }

            _fadeTW = _image.DOFade(endValue, _animationTime)
                .SetEase(_animationEase)
                .SetDelay(_startDelay);
        }

        public override void Stop() =>
            _fadeTW?.Kill();

        private enum Fade
        {
            FadeIn,
            FadeOut,
            FadeTo
        }
    }
}