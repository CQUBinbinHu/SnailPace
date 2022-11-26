using Core;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace DefaultNamespace.SkillTasks
{
    [Category("Snail")]
    public class PerformCurrentSkill : ActionTask
    {
        private Character Owner;
        public CompactStatus finishStatus = CompactStatus.Success;

        protected override string OnInit()
        {
            Owner = this.agent.gameObject.GetComponent<Character>();
            return base.OnInit();
        }

        protected override void OnExecute()
        {
            base.OnExecute();
            Owner.BehaviourController.CurrentSkill.OnUse();
            EndAction(finishStatus == CompactStatus.Success ? true : false);
        }

        protected override void OnStop()
        {
            base.OnStop();
            Owner.BehaviourController.CurrentSkill.OnCancel();
        }
    }
}