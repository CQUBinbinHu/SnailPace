using Core;
using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class CurePotion : SkillComponent
    {
        public float Duration = 10;
        public int TotalCure = 30;

        public override void OnUse()
        {
            CureBuff cure = (Owner.AddBuff<CureBuff>(BuffType.Cure, Duration)) as CureBuff;
            if (cure != null)
            {
                cure.SetCure(TotalCure / Duration);
            }

            base.OnUse();
        }
    }
}