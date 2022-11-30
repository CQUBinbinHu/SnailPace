using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class Attack : SkillComponent
    {
        public int Atk;
        private int _atk;
        [SerializeField] private bool UseRandom;
        public int MinAtk;
        public int MaxAtk;

        public override void Initialize()
        {
            base.Initialize();
            _atk = Atk;
        }

        private int GetLevelVal(int val)
        {
            return val + 2 * Level;
        }

        public override void RefreshStatus()
        {
            if (UseRandom)
            {
                _atk = Random.Range(GetLevelVal(MinAtk), GetLevelVal(MaxAtk));
            }
            else
            {
                _atk = GetLevelVal(Atk);
            }
        }

        public override int GetDamage()
        {
            return base.GetDamage(_atk);
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