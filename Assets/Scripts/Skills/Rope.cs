using Core;
using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class Rope : SkillComponent
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

            Target.AddBuff<WeekBuff>(BuffType.Week, Duration);
            base.OnUse();
        }

        public override void OnCancel()
        {
            base.OnCancel();
        }
    }
}