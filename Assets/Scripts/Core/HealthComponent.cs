using System;
using DefaultNamespace;
using UnityEngine;

namespace Core
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int MaxHp;
        [SerializeField] private bool ShowHealthBar;
        private HealthBar _healthBar;
        public int CurrentHp { get; private set; }
        public float HpRatio => (float)CurrentHp / (float)MaxHp;

        private void Awake()
        {
            _healthBar = GetComponentInChildren<HealthBar>();
        }

        private void Start()
        {
            ResetHp();
            _healthBar.gameObject.SetActive(ShowHealthBar);
            if (ShowHealthBar)
            {
                _healthBar.UpdateHealthBar();
            }
        }

        private void ResetHp()
        {
            CurrentHp = MaxHp;
        }

        public void Cure(int cure)
        {
            CurrentHp += cure;
            CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHp);
        }

        public void TakeDamage(int damage)
        {
            CurrentHp -= damage;
            CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHp);
        }
    }
}