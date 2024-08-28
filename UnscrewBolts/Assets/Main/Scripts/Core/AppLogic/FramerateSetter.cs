using UnityEngine;

namespace Scripts.Core.AppLogic
{
    public static class FramerateSetter
    {
        private const int DEFAULT_FPS = 60;

        public static void SetDefaultFramerate() =>
            SetTargetFramerate(DEFAULT_FPS);

        public static void SetTargetFramerate(int framerate)
        {
            Application.targetFrameRate = framerate;
            Debug.Log("Set target framerate to " + framerate);
        }
    }
}