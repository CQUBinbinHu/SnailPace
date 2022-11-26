using System;
using System.Collections;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class HelpComponent : MonoBehaviour
    {
        public GameObject HelpCanvas;
        public Image HelpPanel;
        public bool IsTurnOn;

        private void Start()
        {
            IsTurnOn = false;
            SetPanelAlpha(0);
            HelpCanvas.SetActive(false);
        }

        public void OnTriggerHelp(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OnTurnHelp();
                    break;
            }
        }

        private void OnTurnHelp()
        {
            EnableHelpContent(!IsTurnOn);
        }

        public void EnableHelpContent(bool enable)
        {
            IsTurnOn = enable;
            if (enable)
            {
                GameEventManager.Instance.OnGamePause.Invoke();
                HelpCanvas.SetActive(true);
                SetPanelAlpha(0);
                HelpPanel.DOFade(0.7f, 0.2f);
            }
            else
            {
                SetPanelAlpha(0.7f);
                HelpPanel.DOFade(0, 0.2f);
                StartCoroutine(DeActiveDelay(0.2f));
            }
        }

        IEnumerator DeActiveDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            HelpPanel.DOKill();
            HelpCanvas.SetActive(false);
            GameEventManager.Instance.OnGameContinue.Invoke();
        }

        private void SetPanelAlpha(float a)
        {
            var color = HelpPanel.color;
            color.a = a;
            HelpPanel.color = color;
        }
    }
}