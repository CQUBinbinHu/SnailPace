using MoreMountains.Tools;
using UnityEngine;

namespace Core
{
    public enum CoreGameEventTypes
    {
        Start,
        GameOver,
        Pause,
        EncounterBattle
    }

    public struct CoreGameEvent
    {
        public CoreGameEventTypes EventType;

        public CoreGameEvent(CoreGameEventTypes eventType)
        {
            EventType = eventType;
        }

        static CoreGameEvent e;

        public static void Trigger(CoreGameEventTypes eventType)
        {
            e.EventType = eventType;
            MMEventManager.TriggerEvent(e);
        }
    }

    public class GameManager :
        MMPersistentSingleton<GameManager>,
        MMEventListener<CoreGameEvent>
    {

        public GameObject EncounterGo;
        
        public void OnMMEvent(CoreGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case CoreGameEventTypes.Start:
                    break;
                case CoreGameEventTypes.GameOver:
                    break;
                case CoreGameEventTypes.Pause:
                    break;
                case CoreGameEventTypes.EncounterBattle:
                    Debug.Log("Debug EncounterBattle");
                    break;
            }
        }

        /// <summary>
        /// OnDisable, we start listening to events.
        /// </summary>
        protected virtual void OnEnable()
        {
            this.MMEventStartListening<CoreGameEvent>();
        }

        /// <summary>
        /// OnDisable, we stop listening to events.
        /// </summary>
        protected virtual void OnDisable()
        {
            this.MMEventStopListening<CoreGameEvent>();
        }
    }
}