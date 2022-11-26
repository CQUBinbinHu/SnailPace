using Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class EnableViewPanelComponent : MonoBehaviour
    {
        public void OnViewSkills(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    BattleManager.Instance.ChangeSkillView();               
                    break;
            }
        }
    }
}