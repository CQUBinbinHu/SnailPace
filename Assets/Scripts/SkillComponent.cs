using System;
using System.Collections;
using Core;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class LoopSocket
    {
        private InputConnector _inputConnector;
        private readonly int _index;
        private readonly Transform _trans;
        public int Index => _index;
        public Transform Trans => _trans;

        public LoopSocket(int index, Transform transform)
        {
            _index = index;
            _trans = transform;
            _inputConnector = transform.GetComponent<InputConnector>();
        }

        public LoopSocket Next;
        public LoopSocket Prev;

        public void SetSkill(SkillComponent skill)
        {
            _inputConnector.SetSkill(skill);
        }
    }

    public abstract class SkillComponent : MonoBehaviour,
        MMEventListener<CoreGameEvent>
    {
        [SerializeField] private int NeedEnergy = 10;
        [SerializeField] private Image SkillMask;
        public string SkillName;
        protected Character Owner;
        protected Character Target;
        private LoopSocket _follow;
        private bool IsEnergySatisfied;

        private void Start()
        {
            IsEnergySatisfied = false;
            EnableSkillMask(IsEnergySatisfied);
        }

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
        public abstract void OnCancel();

        private void Update()
        {
            CheckEnergy();
        }

        public bool TryCostEnergy()
        {
            bool ok = CheckEnergy();
            if (ok)
            {
                Owner.Energy.CostEnemy(NeedEnergy);
            }

            return ok;
        }

        private bool CheckEnergy()
        {
            if (GetEnergyStatus())
            {
                EnableSkillMask(IsEnergySatisfied);
            }

            return IsEnergySatisfied;
        }

        private bool GetEnergyStatus()
        {
            bool temp = IsEnergySatisfied;
            IsEnergySatisfied = NeedEnergy <= Owner.CurrentEnergy;
            return temp != IsEnergySatisfied;
        }

        private void EnableSkillMask(bool enable)
        {
            SkillMask.DOFade(enable ? 0 : 0.6f, 0.2f);
        }

        public void SetFollow(LoopSocket follow)
        {
            _follow = follow;
            _follow.SetSkill(this);
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
            if (!BattleManager.Instance.IsFullSkill)
            {
                return;
            }

            if (_follow == null)
            {
                return;
            }

            _follow = _follow.Prev;
            transform.DOMove(_follow.Trans.position, 0.5f);
            if (_follow.Index == 0)
            {
                Kill();
            }
            else
            {
                _follow.SetSkill(this);
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