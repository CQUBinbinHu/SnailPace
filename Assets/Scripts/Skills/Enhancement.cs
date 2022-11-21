using Core;
using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class Enhancement : SkillComponent
    {
        public override void OnUse()
        {
            Owner.AddBuff(BuffType.Enhancement);
        }

        public override void OnCancel()
        {
        }
    }
}