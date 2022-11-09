using System;
using DefaultNamespace;
using MoreMountains.Tools;
using UnityEngine;

namespace Core
{
    public class BattleManager : MMSingleton<BattleManager>,
        MMEventListener<RunGameEvent>
    {
        [SerializeField] private GameObject BattlePanel;
        private PackSystem _packSystem;
        private Character _encounterEnemy;

        protected override void Awake()
        {
            base.Awake();
            BattlePanel.SetActive(false);
            _packSystem = GetComponent<PackSystem>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnBattleEnd();
            }
        }

        public void SetEncounter(Character character)
        {
            _encounterEnemy = character;
        }

        /// <summary>
        /// OnDisable, we start listening to events.
        /// </summary>
        protected virtual void OnEnable()
        {
            // this.MMEventStartListening<CoreGameEvent>();
            this.MMEventStartListening<RunGameEvent>();
        }

        /// <summary>
        /// OnDisable, we stop listening to events.
        /// </summary>
        protected virtual void OnDisable()
        {
            // this.MMEventStopListening<CoreGameEvent>();
            this.MMEventStopListening<RunGameEvent>();
        }

        public void OnMMEvent(RunGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case RunEventTypes.Encounter:
                    BattlePanel.SetActive(true);
                    break;
            }
        }

        public void OnBattleEnd()
        {
            if (_encounterEnemy)
            {
                Destroy(_encounterEnemy.gameObject);
            }

            BattlePanel.SetActive(false);
            _packSystem.OpenReward();
        }
    }
}