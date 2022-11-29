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
            base.OnUse();
        }

        public override void OnCancel()
        {
            base.OnCancel();
        }
    }
}