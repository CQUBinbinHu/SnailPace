using Core;
using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class MeteorHammer : SkillComponent
    {
        [SerializeField] private bool UseRandom;
        [SerializeField] private float Duration;
        public int Atk;
        public int MinAtk;
        public int MaxAtk;
        private int _atk;

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

            TriggerAttack(() =>
            {
                Target.Health.TakeDamage(GetDamage());
                Target.AddBuff<FrailBuff>(BuffType.Frail, Duration);
            });
            base.OnUse();
        }
    }
}