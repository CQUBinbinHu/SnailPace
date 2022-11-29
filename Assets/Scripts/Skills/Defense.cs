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
            base.OnUse();
        }

        public override void OnCancel()
        {
            base.OnCancel();
        }

        private void FixedUpdate()
        {
        }
    }
}