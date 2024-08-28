using System;
using Scripts.Core.Utilities;
using Scripts.GameLogic.Levels.Anchors;
using Scripts.GameLogic.Levels.Core;
using Scripts.GameLogic.Sound;
using Scripts.Infrastructure.Providers.Events;
using Scripts.UI.Common.UIAnimator.Main;
using UnityEngine;
using Zenject;

namespace Scripts.GameLogic.Levels.Bolts
{
    public class Bolt : VisualElement
    {
        [SerializeField]
        private GameObject _model;

        [SerializeField]
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private Collider2D _collider;

        [SerializeField]
        private BoltAnimator _boltAnimator;

        [SerializeField]
        private SpriteRenderer _outline;

        [SerializeField]
        private UIAnimator _outlineAnimator;

        [SerializeField]
        private ParticleSystem _boltScrewPS;

        [SerializeField]
        private ParticleSystem _boltDestroyPS;

        private LocalEventProvider _localEventProvider;
        private ISoundService _soundService;
        private AnchorPoint _anchorPoint;
        private Action _onBoltRemove;

        [Inject]
        public void Construct(LocalEventProvider localEventProvider, ISoundService soundService)
        {
            _soundService = soundService;
            _localEventProvider = localEventProvider;
        }

        public void Initialize(AnchorPoint anchorPoint, Action onBoltRemove)
        {
            _onBoltRemove = onBoltRemove;
            _anchorPoint = anchorPoint;
        }

        public override void Initialize() =>
            SendScrewedEvent();

        public void MoveTo(AnchorPoint anchorPoint, Action onBoltRemove, Action<Bolt> onBoltSet)
        {
            _onBoltRemove?.Invoke();

            _anchorPoint = anchorPoint;
            _onBoltRemove = onBoltRemove;
            onBoltSet?.Invoke(this);

            MoveToAnchorPosition();
        }

        public void RemoveFromScene()
        {
            _onBoltRemove?.Invoke();
            Hide(true);
            _soundService.PlayBoltDestroySound();
            _rigidbody.simulated = false;
            _collider.enabled = false;
            SetGlow(false);
            _boltDestroyPS.Play();

            SendUnscrewedEvent();

            Utils.PerformWithDelay(this, 0.35f, () => Destroy(gameObject));
        }

        public void SetUnscrewed()
        {
            _boltAnimator.Unscrew();
            _soundService.PlayBoltChooseSound();
            _boltScrewPS.Play();
        }

        public void SetScrewed()
        {
            _boltAnimator.Screw();
            _soundService.PlayBoltPlaceSound();
            _boltScrewPS.Play();
        }

        public override void Show(bool instant = false)
        {
            base.Show(instant);
            _model.SetActive(true);
        }

        public override void Hide(bool instant = false)
        {
            base.Hide(instant);
            _model.SetActive(false);
        }

        public void SetGlow(bool isGlowing)
        {
            if (isGlowing)
            {
                _outline.enabled = true;
                _outlineAnimator.Play();
            }
            else
            {
                _outline.enabled = false;
                _outlineAnimator.Stop();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _boltAnimator.Destroy();
            StopAllCoroutines();
        }

        private void MoveToAnchorPosition()
        {
            SendUnscrewedEvent();
            Vector3 newPosition = _anchorPoint.Position;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
            SendScrewedEvent();
        }

        private void SendUnscrewedEvent() => _localEventProvider.Invoke<BoltMoveEvent, BoltMoveData>(
            new BoltMoveData(_rigidbody, _collider, transform.position, false));

        private void SendScrewedEvent() => _localEventProvider.Invoke<BoltMoveEvent, BoltMoveData>(
            new BoltMoveData(_rigidbody, _collider, transform.position, true));
    }
}