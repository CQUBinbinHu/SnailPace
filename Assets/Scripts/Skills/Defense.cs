using System;
using Core;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace HeroPerform
{
    public class Defense : SkillComponent
    {
        [SerializeField] public int Armor = 6;
        public override void OnUse()
        {
            if (!TryGetPermission())
            {
                return;
            }

            Owner.Health.AddArmor(Armor);
        }

        public override void OnCancel()
        {
        }

        private void FixedUpdate()
        {
        }
    }
}