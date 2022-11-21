﻿using Core;
using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class Attack : SkillComponent
    {
        public int Atk;
        private int Damage => (int)(Owner.GetBuffAtkMultiplier() * Atk);

        public override void OnUse()
        {
            Target.Health.TakeDamage(Damage);
        }

        public override void OnCancel()
        {
        }
    }
}