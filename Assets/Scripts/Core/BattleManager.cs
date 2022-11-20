using System;
using System.Collections.Generic;
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
        [SerializeField] private SkillObject SkillObject;
        [SerializeField] private Transform SkillTransform;
        [SerializeField] private Transform[] SkillSockets;
        [SerializeField] private Transform RewardSkillSocket;
        [SerializeField] private GameObject SkillPrefab;
        [SerializeField] private GameObject HeroPrefab;
        [SerializeField] private Transform SpawnSocket;
        [SerializeField] private GameObject PlayerControlPanel;
        [SerializeField] private GameObject RewardPanel;

        public bool IsFullSkill;
        private List<LoopSocket> _loopSockets;
        private LoopSocket _currentSocket;
        private bool _enableTick;
        private Button[] PlayerControlButtons;
        private Character _hero;
        private Character _encounterEnemy;
        public Character Hero => _hero;
        public Character EncounterEnemy => _encounterEnemy;
        public LoopSocket CurrentSkillSocket => _currentSocket;

        protected override void Awake()
        {
            base.Awake();
            _loopSockets = new List<LoopSocket>();
            _enableTick = false;
        }

        private void Start()
        {
            ResetRewardPanel();
            InitializeBattlePanel();
            EnablePlayerController(false);
            InitializeSkillSockets();
        }

        private void InitializeSkillSockets()
        {
            IsFullSkill = false;
            int index = 0;
            foreach (var trans in SkillSockets)
            {
                _loopSockets.Add(new LoopSocket(index, trans));
                index++;
            }

            for (int i = 1; i < _loopSockets.Count; i++)
            {
                _loopSockets[i].Prev = _loopSockets[i - 1];
            }

            for (int i = 0; i < _loopSockets.Count - 1; i++)
            {
                _loopSockets[i].Next = _loopSockets[i + 1];
            }

            _currentSocket = _loopSockets[0];
        }

        private void Update()
        {
        }

        public void AddSkill(Vector3 initPosition)
        {
            if (_currentSocket.Index == 3)
            {
                IsFullSkill = true;
            }
            else
            {
                IsFullSkill = false;
                _currentSocket = _currentSocket.Next;
            }

            CoreGameEvent.Trigger(CoreGameEventTypes.AddSkill);
            var skill = Instantiate(SkillObject, SkillTransform);
            skill.transform.position = initPosition;
            skill.SetFollow(_currentSocket);
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
                button.interactable = enable;
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
                    InitializeCharacterBattle();
                    StartEncounter();
                    break;
                case RunEventTypes.Continue:
                    EnablePlayerController(false);
                    ResetRewardPanel();
                    break;
            }
        }

        private void StartEncounter()
        {
            _enableTick = true;
            EnablePlayerController(true);
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
            RunGameEvent.Trigger(RunEventTypes.Reward);
            EnablePlayerController(false);
            AddRandomRewards();
        }

        private void AddRandomRewards()
        {
            RewardPanel.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                Instantiate(SkillPrefab, RewardSkillSocket);
            }
        }

        private void ResetRewardPanel()
        {
            for (int i = 0; i < RewardSkillSocket.childCount; i++)
            {
                Destroy(RewardSkillSocket.GetChild(i).gameObject);
            }

            RewardPanel.SetActive(false);
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
        }
    }
}