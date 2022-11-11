using Core;
using UnityEngine;

namespace HeroPerform
{
    public class Defense : MonoBehaviour, IUsePerform
    {
        public void OnUse()
        {
            BattleManager.Instance.Hero.BehaviourController.SetCurrent("Defense");
            BattleManager.Instance.OnPerform();
        }
    }
}