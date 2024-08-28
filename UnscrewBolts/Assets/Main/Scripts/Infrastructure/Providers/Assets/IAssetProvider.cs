using UnityEngine;

namespace Scripts.Infrastructure.Providers.Assets
{
    public interface IAssetProvider
    {
        T Load<T>(string path) where T : Object;
    }
}