using System;
using Core;
using MoreMountains.Tools;
using UnityEngine;

namespace DefaultNamespace
{
    public class ContinueRunComponent : MonoBehaviour
    {
        [SerializeField] private GameObject Target;

        public void OnContinue()
        {
            GameEventManager.Instance.OnRunContinue.Invoke();
        }

        private void OnEnable()
        {
            GameEventManager.Instance.OnRunContinue += RunContinue;
            GameEventManager.Instance.OnAddSkill += RunContinue;
            GameEventManager.Instance.OnRunReward += OnReward;
        }

        private void OnDisable()
        {
            GameEventManager.Instance.OnRunContinue -= RunContinue;
            GameEventManager.Instance.OnAddSkill -= RunContinue;
            GameEventManager.Instance.OnRunReward -= OnReward;
        }

        private void RunContinue(SkillReward skillReward)
        {
            Target.SetActive(false);
        }

        private void RunContinue()
        {
            Target.SetActive(false);
        }

        private void OnReward()
        {
            Target.SetActive(true);
        }
    }
}