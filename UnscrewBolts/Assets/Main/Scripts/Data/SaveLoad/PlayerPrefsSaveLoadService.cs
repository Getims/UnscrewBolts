using System;
using Scripts.Data.Core;
using UnityEngine;

namespace Scripts.Data.SaveLoad
{
    public class PlayerPrefsSaveLoadService : ISaveLoadService
    {
        public void TrySetData<TData>(string data, ref TData tData) where TData : GameData
        {
            if (string.IsNullOrWhiteSpace(data))
                return;

            tData ??= (TData) Activator.CreateInstance(tData.GetType());
            JsonUtility.FromJsonOverwrite(data, tData);
        }

        public void TryLoadData<TData>(ref TData tData) where TData : GameData
        {
            string dataKey = tData.DataKey;
            string data = PlayerPrefs.GetString(dataKey);

            if (string.IsNullOrEmpty(data))
                tData = (TData) Activator.CreateInstance(tData.GetType());
            else
                TrySetData(data, ref tData);
        }

        public void TrySaveData<TData>(TData tData) where TData : GameData
        {
            if (tData == null)
                return;

            string dataKey = tData.DataKey;
            string data = JsonUtility.ToJson(tData);

            PlayerPrefs.SetString(dataKey, data);
            PlayerPrefs.Save();
        }

        public void TryDeleteData<TData>(ref TData tData) where TData : GameData
        {
            tData ??= (TData) Activator.CreateInstance(tData.GetType());

            string dataKey = tData.DataKey;
            PlayerPrefs.DeleteKey(dataKey);

            TData newData = (TData) Activator.CreateInstance(tData.GetType());
            string data = JsonUtility.ToJson(newData);
            TrySetData(data, ref tData);
        }
    }
}