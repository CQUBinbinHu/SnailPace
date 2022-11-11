using UnityEngine;

namespace Core
{
    public class SleepBehaviour : BaseBehaviour
    {
        public override void Perform()
        {
            Character.BehaviourController.CoolDownTimer = CoolDown;
            BattleManager.Instance.OnPlayerContinue(Character.CharacterType);
        }
    }
}