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
        [SerializeField] private CoolDownBar HeroCoolDownBar;
        [SerializeField] private CoolDownBar EnemyCoolDownBar;
        [SerializeField] private int FixedFrame = 3;

        private Button[] PlayerControlButtons;
        [SerializeField] private PackSystem _packSystem;
        private Character _hero;
        private Character _encounterEnemy;
        public Character EncounterEnemy => _encounterEnemy;

        private int _frameCount;
        private bool _heroEnableTick;
        private bool _enemyEnableTick;
        private bool _enableTick;
        public Character Hero => _hero;

        public float HeroCoolDownRatio => (float)_hero.BehaviourController.CoolDownTimer
                                          / _hero.BehaviourController.CurrentBehaviour.CoolDown;

        public float EnemyCoolDownRatio => (float)_encounterEnemy.BehaviourController.CoolDownTimer
                                           / _encounterEnemy.BehaviourController.CurrentBehaviour.CoolDown;

        protected override void Awake()
        {
            base.Awake();
            BattlePanel.SetActive(false);
            _enableTick = false;
        }

        private void Start()
        {
            _frameCount = 0;
            SetHeroTickEnable(true);
            SetEnemyTickEnable(true);
        }

        public void SetHeroTickEnable(bool enable)
        {
            _heroEnableTick = enable;
        }

        public void SetEnemyTickEnable(bool enable)
        {
            _enemyEnableTick = enable;
        }

        private void Update()
        {
            if (_enableTick)
            {
                HeroCoolDownBar.SetFillAmount(HeroCoolDownRatio);
                EnemyCoolDownBar.SetFillAmount(EnemyCoolDownRatio);
            }
        }

        private void FixedUpdate()
        {
            if (!_enableTick)
            {
                return;
            }

            CheckBehaviourCoolDown();
            if (!(_enemyEnableTick && _heroEnableTick))
            {
                return;
            }

            if (_frameCount == FixedFrame)
            {
                _frameCount = 0;
                Hero.BehaviourController.TickCoolDown();
                _encounterEnemy.BehaviourController.TickCoolDown();
            }
            else
            {
                _frameCount += 1;
            }
        }

        private void CheckBehaviourCoolDown()
        {
            if (_hero.BehaviourController.CoolDownTimer == 0)
            {
                _enableTick = false;
                SetHeroTickEnable(false);
                EnablePlayerController(true);
            }
            else if (_encounterEnemy.BehaviourController.CoolDownTimer == 0)
            {
                SetEnemyTickEnable(false);
                _encounterEnemy.BehaviourController.Tick();
            }
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
            EnablePlayerController(false);
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
            EnablePlayerController(false);
            Hero.BehaviourController.CurrentBehaviour.Perform();
        }

        public void OnPlayerContinue(CharacterType characterType)
        {
            _enableTick = true;
            switch (characterType)
            {
                case CharacterType.Hero:
                    SetHeroTickEnable(true);
                    break;
                case CharacterType.Enemy:
                    SetEnemyTickEnable(true);
                    break;
            }
        }
    }
}