﻿using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnergyComponent : MonoBehaviour
    {
        [SerializeField] private float MaxEnergy = 100;
        [SerializeField] private float Recovery = 20;

        private EnergyBar _energyBar;
        private float _current;
        public float Current => _current;

        public float EnergyRatio => (float)_current / MaxEnergy;

        private void Awake()
        {
            _energyBar = GetComponentInChildren<EnergyBar>();
        }

        private void Start()
        {
            _current = 0;
        }

        public void FixedTick(float deltaTime)
        {
            _current += deltaTime * Recovery;
            _current = Mathf.Clamp(_current, 0, MaxEnergy);
        }

        public void CostEnemy(float value)
        {
            _current -= value;
            _current = Mathf.Clamp(_current, 0, MaxEnergy);
        }
    }
}