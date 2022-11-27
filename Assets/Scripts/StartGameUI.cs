using System;
using System.Collections;
using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class StartGameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI StartGameText;
        [SerializeField] private TextMeshProUGUI StartGameTextSubTitle;
        [SerializeField] private LoadProgress LoadProgress;
        [SerializeField] private float JumpForce;
        [SerializeField] private float JumpDuration;
        [SerializeField] private float FadeDuration;
        private Color _fadeColor;
        private Vector3 _startPos;
        private Vector3 EndPos => _startPos + JumpForce * Vector3.right;
        private bool _isOnSplash;

        private void Start()
        {
            _isOnSplash = true;
            _fadeColor = StartGameText.color;
            _startPos = StartGameText.transform.localPosition;
            LoadProgress.gameObject.SetActive(false);
            StartCoroutine(ShowText_Cro());
        }

        IEnumerator ShowText_Cro()
        {
            while (_isOnSplash)
            {
                StartGameText.color = _fadeColor;
                StartGameText.transform.localPosition = _startPos;
                StartGameText.transform.DOLocalMove(EndPos, JumpDuration);
                yield return new WaitForSeconds(JumpDuration - FadeDuration);
                StartGameText.DOFade(0, FadeDuration);
                yield return new WaitForSeconds(FadeDuration);
            }

            StartGameText.DOFade(0, 0.2f);
            StartGameTextSubTitle.DOFade(0, 0.2f);
        }

        public void OnStartGame(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    StartGame();
                    break;
            }
        }

        private void StartGame()
        {
            _isOnSplash = false;
            LoadProgress.gameObject.SetActive(true);
            LoadProgress.SetWalk();
            GameManager.Instance.StartGame();
        }
    }
}