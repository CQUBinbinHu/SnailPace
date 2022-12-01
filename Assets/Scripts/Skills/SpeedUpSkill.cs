using Core;
using DefaultNamespace;

namespace HeroPerform
{
    public class SpeedUpSkill : SkillComponent
    {
        public int Atk;
        public int Speed;

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

            TriggerAttack(() =>
            {
                Target.Health.TakeDamage(GetDamage(), () =>
                    {
                        SpeedBuff buff = (Owner.AddBuff<SpeedBuff>(BuffType.Speed)) as SpeedBuff;
                        if (buff != null)
                        {
                            buff.AddSpeed(Speed);
                        }
                    }
                );
            });
            base.OnUse();
        }
    }
}