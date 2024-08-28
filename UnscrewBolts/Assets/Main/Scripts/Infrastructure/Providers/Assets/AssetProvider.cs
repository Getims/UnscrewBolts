using UnityEngine;

namespace Scripts.Infrastructure.Providers.Assets
{
    public class AssetProvider : IAssetProvider
    {
        public T Load<T>(string path) where T : Object =>
            Resources.Load<T>(path);
    }
}