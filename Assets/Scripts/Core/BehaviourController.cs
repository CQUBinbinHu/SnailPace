using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class BehaviourController : MonoBehaviour
    {
        private Character _target;
        protected Dictionary<string, BaseBehaviour> _behaviours;
        protected BaseBehaviour _currentBehaviour;
        public int CoolDownTimer { get; set; }
        public BaseBehaviour CurrentBehaviour => _currentBehaviour;
        public Character Target => _target;

        private void Awake()
        {
            _behaviours = new Dictionary<string, BaseBehaviour>();
            foreach (var behaviour in GetComponents<BaseBehaviour>())
            {
                _behaviours.Add(behaviour.BehaviourName, behaviour);
            }
        }

        public virtual void Initialize()
        {
        }

        public void SetTarget(Character target)
        {
            _target = target;
        }

        public virtual void TickCoolDown()
        {
            CoolDownTimer -= 1;
        }

        public virtual void Tick()
        {
        }

        public void SetCurrent(string behaviourName)
        {
            _currentBehaviour = _behaviours[behaviourName];
        }
    }
}