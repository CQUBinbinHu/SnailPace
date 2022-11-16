using System;
using DefaultNamespace;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class BattleManager : MMSingleton<BattleManager>,
        MMEventListener<RunGameEvent>,
        MMEventListener<CoreGameEvent>
    {
        [SerializeField] private GameObject BattlePanel;
        [SerializeField] private GameObject HeroPrefab;
        [SerializeField] private Transform SpawnSocket;
        [SerializeField] private GameObject PlayerControlPanel;

        private Button[] PlayerControlButtons;
        [SerializeField] private PackSystem _packSystem;
        private bool _enableTick;
        private Character _hero;
        private Character _encounterEnemy;
        public Character EncounterEnemy => _encounterEnemy;

        public Character Hero => _hero;

        protected override void Awake()
        {
            base.Awake();
            BattlePanel.SetActive(false);
            _enableTick = false;
        }

        private void FixedUpdate()
        {
            if (!_enableTick)
            {
                return;
            }

            Hero.BehaviourController.FixedTick(Time.deltaTime);
            _encounterEnemy.BehaviourController.FixedTick(Time.deltaTime);
        }

        private void EnablePlayerController(bool enable)
        {
            // PlayerControlPanel.SetActive(enable);
            foreach (var button in PlayerControlButtons)
            {
                button.enabled = enable;
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
                    InitializeBattlePanel();
                    InitializeCharacterBattle();
                    break;
            }
        }

        public void OnMMEvent(CoreGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case CoreGameEventTypes.Start:
                    OnGameStart();
                    break;
                case CoreGameEventTypes.EnemyDead:
                    OnBattleEnd();
                    break;
                case CoreGameEventTypes.GameOver:
                    OnGameOver();
                    break;
            }
        }

        private void InitializeBattlePanel()
        {
            _enableTick = true;
            BattlePanel.SetActive(true);
            PlayerControlButtons = PlayerControlPanel.GetComponentsInChildren<Button>();
            EnablePlayerController(true);
        }

        private void InitializeCharacterBattle()
        {
            Hero.BehaviourController.Initialize();
            _encounterEnemy.BehaviourController.Initialize();
        }

        private void OnBattleEnd()
        {
            _enableTick = false;
            BattlePanel.SetActive(false);
            _packSystem.OpenReward();
        }

        private void OnGameOver()
        {
            // TODO: do GameOver
        }

        private void OnGameStart()
        {
            var hero = Instantiate(HeroPrefab, SpawnSocket.position, Quaternion.identity);
            SetHero(hero.GetComponent<Character>());
        }

        public void OnPerform()
        {
            Hero.BehaviourController.CurrentBehaviour.Perform();
        }

        public void BattleCallBack(CharacterType characterType)
        {
            _enableTick = true;
        }
    }
}