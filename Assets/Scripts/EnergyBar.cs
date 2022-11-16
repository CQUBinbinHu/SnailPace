using System;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class EnergyBar : MonoBehaviour
    {
        [SerializeField] private Image EnergyBarImage;
        private BehaviourController _behaviourController;

        private void Awake()
        {
            _behaviourController = GetComponentInParent<BehaviourController>();
        }

        public void UpdateBar()
        {
            EnergyBarImage.fillAmount = _behaviourController.EnergyRatio;
        }
    }
}