using System;
using DefaultNamespace;
using MoreMountains.Tools;
using UnityEngine;

namespace Core
{
    public class BattleManager : MMSingleton<BattleManager>,
        MMEventListener<RunGameEvent>,
        MMEventListener<CoreGameEvent>
    {
        [SerializeField] private GameObject BattlePanel;
        [SerializeField] private GameObject HeroPrefab;
        [SerializeField] private Transform SpawnSocket;

        private PackSystem _packSystem;
        private Character _hero;
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

        private void SetHero(Character character)
        {
            _hero = character;
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
            this.MMEventStartListening<CoreGameEvent>();
            this.MMEventStartListening<RunGameEvent>();
        }

        /// <summary>
        /// OnDisable, we stop listening to events.
        /// </summary>
        protected virtual void OnDisable()
        {
            this.MMEventStopListening<CoreGameEvent>();
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

        public void OnMMEvent(CoreGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case CoreGameEventTypes.Start:
                    OnGameStart();
                    break;
            }
        }

        private void OnGameStart()
        {
            var hero = Instantiate(HeroPrefab, SpawnSocket.position, Quaternion.identity);
            SetHero(hero.GetComponent<Character>());
        }
    }
}