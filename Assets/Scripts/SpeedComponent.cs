﻿using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class SpeedComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI SpeedText;
        [SerializeField] private GameObject ShowSpeedObj;
        private int _speed;
        public int Speed => _speed;
        public float MoveSpeed => SpeedTransition.GetMoveSpeed(_speed);

        public void AddSped(int spd)
        {
            _speed += spd;
            ShowSpeed();
        }

        public void SetSpeed(int spd)
        {
            _speed += spd;
            ShowSpeed();
        }

        private void ShowSpeed()
        {
            SpeedText.text = _speed.ToString();
            if (_speed == 0)
            {
                ShowSpeedObj.SetActive(false);
            }
            else
            {
                ShowSpeedObj.SetActive(true);
                SpeedText.text = _speed.ToString();
            }
        }
    }
}