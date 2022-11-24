using System;
using System.Collections;
using DefaultNamespace;
using Lean.Pool;
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
        public int Armors => _armor;
        public bool IsWithArmor => Armors != 0;

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

        public float GetHpRatio()
        {
            if (CurrentHp + Armors > MaxHp)
            {
                return (float)CurrentHp / (CurrentHp + Armors);
            }
            else
            {
                return (float)CurrentHp / MaxHp;
            }
        }

        public float GetArmorRatio()
        {
            if (CurrentHp + Armors > MaxHp)
            {
                return 1;
            }
            else
            {
                return (float)(CurrentHp + Armors) / MaxHp;
            }
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
            // show tip
            var tip = LeanPool.Spawn(GameManager.Instance.ShowTipComponent);
            tip.transform.position = _owner.TipSocket.position;
            tip.SetTips(damage.ToString(), Color.white);
            // 
            _armor -= damage;
            damage = _armor;
            _armor = Mathf.Clamp(_armor, 0, Int32.MaxValue);
            damage = Mathf.Clamp(damage, Int32.MinValue, 0);
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
                _healthBar.UpdateDamageBar();
            }
        }

        IEnumerator DelayDead_Cro(float delay)
        {
            yield return new WaitForSeconds(delay);
            LeanPool.Despawn(_owner.gameObject);
        }

        public void AddArmor(int armor)
        {
            _armor += armor;
            _armor = Mathf.Clamp(_armor, 0, Int32.MaxValue);
            UpdatePresentation();
        }

        public void RemoveArmor(int armor)
        {
            _armor -= armor;
            _armor = Mathf.Clamp(_armor, 0, Int32.MaxValue);
            UpdatePresentation();
        }
    }
}