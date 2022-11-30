using Core;
using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class SpeedPotion : SkillComponent
    {
        [SerializeField] private int Speed;

        public override void OnUse()
        {
            SpeedBuff buff = (Owner.AddBuff<SpeedBuff>(BuffType.Speed)) as SpeedBuff;
            if (buff != null)
            {
                buff.AddSpeed(Speed);
            }

            base.OnUse();
        }
    }
}