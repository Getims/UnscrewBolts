using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.UI.Common.UIAnimator.Animations
{
    [Serializable]
    public abstract class Animation
    {
        protected const bool _hideAnimationValues = false;

        [SerializeField]
        protected bool _instant = false;

        [SerializeField, Min(0), LabelWidth(125)]
        [HideIf(nameof(_instant))]
        protected float _startDelay;

        [SerializeField, Min(0), LabelWidth(125)]
        [HideIf(nameof(_hideValues))]
        protected float _animationTime;

        [SerializeField, LabelWidth(125)]
        [HideIf(nameof(_hideValues))]
        protected Ease _animationEase;

        public float StartDelay => _startDelay;
        public float AnimationTime => _animationTime;
        protected bool _hideValues => _instant || _hideAnimationValues;

        public abstract void Play();
        public abstract void Stop();
    }
}