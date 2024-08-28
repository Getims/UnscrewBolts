using System;
using Scripts.GameLogic.Levels;
using UnityEngine;

namespace Scripts.Configs.Levels
{
    [Serializable]
    public class StepConfig
    {
        [SerializeField]
        private LevelController _levelPrefab;

        [SerializeField, Min(30)]
        private int _time = 180;

        public LevelController LevelPrefab => _levelPrefab;
        public int Time => _time;
    }
}