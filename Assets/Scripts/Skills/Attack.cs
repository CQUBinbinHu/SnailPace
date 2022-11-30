using DefaultNamespace;

namespace HeroPerform
{
    public class Attack : SkillComponent
    {
        public int Atk;

        public override int GetDamage()
        {
            return base.GetDamage(Atk);
        }

        public override void OnUse()
        {
            if (!Target || Target.IsDead)
            {
                return;
            }

            if (!TryGetPermission())
            {
                return;
            }

            Owner.TriggerAttack();
            DoCallbackDelay(() => { Target.Health.TakeDamage(GetDamage()); }, 0.1f);
            base.OnUse();
        }

        public override void OnCancel()
        {
            base.OnCancel();
        }
    }
}