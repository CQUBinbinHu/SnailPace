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

        public virtual void OnAddBuff(Character owner, float duration = 1)
        {
            Owner = owner;
            owner.AddBuff(this);
            Duration = duration;
            _timer = Duration;
            _showBuffComponentTarget = LeanPool.Spawn(GameManager.Instance.BuffShowData.ShowBuffs[BuffType], Owner.ShowBuffSocket);
            _showBuffComponentTarget.SetOwner(this);
        }

        protected virtual void OnRemoveBuff()
        {
            Owner.RemoveBuff(this);
            StartCoroutine(DespawnNextFrame());
        }

        protected abstract void OnBuffTick(float deltaTime);

        public void FixedTick(float deltaTime)
        {
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

        public virtual void OnOverride()
        {
        }

        IEnumerator DespawnNextFrame()
        {
            yield return new WaitForFixedUpdate();
            LeanPool.Despawn(_showBuffComponentTarget);
            Destroy(this);
        }
    }
}