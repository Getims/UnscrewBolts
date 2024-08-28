using Scripts.Configs;
using Scripts.Core.Constants;
using Scripts.Infrastructure.Providers.Assets;

namespace Scripts.Infrastructure.Providers.Configs
{
    public interface IGlobalConfigProvider
    {
        GlobalConfig Config { get; }
    }

    public class GlobalConfigProvider : IGlobalConfigProvider
    {
        public GlobalConfig Config { get; }

        public GlobalConfigProvider(IAssetProvider assetProvider) =>
            Config = assetProvider.Load<GlobalConfig>(ConfigsPaths.GLOBAL_CONFIG_PATH);
    }
}