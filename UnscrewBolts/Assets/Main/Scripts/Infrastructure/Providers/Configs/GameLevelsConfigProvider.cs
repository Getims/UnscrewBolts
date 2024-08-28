using System.Linq;
using Scripts.Configs.Levels;
using Scripts.Core.Constants;
using Scripts.Infrastructure.Providers.Assets;

namespace Scripts.Infrastructure.Providers.Configs
{
    public interface IGameLevelsConfigProvider
    {
        GameLevelsConfig Config { get; }
        int LevelsCount { get; }
        LevelConfig GetLevel(int i);
    }

    public class GameGameLevelsConfigProvider : IGameLevelsConfigProvider
    {
        public GameLevelsConfig Config { get; }

        public int LevelsCount =>
            Config.LevelConfigs.Count();

        public GameGameLevelsConfigProvider(IAssetProvider assetProvider) =>
            Config = assetProvider.Load<GameLevelsConfig>(ConfigsPaths.LEVELS_CONFIG_PATH);

        public LevelConfig GetLevel(int i) =>
            Config.LevelConfigs.ElementAt(i);
    }
}