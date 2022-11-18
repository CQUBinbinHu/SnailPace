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
        private readonly Transform _current;
        public int Index => _index;
        public Transform Current => _current;

        public LoopSocket(int index, Transform transform)
        {
            _index = index;
            _current = transform;
        }

        public LoopSocket Next;
        public LoopSocket Prev;
    }

    public class SkillObject : MonoBehaviour,
        MMEventListener<CoreGameEvent>
    {
        private LoopSocket _follow;

        public void AddSkill()
        {
            RunGameEvent.Trigger(RunEventTypes.Continue);
        }

        public void OnUse()
        {
            BattleManager.Instance.Hero.BehaviourController.SetCurrent("Attack");
            BattleManager.Instance.OnPerform();
        }

        public void SetFollow(LoopSocket follow)
        {
            _follow = follow;
            transform.DOMove(follow.Current.position, 0.5f);
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
                transform.DOMove(_follow.Current.position, 0.5f);
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