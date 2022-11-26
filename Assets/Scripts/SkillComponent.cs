using System;
using System.Collections;
using Core;
using DG.Tweening;
using Lean.Pool;
using MoreMountains.Tools;
using TMPro;
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
        MMEventListener<CoreGameEvent>,
        IPoolable
    {
        [SerializeField] private int NeedEnergy = 10;
        private SkillShowComponent _skillShow;
        public string SkillName;
        protected Character Owner;
        protected Character Target;
        private LoopSocket _follow;
        private bool IsEnergySatisfied;

        private void Awake()
        {
            TryGetComponent(out _skillShow);
        }

        public void Initialize()
        {
            _follow = null;
            if (_skillShow)
            {
                _skillShow.EnergyText.text = NeedEnergy.ToString();
                IsEnergySatisfied = true;
                EnableSkillMask(IsEnergySatisfied);
            }
        }

        public void SetOwner(Character owner)
        {
            Owner = owner;
        }

        public void SetTarget(Character target)
        {
            Target = target;
        }

        public abstract void OnUse();
        public abstract void OnCancel();

        protected bool TryGetPermission()
        {
            if (!Target)
            {
                return false;
            }

            switch (Owner.CharacterType)
            {
                case CharacterType.Hero:
                    if (!TryCostEnergy())
                    {
                        ShowTips("lack mana");
                        return false;
                    }

                    break;
                case CharacterType.Enemy:
                    return true;
            }

            return true;
        }

        private void ShowTips(string tip)
        {
            var showTip = LeanPool.Spawn(GameManager.Instance.ShowTipComponent);
            showTip.transform.position = transform.position;
            showTip.SetTips(tip, Color.white);
        }

        private void Update()
        {
            if (!Owner)
            {
                return;
            }

            if (Owner.HasEnergy)
            {
                CheckEnergy();
            }
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
            if (_skillShow)
            {
                _skillShow.SkillMask.DOFade(enable ? 0 : 0.6f, 0.2f);
            }
        }

        public void SetFollow(LoopSocket follow)
        {
            _follow = follow;
            _follow.SetSkill(this);
            transform.DOMove(follow.Trans.position, 0.5f);
        }

        public void OnMMEvent(CoreGameEvent eventType)
        {
        }

        // private void OnAddSkill()
        // {
        //     if (!BattleManager.Instance.IsFullSkill)
        //     {
        //         return;
        //     }
        //
        //     if (_follow == null)
        //     {
        //         return;
        //     }
        //
        //     _follow = _follow.Prev;
        //     transform.DOMove(_follow.Trans.position, 0.5f);
        //     if (_follow.Index == 0)
        //     {
        //         Kill();
        //     }
        //     else
        //     {
        //         _follow.SetSkill(this);
        //     }
        // }

        private void Kill()
        {
            StartCoroutine(DelayDestroy(0.6f));
        }

        IEnumerator DelayDestroy(float delay)
        {
            BattleManager.Instance.Hero.BehaviourController.RemoveSkill(this);
            yield return new WaitForSeconds(delay);
            LeanPool.Despawn(this);
        }

        public void OnSpawn()
        {
            Initialize();
            this.MMEventStartListening<CoreGameEvent>();
        }

        public void OnDespawn()
        {
            this.MMEventStopListening<CoreGameEvent>();
        }
    }
}