using System;
using Core;
using MoreMountains.Tools;
using UnityEngine;

namespace DefaultNamespace
{
    public class ContinueRunComponent : MonoBehaviour,
        MMEventListener<CoreGameEvent>,
        MMEventListener<RunGameEvent>
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
                    this.gameObject.SetActive(false);
                    break;
            }
        }

        private void OnEnable()
        {
            this.MMEventStartListening<CoreGameEvent>();
            this.MMEventStartListening<RunGameEvent>();
        }

        private void OnDisable()
        {
            this.MMEventStopListening<CoreGameEvent>();
            this.MMEventStopListening<RunGameEvent>();
        }

        public void OnMMEvent(RunGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case RunEventTypes.Continue:
                    this.gameObject.SetActive(false);
                    break;
            }
        }
    }
}