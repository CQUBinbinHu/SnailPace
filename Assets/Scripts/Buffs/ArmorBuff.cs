using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class ArmorBuff : Buff
    {
        private float _duration = 3;

        public override void OnAddBuff(Character owner, float duration = -1)
        {
            base.OnAddBuff(owner, _duration);
        }

        protected override void OnBuffTick(float deltaTime)
        {
        }

        public override void OnOverride(float duration)
        {
            ResetCoolDown(_duration);
        }

        public override void OnRemoveBuff()
        {
            Owner.Health.RemoveAllArmors();
            base.OnRemoveBuff();
        }

        public void AddArmor(int armor)
        {
            Owner.Health.AddArmor(armor);
        }
    }
}