using Scripts.GameLogic.Levels.Anchors;
using Scripts.GameLogic.Sound;
using Scripts.Infrastructure.Providers.Events;
using UnityEngine;
using Zenject;

namespace Scripts.GameLogic.Levels.Boosters
{
    internal class BoosterController : MonoBehaviour
    {
        [SerializeField]
        private Bomb _bomb;

        private LocalEventProvider _localEventProvider;
        private ISoundService _soundService;

        [Inject]
        public void Construct(LocalEventProvider localEventProvider, ISoundService soundService)
        {
            _soundService = soundService;
            _localEventProvider = localEventProvider;
        }

        public void Initialize()
        {
            _localEventProvider.AddListener<BombBoosterUseEvent>(CreateBomb);
        }

        public void Hide()
        {
            _bomb.Hide();
        }

        private void OnDestroy()
        {
            _localEventProvider.RemoveListener<BombBoosterUseEvent>(CreateBomb);
        }

        private void CreateBomb()
        {
            _bomb.Show();
            _bomb.Initialize();
            _bomb.OnBombPlacedEvent += BombExplosion;
        }

        private void BombExplosion()
        {
            _bomb.OnBombPlacedEvent -= BombExplosion;
            _soundService.PlayBombExplosion();

            Collider2D[] intersectingColliders =
                Physics2D.OverlapCircleAll(_bomb.transform.position, _bomb.ExplosionRadius);

            foreach (var collider in intersectingColliders)
            {
                if (collider.TryGetComponent(out IAnchorStateSetter anchor))
                    anchor.ReactToBomb();
            }

            _localEventProvider.Invoke<BombExplodeEvent>();
        }
    }
}