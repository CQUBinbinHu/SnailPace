using System;
using DefaultNamespace;
using Lean.Pool;
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
        [SerializeField] private ShowBuff ShowBuff;
        public float Duration = 1;
        private float _timer;
        protected Character Owner;
        private ShowBuff ShowBuffTarget;
        public float lastCoolDown => _timer / Duration;

        public virtual void OnAddBuff(Character owner)
        {
            Owner = owner;
            owner.AddBuff(this);
            _timer = Duration;
            ShowBuffTarget = LeanPool.Spawn(ShowBuff, Owner.ShowBuffSocket);
            ShowBuffTarget.SetOwner(this);
        }

        protected virtual void OnRemoveBuff()
        {
            Owner.RemoveBuff(this);
            LeanPool.Despawn(ShowBuffTarget);
        }

        protected abstract void OnBuffTick(float deltaTime);

        public void FixedTick(float deltaTime)
        {
            if (_timer < Duration)
            {
                _timer -= deltaTime;
                OnBuffTick(deltaTime);
            }
            else
            {
                OnRemoveBuff();
            }
        }
    }
}