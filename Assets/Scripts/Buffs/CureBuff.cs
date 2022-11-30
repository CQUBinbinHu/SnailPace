using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class CureBuff : Buff
    {
        private float _cure;

        protected override void OnBuffTick(float deltaTime)
        {
            Owner.Health.Cure(_cure * deltaTime);
        }

        public override void OnOverride(float duration)
        {
            ResetCoolDown(duration);
        }

        public void SetCure(float totalCure)
        {
            _cure = totalCure;
        }
    }
}