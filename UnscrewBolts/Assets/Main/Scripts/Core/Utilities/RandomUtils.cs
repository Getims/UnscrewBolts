using UnityEngine;

namespace Scripts.Core.Utilities
{
    public static partial class Utils
    {
        public static int RandomValue(int value1, int value2)
        {
            if (UnityEngine.Random.value > 0.5f)
                return value1;
            else
                return value2;
        }

        public static int RandomValue(int[] values) =>
            values[UnityEngine.Random.Range(0, values.Length)];

        public static float RandomValue(float[] values) =>
            values[UnityEngine.Random.Range(0, values.Length)];

        public static Vector3 RandomValue(Vector3[] values) =>
            values[UnityEngine.Random.Range(0, values.Length)];

        public static float RandomValue(float value1, float value2)
        {
            if (UnityEngine.Random.value > 0.5f)
                return value1;
            else
                return value2;
        }

        public static bool RandomBool()
        {
            if (UnityEngine.Random.value > 0.5f)
                return true;
            else
                return false;
        }

        public static int RandomValueExcluding(int min, int max, int exclude)
        {
            int randomNumber = UnityEngine.Random.Range(min, max);
            if (randomNumber == exclude)
                randomNumber = (randomNumber + 1) % max;

            return randomNumber;
        }
    }
}