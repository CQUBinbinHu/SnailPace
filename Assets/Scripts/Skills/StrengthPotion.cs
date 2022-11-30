using Core;
using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class StrengthPotion : SkillComponent
    {
        [SerializeField] private int Strength = 1;

        public override void OnUse()
        {
            StrengthBuff buff = (Owner.AddBuff<StrengthBuff>(BuffType.Strength)) as StrengthBuff;
            if (buff != null)
            {
                buff.AddStrength(Strength);
            }

            base.OnUse();
        }
    }
}