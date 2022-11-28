using System;
using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameWinningComponent : MonoBehaviour
    {
        [SerializeField] private GameObject GameWiningPanel;

        private void Awake()
        {
            GameWiningPanel.SetActive(false);
        }

        private void OnEnable()
        {
            GameEventManager.Instance.OnGameWinning += OnGameWinning;
        }

        private void OnDisable()
        {
            GameEventManager.Instance.OnGameWinning -= OnGameWinning;
        }

        private void OnGameWinning()
        {
            // TODO: 排行榜，游戏胜利画面
            GameWiningPanel.SetActive(true);
        }

        public void OnRestart()
        {
            GameManager.Instance.Restart();
            GameWiningPanel.SetActive(false);
        }
    }
}