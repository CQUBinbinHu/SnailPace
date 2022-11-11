using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class BehaviourController : MonoBehaviour
    {
        protected Dictionary<string, BaseBehaviour> _behaviours;
        protected BaseBehaviour _currentBehaviour;

        public BaseBehaviour CurrentBehaviour => _currentBehaviour;

        private void Awake()
        {
            _behaviours = new Dictionary<string, BaseBehaviour>();
            foreach (var behaviour in GetComponents<BaseBehaviour>())
            {
                _behaviours.Add(behaviour.BehaviourName, behaviour);
            }
        }

        public virtual void Tick()
        {
        }
    }
}