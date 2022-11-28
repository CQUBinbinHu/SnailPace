using System;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class InputConnector : MonoBehaviour
    {
        [SerializeField] private Image KeyUp;
        [SerializeField] private Image keyDown;

        private SkillComponent _current;
        private bool _interactable;

        private void Awake()
        {
            _interactable = false;
        }

        private void Start()
        {
            if (KeyUp)
            {
                KeyUp.gameObject.SetActive(true);
            }

            if (keyDown)
            {
                keyDown.gameObject.SetActive(false);
            }
        }

        public void InputCallBack(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    if (GameManager.Instance.IsPaused)
                    {
                        return;
                    }

                    switch (GameManager.Instance.CurrentState)
                    {
                        case GameStatus.Encounter:
                        case GameStatus.Run:
                            // Debug.Log(context.action.name + " " + "Started");
                            KeyUp.gameObject.SetActive(false);
                            keyDown.gameObject.SetActive(true);
                            if (!_interactable)
                            {
                                return;
                            }

                            _current.OnUse();
                            break;
                    }

                    break;
                case InputActionPhase.Performed:
                    // Debug.Log(context.action.name + " " + "Performed");
                    break;
                case InputActionPhase.Canceled:
                    // Debug.Log(context.action.name + " " + "Canceled");
                    KeyUp.gameObject.SetActive(true);
                    keyDown.gameObject.SetActive(false);
                    if (!_interactable)
                    {
                        return;
                    }

                    _current.OnCancel();
                    break;
                case InputActionPhase.Waiting:
                    // Debug.Log(context.action.name + " " + "Waiting");
                    break;
                case InputActionPhase.Disabled:
                    // Debug.Log(context.action.name + " " + "Disabled");
                    break;
            }
        }

        public void SetSkill(SkillComponent skill)
        {
            _current = skill;
            SetInteractable(true);
        }

        public void SetInteractable(bool enable)
        {
            _interactable = enable;
        }
    }
}