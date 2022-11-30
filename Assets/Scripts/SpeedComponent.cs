using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class SpeedComponent : MonoBehaviour
    {
        private int _speed;
        public int Speed => _speed;
        public float MoveSpeed => SpeedTransition.GetMoveSpeed(_speed);

        public void Init()
        {
            _speed = 0;
        }

        public void AddSpeed(int spd)
        {
            _speed += spd;
        }

        public void SetSpeed(int spd)
        {
            _speed += spd;
        }
    }
}