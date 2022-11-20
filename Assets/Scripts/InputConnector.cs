using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class InputConnector : MonoBehaviour
    {
        private SkillComponent _current;
        private bool _interactable;

        private void Awake()
        {
            _interactable = false;
        }

        public void InputCallBack(InputAction.CallbackContext context)
        {
            if (!_interactable)
            {
                return;
            }

            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Debug.Log(context.action.name + " " + "Started");
                    _current.OnUse();
                    break;
                case InputActionPhase.Performed:
                    Debug.Log(context.action.name + " " + "Performed");
                    break;
                case InputActionPhase.Canceled:
                    Debug.Log(context.action.name + " " + "Canceled");
                    _current.OnCancel();
                    break;
                case InputActionPhase.Waiting:
                    Debug.Log(context.action.name + " " + "Waiting");
                    break;
                case InputActionPhase.Disabled:
                    Debug.Log(context.action.name + " " + "Disabled");
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