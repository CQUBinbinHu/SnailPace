using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Core
{
    public abstract class BaseBehaviour : MonoBehaviour
    {
        protected Character Character;
        public string BehaviourName;
        public int CoolDown;
        public abstract void Perform();

        private void Awake()
        {
            Character = GetComponent<Character>();
        }
    }
}