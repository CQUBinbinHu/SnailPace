using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class WeekBuff : Buff
    {
        private const float WeekMultiplier = 0.5f;

        public override void OnAddBuff(Character owner, float duration = -1)
        {
            base.OnAddBuff(owner, duration);
            Owner.AddAtkMultiplier(BuffType.Week, WeekMultiplier);
        }

        public override void OnRemoveBuff()
        {
            Owner.RemoveBuffAtkMultiplier(BuffType.Week);
            base.OnRemoveBuff();
        }

        protected override void OnBuffTick(float deltaTime)
        {
            Owner.AddAtkMultiplier(BuffType.Week, WeekMultiplier);
        }

        public override void OnOverride(float duration)
        {
            base.OnOverride(duration);
            ResetCoolDown(duration);
        }
    }
}