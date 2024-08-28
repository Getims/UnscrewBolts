using Scripts.GameLogic.GameFlow;
using Scripts.Infrastructure.Bootstrap;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.Infrastructure.Providers.Events;
using Scripts.UI;
using UnityEngine;
using Zenject;

namespace Scripts.Infrastructure.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private UIContainerProvider _uiContainerProvider;

        [SerializeField]
        private GameFlowController _gameFlowController;

        private GameSceneBootstrapper _bootstrapper;

        public override void InstallBindings()
        {
            BindConfigProviders();
            BindUIContainerProvider();
            BindEventProvider();
            BindGameFlow();

            CreateSceneBootstrapper();
        }

        private void OnDestroy()
        {
            _bootstrapper.OnDestroy();
        }

        private void BindConfigProviders()
        {
            Container.BindInterfacesTo<PlayerConfigProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<GameGameLevelsConfigProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<UIConfigProvider>().AsSingle().NonLazy();
        }

        private void BindEventProvider() =>
            Container.Bind<LocalEventProvider>().AsSingle().NonLazy();

        private void BindUIContainerProvider() =>
            Container.BindInstance(_uiContainerProvider).AsSingle().NonLazy();

        private void BindGameFlow()
        {
            Container.Bind<IGameFlowProvider>().FromInstance(_gameFlowController).AsSingle().NonLazy();
            Container.Bind<IBoltMediator>().FromInstance(_gameFlowController).AsSingle().NonLazy();
        }

        private void CreateSceneBootstrapper()
        {
            _bootstrapper = Container.Instantiate<GameSceneBootstrapper>();
            _bootstrapper.Initialize();
        }
    }
}