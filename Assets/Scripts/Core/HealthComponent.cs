using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;

namespace Core
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int MaxHp;
        [SerializeField] private bool ShowHealthBar;
        private int _armor;
        private bool _isDead;
        private Character _owner;
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
            _armor = 0;
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
            damage = (int)(_owner.GetBuffDamageMultiplier() * damage);
            damage = _armor - damage;
            damage = Mathf.Clamp(damage, damage, 0);
            ChangeHp(damage);
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

            StartCoroutine(DelayDead_Cro(0.6f));
        }

        private void UpdatePresentation()
        {
            if (ShowHealthBar)
            {
                _healthBar.UpdateHealthBar();
            }
        }

        IEnumerator DelayDead_Cro(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }

        public void AddArmor(int armor)
        {
            _armor += armor;
        }

        public void RemoveArmor(int armor)
        {
            _armor -= armor;
        }
    }
}