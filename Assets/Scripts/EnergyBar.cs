using System;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class EnergyBar : MonoBehaviour
    {
        [SerializeField] private Image EnergyBarImage;
        [SerializeField] private TextMeshProUGUI ShowEnergyAmount;
        private EnergyComponent _energyComponent;

        private void Awake()
        {
            _energyComponent = GetComponentInParent<EnergyComponent>();
        }

        private void Update()
        {
            UpdateBar();
        }

        private void UpdateBar()
        {
            if (_energyComponent)
            {
                EnergyBarImage.fillAmount = _energyComponent.EnergyRatio;
                ShowEnergyAmount.text = _energyComponent.CurrentEnergyAmount
                                        + "/"
                                        + _energyComponent.MaxEnergyAmount;
            }
        }
    }
}