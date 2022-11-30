using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class StrengthPotion : SkillComponent
    {
        [SerializeField] private int Strength = 1;

        public override void OnUse()
        {
            // TODO: UseBuff
            Owner.StrengthComponent.AddStrength(Strength);
            base.OnUse();
        }
    }
}