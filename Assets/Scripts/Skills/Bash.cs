using Core;
using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class Bash : SkillComponent
    {
        public int BaseDamage;
        private int Damage => (int)(Owner.GetBuffAtkMultiplier() * BaseDamage);

        public override void OnUse()
        {
            Target.Health.TakeDamage(Damage);
            Target.AddBuff(BuffType.Vulnerable);
        }

        public override void OnCancel()
        {
        }
    }
}