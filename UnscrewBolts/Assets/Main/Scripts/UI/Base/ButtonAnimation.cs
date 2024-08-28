using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI.Base
{
    public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private bool _useTransform;

        [SerializeField]
        private bool _checkButtonInteractable;

        [SerializeField, Required]
        [ShowIf(nameof(_checkButtonInteractable))]
        private Button _button;

        [SerializeField]
        private float _scaleTime = 0.15f;

        [SerializeField]
        private Vector2 _scale = new Vector2(0.85f, 0.85f);

        [SerializeField]
        [Tooltip("Autoplay of animation on every click")]
        private bool _usePointersEvents = true;

        [Title("References"), HideIf("_useTransform")]
        [InfoBox("Отсуствует ссылка на 'Rect Transform'.", InfoMessageType.Error, "@_rectTransform == null")]
        [SerializeField] private RectTransform _rectTransform;

        [ShowIf("_useTransform")]
        [InfoBox("Отсуствует ссылка на 'Transform'.", InfoMessageType.Error, "@_transform == null")]
        [SerializeField] private Transform _transform;

        private bool _isEnabled = true;
        private Tweener _scaleTwnr;
        private Vector3 _startScale;
        private Vector3 _finalScale;

        private void Start() =>
            _startScale = _useTransform ? _transform.localScale : _rectTransform.localScale;

        public void PlayClickAnimation()
        {
            _scaleTwnr.Complete();
            _finalScale = _startScale * _scale;
            _finalScale.z = _finalScale.x;

            if (_useTransform)
                _scaleTwnr = _transform.DOScale(_finalScale, _scaleTime)
                    .SetUpdate(true)
                    .OnComplete(() =>
                        _scaleTwnr = _transform.DOScale(_startScale, _scaleTime)
                            .SetUpdate(true));
            else
                _scaleTwnr = _rectTransform.DOScale(_finalScale, _scaleTime)
                    .SetUpdate(true)
                    .OnComplete(() =>
                        _scaleTwnr = _rectTransform.DOScale(_startScale, _scaleTime)
                            .SetUpdate(true));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_usePointersEvents)
                return;

            if (_checkButtonInteractable && !_button.interactable)
                return;

            OnButtonDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_usePointersEvents)
                return;

            if (_checkButtonInteractable && !_button.interactable)
                return;

            OnButtonUp();
        }

        private void OnButtonDown()
        {
            if (!_isEnabled) return;

            _scaleTwnr.Complete();
            _finalScale = _startScale * _scale;
            _finalScale.z = _finalScale.x;

            if (_useTransform)
                _scaleTwnr = _transform.DOScale(_finalScale, _scaleTime).SetUpdate(true);
            else
                _scaleTwnr = _rectTransform.DOScale(_finalScale, _scaleTime).SetUpdate(true);
        }

        private void OnButtonUp()
        {
            if (!_isEnabled) return;

            _scaleTwnr.Complete();

            if (_useTransform)
                _scaleTwnr = _transform.DOScale(_startScale, _scaleTime).SetUpdate(true);
            else
                _scaleTwnr = _rectTransform.DOScale(_startScale, _scaleTime).SetUpdate(true);
        }
    }
}