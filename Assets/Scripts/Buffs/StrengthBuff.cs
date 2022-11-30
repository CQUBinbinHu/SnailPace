using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class StrengthBuff : Buff
    {
        protected override void OnBuffTick(float deltaTime)
        {
        }

        public override void OnOverride(float duration)
        {
        }

        public void AddStrength(int value)
        {
            Owner.StrengthComponent.AddStrength(value);
        }

        public override int GetLayers()
        {
            return Owner.StrengthComponent.Strength;
        }
    }
}