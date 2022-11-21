using System;
using UnityEngine;

namespace Core
{
    public enum BuffType
    {
        Week,
        Enhancement,
        Vulnerable
    }

    public abstract class Buff : MonoBehaviour
    {
        public float Duration = 1;
        private float _timer;
        protected Character Owner;

        public virtual void OnAddBuff()
        {
            _timer = Duration;
        }

        public abstract void OnRemoveBuff();
        protected abstract void OnBuffTick(float deltaTime);

        private void FixedUpdate()
        {
            if (_timer < Duration)
            {
                _timer += Time.fixedDeltaTime;
                OnBuffTick(Time.fixedDeltaTime);
            }
            else
            {
                OnRemoveBuff();
            }
        }
    }
}