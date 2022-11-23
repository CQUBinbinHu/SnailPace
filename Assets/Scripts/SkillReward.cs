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
        private Image[] _images;

        private void Awake()
        {
            _images = GetComponentsInChildren<Image>();
        }

        private void InitializeSkillObject()
        {
            _skillTarget = LeanPool.Spawn(_skillObject, SkillSocket);
            TextSkillName.text = SkillName;
            TextIntroduction.text = Introduction;
        }

        public void OnAddSkill()
        {
            BattleManager.Instance.AddSkill(_skillTarget);
        }

        private void DestroySelf()
        {
            foreach (var image in _images)
            {
                image.DOFade(0, 0.2f);
            }

            StartCoroutine(DestroyDelay(0.3f));
        }

        IEnumerator DestroyDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
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
            this.MMEventStartListening<CoreGameEvent>();
            this.MMEventStartListening<RunGameEvent>();
        }

        public void OnDespawn()
        {
            this.MMEventStopListening<CoreGameEvent>();
            this.MMEventStopListening<RunGameEvent>();
        }
    }
}