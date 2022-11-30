using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnhancementBuff : Buff
    {
        private const float AtkMultiplier = 2f;

        public override void OnAddBuff(Character owner, float duration = -1)
        {
            base.OnAddBuff(owner, duration);
            Owner.AddAtkMultiplier(BuffType.Enhancement, AtkMultiplier);
        }

        public override void OnRemoveBuff()
        {
            Owner.RemoveBuffAtkMultiplier(BuffType.Enhancement);
            base.OnRemoveBuff();
        }

        protected override void OnBuffTick(float deltaTime)
        {
            Owner.AddAtkMultiplier(BuffType.Enhancement, AtkMultiplier);
        }

        public override void OnOverride(float duration)
        {
            ResetCoolDown(duration);
        }
    }
}