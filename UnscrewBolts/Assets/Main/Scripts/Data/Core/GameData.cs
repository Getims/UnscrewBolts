using System;
using UnityEngine;

namespace Scripts.Data.Core
{
    [Serializable]
    public abstract class GameData
    {
        public string DataKey => GetType().Name;

        public string GetDataPath() =>
            $"{Application.persistentDataPath}/{DataKey}.json";
    }
}