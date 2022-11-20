using System;
using System.Collections;
using Core;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class SkillReward : MonoBehaviour,
        MMEventListener<CoreGameEvent>,
        MMEventListener<RunGameEvent>
    {
        public string SkillName;
        private SkillComponent _skillObject;
        private Image[] _images;

        private void Awake()
        {
            _images = GetComponentsInChildren<Image>();
        }

        public void OnAddSkill()
        {
            BattleManager.Instance.AddSkill(Instantiate(_skillObject), transform.position);
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
            Destroy(gameObject);
        }

        private void OnEnable()
        {
            this.MMEventStartListening<CoreGameEvent>();
            this.MMEventStartListening<RunGameEvent>();
        }

        private void OnDisable()
        {
            this.MMEventStopListening<CoreGameEvent>();
            this.MMEventStopListening<RunGameEvent>();
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

        public void SetSkillObject(SkillComponent skill)
        {
            _skillObject = skill;
        }
    }
}