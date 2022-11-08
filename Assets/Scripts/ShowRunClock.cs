using System;
using Core;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class ShowRunClock : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI TextMeshPro;

        private int _count;
        private float _timer;

        private void Start()
        {
            _count = 3;
            _timer = 1;
            TextMeshPro.text = _count.ToString();
        }

        private void Update()
        {
            switch (GameManager.Instance.CurrentRun)
            {
                case MoveStatus.Idle:
                    if (_timer < 0)
                    {
                        _timer = 1;
                        _count -= 1;
                        TextMeshPro.text = _count.ToString();
                    }
                    else
                    {
                        _timer -= Time.deltaTime;
                    }

                    break;
                case MoveStatus.Run:
                    TextMeshPro.text = GameManager.Instance.RunClock.ToString("0.00");
                    break;
            }
        }
    }
}