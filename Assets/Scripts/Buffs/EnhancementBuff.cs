﻿using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnhancementBuff : Buff
    {
        private const float AtkMultiplier = 2f;

        public override void OnAddBuff(Character owner)
        {
            base.OnAddBuff(owner);
            Owner.AddAtkMultiplier(BuffType.Enhancement, AtkMultiplier);
        }

        protected override void OnRemoveBuff()
        {
            Owner.RemoveBuffAtkMultiplier(BuffType.Enhancement);
        }

        protected override void OnBuffTick(float deltaTime)
        {
            Owner.AddAtkMultiplier(BuffType.Enhancement, AtkMultiplier);
        }
    }
}