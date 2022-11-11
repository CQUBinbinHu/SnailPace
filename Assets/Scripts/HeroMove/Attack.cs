using Core;
using UnityEngine;

namespace DefaultNamespace.HeroMove
{
    public class Attack : MonoBehaviour
    {
        public void OnUse()
        {
            BattleManager.Instance.Hero.BehaviourController.SetCurrent("Attack");
            BattleManager.Instance.OnPerform();
        }
    }
}