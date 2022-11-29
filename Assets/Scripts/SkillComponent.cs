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
        private readonly Transform _trans;
        public Transform Trans => _trans;

        public LoopSocket(Transform transform)
        {
            _trans = transform;
            _inputConnector = transform.GetComponent<InputConnector>();
        }

        public void SetSkill(SkillComponent skill)
        {
            _inputConnector.SetSkill(skill);
        }

        public void RemoveSkill()
        {
            _inputConnector.SetInteractable(false);
        }
    }

    public abstract class SkillComponent : MonoBehaviour, IPoolable
    {
        [SerializeField] public string SkillName;
        [SerializeField] public string Introduction;
        [SerializeField] private int NeedEnergy = 10;
        private SkillShowComponent _skillShow;
        protected Character Owner;
        protected Character Target;
        private LoopSocket _follow;
        private bool IsEnergySatisfied;
        public bool IsActive;
        protected bool RefreshOnCancel = false;

        public delegate void CallBack();

        public virtual int GetDamage()
        {
            return 0;
        }

        private void Awake()
        {
            TryGetComponent(out _skillShow);
        }

        private void Initialize()
        {
            _follow = null;
            IsActive = true;
            ResetStatus();
        }

        public void ResetStatus()
        {
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

        public virtual void OnUse()
        {
            if (!RefreshOnCancel)
            {
                OnRefresh();
            }
        }

        public virtual void OnCancel()
        {
            if (RefreshOnCancel)
            {
                OnRefresh();
            }
        }

        protected bool TryGetPermission()
        {
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
                Owner.Energy.CostEnergy(NeedEnergy);
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
            var position = follow.Trans.position;
            transform.position = position + Vector3.down;
            transform.DOMove(position, 0.5f);
        }

        public void OnRefresh()
        {
            if (_follow == null)
            {
                return;
            }

            var position = _follow.Trans.position + Vector3.down;
            _follow.RemoveSkill();
            _follow = null;
            IsActive = false;
            transform.DOMove(position, 0.3f);
            StartCoroutine(DelayDeActive(0.4f));
        }

        IEnumerator DelayDeActive(float delay)
        {
            yield return new WaitForSeconds(delay);
            this.gameObject.SetActive(false);
        }

        public void OnSpawn()
        {
            Initialize();
        }

        public void OnDespawn()
        {
        }

        protected void DoCallbackDelay(CallBack callBack, float delay)
        {
            StartCoroutine(DoCallbackDelay_Cro(callBack, delay));
        }

        IEnumerator DoCallbackDelay_Cro(CallBack callBack, float delay)
        {
            yield return new WaitForSeconds(delay);
            callBack.Invoke();
        }
    }
}