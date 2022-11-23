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
    public class SkillReward : MonoBehaviour,
        MMEventListener<CoreGameEvent>,
        MMEventListener<RunGameEvent>,
        IPoolable
    {
        [SerializeField] private Transform SkillSocket;
        [SerializeField] private TextMeshProUGUI TextSkillName;
        [SerializeField] private TextMeshProUGUI TextIntroduction;
        public string SkillName;
        public string Introduction;
        private SkillComponent _skillObject;
        private SkillComponent _skillTarget;
        private bool _isAddSkill;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Initialize()
        {
            _isAddSkill = false;
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
            _isAddSkill = true;
            BattleManager.Instance.AddSkill(_skillTarget);
        }

        private void DestroySelf()
        {
            StartCoroutine(DestroyDelay(0.3f));
        }

        IEnumerator DestroyDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (!_isAddSkill)
            {
                LeanPool.Despawn(_skillTarget);
            }

            LeanPool.Despawn(this);
        }

        public void OnMMEvent(RunGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case RunEventTypes.Continue:
                    DestroySelf();
                    break;
            }
        }

        public void OnMMEvent(CoreGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case CoreGameEventTypes.AddSkill:
                    DestroySelf();
                    break;
            }
        }

        public void SetSkillObject(SkillComponent skill)
        {
            _skillObject = skill;
            InitializeSkillObject();
        }

        public void OnSpawn()
        {
            Initialize();
            this.MMEventStartListening<CoreGameEvent>();
            this.MMEventStartListening<RunGameEvent>();
        }

        public void OnDespawn()
        {
            _button.interactable = false;
            this.MMEventStopListening<CoreGameEvent>();
            this.MMEventStopListening<RunGameEvent>();
        }
    }
}