using System;
using System.IO;
using Scripts.Data.Core;
using UnityEngine;

namespace Scripts.Data.SaveLoad
{
    public class JsonSaveLoadService : ISaveLoadService
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
            string path = tData.GetDataPath();
            bool isFileExists = File.Exists(path);

            if (!isFileExists)
            {
                tData = (TData) Activator.CreateInstance(tData.GetType());
                string json = JsonUtility.ToJson(tData);
                File.WriteAllText(path, json);
            }

            string data = File.ReadAllText(path);
            TrySetData(data, ref tData);
        }

        public void TrySaveData<TData>(TData tData) where TData : GameData
        {
            if (tData == null)
                return;

            string path = tData.GetDataPath();
            string data = JsonUtility.ToJson(tData);

            File.WriteAllText(path, data);
        }

        public void TryDeleteData<TData>(ref TData tData) where TData : GameData
        {
            tData ??= (TData) Activator.CreateInstance(tData.GetType());

            string path = tData.GetDataPath();

            if (File.Exists(path))
                File.Delete(path);

            var newT = (TData) Activator.CreateInstance(tData.GetType());
            string data = JsonUtility.ToJson(newT);

            TrySetData(data, ref tData);
        }
    }
}