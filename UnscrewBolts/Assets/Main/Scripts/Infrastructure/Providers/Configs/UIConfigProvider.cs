using Scripts.Configs.UI;
using Scripts.Core.Constants;
using Scripts.Infrastructure.Providers.Assets;

namespace Scripts.Infrastructure.Providers.Configs
{
    public interface IUIConfigProvider
    {
        UIConfig Config { get; }
    }

    public class UIConfigProvider : IUIConfigProvider
    {
        public UIConfig Config { get; }

        public UIConfigProvider(IAssetProvider assetProvider) =>
            Config = assetProvider.Load<UIConfig>(ConfigsPaths.UI_CONFIG_PATH);
    }
}