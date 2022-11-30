using Core;
using HeroPerform;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DefaultNamespace.SkillTasks
{
    [Category("Snail")]
    public class SetIntentSkill : ActionTask
    {
        public SkillComponent Current;
        private Character Owner;

        protected override string OnInit()
        {
            Owner = this.agent.gameObject.GetComponent<Character>();
            Current.SetTarget(BattleManager.Instance.Hero);
            return base.OnInit();
        }

        protected override void OnExecute()
        {
            base.OnExecute();
            Current.RefreshStatus();
            Owner.BehaviourController.SetCurrent(Current);
            EndAction(true);
        }
    }
}