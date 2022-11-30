using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using Lean.Pool;
using ParadoxNotion;
using UnityEngine;

namespace Core
{
    public enum CharacterType
    {
        Hero,
        Enemy,
        Winning
    }

    public class Character : MonoBehaviour, IPoolable
    {
        [SerializeField] public Transform SkillSocket;
        [SerializeField] public Transform ShowBuffSocket;
        [SerializeField] public Transform BuffSocket;
        [SerializeField] public Transform TipSocket;
        [SerializeField] public CharacterType CharacterType;
        [SerializeField] private string Name;
        [SerializeField] private SpriteRenderer Portrait;
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
        private SpeedComponent _speedComponent;
        public SpeedComponent SpeedComponent => _speedComponent;
        private Animator _animator;
        private StrengthComponent _strengthComponent;
        public StrengthComponent StrengthComponent => _strengthComponent;
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
        public bool HasEnergy => _energyComponent;
        public bool IsDead => _health.IsDead;

        private void Awake()
        {
            TryGetComponent(out _strengthComponent);
            TryGetComponent(out _energyComponent);
            TryGetComponent(out _health);
            _speedComponent = GetComponentInChildren<SpeedComponent>();
            _animator = GetComponentInChildren<Animator>();
            _buffs = new Dictionary<BuffType, Buff>();
            _health = GetComponent<HealthComponent>();
            _buffRemoveCurrent = new List<Buff>();
            _buffAddCurrent = new List<Buff>();
            _behaviourController = GetComponent<BehaviourController>();
            _buffAtkMultiplier = new Dictionary<BuffType, float>();
            _buffDamageMultiplier = new Dictionary<BuffType, float>();
        }

        public void TriggerAttack()
        {
            _animator.SetTrigger(Attack);
        }

        public void TriggerIdle()
        {
            _animator.SetTrigger(Idle);
        }

        public void TriggerWalk()
        {
            Debug.Log("Trigger Walk");
            _animator.SetTrigger(Walk);
        }

        public void TriggerHurt()
        {
            if (IsDead)
            {
                return;
            }

            StartCoroutine(HurtPresentation_Cro());
        }

        IEnumerator HurtPresentation_Cro()
        {
            DOTween.To(() => Portrait.material.GetFloat(Opacity),
                (x) => Portrait.material.SetFloat(Opacity, x),
                1, 0.15f);
            yield return new WaitForSeconds(0.2f);
            DOTween.To(() => Portrait.material.GetFloat(Opacity),
                (x) => Portrait.material.SetFloat(Opacity, x),
                0, 0.15f);
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
            if (_speedComponent)
            {
                _speedComponent.Init();
            }

            if (_strengthComponent)
            {
                _strengthComponent.Init();
            }

            if (_health)
            {
                _health.Init();
            }

            if (_energyComponent)
            {
                _energyComponent.Init();
            }

            if (_speedComponent)
            {
                _speedComponent.SetSpeed(GameManager.Instance.InitSpeed);
            }

            if (BehaviourController.Intent)
            {
                _behaviourController.Intent.SetIntent(Intent.None);
                _behaviourController.IsOnCountDown = false;
            }

            _buffs.Clear();
            _buffAtkMultiplier.Clear();
            _buffDamageMultiplier.Clear();
            _behaviourController.Init();
        }

        public void OnDespawn()
        {
            foreach (var buff in BuffSocket.GetComponents<Buff>()) buff.OnRemoveBuff();
        }

        private void OnEnable()
        {
            GameEventManager.Instance.OnGameRestart += OnRestart;
        }

        private void OnRestart()
        {
            LeanPool.Despawn(this.gameObject);
        }

        private void OnDisable()
        {
            GameEventManager.Instance.OnGameRestart -= OnRestart;
        }

        public void AddStrength(int strength)
        {
        }
    }
}