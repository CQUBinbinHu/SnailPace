using System;
using System.Collections.Generic;
using DefaultNamespace;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;

namespace Core
{
    public class BehaviourController : MonoBehaviour
    {
        [SerializeField] private BehaviourTreeOwner _behaviourTreeOwner;
        private Character _owner;
        private Character _target;
        private EnergyComponent _energy;
        private HashSet<SkillComponent> _skills;
        private BehaviourTree _behaviourTree;
        public Character Target => _target;

        private void Awake()
        {
            TryGetComponent(out _owner);
            TryGetComponent(out _energy);
            if (_behaviourTreeOwner)
            {
                _behaviourTreeOwner.updateMode = Graph.UpdateMode.Manual;
                _behaviourTreeOwner.updateInterval = Time.fixedDeltaTime;
                // _behaviourTree = _behaviourTreeOwner.behaviour;
                // _behaviourTree = Instantiate(_behaviourTreeOwner.behaviour);
            }

            _skills = new HashSet<SkillComponent>();
            foreach (var skill in _owner.SkillSocket.GetComponents<SkillComponent>())
            {
                skill.SetOwner(_owner);
                _skills.Add(skill);
            }
        }

        public virtual void Initialize()
        {
            if (_behaviourTree)
            {
                _behaviourTree.StartGraph(_behaviourTreeOwner, _behaviourTreeOwner.blackboard, Graph.UpdateMode.Manual);
            }
        }

        public void SetTarget(Character target)
        {
            _target = target;
            foreach (var skill in _skills)
            {
                if (skill)
                {
                    skill.SetTarget(target);
                }
            }
        }

        public void AddSkill(SkillComponent skill)
        {
            _skills.Add(skill);
        }

        public void RemoveSkill(SkillComponent skill)
        {
            _skills.Remove(skill);
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

            if (_behaviourTreeOwner)
            {
                _behaviourTreeOwner.UpdateBehaviour();
            }
            // if (_behaviourTree)
            // {
            //     _behaviourTree.UpdateGraph(deltaTime);
            // }
        }
    }
}