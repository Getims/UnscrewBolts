using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Core.Utilities
{
    public static partial class Utils
    {
        public static void PerformWithDelay(MonoBehaviour obj, float delay, Action func)
        {
            if (func == null)
                return;

            obj.StartCoroutine(Perform(delay, func));
        }

        private static IEnumerator Perform(float seconds, Action func)
        {
            yield return new WaitForSeconds(seconds);
            func();
        }
    }
}