using System;
using UnityEngine;

namespace Core
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int MaxHp;
        public int CurrentHp { get; private set; }

        private void Awake()
        {
        }

        private void Start()
        {
            ResetHp();
        }

        private void ResetHp()
        {
            CurrentHp = MaxHp;
        }
    }
}