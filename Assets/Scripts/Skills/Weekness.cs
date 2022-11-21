using Core;
using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class Weekness : SkillComponent
    {
        public override void OnUse()
        {
            Target.AddBuff(BuffType.Week);
        }

        public override void OnCancel()
        {
        }
    }
}