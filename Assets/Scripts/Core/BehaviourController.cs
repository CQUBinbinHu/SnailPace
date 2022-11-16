using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Core
{
    public class BehaviourController : MonoBehaviour
    {
        [SerializeField] private int MaxEnergy;
        [SerializeField] private int EnergyRecovery;
        private Character _target;
        private EnergyBar _energyBar;
        private int CurrentEnergy;
        protected Dictionary<string, BaseBehaviour> _behaviours;
        protected BaseBehaviour _currentBehaviour;
        public BaseBehaviour CurrentBehaviour => _currentBehaviour;
        public Character Target => _target;
        public float EnergyRatio => (float)CurrentEnergy / MaxEnergy;

        private void Awake()
        {
            _energyBar = GetComponentInChildren<EnergyBar>();
            _behaviours = new Dictionary<string, BaseBehaviour>();
            foreach (var behaviour in GetComponents<BaseBehaviour>())
            {
                _behaviours.Add(behaviour.BehaviourName, behaviour);
            }
        }

        public virtual void Initialize()
        {
            CurrentEnergy = MaxEnergy;
        }

        private void UpdateEnergyBar()
        {
            if (_energyBar)
            {
                _energyBar.UpdateBar();
            }
        }

        public bool TryCostEnergy(int energy)
        {
            if (energy > CurrentEnergy)
            {
                return false;
            }

            CurrentEnergy -= energy;
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MaxEnergy);
            UpdateEnergyBar();
            return true;
        }

        protected void SetTarget(Character target)
        {
            _target = target;
        }

        public virtual void FixedTick(float deltaTime)
        {
            CurrentEnergy += (int)(deltaTime * EnergyRecovery);
            UpdateEnergyBar();
        }

        public void SetCurrent(string behaviourName)
        {
            _currentBehaviour = _behaviours[behaviourName];
        }
    }
}