using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class ArmorBuff : Buff
    {
        protected override void OnBuffTick(float deltaTime)
        {
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