using Core;
using HeroPerform;
using NodeCanvas.Framework;
using UnityEngine;

namespace DefaultNamespace.SkillTasks
{
    public class AttackTask : ActionTask
    {
        public Attack Attack;
        public int Atk;

        protected override string OnInit()
        {
            Attack.SetTarget(BattleManager.Instance.Hero);
            return base.OnInit();
        }

        protected override void OnExecute()
        {
            base.OnExecute();
            Attack.Atk = Atk;
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