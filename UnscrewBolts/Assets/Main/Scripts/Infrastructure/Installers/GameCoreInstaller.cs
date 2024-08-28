using Scripts.Data;
using Scripts.Data.Core;
using Scripts.Data.Services;
using Scripts.GameLogic.Sound;
using Scripts.Infrastructure.Bootstrap;
using Scripts.Infrastructure.Providers.Assets;
using Scripts.Infrastructure.Providers.Configs;
using Scripts.Infrastructure.Providers.Events;
using Scripts.Infrastructure.ScenesManager;
using Scripts.Infrastructure.StateMachines;
using Scripts.UI.Factories;
using Scripts.UI.Loadscreen;
using UnityEngine;
using Zenject;

namespace Scripts.Infrastructure.Installers
{
    public class GameCoreInstaller : MonoInstaller
    {
        [SerializeField]
        private SoundPlayer _soundPlayer;

        [SerializeField]
        private GameCoreBootstrapper _gameCoreBootstrapper;

        [SerializeField]
        private LoadingPanel _loadingPanel;

        public override void InstallBindings()
        {
            BindProviders();
            BindDatabase();
            BindServices();

            BindSoundPlayer();

            BindFactories();
            BindGameStateMachine();

            BindGameBootstrapper();
            BindSceneLoader();
        }

        private void BindProviders()
        {
            Container.BindInterfacesTo<AssetProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<GlobalConfigProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<SoundConfigProvider>().AsSingle().NonLazy();
            Container.Bind<GlobalEventProvider>().AsSingle().NonLazy();
        }

        private void BindDatabase()
        {
            IDatabase database = new GameDatabase();
            database.Initialize();
            Container.BindInstance(database).AsSingle().NonLazy();

#if UNITY_EDITOR
            DataEditorMediator.SetDatabase(database);
#endif
        }

        private void BindServices()
        {
            Container.BindInterfacesTo<PlayerDataService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<ProgressDataService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<ThemeDataService>().AsSingle().NonLazy();
        }

        private void BindFactories()
        {
            Container.Bind<StateMachineFactory>().AsSingle().NonLazy();
            Container.BindInterfacesTo<UIElementFactory>().AsSingle().NonLazy();
        }

        private void BindGameStateMachine() =>
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle().NonLazy();

        private void BindGameBootstrapper() =>
            Container.Bind<ICoroutineRunner>().FromInstance(_gameCoreBootstrapper).AsSingle().NonLazy();

        private void BindSceneLoader()
        {
            Container.Bind<LoadingPanel>().FromInstance(_loadingPanel).AsSingle().NonLazy();
            Container.BindInterfacesTo<SceneLoader>().AsSingle().NonLazy();
        }

        private void BindSoundPlayer()
        {
            Container.Bind<SoundPlayer>().FromInstance(_soundPlayer).AsSingle().NonLazy();
            Container.BindInterfacesTo<SoundService>().AsSingle().NonLazy();
        }
    }
}