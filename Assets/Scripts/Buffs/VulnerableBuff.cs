using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class VulnerableBuff : Buff
    {
        private float DamageMultiplier = 1.5f;

        public override void OnAddBuff()
        {
            base.OnAddBuff();
            Owner.AddDamageMultiplier(BuffType.Vulnerable, DamageMultiplier);
        }

        public override void OnRemoveBuff()
        {
            Owner.RemoveBuffDamageMultiplier(BuffType.Vulnerable);
        }

        protected override void OnBuffTick(float deltaTime)
        {
            Owner.AddDamageMultiplier(BuffType.Vulnerable, DamageMultiplier);
        }
    }
}