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
        public int CoolDownTimer { get; set; }
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

        public bool TryCostEnergy(int energy)
        {
            if (energy > CurrentEnergy)
            {
                return false;
            }

            CurrentEnergy -= energy;
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MaxEnergy);
            _energyBar.UpdateBar();
            return true;
        }

        protected void SetTarget(Character target)
        {
            _target = target;
        }

        public virtual void TickCoolDown()
        {
            CoolDownTimer -= 1;
        }

        public virtual void FixedTick(float deltaTime)
        {
            CurrentEnergy += (int)(deltaTime * EnergyRecovery);
        }

        public virtual void Tick(float deltaTime)
        {
            _energyBar.UpdateBar();
        }

        public void SetCurrent(string behaviourName)
        {
            _currentBehaviour = _behaviours[behaviourName];
        }
    }
}