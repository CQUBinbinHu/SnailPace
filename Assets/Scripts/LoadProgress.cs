using System;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class LoadProgress : MonoBehaviour
    {
        [SerializeField] private GameObject MoveTarget;
        [SerializeField] private Image FillImage;
        private static readonly int Walk = Animator.StringToHash("Walk");
        private Animator _animator;
        private StartGameUI _startGameUI;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _startGameUI = GetComponentInParent<StartGameUI>();
        }

        public void SetWalk()
        {
            _animator.SetTrigger(Walk);
        }

        private void Update()
        {
            FillImage.fillAmount = _startGameUI.ProgressValue;
        }
    }
}