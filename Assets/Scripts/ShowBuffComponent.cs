using System;
using Core;
using Lean.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ShowBuffComponent : MonoBehaviour, IPoolable
    {
        [SerializeField] private Image CoolDown;
        [SerializeField] private TextMeshProUGUI LayersText;
        [SerializeField] private bool ShowLayers;
        private Buff _buff;

        public void OnSpawn()
        {
            CoolDown.fillAmount = 1;
        }

        public void SetOwner(Buff buff)
        {
            _buff = buff;
        }

        private void Update()
        {
            CoolDown.fillAmount = _buff.lastCoolDown;
            LayersText.text = ShowLayers ? _buff.GetLayers().ToString() : string.Empty;
        }

        public void OnDespawn()
        {
        }
    }
}