using System;
using Scripts.Core.Constants;
using Scripts.GameLogic.Levels.Bolts;
using Scripts.GameLogic.Levels.Core;
using Scripts.Infrastructure.Providers.Events;
using UnityEngine;
using Zenject;

namespace Scripts.GameLogic.Levels.GameElements
{
    public class GameElementHole : VisualElement
    {
        [SerializeField]
        private SpriteMask _spriteMask;

        [SerializeField]
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private HingeJoint2D _hingeJoint2D;

        [SerializeField]
        private FixedJoint2D _fixedJoint2D;

        private LocalEventProvider _localEventProvider;

        public event Action<Collider2D, bool> OnBoltInteractionEvent;

        [Inject]
        public void Construct(LocalEventProvider localEventProvider)
        {
            _localEventProvider = localEventProvider;
            _localEventProvider.AddListener<BoltMoveEvent, BoltMoveData>(OnBoltMove);
        }

        public override void Initialize()
        {
            _hingeJoint2D.enabled = _hingeJoint2D.connectedBody != null;
            _rigidbody.simulated = true;
        }

        internal void SetLayer(int currentLayer)
        {
            _sprite.sortingOrder = currentLayer;
            _spriteMask.frontSortingOrder = currentLayer;
            _spriteMask.backSortingOrder = currentLayer - 1;
        }

        internal void Setup(Rigidbody2D parent)
        {
#if UNITY_EDITOR
            _fixedJoint2D.connectedBody = parent;
            _fixedJoint2D.connectedAnchor = transform.localPosition;
#endif
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _localEventProvider.RemoveListener<BoltMoveEvent, BoltMoveData>(OnBoltMove);
        }

        private void SetBolt(Rigidbody2D boltRigidbody, bool updateState)
        {
            _hingeJoint2D.connectedBody = boltRigidbody;
            if (updateState)
                _hingeJoint2D.enabled = boltRigidbody != null;
        }

        private void RemoveBolt() => SetBolt(null, true);

        public bool PointInHole(Vector3 point)
        {
            var pointDistance = Vector2.Distance(transform.position, point);
            return pointDistance <= GameLogicConstants.MAX_DISTANCE_TO_CONNECTION;
        }

        private void OnBoltMove(BoltMoveData boltMoveData)
        {
            if (!NeedBoltCheck(boltMoveData.BoltRigidbody, boltMoveData.SetScrewed))
                return;

            if (!PointInHole(boltMoveData.BoltPosition))
                return;

            if (boltMoveData.SetScrewed)
            {
                SetBolt(boltMoveData.BoltRigidbody, true);
                OnBoltInteractionEvent?.Invoke(boltMoveData.BoltCollider, true);
            }
            else
            {
                RemoveBolt();
                OnBoltInteractionEvent?.Invoke(boltMoveData.BoltCollider, false);
            }
        }

        private bool NeedBoltCheck(Rigidbody2D boltRigidbody, bool setScrewed)
        {
            if (_hingeJoint2D.enabled == false)
                return true;

            if (_hingeJoint2D.connectedBody == null)
                return true;

            bool isCurrentBolt = _hingeJoint2D.connectedBody == boltRigidbody;
            if (!isCurrentBolt)
                return false;

            if (setScrewed)
                return false;

            return true;
        }
    }
}