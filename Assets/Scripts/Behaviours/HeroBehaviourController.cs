using UnityEngine;

namespace Core
{
    public class HeroBehaviourController : BehaviourController
    {
        public override void Initialize()
        {
            base.Initialize();
            _currentBehaviour = _behaviours["Attack"];
            CoolDownTimer = 0;
        }
    }
}