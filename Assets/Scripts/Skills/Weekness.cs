using Core;
using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class Weekness : SkillComponent
    {
        public override void OnUse()
        {
            if (!TryGetPermission())
            {
                return;
            }

            Target.AddBuff(BuffType.Week);
        }

        public override void OnCancel()
        {
        }
    }
}