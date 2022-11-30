using UnityEngine;

namespace DefaultNamespace
{
    public class StrengthComponent : MonoBehaviour
    {
        private int _strength;
        public int Strength => _strength;

        public void Init()
        {
            _strength = 0;
        }

        public void AddStrength(int strength)
        {
            _strength += strength;
        }
    }
}