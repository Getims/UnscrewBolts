using System;
using Scripts.Core.Enums;

namespace Scripts.Infrastructure.ScenesManager
{
    public interface ISceneLoader
    {
        void Load(string name, Action onLoaded = null);
        void Load(Scenes scene, Action onLoaded = null);
    }
}