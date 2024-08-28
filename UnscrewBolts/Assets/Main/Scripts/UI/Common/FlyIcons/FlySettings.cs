using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.UI.Common.FlyIcons
{
    [Serializable]
    public class FlySettings
    {
        [Title("Show")]
        [SerializeField, Min(0)]
        private float _startScale = 0.5f;

        [SerializeField, Min(0)]
        private float _scaleFromZeroTime = 0;

        [SerializeField, Min(0)]
        private float _showTime;

        [SerializeField, Min(0)]
        private float _showDelay;

        [Title("Move")]
        [SerializeField, Min(0)]
        private float _moveDelay = 0;

        [SerializeField, Min(0)]
        private float _moveTime = 0;

        [SerializeField]
        private Ease _moveEase;

        [SerializeField]
        private float _extraMoveTimeRandom = 0;

        [Title("Hide")]
        [SerializeField, Min(0)]
        private float _endScale = 0.5f;

        [SerializeField, Min(0)]
        private float _scaleTime = 0f;

        [SerializeField, Min(0)]
        private Ease _scaleEase;

        [SerializeField, Min(0)]
        private float _scaleDelay = 0.5f;

        [SerializeField, Min(0)]
        private float _hideTime;

        [SerializeField, Min(0)]
        private float _hideDelay;

        public float MoveDelay => _moveDelay;
        public float MoveTime => _moveTime;
        public Ease MoveEase => _moveEase;
        public float StartScale => _startScale;
        public float EndScale => _endScale;
        public float ShowTime => _showTime;
        public float ShowDelay => _showDelay;
        public float HideTime => _hideTime;
        public float HideDelay => _hideDelay;
        public float ScaleTime => _scaleTime;
        public Ease ScaleEase => _scaleEase;
        public float ScaleDelay => _scaleDelay;
        public float ScaleFromZeroTime => _scaleFromZeroTime;
        public float ExtraMoveTimeRandom => _extraMoveTimeRandom;

        public float GetAnimationTime()
        {
            float time = _moveTime + _moveDelay + _extraMoveTimeRandom;

            if (time < _scaleDelay + _scaleTime)
                time = _scaleDelay + _scaleTime;


            if (time < _hideDelay + _hideTime)
                time = _hideDelay + _hideTime;

            return time;
        }
    }
}