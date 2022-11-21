using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnhancementBuff : Buff
    {
        private const float AtkMultiplier = 2f;

        public override void OnAddBuff()
        {
            base.OnAddBuff();
            Owner.AddAtkMultiplier(BuffType.Enhancement, AtkMultiplier);
        }

        public override void OnRemoveBuff()
        {
            Owner.RemoveBuffAtkMultiplier(BuffType.Enhancement);
        }

        protected override void OnBuffTick(float deltaTime)
        {
            Owner.AddAtkMultiplier(BuffType.Enhancement, AtkMultiplier);
        }
    }
}