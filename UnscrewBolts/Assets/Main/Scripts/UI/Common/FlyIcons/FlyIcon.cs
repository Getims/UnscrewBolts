using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Scripts.UI.Common.FlyIcons
{
    public class FlyIcon : MonoBehaviour
    {
        [SerializeField]
        private Image _iconImg;

        [SerializeField]
        private RectTransform _iconRT;

        [SerializeField]
        private CanvasGroup _iconCG;

        private Sequence _flySequence;
        private float _time;
        private float _endScale;
        private FlySettings _flySettings = new FlySettings();
        private Action _onComplete;
        private Action _onMoved;

        public void Setup(Sprite sprite, FlySettings flySettings, Action onMoved = null, Action onComplete = null)
        {
            KillTweeners();

            _iconImg.sprite = sprite;
            _flySettings = flySettings;
            _onMoved = onMoved;
            _onComplete = onComplete;

            _iconCG.alpha = _flySettings.ShowTime > 0 ? 0 : 1;
            if (_flySettings.ScaleFromZeroTime > 0)
                _iconRT.localScale = Vector3.zero;
            else
                _iconRT.localScale = _flySettings.StartScale * Vector3.one;
            _iconRT.anchoredPosition = new Vector2(0, 0);
        }

        public void PlayAnimation(Vector3 startPosition, Vector3 targetPosition)
        {
            KillTweeners();
            _flySequence = DOTween.Sequence();

            _flySequence.Append(_iconRT
                .DOScale(Vector3.one * _flySettings.StartScale, _flySettings.ScaleFromZeroTime)
                .SetEase(_flySettings.ScaleEase));
            _flySequence.Join(_iconCG.DOFade(1, _flySettings.ShowTime)
                .SetDelay(_flySettings.ShowDelay));

            _iconRT.position = startPosition;
            float moveTime = _flySettings.MoveTime;
            moveTime += Random.Range(0, _flySettings.ExtraMoveTimeRandom);

            _flySequence.Append(_iconRT.DOMove(targetPosition, moveTime)
                .SetEase(_flySettings.MoveEase)
                .SetDelay(_flySettings.MoveDelay)
                .OnComplete(OnMoved)
            );

            _flySequence.Append(_iconRT.DOScale(Vector3.one * _flySettings.EndScale, _flySettings.ScaleTime)
                .SetEase(_flySettings.ScaleEase)
                .SetDelay(_flySettings.ScaleDelay));

            _flySequence.Join(_iconCG.DOFade(0, _flySettings.HideTime)
                .SetDelay(_flySettings.HideDelay));

            _flySequence.OnComplete(() => _onComplete?.Invoke());
        }

        private void OnDestroy() => KillTweeners();

        private void KillTweeners() => _flySequence?.Kill();
        private void OnMoved() => _onMoved?.Invoke();
    }
}