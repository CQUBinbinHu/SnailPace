﻿using System;
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
        [SerializeField] private GameObject Model;
        public string SkillName;
        public string Introduction;
        private SkillComponent _skillObject;
        private SkillComponent _skillTarget;
        public SkillComponent SkillTarget => _skillTarget;
        private Button _button;
        private bool _isDestroyed;
        private bool _isAdded;

        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
        }

        private void Initialize()
        {
            Model.SetActive(true);
            _isAdded = false;
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
            _button.interactable = false;
            _isAdded = true;
            GameEventManager.Instance.OnAddSkill?.Invoke(this);
        }

        private void DestroySelf()
        {
            if (_isDestroyed)
            {
                return;
            }

            _button.interactable = false;
            _isDestroyed = true;
            Model.SetActive(false);
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
            if (_isAdded)
            {
                return;
            }

            DestroySelf();
        }
    }
}