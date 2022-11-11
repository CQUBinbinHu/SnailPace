using System;
using UnityEngine;

namespace Core
{
    public class SlimeBehaviourController : BehaviourController
    {
        private void Start()
        {
            _currentBehaviour = _behaviours["Sleep"];
        }

        public override void Tick()
        {
            base.Tick();
            _currentBehaviour = _behaviours["Attack"];
        }
    }
}