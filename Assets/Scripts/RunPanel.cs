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
                    TextMeshPro.text = GameManager.Instance.RunClock;
                    break;
            }
        }

        private void OnEnable()
        {
            GameEventManager.Instance.OnGameWinning += OnWinning;
        }

        private void OnWinning()
        {
            TextMeshPro.text = GameManager.Instance.RunClock;
        }

        private void OnDisable()
        {
            GameEventManager.Instance.OnGameWinning -= OnWinning;
        }
    }
}