using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Core.Utilities
{
    public static partial class Utils
    {
        private const string DEBUG_POINT_NAME = "Debug point";
        private const string REWORK_POINT_NAME = "Rework point";
        private const string INFO_POINT_NAME = "Info point";

        private static readonly Color _debugLogColor = Color.red;
        private static readonly Color _reworkLogColor = Color.yellow;
        private static readonly Color _infoLogColor = Color.yellow;
        private static readonly Color _objectLogColor = Color.green;

        public static void DebugPoint() =>
            Log(DEBUG_POINT_NAME, null, LogType.Error, _debugLogColor);

        public static void DebugPoint(object log) =>
            Log(DEBUG_POINT_NAME, log, LogType.Error, _debugLogColor);

        public static void ReworkPoint() =>
            Log(REWORK_POINT_NAME, null, LogType.Warning, _reworkLogColor);

        public static void ReworkPoint(object log) =>
            Log(REWORK_POINT_NAME, log, LogType.Warning, _reworkLogColor);

        public static void InfoPoint(object log) =>
            Log(INFO_POINT_NAME, log, LogType.Log, _infoLogColor);

        public static void Log(string objectName, object log) =>
            Log(objectName, log, LogType.Log, _objectLogColor); 
        
        public static void Log(object objectName, object log) =>
            Log(objectName.GetType().Name, log, LogType.Log, _objectLogColor);

        private static void Log(string pointName, object log, LogType logType, Color logColor)
        {
            string logString = string.Empty;

            if (log == null)
                logString = $"<color=#{logColor.ToHexString()}>{pointName}</color>";
            else
                logString = $"<color=#{logColor.ToHexString()}>{pointName}:</color> {log?.ToString()}";

            switch (logType)
            {
                case LogType.Error:
                    Debug.LogError(logString);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(logString);
                    break;
                default:
                    Debug.Log(logString);
                    break;
            }
        }
    }
}