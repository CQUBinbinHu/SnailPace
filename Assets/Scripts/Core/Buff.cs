using System;
using System.Collections;
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
        public BuffType BuffType;
        public float Duration;
        private float _timer;
        protected Character Owner;
        private ShowBuffComponent _showBuffComponentTarget;
        public float lastCoolDown => _timer / Duration;
        public bool IsBuffActivated { get; private set; }

        public virtual void OnAddBuff(Character owner, float duration = 1)
        {
            IsBuffActivated = true;
            Owner = owner;
            owner.AddBuff(this);
            Duration = duration;
            _timer = Duration;
            _showBuffComponentTarget = LeanPool.Spawn(GameManager.Instance.BuffShowData.ShowBuffs[BuffType], Owner.ShowBuffSocket);
            _showBuffComponentTarget.SetOwner(this);
        }

        protected virtual void OnRemoveBuff()
        {
            IsBuffActivated = false;
            Owner.RemoveBuff(this);
            StartCoroutine(DespawnNextFrame());
        }

        protected abstract void OnBuffTick(float deltaTime);

        public void FixedTick(float deltaTime)
        {
            if (!IsBuffActivated)
            {
                return;
            }

            if (_timer > 0)
            {
                _timer -= deltaTime;
                OnBuffTick(deltaTime);
            }
            else
            {
                OnRemoveBuff();
            }
        }

        public virtual void OnOverride(float duration)
        {
        }

        protected void ResetCoolDown(float duration)
        {
            Duration = duration;
            _timer = duration;
        }

        IEnumerator DespawnNextFrame()
        {
            yield return new WaitForFixedUpdate();
            LeanPool.Despawn(_showBuffComponentTarget);
            Destroy(this);
        }
    }
}