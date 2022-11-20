using System;
using Core;
using MoreMountains.Tools;
using UnityEngine;

namespace DefaultNamespace
{
    public class ContinueRunComponent : MonoBehaviour, MMEventListener<CoreGameEvent>
    {
        public void OnContinue()
        {
            RunGameEvent.Trigger(RunEventTypes.Continue);
        }

        public void OnMMEvent(CoreGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case CoreGameEventTypes.AddSkill:
                    OnContinue();
                    break;
            }
        }

        private void OnEnable()
        {
            this.MMEventStartListening<CoreGameEvent>();
        }

        private void OnDisable()
        {
            this.MMEventStopListening<CoreGameEvent>();
        }
    }
}