﻿using System;
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
        private IntentComponent _intentComponent;
        public IntentComponent Intent => _intentComponent;
        public int CountDown { get; set; }
        public bool IsOnCountDown { get; set; }
        public float CountDownRatio { get; set; }

        private void Awake()
        {
            TryGetComponent(out _owner);
            TryGetComponent(out _energy);
            _intentComponent = GetComponentInChildren<IntentComponent>();
            if (_behaviourTreeOwner)
            {
                _behaviourTreeOwner.updateMode = Graph.UpdateMode.Manual;
                _behaviourTreeOwner.updateInterval = Time.fixedDeltaTime;
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
            if (_behaviourTreeOwner)
            {
                _behaviourTreeOwner.StartBehaviour();
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
        }
    }
}