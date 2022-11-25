using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Core
{
    public class BehaviourController : MonoBehaviour
    {
        private Character _owner;
        private Character _target;
        private EnergyComponent _energy;
        private HashSet<SkillComponent> _skills;
        protected Dictionary<string, BaseBehaviour> _behaviours;
        protected BaseBehaviour _currentBehaviour;
        public BaseBehaviour CurrentBehaviour => _currentBehaviour;
        public Character Target => _target;

        private void Awake()
        {
            TryGetComponent(out _owner);
            TryGetComponent(out _energy);
            _behaviours = new Dictionary<string, BaseBehaviour>();
            _skills = new HashSet<SkillComponent>();
            foreach (var behaviour in GetComponents<BaseBehaviour>())
            {
                _behaviours.Add(behaviour.BehaviourName, behaviour);
            }
        }

        public virtual void Initialize()
        {
        }

        public void AddSkill(SkillComponent skill)
        {
            _skills.Add(skill);
        }

        private void RemoveSkill(SkillComponent skill)
        {
            _skills.Remove(skill);
        }

        protected void SetTarget(Character target)
        {
            _target = target;
        }

        public virtual void FixedTick(float deltaTime)
        {
            if (_energy)
            {
                _energy.FixedTick(deltaTime);
            }

            foreach (var buff in _owner.Buffs.Values)
            {
                buff.FixedTick(deltaTime);
            }
        }

        public void SetCurrent(string behaviourName)
        {
            _currentBehaviour = _behaviours[behaviourName];
        }

        public void SetEncounter(Character target)
        {
            foreach (var skill in _skills)
            {
                skill.SetTarget(target);
            }
        }
    }
}