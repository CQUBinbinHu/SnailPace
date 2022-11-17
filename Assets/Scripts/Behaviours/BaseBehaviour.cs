using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Core
{
    public abstract class BaseBehaviour : MonoBehaviour
    {
        protected Character Character;
        protected BehaviourController Controller;
        public string BehaviourName;
        public abstract void Perform();

        private void Awake()
        {
            Character = GetComponent<Character>();
            Controller = GetComponent<BehaviourController>();
        }
    }
}