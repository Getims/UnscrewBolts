using System;
using System.Collections.Generic;
using Scripts.Core.Constants;
using Scripts.Core.Utilities;
using Scripts.GameLogic.Levels.Core;
using Scripts.GameLogic.Sound;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Scripts.GameLogic.Levels.GameElements
{
    public class GameElement : VisualElement
    {
        [Title("References")]
        [SerializeField]
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private List<Collider2D> _colliders = new List<Collider2D>();

        [SerializeField]
        private List<GameElementHole> _elementHoles = new List<GameElementHole>();

        [Title("Editor")]
        [SerializeField]
        private bool _isEditMode = false;

        [SerializeField, OnValueChanged(nameof(UpdateColor))]
        private Color _color;

        [SerializeField, HideInInspector]
        private LayerSetter _layerSetter;

        private ISoundService _soundService;
        private bool _isDestroyed = false;

        public event Action<GameElement> OnDestroyEvent;
        public List<GameElementHole> ElementHoles => _elementHoles;
        public bool IsEditMode => _isEditMode;
        public bool IsDestroyed => _isDestroyed;

        [Inject]
        public void Construct(ISoundService soundService) =>
            _soundService = soundService;

        public override void Initialize()
        {
            foreach (GameElementHole elementHole in _elementHoles)
            {
                elementHole.Initialize();
                elementHole.OnBoltInteractionEvent += SetBoltCollision;
            }

            _rigidbody.simulated = true;
        }

        public bool HasHoleInPoint(Vector3 point)
        {
            foreach (GameElementHole elementHole in _elementHoles)
            {
                if (elementHole.PointInHole(point))
                    return true;
            }

            return false;
        }

        public void DestroySelf()
        {
            _isDestroyed = true;
            OnDestroyEvent?.Invoke(this);
            foreach (Collider2D collider in _colliders)
                collider.enabled = false;

            Utils.PerformWithDelay(this, 0.25f, () => Destroy(gameObject));
        }

        public override void Show(bool instant = false)
        {
            base.Show(instant);
            foreach (GameElementHole elementHole in _elementHoles)
                elementHole.Show(instant);
        }

        public override void Hide(bool instant = false)
        {
            base.Hide(instant);
            foreach (GameElementHole elementHole in _elementHoles)
                elementHole.Hide(instant);
        }

        public void UpdateLayer(int newLayer)
        {
#if UNITY_EDITOR
            _layerSetter.Update(_sprite, _elementHoles, newLayer);
#endif
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _isDestroyed = true;
            StopAllCoroutines();
            foreach (GameElementHole elementHole in _elementHoles)
                elementHole.OnBoltInteractionEvent -= SetBoltCollision;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform == transform)
                return;

            float collisionForce = other.relativeVelocity.magnitude;
            if (collisionForce < GameLogicConstants.ELEMENT_HIT_SOUND_FORCE)
                return;

            _soundService.PlayGameElementHitSound();
        }

        private void SetBoltCollision(Collider2D boltCollider, bool ignore)
        {
            foreach (Collider2D collider in _colliders)
                Physics2D.IgnoreCollision(boltCollider, collider, ignore);

            if (!ignore)
                _rigidbody.AddTorque(Utils.RandomValue(-2, 2), ForceMode2D.Impulse);
        }

        [Button]
        private void UpdateElement()
        {
            GameElementHole[] elementHoles = GetComponentsInChildren<GameElementHole>();
            _elementHoles.Clear();
            _elementHoles.AddRange(elementHoles);

            foreach (GameElementHole elementHole in _elementHoles)
                elementHole.Setup(_rigidbody);

            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
            _colliders.Clear();
            _colliders.AddRange(colliders);
            foreach (var collider in colliders)
            {
                if (collider.GetComponent<GameElement>())
                    continue;
                if (collider.GetComponent<GameElementLink>())
                    continue;
                var elementLink = collider.AddComponent<GameElementLink>();
                elementLink.GameElement = this;
            }

            _layerSetter.Update(_sprite, _elementHoles);
            UpdateColor();
        }

        [Button]
        private void ResetRotation() => transform.localEulerAngles = Vector3.zero;

        private void UpdateColor()
        {
            if (_sprite == null)
                Utils.Log(this, "Нет ссылки на SpriteRenderer");
            _sprite.color = _color;
        }
    }
}