using System;
using UnityEngine;

namespace Core
{
    public class SlimeBehaviourController : BehaviourController
    {
        private float CoolDown = 3;
        private float _coolDownTimer;

        public override void Initialize()
        {
            base.Initialize();
            _coolDownTimer = CoolDown;
            SetTarget(BattleManager.Instance.Hero);
            SetCurrent("Sleep");
        }

        public override void FixedTick(float deltaTime)
        {
            base.FixedTick(deltaTime);
            if (_coolDownTimer > 0)
            {
                _coolDownTimer -= deltaTime;
            }
            else
            {
                _coolDownTimer = CoolDown;
                _currentBehaviour = _behaviours["Attack"];
                _currentBehaviour.Perform();
            }
        }
    }
}