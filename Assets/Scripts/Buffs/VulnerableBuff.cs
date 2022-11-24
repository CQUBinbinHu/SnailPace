﻿using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class VulnerableBuff : Buff
    {
        private float DamageMultiplier = 1.5f;

        public override void OnAddBuff(Character owner)
        {
            base.OnAddBuff(owner);
            Owner.AddDamageMultiplier(BuffType.Vulnerable, DamageMultiplier);
        }

        protected override void OnRemoveBuff()
        {
            Owner.RemoveBuffDamageMultiplier(BuffType.Vulnerable);
            base.OnRemoveBuff();
        }

        protected override void OnBuffTick(float deltaTime)
        {
            Owner.AddDamageMultiplier(BuffType.Vulnerable, DamageMultiplier);
        }
    }
}