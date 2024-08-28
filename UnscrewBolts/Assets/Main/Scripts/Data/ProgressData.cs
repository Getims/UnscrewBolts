using System;
using System.Collections.Generic;
using Scripts.Data.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Data
{
    [Serializable]
    public class ProgressData : GameData
    {
        [SerializeField]
        [ListDrawerSettings(ListElementLabelName = "ElementName")]
        public List<LevelData> Levels = new List<LevelData>();

        [SerializeField]
        public int CurrentLevel = 0;
        
        [SerializeField]
        public int CurrentLevelStep = 0;

        [SerializeField]
        public bool HasReward = false;

        [SerializeField]
        public bool IsTutorialComplete = false;

        [Button]
        private void LockAllLevels()
        {
            foreach (LevelData level in Levels)
            {
                level.SetLockState(false);
                level.SetCompleteState(false);
            }

            Levels[0].SetLockState(true);
            CurrentLevel = 0;
        }

        [Button]
        private void UnlockAllLevels()
        {
            foreach (LevelData level in Levels)
                level.SetLockState(true);
        }
    }
}