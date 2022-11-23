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
        [SerializeField] private GameObject Target;
        public void OnContinue()
        {
            RunGameEvent.Trigger(RunEventTypes.Continue);
        }

        public void OnMMEvent(CoreGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case CoreGameEventTypes.AddSkill:
                    Target.SetActive(false);
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
                    Target.SetActive(false);
                    break;
            }
        }
    }
}