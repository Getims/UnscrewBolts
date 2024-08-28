using Scripts.Data.Services;
using Scripts.Infrastructure.Providers.Events;
using UnityEngine;
using Zenject;

namespace Scripts.UI.GameMenu
{
    internal class StepTracker : MonoBehaviour
    {
        [SerializeField]
        private Step _step1;

        [SerializeField]
        private Step _step2;

        private IProgressDataService _progressDataService;
        private GlobalEventProvider _globalEventProvider;

        [Inject]
        public void Construct(IProgressDataService progressDataService, GlobalEventProvider globalEventProvider)
        {
            _globalEventProvider = globalEventProvider;
            _progressDataService = progressDataService;
            _globalEventProvider.AddListener<LevelStepSwitchEvent, int>(UpdateInfo);
        }

        public void UpdateInfo()
        {
            if (_progressDataService == null)
                return;

            UpdateInfo(_progressDataService.CurrentLevelStep);
        }

        private void Start() =>
            UpdateInfo();

        private void OnDestroy() =>
            _globalEventProvider?.RemoveListener<LevelStepSwitchEvent, int>(UpdateInfo);

        private void UpdateInfo(int step)
        {
            switch (step)
            {
                case 0:
                    _step1.Show();
                    _step2.Hide();
                    break;
                case 1:
                    _step1.Show();
                    _step2.Show();
                    break;
            }
        }
    }
}