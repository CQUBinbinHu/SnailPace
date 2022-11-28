using DefaultNamespace;

namespace HeroPerform
{
    public class Attack : SkillComponent
    {
        public int Atk;

        public override int GetDamage()
        {
            return (int)(Owner.GetBuffAtkMultiplier() * Atk);
        }

        public override void OnUse()
        {
            if (!Target)
            {
                return;
            }

            if (!TryGetPermission())
            {
                return;
            }

            Target.Health.TakeDamage(GetDamage());
        }

        public override void OnCancel()
        {
        }
    }
}