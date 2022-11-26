using System;
using System.Collections;
using Core;
using DG.Tweening;
using Lean.Pool;
using MoreMountains.Tools;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using IPoolable = Lean.Pool.IPoolable;

namespace DefaultNamespace
{
    public class SkillReward : MonoBehaviour, IPoolable
    {
        [SerializeField] private Transform SkillSocket;
        [SerializeField] private TextMeshProUGUI TextSkillName;
        [SerializeField] private TextMeshProUGUI TextIntroduction;
        public string SkillName;
        public string Introduction;
        private SkillComponent _skillObject;
        private SkillComponent _skillTarget;
        public SkillComponent SkillTarget => _skillTarget;
        private Button _button;
        private bool _isDestroyed;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Initialize()
        {
            _isDestroyed = false;
            _button.interactable = true;
        }

        private void InitializeSkillObject()
        {
            _skillTarget = LeanPool.Spawn(_skillObject, SkillSocket);
            TextSkillName.text = SkillName;
            TextIntroduction.text = Introduction;
        }

        public void OnAddSkill()
        {
            GameEventManager.Instance.OnAddSkill(this);
        }

        private void DestroySelf()
        {
            if (_isDestroyed)
            {
                return;
            }

            gameObject.SetActive(false);
            _isDestroyed = true;
            _button.interactable = false;
            StartCoroutine(DestroyDelay(0.3f));
        }

        IEnumerator DestroyDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            LeanPool.Despawn(_skillTarget);
            LeanPool.Despawn(this);
        }

        public void SetSkillObject(SkillComponent skill)
        {
            _skillObject = skill;
            InitializeSkillObject();
        }

        public void OnSpawn()
        {
            Initialize();
        }

        public void OnDespawn()
        {
        }

        private void OnEnable()
        {
            GameEventManager.Instance.OnAddSkill += DoOnAddSkill;
            GameEventManager.Instance.OnRunContinue += OnRunContinue;
        }

        private void OnDisable()
        {
            GameEventManager.Instance.OnAddSkill -= DoOnAddSkill;
            GameEventManager.Instance.OnRunContinue -= OnRunContinue;
        }

        private void OnRunContinue()
        {
            DestroySelf();
        }

        private void DoOnAddSkill(SkillReward skillReward)
        {
            if (skillReward == this)
            {
                return;
            }

            DestroySelf();
        }
    }
}