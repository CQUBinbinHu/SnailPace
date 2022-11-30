using UnityEngine;

namespace DefaultNamespace
{
    public static class SpeedTransition
    {
        public static float MaxMoveSpeed = 10f;
        public static float MinMoveSpeed = 0f;
        public static float MaxEnergyRecovery = 20;
        public static float MinEnergyRecovery = 0;
        private const float Multiplier = 0.5f;

        private static float GetSpeedMultiplier(int spd)
        {
            return Multiplier * spd / (1 + Multiplier * Mathf.Abs(spd));
        }

        public static float GetMoveSpeed(int spd)
        {
            return 0.5f * (MaxMoveSpeed + MinMoveSpeed + (MaxMoveSpeed - MinMoveSpeed) * GetSpeedMultiplier(spd));
        }

        public static float GetEnergyRecovery(int spd)
        {
            return 0.5f * (MaxEnergyRecovery + MinEnergyRecovery + (MaxEnergyRecovery - MinEnergyRecovery) * GetSpeedMultiplier(spd));
        }
    }
}