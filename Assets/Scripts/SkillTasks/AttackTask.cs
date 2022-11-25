using Core;
using HeroPerform;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace DefaultNamespace.SkillTasks
{
    [Category("Custom")]
    public class AttackTask : ActionTask
    {
        public Attack Attack;
        public BBParameter<int> Atk = 0;

        protected override string OnInit()
        {
            Attack.SetTarget(BattleManager.Instance.Hero);
            return base.OnInit();
        }

        protected override void OnExecute()
        {
            base.OnExecute();
            Attack.Atk = Atk.value;
            Attack.OnUse();
            EndAction(true);
        }

        protected override void OnStop()
        {
            base.OnStop();
            Attack.OnCancel();
        }
    }
}