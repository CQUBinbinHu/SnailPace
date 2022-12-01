using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class FrailBuff : Buff
    {
        private const float ArmorMultiplier = 0.5f;

        public override void OnAddBuff(Character owner, float duration = -1)
        {
            base.OnAddBuff(owner, duration);
            Owner.AddArmorMultiplier(BuffType.Frail, ArmorMultiplier);
        }

        public override void OnRemoveBuff()
        {
            Owner.RemoveBuffArmorMultiplier(BuffType.Enhancement);
            base.OnRemoveBuff();
        }

        protected override void OnBuffTick(float deltaTime)
        {
            Owner.AddArmorMultiplier(BuffType.Enhancement, ArmorMultiplier);
        }

        public override void OnOverride(float duration)
        {
            ResetCoolDown(duration);
        }
    }
}