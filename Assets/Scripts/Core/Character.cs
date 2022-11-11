using System;
using UnityEngine;

namespace Core
{
    public enum CharacterType
    {
        Hero,
        Enemy
    }

    public class Character : MonoBehaviour
    {
        [SerializeField] public CharacterType CharacterType;
        [SerializeField] private string Name;

        private HealthComponent _health;
        private BehaviourController _behaviourController;

        public HealthComponent Health => _health;
        public BehaviourController BehaviourController => _behaviourController;

        private void Awake()
        {
            _health = GetComponent<HealthComponent>();
            _behaviourController = GetComponent<BehaviourController>();
        }
    }
}