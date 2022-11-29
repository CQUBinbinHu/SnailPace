using UnityEngine;

namespace DefaultNamespace
{
    public static class SpeedTransition
    {
        private const float MeanSpeed = 3;
        private const float Multiplier = 0.06f;
        private const float Scale = 3;

        public static float GetSpeedMultiplier(int spd)
        {
            return Multiplier * spd / (1 + Multiplier * Mathf.Abs(spd));
        }

        public static float GetMoveSpeed(int spd)
        {
            return MeanSpeed + Scale * GetSpeedMultiplier(spd);
        }
    }
}