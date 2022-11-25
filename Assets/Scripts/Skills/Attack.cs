using DefaultNamespace;

namespace HeroPerform
{
    public class Attack : SkillComponent
    {
        public int Atk;
        private int Damage => (int)(Owner.GetBuffAtkMultiplier() * Atk);

        public override void OnUse()
        {
            if (!TryGetPermission())
            {
                return;
            }

            Target.Health.TakeDamage(Damage);
        }

        public override void OnCancel()
        {
        }
    }
}