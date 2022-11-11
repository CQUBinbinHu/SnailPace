using System;
using DefaultNamespace;
using UnityEngine;

namespace Core
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int MaxHp;
        [SerializeField] private bool ShowHealthBar;
        private Character _owner;
        private bool _isDead;
        private HealthBar _healthBar;
        public int CurrentHp { get; private set; }
        public float HpRatio => (float)CurrentHp / (float)MaxHp;

        private void Awake()
        {
            _owner = GetComponent<Character>();
            _healthBar = GetComponentInChildren<HealthBar>();
        }

        private void Start()
        {
            _isDead = false;
            ResetHp();
            _healthBar.gameObject.SetActive(ShowHealthBar);
            UpdatePresentation();
        }

        private void ResetHp()
        {
            CurrentHp = MaxHp;
        }

        public void Cure(int cure)
        {
            ChangeHp(cure);
            UpdatePresentation();
        }

        public void TakeDamage(int damage)
        {
            ChangeHp(-damage);
            UpdatePresentation();
        }

        private void ChangeHp(int hp)
        {
            CurrentHp += hp;
            CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHp);
            if (CurrentHp == 0)
            {
                Dead();
            }
        }

        private void Dead()
        {
            if (_isDead)
            {
                return;
            }

            _isDead = true;
            switch (_owner.CharacterType)
            {
                case CharacterType.Hero:
                    CoreGameEvent.Trigger(CoreGameEventTypes.GameOver);
                    break;
                case CharacterType.Enemy:
                    CoreGameEvent.Trigger(CoreGameEventTypes.EnemyDead);
                    break;
            }

            Destroy(gameObject);
        }

        private void UpdatePresentation()
        {
            if (ShowHealthBar)
            {
                _healthBar.UpdateHealthBar();
            }
        }
    }
}