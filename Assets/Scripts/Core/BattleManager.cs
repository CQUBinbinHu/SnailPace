using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DefaultNamespace;
using DG.Tweening;
using Lean.Pool;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core
{
    public class BattleManager : MMSingleton<BattleManager>,
        MMEventListener<RunGameEvent>,
        MMEventListener<CoreGameEvent>
    {
        [SerializeField] public GameObject EncounterEnemyPrefab;
        [SerializeField] private SkillData SkillData;
        [SerializeField] private Transform SkillTransform;
        [SerializeField] private Transform[] SkillSockets;
        [SerializeField] private Transform RewardSkillSocket;
        [SerializeField] private GameObject HeroPrefab;
        [SerializeField] private Transform SpawnSocket;
        [SerializeField] private GameObject ContinueButton;
        [SerializeField] private Image ChoosePanel;

        public bool IsFullSkill;
        private bool _enableTick;
        private List<string> _skillNames;
        private List<LoopSocket> _loopSockets;
        private LoopSocket _currentSocket;
        private Character _hero;
        private Character _encounterEnemy;
        private Dictionary<string, SkillReward> _skillRewardDict;
        private Dictionary<string, SkillComponent> _skillDict;
        public Character Hero => _hero;
        public Character EncounterEnemy => _encounterEnemy;

        protected override void Awake()
        {
            base.Awake();
            _loopSockets = new List<LoopSocket>();
            _skillRewardDict = new Dictionary<string, SkillReward>();
            _skillDict = new Dictionary<string, SkillComponent>();
            _skillNames = new List<string>();
            _enableTick = false;
        }

        private void Start()
        {
            ContinueButton.SetActive(false);
            InitSkillData();
            ResetBattlePanel();
            InitializeSkillSockets();
        }

        private void InitSkillData()
        {
            foreach (var skillReward in SkillData.SkillRewards)
            {
                if (_skillRewardDict.ContainsKey(skillReward.SkillName))
                {
                    Debug.LogWarning("the skill with this name already exists: " + skillReward.name, gameObject);
                }
                else
                {
                    _skillRewardDict.Add(skillReward.SkillName, skillReward);
                }
            }

            _skillNames.Clear();
            _skillNames = _skillRewardDict.Keys.ToList();

            var skills = Resources.LoadAll<SkillComponent>("Skills");
            foreach (var skill in skills)
            {
                if (!_skillDict.ContainsKey(skill.SkillName))
                {
                    _skillDict.Add(skill.SkillName, skill);
                }
            }
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

        public void AddSkill(SkillComponent skill)
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
            skill.transform.SetParent(SkillTransform);
            skill.SetOwner(Hero);
            skill.SetFollow(_currentSocket);
            RunGameEvent.Trigger(RunEventTypes.Continue);
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

        private void SetHero(Character character)
        {
            _hero = character;
        }

        public void SetEncounter(Character target)
        {
            _encounterEnemy = target;
            Hero.BehaviourController.SetEncounter(target);
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
                    ChoosePanel.DOFade(0, 0.3f);
                    ResetBattlePanel();
                    break;
                case RunEventTypes.Reward:
                    ChoosePanel.DOFade(0.5f, 0.3f);
                    break;
            }
        }

        private void StartEncounter()
        {
            _enableTick = true;
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

        private void ResetBattlePanel()
        {
            // TODO: ResetBattlePanel
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
            AddRandomRewards();
            ContinueButton.SetActive(true);
        }

        private void AddRandomRewards()
        {
            List<int> record = new List<int>();
            int count = 0;
            while (count < 3)
            {
                int rand = Random.Range(0, _skillNames.Count);
                bool ok = false;
                while (!ok)
                {
                    if (record.Contains(rand))
                    {
                        rand = Random.Range(0, _skillNames.Count);
                    }
                    else
                    {
                        ok = true;
                    }
                }

                record.Add(rand);
                count += 1;
            }

            foreach (var index in record)
            {
                var skillName = _skillNames[index];
                var skillReward = LeanPool.Spawn(_skillRewardDict[skillName], RewardSkillSocket);
                skillReward.SetSkillObject(_skillDict[skillName]);
            }
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
    }
}