using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class SpeedPotion : SkillComponent
    {
        [SerializeField] private int Speed;

        public override void OnUse()
        {
            Owner.SpeedComponent.AddSpeed(Speed);
            base.OnUse();
        }
    }
}