using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Core.Constants;
using Scripts.Data.Services;
using Scripts.Infrastructure.Actions;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.Infrastructure.Providers.Events;
using Scripts.UI.Base;
using Scripts.UI.Common;
using Scripts.UI.Common.FlyIcons;
using Scripts.UI.Common.UIAnimator.Main;
using Scripts.UI.GameMenu.Actions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI.GameMenu
{
    public class TopGamePanel : UIPanel
    {
        private readonly Queue<IAction> _moneyActions = new Queue<IAction>();

        [SerializeField]
        private Button _settingsButton;

        [SerializeField]
        private LevelTracker _levelTracker;

        [SerializeField]
        private StepTracker _stepTracker;

        [SerializeField]
        private MoneyCounter _moneyCounter;

        [SerializeField]
        private UIAnimator _showAnimator;

        [SerializeField]
        private UIAnimator _hideAnimator;

        private FlyIconAnimationHandler _flyIconSpawner;
        private LocalEventProvider _localEventProvider;
        private Coroutine _showMoneyCO;
        private IPlayerDataService _playerDataService;
        private int _coinsPerGameElement;

        public event Action OnSettingsClick;

        public new float FadeTime => base.FadeTime > _showAnimator.GetAnimatorWorkTime()
            ? base.FadeTime
            : _showAnimator.GetAnimatorWorkTime();

        private Vector3 MoneyIconPosition => _moneyCounter.IconPosition;

        [Inject]
        public void Construct(LocalEventProvider localEventProvider, IPlayerDataService playerDataService,
            IGlobalConfigProvider globalConfigProvider)
        {
            _playerDataService = playerDataService;
            _localEventProvider = localEventProvider;
            _coinsPerGameElement = globalConfigProvider.Config.CoinsPerGameElement;
        }

        public void Initialize(FlyIconAnimationHandler flyIconAnimationHandler)
        {
            _flyIconSpawner = flyIconAnimationHandler;
            _localEventProvider.AddListener<GameElementDestroyEvent, Vector3>(OnGameElementDestroy);
        }

        public override void Show()
        {
            base.Show();
            _hideAnimator.Stop();
            _showAnimator.Play();

            _moneyCounter.UpdateInfo();
            _levelTracker.UpdateInfo();
            _stepTracker.UpdateInfo();
        }

        public override void Hide()
        {
            base.Hide();
            _showAnimator.Stop();
            _hideAnimator.Play();
        }

        private void Start()
        {
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
            _localEventProvider?.RemoveListener<GameElementDestroyEvent, Vector3>(OnGameElementDestroy);

            if (_showMoneyCO != null)
                StopCoroutine(_showMoneyCO);
        }

        private void OnSettingsButtonClick() => OnSettingsClick?.Invoke();

        private void OnGameElementDestroy(Vector3 worldPosition)
        {
            _flyIconSpawner.StartAnimation(worldPosition, MoneyIconPosition);

            _moneyActions.Enqueue(new ShowRewardAction(worldPosition, MoneyIconPosition, _flyIconSpawner,
                _playerDataService, _coinsPerGameElement));

            if (_moneyActions.Count == 1)
                _showMoneyCO = StartCoroutine(PlayActionsQueue());
        }

        private IEnumerator PlayActionsQueue()
        {
            WaitForSeconds coinsSpawnDelay = new WaitForSeconds(GameLogicConstants.COINS_SPAWN_DELAY);
            while (_moneyActions.Count > 0)
            {
                IAction currentAction = _moneyActions.Dequeue();
                StartCoroutine(currentAction.Execute());
                yield return coinsSpawnDelay;
            }
        }
    }
}