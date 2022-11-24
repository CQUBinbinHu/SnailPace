using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class WeekBuff : Buff
    {
        private const float WeekMultiplier = 0.5f;

        public override void OnAddBuff(Character owner)
        {
            base.OnAddBuff(owner);
            Owner.AddAtkMultiplier(BuffType.Week, WeekMultiplier);
        }

        protected override void OnRemoveBuff()
        {
            Owner.RemoveBuffAtkMultiplier(BuffType.Week);
            base.OnRemoveBuff();
        }

        protected override void OnBuffTick(float deltaTime)
        {
            Owner.AddAtkMultiplier(BuffType.Week, WeekMultiplier);
        }
    }
}