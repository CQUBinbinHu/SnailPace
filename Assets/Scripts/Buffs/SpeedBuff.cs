using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class SpeedBuff : Buff
    {
        protected override void OnBuffTick(float deltaTime)
        {
        }

        public void AddSpeed(int value)
        {
            Owner.SpeedComponent.AddSpeed(value);
        }
    }
}