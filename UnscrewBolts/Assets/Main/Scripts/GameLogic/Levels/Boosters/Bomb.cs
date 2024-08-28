using System;
using DG.Tweening;
using Scripts.GameLogic.Levels.Core;
using Scripts.UI.Common.UIAnimator.Main;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.GameLogic.Levels.Boosters
{
    public class Bomb : VisualElement
    {
        [SerializeField]
        private Transform _radiusContainer;

        [SerializeField, Range(0.5f, 10), OnValueChanged(nameof(ChangeRadius))]
        private float _explosionRadius = 1;

        [Title("Animation")]
        [SerializeField]
        private float _scalePower = 0.1f;

        [SerializeField]
        private float _punchDuration = 0.15f;

        [SerializeField]
        private UIAnimator _bombAnimator;

        [Title("Particles")]
        [SerializeField]
        private GameObject _firePS;

        [SerializeField]
        private GameObject _explosionPS;

        private bool _canTrackCursor = false;
        private Tweener _placeTW;
        private Camera _camera;

        public event Action OnBombPlacedEvent;
        public float ExplosionRadius => _explosionRadius;

        public override void Initialize()
        {
            _camera = Camera.main;
            _canTrackCursor = true;
        }

        public override void Show(bool instant = false)
        {
            base.Show(instant);

            gameObject.SetActive(true);
            _firePS.SetActive(true);

            _bombAnimator.Play();
        }

        public override void Hide(bool instant = false)
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            _bombAnimator.Stop();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _placeTW?.Kill();
        }

        private void Update()
        {
            if (!_canTrackCursor)
                return;

            transform.localPosition = ConvertScreenPointToWorld(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
                OnClick();
        }

        private void OnClick()
        {
            transform.localPosition = ConvertScreenPointToWorld(Input.mousePosition);
            _canTrackCursor = false;
            _placeTW?.Kill();
            _placeTW = transform
                .DOPunchScale(-Vector3.one * _scalePower, _punchDuration, 0, 0)
                .OnComplete(Explosion);
        }

        private void Explosion()
        {
            base.Hide();

            _bombAnimator.Stop();

            _firePS.SetActive(false);
            _explosionPS.SetActive(true);

            OnBombPlacedEvent?.Invoke();
        }

        private void ChangeRadius()
        {
#if UNITY_EDITOR
            _radiusContainer.localScale = Vector3.one * _explosionRadius / 2.35f;
#endif
        }

        private Vector3 ConvertScreenPointToWorld(Vector3 mousePoint)
        {
            Vector3 position = _camera.ScreenToWorldPoint(mousePoint);
            position.z = 0;
            return position;
        }
    }
}