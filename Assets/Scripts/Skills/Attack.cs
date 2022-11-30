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

        public override void RefreshStatus()
        {
            if (UseRandom)
            {
                _atk = Random.Range(NumFunc.GetLevelUpAtk(MinAtk, Level), NumFunc.GetLevelUpAtk(MaxAtk, Level));
            }
            else
            {
                _atk = NumFunc.GetLevelUpAtk(Atk, Level);
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