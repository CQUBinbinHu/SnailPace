using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class SkillObject : MonoBehaviour
    {
        public void AddSkill()
        {
            RunGameEvent.Trigger(RunEventTypes.Continue);
        }

        public void OnUse()
        {
            BattleManager.Instance.Hero.BehaviourController.SetCurrent("Attack");
            BattleManager.Instance.OnPerform();
        }
    }
}