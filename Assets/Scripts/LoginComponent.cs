using System;
using System.Collections;
using Core;
using LootLocker;
using LootLocker.Requests;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class LoginComponent : MonoBehaviour
    {
        [SerializeField] private GameObject LoginPanel;
        [SerializeField] private TMP_InputField InputField;
        [SerializeField] private TextMeshProUGUI InfoText;
        [SerializeField] private StartGameUI StartGameUI;

        private void Awake()
        {
            LoginPanel.SetActive(true);
            InfoText.text = string.Empty;
        }

        private void Start()
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerName")))
            {
                LoginPanel.SetActive(false);
                GameManager.Instance.IsSuccessRegistered = true;
                StartGameUI.InitSplash();
            }
        }

        public void OnConfirmLogin()
        {
            StartCoroutine(SetupRoutine());
        }

        IEnumerator SetupRoutine()
        {
            // Set the info text to loading
            InfoText.text = "Logging in...";
            // Wait while the login is happening
            yield return GameManager.Instance.LoginRoutine();
            // If the player couldn't log in, let them know, and then retry
            if (!GameManager.Instance.LoggedIn)
            {
                float loginCountdown = 4;
                float timer = loginCountdown;
                while (timer >= -1f)
                {
                    timer -= Time.deltaTime;
                    // Update the text when we get to a new number
                    if (Mathf.CeilToInt(timer) != Mathf.CeilToInt(loginCountdown))
                    {
                        InfoText.text = "Failed to login retrying in " + Mathf.CeilToInt(timer).ToString();
                        loginCountdown -= 1f;
                    }

                    yield return null;
                }

                StartCoroutine(SetupRoutine());
                yield break;
            }

            SuccessRegister();
            yield return null;
        }

        private void SuccessRegister()
        {
            LoginPanel.SetActive(false);
            PlayerPrefs.SetString("PlayerName", InputField.text);
            GameManager.Instance.IsSuccessRegistered = true;
            StartGameUI.InitSplash();
        }
    }
}