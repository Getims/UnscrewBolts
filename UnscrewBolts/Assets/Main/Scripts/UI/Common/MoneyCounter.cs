using Scripts.Data.Services;
using Scripts.Infrastructure.Providers.Events;
using TMPro;
using UnityEngine;
using Zenject;

namespace Scripts.UI.Common
{
    public class MoneyCounter : MonoBehaviour
    {
        [SerializeField]
        private Transform _moneyIcon;

        [SerializeField]
        private TextMeshProUGUI _valueTMP;

        private IPlayerDataService _playerDataService;
        private GlobalEventProvider _globalEventProvider;

        public Vector3 IconPosition => _moneyIcon.position;

        [Inject]
        public void Construct(IPlayerDataService playerDataService, GlobalEventProvider globalEventProvider)
        {
            _globalEventProvider = globalEventProvider;
            _playerDataService = playerDataService;
            _globalEventProvider.AddListener<MoneyChangedEvent, int>(UpdateInfo);
        }

        public void UpdateInfo()
        {
            if (_playerDataService == null)
                return;

            UpdateInfo(_playerDataService.Money);
        }

        private void Start() =>
            UpdateInfo();

        private void OnDestroy() =>
            _globalEventProvider?.RemoveListener<MoneyChangedEvent, int>(UpdateInfo);

        private void UpdateInfo(int moneyCount) =>
            _valueTMP.text = moneyCount.ToString();
    }
}