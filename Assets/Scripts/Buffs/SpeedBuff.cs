using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class SpeedBuff : Buff
    {
        protected override void OnBuffTick(float deltaTime)
        {
        }

        public override void OnOverride(float duration)
        {
        }

        public void AddSpeed(int value)
        {
            Owner.SpeedComponent.AddSpeed(value);
        }

        public override int GetLayers()
        {
            return Owner.SpeedComponent.Speed;
        }
    }
}