using System;
using Core;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ShowBuff : MonoBehaviour, IPoolable
    {
        [SerializeField] private Image Progress;
        private Buff _buff;

        public void OnSpawn()
        {
            Progress.fillAmount = 1;
        }

        public void SetOwner(Buff buff)
        {
            _buff = buff;
        }

        private void Update()
        {
            Progress.fillAmount = _buff.lastCoolDown;
        }

        public void OnDespawn()
        {
        }
    }
}