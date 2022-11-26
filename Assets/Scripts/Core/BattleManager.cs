﻿using System;
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
    public class BattleManager : MMSingleton<BattleManager>
    {
        [SerializeField] private GameObject HeroPrefab;
        [SerializeField] public GameObject EncounterEnemyPrefab;
        [SerializeField] private SkillData SkillData;
        [SerializeField] private Transform SkillTransform;
        [SerializeField] private Transform[] SkillSockets;
        [SerializeField] private Transform RewardSkillSocket;
        [SerializeField] private Transform SpawnSocket;
        [SerializeField] private GameObject ContinueButton;
        [SerializeField] private Image ChoosePanel;
        [SerializeField] private Transform SkillViewSocket;
        [SerializeField] private Transform SkillView;

        private List<string> _skillNames;
        private Character _hero;
        private Character _encounterEnemy;
        public BattleStatus Status;
        private Dictionary<string, SkillReward> _skillRewardDict;
        private Dictionary<string, SkillComponent> _skillDict;
        public Character Hero => _hero;
        public Character EncounterEnemy => _encounterEnemy;

        public enum BattleStatus
        {
            Run,
            Encounter,
            Reward
        }

        protected override void Awake()
        {
            base.Awake();
            _skillRewardDict = new Dictionary<string, SkillReward>();
            _skillDict = new Dictionary<string, SkillComponent>();
            _skillNames = new List<string>();
        }

        private void Start()
        {
            Status = BattleStatus.Run;
            ContinueButton.SetActive(false);
            SkillView.gameObject.SetActive(false);
            //
            var color = ChoosePanel.color;
            color.a = 0;
            ChoosePanel.color = color;
            //
            InitSkillData();
            ResetBattlePanel();
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

        private void OnAddSkill(SkillReward skillReward)
        {
            skillReward.transform.SetParent(SkillViewSocket);
            CopySkill(skillReward.SkillTarget);
            // TODO: Run Continue Delay
            GameEventManager.Instance.OnRunContinue.Invoke();
        }

        private void CopySkill(SkillComponent skillTarget)
        {
            var skill = Instantiate(skillTarget, SkillTransform);
            _hero.BehaviourController.AddSkill(skill);
            skill.SetOwner(Hero);
            skill.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (Status != BattleStatus.Encounter || GameManager.Instance.IsPaused)
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

        /// <summary>
        /// OnDisable, we start listening to events.
        /// </summary>
        protected virtual void OnEnable()
        {
            // GameEventManager.Instance.OnGameStart += OnGameStart;
            // GameEventManager.Instance.OnGamePause += OnGamePause;
            // GameEventManager.Instance.OnGameContinue += OnGameContinue;

            GameEventManager.Instance.OnRunEncounter += OnRunEncounter;
            GameEventManager.Instance.OnRunReward += OnRunReward;
            GameEventManager.Instance.OnRunContinue += OnRunContinue;
            GameEventManager.Instance.OnEnemyDead += OnBattleEnd;
            GameEventManager.Instance.OnGameOver += OnGameOver;
            GameEventManager.Instance.OnAddSkill += OnAddSkill;
        }

        /// <summary>
        /// OnDisable, we stop listening to events.
        /// </summary>
        protected virtual void OnDisable()
        {
            GameEventManager.Instance.OnRunEncounter -= OnRunEncounter;
            GameEventManager.Instance.OnRunReward -= OnRunReward;
            GameEventManager.Instance.OnRunContinue -= OnRunContinue;
            GameEventManager.Instance.OnEnemyDead -= OnBattleEnd;
            GameEventManager.Instance.OnGameOver -= OnGameOver;
            GameEventManager.Instance.OnAddSkill -= OnAddSkill;
        }

        private void OnRunEncounter(Character target)
        {
            _encounterEnemy = target;
            Status = BattleStatus.Encounter;
            Hero.BehaviourController.SetTarget(EncounterEnemy);
            Hero.BehaviourController.Initialize();
            _encounterEnemy.BehaviourController.SetTarget(Hero);
            _encounterEnemy.BehaviourController.Initialize();
        }

        private void OnRunContinue()
        {
            ResetBattlePanel();
        }

        private void OnRunReward()
        {
            ChoosePanel.DOFade(0.5f, 0.3f);
        }

        private void ResetBattlePanel()
        {
            // TODO: ResetBattlePanel
            ChoosePanel.DOFade(0, 0.3f);
        }

        private void OnBattleEnd()
        {
            Status = BattleStatus.Reward;
            GameEventManager.Instance.OnRunReward.Invoke();
            AddRandomRewards();
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

        public void OnGameStart()
        {
            var hero = LeanPool.Spawn(HeroPrefab, SpawnSocket.position, Quaternion.identity);
            SetHero(hero.GetComponent<Character>());
        }

        public void EnableSkillView(bool enable)
        {
            SkillView.gameObject.SetActive(enable);
            if (enable)
            {
                GameEventManager.Instance.OnGameContinue.Invoke();
            }
            else
            {
                GameEventManager.Instance.OnGamePause.Invoke();
            }
        }
    }
}