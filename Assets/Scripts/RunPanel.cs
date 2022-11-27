using System;
using Core;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class RunPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI TextMeshPro;

        private void Update()
        {
            switch (GameManager.Instance.CurrentState)
            {
                case GameStatus.Idle:
                    TextMeshPro.text = GameManager.Instance.CountDown.ToString();
                    break;
                case GameStatus.Run:
                case GameStatus.Encounter:
                    TextMeshPro.text = GameManager.Instance.RunClock.ToString("0.00");
                    break;
            }
        }
    }
}