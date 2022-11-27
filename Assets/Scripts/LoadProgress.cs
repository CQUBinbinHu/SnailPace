using System;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class LoadProgress : MonoBehaviour
    {
        private GameObject MoveTarget;
        private Image FillImage;
        private Animator _animator;
        private static readonly int Walk = Animator.StringToHash("Walk");

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }
        
        public void SetWalk()
        {
            _animator.SetTrigger(Walk);
        }

        private void Update()
        {
            FillImage.fillAmount = GameManager.Instance.Progress;
        }
    }
}