using System;
using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class RestartGameComponent : MonoBehaviour
    {
        [SerializeField] private GameObject GameOverPanel;

        private void Awake()
        {
            GameOverPanel.SetActive(false);
        }

        private void OnEnable()
        {
            GameEventManager.Instance.OnGameOver += OnGameOver;
        }

        private void OnGameOver()
        {
            GameOverPanel.SetActive(true);
        }

        private void OnDisable()
        {
            GameEventManager.Instance.OnGameOver -= OnGameOver;
        }

        public void OnRestart()
        {
            GameManager.Instance.Restart();
            GameOverPanel.SetActive(false);
        }
    }
}