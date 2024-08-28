using Scripts.Infrastructure.Bootstrap;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.Infrastructure.Providers.Events;
using Scripts.UI;
using UnityEngine;
using Zenject;

namespace Scripts.Infrastructure.Installers
{
    public class MainMenuSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private UIContainerProvider _uiContainerProvider;

        public override void InstallBindings()
        {
            BindEventProvider();
            BindUIContainerProvider();
            BindConfigProviders();

            CreateSceneBootstrapper();
        }

        private void BindEventProvider() =>
            Container.Bind<LocalEventProvider>().AsSingle().NonLazy();

        private void BindUIContainerProvider() =>
            Container.BindInstance(_uiContainerProvider).AsSingle().NonLazy();

        private void BindConfigProviders()
        {
            Container.BindInterfacesTo<PlayerConfigProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<GameGameLevelsConfigProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<UIConfigProvider>().AsSingle().NonLazy();
        }

        private void CreateSceneBootstrapper()
        {
            MainMenuSceneBootstrapper bootstrapper = Container.Instantiate<MainMenuSceneBootstrapper>();
            bootstrapper.Initialize();
        }
    }
}