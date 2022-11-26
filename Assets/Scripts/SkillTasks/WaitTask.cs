using Core;
using NodeCanvas.Framework;
using ParadoxNotion;
using UnityEngine;

namespace DefaultNamespace.SkillTasks
{
    public class WaitTask : ActionTask
    {
        public Character Owner;
        public Intent ShowIntent;
        public BBParameter<float> waitTime = 1f;
        public BBParameter<int> IntentAttack;
        public CompactStatus finishStatus = CompactStatus.Success;

        protected override string info
        {
            get { return string.Format("Wait {0} sec.", waitTime); }
        }

        protected override void OnUpdate()
        {
            if (elapsedTime >= waitTime.value)
            {
                Owner.BehaviourController.IsOnCountDown = false;
                EndAction(finishStatus == CompactStatus.Success ? true : false);
            }
            else
            {
                Owner.BehaviourController.IsOnCountDown = true;
                Owner.BehaviourController.CountDownRatio = 1 - elapsedTime / waitTime.value;
                Owner.BehaviourController.CountDown = 1 + (int)(waitTime.value - elapsedTime);
            }
        }

        protected override string OnInit()
        {
            Owner = this.agent.gameObject.GetComponent<Character>();
            return base.OnInit();
        }

        protected override void OnExecute()
        {
            base.OnExecute();
            Owner.BehaviourController.Intent.SetIntent(ShowIntent, IntentAttack.value);
        }
    }
}