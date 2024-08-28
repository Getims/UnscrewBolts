using System.Collections;
using Scripts.GameLogic.Sound;
using Scripts.UI.Base;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Scripts.UI.Common.FlyIcons
{
    public class FlyIconAnimationHandler : UIPanel
    {
        [Title("References")]
        [SerializeField, Required]
        private Sprite _iconSprite;

        [SerializeField, Required]
        private FlyIconSpawner _flyIconSpawner;

        [Title("Settings")]
        [SerializeField, MinMaxSlider(minValue: 0, maxValue: 20, showFields: true)]
        private Vector2Int _iconsNumber;

        [SerializeField, MinMaxSlider(minValue: 0, maxValue: 1, showFields: true)]
        private Vector2 _iconsSpawnDelay;

        [SerializeField]
        private bool _customFlySettings;

        [SerializeField, Required]
        [ShowIf(nameof(_customFlySettings))]
        private FlySettings _flySettings;

        [SerializeField]
        private float _startPointRandomOffset;

        private Coroutine _animationCO;

        private ISoundService _soundService;

        [Inject]
        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;
        }

        [Button]
        public void StartAnimation(Vector3 startPosition, Vector3 targetPosition)
        {
            _soundService.PlayCoinsFlySound();
            FlySettings flySettings = _customFlySettings ? _flySettings : null;
            int iconsNumber = Random.Range(_iconsNumber.x, _iconsNumber.y + 1);

            if (_animationCO != null)
                StopCoroutine(_animationCO);

            _animationCO = StartCoroutine(SpawnIcons(startPosition, targetPosition, iconsNumber, flySettings));
        }

        public float GetAnimationTime() => _flySettings.GetAnimationTime();

        protected override void OnDestroy()
        {
            if (_animationCO != null)
                StopCoroutine(_animationCO);
        }

        private IEnumerator SpawnIcons(Vector3 startPosition, Vector3 targetPosition, int iconsNumber,
            FlySettings flySettings)
        {
            Vector2 startPositionRandomOffset = Vector2.zero;
            for (int i = 0; i < iconsNumber; i++)
            {
                if (i % 2 == 0)
                {
                    startPositionRandomOffset = Random.insideUnitCircle;
                    startPositionRandomOffset *= _startPointRandomOffset;
                }
                else
                {
                    startPositionRandomOffset = -startPositionRandomOffset;
                }

                startPosition += (Vector3) startPositionRandomOffset;

                _flyIconSpawner.SpawnIcon(_iconSprite, startPosition, targetPosition, flySettings);

                float delay = Random.Range(_iconsSpawnDelay.x, _iconsSpawnDelay.y);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}