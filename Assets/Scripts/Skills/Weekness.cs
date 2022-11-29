using Core;
using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class Weekness : SkillComponent
    {
        public float Duration = 3;

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

            Target.AddBuff(BuffType.Week, Duration);
            base.OnUse();
        }

        public override void OnCancel()
        {
            base.OnCancel();
        }
    }
}