using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class WeekBuff : Buff
    {
        private const float WeekMultiplier = 0.5f;

        public override void OnAddBuff()
        {
            base.OnAddBuff();
            Owner.AddAtkMultiplier(BuffType.Week, WeekMultiplier);
        }

        public override void OnRemoveBuff()
        {
            Owner.RemoveBuffAtkMultiplier(BuffType.Week);
        }

        protected override void OnBuffTick(float deltaTime)
        {
            Owner.AddAtkMultiplier(BuffType.Week, WeekMultiplier);
        }
    }
}