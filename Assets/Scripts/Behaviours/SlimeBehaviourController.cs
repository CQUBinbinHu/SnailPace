using System;
using UnityEngine;

namespace Core
{
    public class SlimeBehaviourController : BehaviourController
    {
        public override void Initialize()
        {
            base.Initialize();
            _currentBehaviour = _behaviours["Sleep"];
            CoolDownTimer = _currentBehaviour.CoolDown;
        }

        public override void Tick()
        {
            base.Tick();
            _currentBehaviour = _behaviours["Attack"];
            _currentBehaviour.Perform();
        }
    }
}