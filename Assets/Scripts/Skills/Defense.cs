using System;
using Core;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace HeroPerform
{
    public class Defense : SkillComponent
    {
        [SerializeField] private float Duration = 2;
        [SerializeField] public int Armor = 6;
        private float _timer;

        public override void OnUse()
        {
            Owner.Health.AddArmor(Armor);
            _timer = 0;
        }

        public override void OnCancel()
        {
        }

        private void FixedUpdate()
        {
            if (_timer < Duration)
            {
                _timer += Time.fixedDeltaTime;
            }
            else
            {
                _timer = 0;
                Owner.Health.RemoveArmor(Armor);
            }
        }
    }
}