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
            if (!TryGetPermission())
            {
                return;
            }

            Target.AddBuff(BuffType.Week, Duration);
        }

        public override void OnCancel()
        {
        }
    }
}