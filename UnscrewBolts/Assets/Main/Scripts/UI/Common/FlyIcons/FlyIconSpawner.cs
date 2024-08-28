using Scripts.GameLogic.Sound;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Scripts.UI.Common.FlyIcons
{
    public class FlyIconSpawner : MonoBehaviour
    {
        [SerializeField, Required]
        private IconsPool _iconsPool;

        private ISoundService _soundService;

        [Inject]
        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;
        }

        public void SpawnIcon(Sprite icon, Vector3 startPosition, Vector3 targetPosition,
            FlySettings flySettings = null)
        {
            FlyIcon flyIcon = _iconsPool.Spawn();
            flyIcon.Setup(icon, flySettings, PlaySound, () => OnFlyComplete(flyIcon));
            flyIcon.PlayAnimation(startPosition, targetPosition);
        }

        private void Start() =>
            _iconsPool.Initialize();

        private void PlaySound() =>
            _soundService.PlayCoinsDropSound();

        private void OnFlyComplete(FlyIcon flyIcon) =>
            _iconsPool.Despawn(flyIcon);
    }
}