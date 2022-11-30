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

        public void SetCure(float totalCure)
        {
            _cure = totalCure;
        }
    }
}