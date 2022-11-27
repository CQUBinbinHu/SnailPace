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
        private Vector3 _initPosition;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _initPosition = MoveTarget.transform.localPosition;
        }

        public void SetWalk()
        {
            _animator.SetTrigger(Walk);
        }

        private void Update()
        {
            FillImage.fillAmount = GameManager.Instance.ProgressValue;
            MoveTarget.transform.localPosition = _initPosition + GameManager.Instance.ProgressValue * 5.0f * Vector3.right;
        }
    }
}