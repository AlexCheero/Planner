using UnityEngine;

namespace GOAP
{
    internal static class PlannerUtils
    {
        public static bool ValueBetween(float val, float indication, float disp)
        {
            return val >= indication - disp && val <= indication + disp;
        }

        public static float PythagoreanTheorem(float val1, float val2)
        {
            return Mathf.Sqrt(Mathf.Pow(val1, 2) + Mathf.Pow(val2, 2));
        }
    }
}