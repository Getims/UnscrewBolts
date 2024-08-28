using Scripts.GameLogic.GameFlow;
using Scripts.GameLogic.Levels.Bolts;
using Scripts.GameLogic.Levels.Core;
using Scripts.GameLogic.Levels.GameElements;
using Scripts.GameLogic.Sound;
using Scripts.Infrastructure.Providers.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Scripts.GameLogic.Levels.Anchors
{
    public class AnchorPoint : VisualElement, IAnchorStateSetter
    {
        private const float SCREW_BOUNDS = 0.85f;

        [HideInInspector]
        public bool IsMagnetMod;

        [SerializeField]
        private CircleCollider2D _circleCollider;

        [SerializeField]
        private Bolt _bolt;

        [SerializeField, ReadOnly]
        private bool _hasBoltOnStart = true;

        [SerializeField]
        private GameObject _lock;

        [SerializeField, ReadOnly]
        private bool _isLocked = false;

        [SerializeField]
        private ParticleSystem _boltBlockPS;

        private IBoltMediator _boltMediator;
        private ISoundService _soundService;
        private LocalEventProvider _localEventProvider;
        private bool _isUnscrewBoosterState;

        public Vector3 Position => transform.position;
        public Bolt Bolt => _bolt;

        [Inject]
        public void Construct(IBoltMediator boltMediator, ISoundService soundService,
            LocalEventProvider localEventProvider)
        {
            _localEventProvider = localEventProvider;
            _soundService = soundService;
            _boltMediator = boltMediator;
        }

        public override void Initialize()
        {
            if (!_hasBoltOnStart)
            {
                if (_bolt.gameObject.activeSelf)
                    _bolt.gameObject.SetActive(false);
                _bolt = null;
            }
            else
                _bolt.Initialize(this, RemoveBolt);

            _localEventProvider.AddListener<UnscrewBoosterUseEvent, bool>(OnUnscrewBoosterUse);
        }

        public override void Show(bool instant = false)
        {
            base.Show(instant);
            if (_isLocked)
                _lock.SetActive(true);
        }

        public override void Hide(bool instant = false)
        {
            base.Hide(instant);
            _lock.SetActive(false);
        }

        public void Unlock()
        {
            _isLocked = false;
            _lock.SetActive(_isLocked);
        }

        public void ReactToBomb()
        {
            if (!_isLocked && _bolt != null)
            {
                _bolt.RemoveFromScene();
                _localEventProvider.Invoke<RemoveBoltEvent>();
            }
        }

        public void OnSelect()
        {
            if (TryUseBooster()) return;
            if (LockedFromStart()) return;

            Bolt selectedBolt = _boltMediator.GetSelectedBolt();

            if (selectedBolt == null)
                TryToUnscrewBolt();
            else
                TryToScrewBolt(selectedBolt);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _localEventProvider.RemoveListener<UnscrewBoosterUseEvent, bool>(OnUnscrewBoosterUse);
        }

        private bool LockedFromStart()
        {
            if (!_isLocked)
                return false;

            _localEventProvider.Invoke<TryToUnlockAnchorEvent, IAnchorStateSetter>(this);
            return true;
        }

        private void RemoveBolt() => _bolt = null;

        private void SetBolt(Bolt bolt) => _bolt = bolt;

        private bool TryUseBooster()
        {
            if (!_isUnscrewBoosterState)
                return false;

            if (!_isLocked && _bolt != null)
            {
                _bolt.RemoveFromScene();
                _localEventProvider.Invoke<RemoveBoltEvent>();
            }

            return true;
        }

        private void TryToScrewBolt(Bolt selectedBolt)
        {
            _localEventProvider.Invoke<AnchorClickEvent, AnchorClickData>
                (new AnchorClickData(transform.position, true));

            if (_bolt == null)
            {
                if (!IsBlocked())
                    selectedBolt.MoveTo(this, RemoveBolt, SetBolt);

                selectedBolt.SetScrewed();
                _boltMediator.SetSelectedBolt(null);
                return;
            }

            if (_bolt == selectedBolt)
            {
                _bolt.SetScrewed();
                _boltMediator.SetSelectedBolt(null);
            }
            else
            {
                selectedBolt.SetScrewed();
                _bolt.SetUnscrewed();
                _boltMediator.SetSelectedBolt(_bolt);
            }
        }

        private void TryToUnscrewBolt()
        {
            if (_bolt == null)
                return;

            _bolt.SetUnscrewed();
            _boltMediator.SetSelectedBolt(_bolt);
            _localEventProvider.Invoke<AnchorClickEvent, AnchorClickData>
                (new AnchorClickData(transform.position, false));
        }

        private bool IsBlocked()
        {
            Collider2D[] intersectingColliders =
                Physics2D.OverlapCircleAll(transform.position, _circleCollider.bounds.extents.x * SCREW_BOUNDS);

            foreach (var collider in intersectingColliders)
            {
                if (collider == _circleCollider)
                    continue;

                if (!collider.TryGetComponent(out GameElement gameElement))
                {
                    if (collider.TryGetComponent(out GameElementLink gameElementLink))
                        gameElement = gameElementLink.GameElement;
                    else
                        continue;
                }

                if (!gameElement.HasHoleInPoint(transform.position))
                {
                    _soundService.PlayBoltCantPlaceSound();
                    _boltBlockPS.Play();
                    return true;
                }
            }

            return false;
        }

        private void OnUnscrewBoosterUse(bool isUsed)
        {
            _isUnscrewBoosterState = isUsed;
            if (_bolt == null)
                return;

            _bolt.SetGlow(isUsed);
        }

        //-------------------------------------------------------------------------------

        #region EDITOR_METHODS

        [Button]
        private void SwitchBolt()
        {
#if UNITY_EDITOR
            if (_isLocked)
            {
                Debug.LogWarning("Can't switch bolt: anchor is locked!");
                return;
            }

            bool enabled = _bolt.gameObject.activeInHierarchy;
            _hasBoltOnStart = !enabled;
            _bolt.gameObject.SetActive(_hasBoltOnStart);
#endif
        }

        [Button]
        private void SwitchLock()
        {
#if UNITY_EDITOR
            bool isLocked = _lock.activeInHierarchy;
            _isLocked = !isLocked;
            _lock.SetActive(_isLocked);
            if (_isLocked)
            {
                _hasBoltOnStart = false;
                _bolt.gameObject.SetActive(_hasBoltOnStart);
            }
#endif
        }

        [Button("Magnet to hole")]
        private void SetMagnetMode()
        {
#if UNITY_EDITOR
            IsMagnetMod = !IsMagnetMod;
#endif
        }

        #endregion
    }
}