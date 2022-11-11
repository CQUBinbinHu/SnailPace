using Core;
using UnityEngine;

namespace HeroPerform
{
    public class Attack : MonoBehaviour, IUsePerform
    {
        public void OnUse()
        {
            BattleManager.Instance.Hero.BehaviourController.SetCurrent("Attack");
            BattleManager.Instance.OnPerform();
        }
    }
}