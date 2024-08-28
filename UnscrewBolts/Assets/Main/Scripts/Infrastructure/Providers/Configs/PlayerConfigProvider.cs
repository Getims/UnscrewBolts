using Scripts.Configs.Player;
using Scripts.Core.Constants;
using Scripts.Infrastructure.Providers.Assets;

namespace Scripts.Infrastructure.Providers.Configs
{
    public interface IPlayerConfigProvider
    {
        PlayerConfig Config { get; }
    }

    public class PlayerConfigProvider : IPlayerConfigProvider
    {
        public PlayerConfig Config { get; }

        public PlayerConfigProvider(IAssetProvider assetProvider) =>
            Config = assetProvider.Load<PlayerConfig>(ConfigsPaths.PLAYER_CONFIG_PATH);
    }
}