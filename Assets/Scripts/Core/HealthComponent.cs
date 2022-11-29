using System;
using System.Collections;
using DefaultNamespace;
using Lean.Pool;
using UnityEngine;

namespace Core
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] public int MaxHp;
        [SerializeField] private bool ShowHealthBar;
        private int _armor;
        private bool _isDead;
        private Character _owner;
        private HealthBar _healthBar;
        public int CurrentHp { get; private set; }
        public int Armors => _armor;
        public bool IsWithArmor => Armors != 0;
        public float ArmorCountDown => _armorTimer / RemoveArmorAfter;
        public bool IsDead => _isDead;

        private float _armorTimer;
        private const float RemoveArmorAfter = 3;

        private void Awake()
        {
            _owner = GetComponent<Character>();
            _healthBar = GetComponentInChildren<HealthBar>();
        }

        public void Initialize()
        {
            _isDead = false;
            ResetArmorTimer();
            ResetHp();
            if (_healthBar)
            {
                _healthBar.gameObject.SetActive(ShowHealthBar);
                _healthBar.Initialize();
            }
            else
            {
                ShowHealthBar = false;
            }
        }

        private void FixedUpdate()
        {
            TickUpdateArmor(Time.fixedDeltaTime);
        }

        private void TickUpdateArmor(float fixedDeltaTime)
        {
            if (_armor == 0) return;
            if (_armorTimer > 0)
            {
                _armorTimer -= fixedDeltaTime;
            }
            else
            {
                Debug.Log("debug RemoveArmor");
                RemoveArmor(Int32.MaxValue);
            }
        }

        private void Update()
        {
            if (ShowHealthBar)
            {
                _healthBar.UpdateArmorPresentation();
            }
        }

        private void ResetArmorTimer()
        {
            _armorTimer = RemoveArmorAfter;
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
            _armor = 0;
            CurrentHp = MaxHp;
        }

        public void Cure(int cure)
        {
            ChangeHp(cure);
            _healthBar.UpdateDamageBar();
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
            _healthBar.UpdateDamageBar();
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
                    GameEventManager.Instance.OnGameOver.Invoke();
                    break;
                case CharacterType.Enemy:
                    Debug.Log("OnEnemyDead");
                    GameEventManager.Instance.OnEnemyDead.Invoke();
                    break;
            }

            LeanPool.Despawn(this);
            // StartCoroutine(DelayDead_Cro(0.6f));
        }

        // IEnumerator DelayDead_Cro(float delay)
        // {
        //     yield return new WaitForSeconds(delay);
        //     LeanPool.Despawn(_owner.gameObject);
        // }

        public void AddArmor(int armor)
        {
            _armor += armor;
            _armor = Mathf.Clamp(_armor, 0, Int32.MaxValue);
            ResetArmorTimer();
            if (ShowHealthBar)
            {
                _healthBar.UpdateDamageBar();
                _healthBar.UpdateArmorPresentation();
            }
        }

        private void RemoveArmor(int armor)
        {
            _armor -= armor;
            _armor = Mathf.Clamp(_armor, 0, Int32.MaxValue);
            if (ShowHealthBar)
            {
                _healthBar.UpdateDamageBar();
                _healthBar.UpdateArmorPresentation();
            }
        }
    }
}