using System;
using Core;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace HeroPerform
{
    public class Shield : SkillComponent
    {
        [SerializeField] public int Armor = 6;

        public override void OnUse()
        {
            if (!TryGetPermission())
            {
                return;
            }

            ArmorBuff armorBuff = (Owner.AddBuff<ArmorBuff>(BuffType.Armor)) as ArmorBuff;
            if (armorBuff != null)
            {
                armorBuff.AddArmor(Armor);
            }

            base.OnUse();
        }
    }
}