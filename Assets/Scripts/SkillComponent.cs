using System;
using System.Collections;
using Core;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;

namespace DefaultNamespace
{
    public class LoopSocket
    {
        private readonly int _index;
        private readonly Transform _trans;
        public int Index => _index;
        public Transform Trans => _trans;

        public LoopSocket(int index, Transform transform)
        {
            _index = index;
            _trans = transform;
        }

        public LoopSocket Next;
        public LoopSocket Prev;
    }

    public abstract class SkillComponent : MonoBehaviour,
        MMEventListener<CoreGameEvent>
    {
        public string SkillName;
        protected Character Owner;
        protected Character Target;
        private LoopSocket _follow;

        public void SetOwner(Character owner)
        {
            Owner = owner;
            owner.BehaviourController.AddSkill(this);
        }

        public void SetTarget(Character target)
        {
            Target = target;
        }

        public abstract void OnUse();

        public void SetFollow(LoopSocket follow)
        {
            _follow = follow;
            transform.DOMove(follow.Trans.position, 0.5f);
        }

        public void OnMMEvent(CoreGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case CoreGameEventTypes.AddSkill:
                    OnAddSkill();
                    break;
            }
        }

        private void OnAddSkill()
        {
            if (_follow.Index == 0)
            {
                Kill();
            }
            else if (BattleManager.Instance.IsFullSkill)
            {
                _follow = _follow.Prev;
                transform.DOMove(_follow.Trans.position, 0.5f);
            }
        }

        private void Kill()
        {
            StartCoroutine(DelayDestroy(0.6f));
        }

        IEnumerator DelayDestroy(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(this.gameObject);
        }

        /// <summary>
        /// OnDisable, we start listening to events.
        /// </summary>
        protected virtual void OnEnable()
        {
            this.MMEventStartListening<CoreGameEvent>();
        }

        /// <summary>
        /// OnDisable, we stop listening to events.
        /// </summary>
        protected virtual void OnDisable()
        {
            this.MMEventStopListening<CoreGameEvent>();
        }
    }
}