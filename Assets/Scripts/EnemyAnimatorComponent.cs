using System;
using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnemyAnimatorComponent : MonoBehaviour
    {
        [SerializeField] private string AnimationName;
        private Character Owner;

        private void Awake()
        {
            Owner = GetComponent<Character>();
        }

        public void TriggerAnimator()
        {
            Owner.Animator.SetTrigger(AnimationName);
        }
    }
}