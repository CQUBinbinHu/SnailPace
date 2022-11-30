using Core;
using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class RedBook : SkillComponent
    {
        [SerializeField] private float Duration = 3;

        public override void OnUse()
        {
            if (!TryGetPermission())
            {
                return;
            }

            Owner.AddBuff(BuffType.Enhancement, Duration);
            base.OnUse();
        }

        public override void OnCancel()
        {
            base.OnCancel();
        }
    }
}