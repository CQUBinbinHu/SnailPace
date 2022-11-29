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
            if (!Target || Target.IsDead)
            {
                return;
            }

            if (!TryGetPermission())
            {
                return;
            }

            Target.Health.TakeDamage(Damage);
            Target.AddBuff(BuffType.Vulnerable);
            base.OnUse();
        }

        public override void OnCancel()
        {
            base.OnCancel();
        }
    }
}