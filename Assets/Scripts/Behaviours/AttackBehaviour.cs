using UnityEngine;

namespace Core
{
    public class AttackBehaviour : BaseBehaviour
    {
        public int Atk;

        public override void Perform()
        {
            Character.BehaviourController.CoolDownTimer = CoolDown;
            BattleManager.Instance.OnPlayerContinue(Character.CharacterType);
        }
    }
}