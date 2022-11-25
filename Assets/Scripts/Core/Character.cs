using System;
using System.Collections.Generic;
using DefaultNamespace;
using Lean.Pool;
using UnityEngine;

namespace Core
{
    public enum CharacterType
    {
        Hero,
        Enemy
    }

    public class Character : MonoBehaviour, IPoolable
    {
        [SerializeField] public Transform SkillSocket;
        [SerializeField] public Transform ShowBuffSocket;
        [SerializeField] public Transform BuffSocket;
        [SerializeField] public Transform TipSocket;
        [SerializeField] public CharacterType CharacterType;
        [SerializeField] private string Name;

        private HealthComponent _health;
        private EnergyComponent _energyComponent;
        private BehaviourController _behaviourController;
        private List<Buff> _buffAddCurrent;
        private List<Buff> _buffRemoveCurrent;
        private Dictionary<BuffType, Buff> _buffs;
        private Dictionary<BuffType, float> _buffAtkMultiplier;
        private Dictionary<BuffType, float> _buffDamageMultiplier;
        public Dictionary<BuffType, Buff> Buffs => _buffs;
        public HealthComponent Health => _health;
        public EnergyComponent Energy => _energyComponent;
        public BehaviourController BehaviourController => _behaviourController;
        public float CurrentEnergy => _energyComponent.Current;

        private void Awake()
        {
            TryGetComponent(out _energyComponent);
            TryGetComponent(out _health);
            _buffAddCurrent = new List<Buff>();
            _buffRemoveCurrent = new List<Buff>();
            _buffs = new Dictionary<BuffType, Buff>();
            _health = GetComponent<HealthComponent>();
            _behaviourController = GetComponent<BehaviourController>();
            _buffAtkMultiplier = new Dictionary<BuffType, float>();
            _buffDamageMultiplier = new Dictionary<BuffType, float>();
        }

        public void AddBuff(Buff buff)
        {
            _buffAddCurrent.Add(buff);
        }

        public void RemoveBuff(Buff buff)
        {
            _buffRemoveCurrent.Add(buff);
        }

        private void LateUpdate()
        {
            // Add Buffs
            foreach (var buff in _buffAddCurrent)
            {
                if (!_buffs.ContainsKey(buff.BuffType))
                {
                    _buffs.Add(buff.BuffType, buff);
                }
            }

            // Remove Buffs
            foreach (var buff in _buffRemoveCurrent)
            {
                if (_buffs.ContainsKey(buff.BuffType))
                {
                    _buffs.Remove(buff.BuffType);
                }
            }

            _buffAddCurrent.Clear();
            _buffRemoveCurrent.Clear();
        }

        public float GetBuffAtkMultiplier()
        {
            float multiplier = 1;
            foreach (var value in _buffAtkMultiplier)
            {
                multiplier *= value.Value;
            }

            return multiplier;
        }

        public float GetBuffDamageMultiplier()
        {
            float multiplier = 1;
            foreach (var value in _buffDamageMultiplier)
            {
                multiplier *= value.Value;
            }

            return multiplier;
        }

        public void AddAtkMultiplier(BuffType origin, float multiplier)
        {
            if (!_buffAtkMultiplier.ContainsKey(origin))
            {
                _buffAtkMultiplier.Add(origin, multiplier);
            }
        }

        public void AddDamageMultiplier(BuffType origin, float multiplier)
        {
            if (!_buffDamageMultiplier.ContainsKey(origin))
            {
                _buffDamageMultiplier.Add(origin, multiplier);
            }
        }

        public void RemoveBuffAtkMultiplier(BuffType buff)
        {
            _buffAtkMultiplier.Remove(buff);
        }

        public void RemoveBuffDamageMultiplier(BuffType buff)
        {
            _buffDamageMultiplier.Remove(buff);
        }

        public void OnSpawn()
        {
            _health.Initialize();
        }

        public void OnDespawn()
        {
        }
    }
}