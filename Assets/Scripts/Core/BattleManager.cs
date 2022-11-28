﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DefaultNamespace;
using DG.Tweening;
using Lean.Pool;
using MoreMountains.Tools;
using ParadoxNotion;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core
{
    public class BattleManager : MMSingleton<BattleManager>
    {
        [SerializeField] private SkillReward[] InitSkills;
        [SerializeField] private GameObject HeroPrefab;
        [SerializeField] private GameObject EncounterEnemyPrefab;
        [SerializeField] private SkillData SkillData;
        [SerializeField] private Transform SkillTransform;
        [SerializeField] private Transform[] SkillSockets;
        [SerializeField] private Transform RewardSkillSocket;
        [SerializeField] private Transform SpawnSocket;
        [SerializeField] private GameObject ContinueButton;
        [SerializeField] private Image ChoosePanel;
        [SerializeField] private Transform SkillViewSocket;
        [SerializeField] private Transform SkillView;
        private LoopMoveGrid _loopMoveGrid;

        public GameObject EncounterPrefab => EncounterEnemyPrefab;
        private List<LoopSocket> _loopSockets;
        private List<SkillComponent> _currentSkills;
        private List<string> _skillNames;
        private Character _hero;
        private Character _encounterEnemy;
        public BattleStatus Status;
        private Dictionary<string, SkillReward> _skillRewardDict;
        private Dictionary<string, SkillComponent> _skillDict;
        public Character Hero => _hero;
        public Character EncounterEnemy => _encounterEnemy;
        private bool _isRefreshOpen;

        public enum BattleStatus
        {
            Run,
            Encounter,
            Reward
        }

        protected override void Awake()
        {
            base.Awake();
            _isRefreshOpen = true;
            _currentSkills = new List<SkillComponent>();
            _skillRewardDict = new Dictionary<string, SkillReward>();
            _skillDict = new Dictionary<string, SkillComponent>();
            _skillNames = new List<string>();
            _loopSockets = new List<LoopSocket>();
            _loopMoveGrid = GetComponentInChildren<LoopMoveGrid>();
            Status = BattleStatus.Run;
        }

        public void OnGameStart()
        {
            var hero = LeanPool.Spawn(HeroPrefab, SpawnSocket.position, Quaternion.identity);
            SetHero(hero.GetComponent<Character>());
            ContinueButton.SetActive(false);
            SkillView.gameObject.SetActive(false);
            //
            var color = ChoosePanel.color;
            color.a = 0;
            ChoosePanel.color = color;
            //
            _loopSockets.Clear();
            foreach (var trans in SkillSockets)
            {
                _loopSockets.Add(new LoopSocket(trans));
            }

            InitSkillData();
            ResetBattlePanel();
            foreach (var skill in InitSkills)
            {
                var skillReward = LeanPool.Spawn(skill);
                skillReward.SetSkillObject(_skillDict[skillReward.SkillName]);
                skillReward.OnAddSkill();
            }

            _loopMoveGrid.OnReset();
        }

        private void InitSkillData()
        {
            _skillRewardDict.Clear();
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
            _skillDict.Clear();
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
            AddSkillTarget(skillReward.SkillTarget);
            // TODO: Run Continue Delay
            GameEventManager.Instance.OnRunContinue.Invoke();
        }

        private void AddSkillTarget(SkillComponent skillTarget)
        {
            int index = _currentSkills.Count;
            var skill = LeanPool.Spawn(skillTarget, SkillTransform);
            skill.transform.position = 10 * Vector3.down;
            _hero.BehaviourController.AddSkill(skill);
            skill.SetOwner(Hero);
            if (index == 3)
            {
                skill.gameObject.SetActive(false);
                return;
            }

            skill.SetFollow(_loopSockets[index]);
            _currentSkills.Add(skill);
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.IsPaused)
            {
                return;
            }

            _loopMoveGrid.Tick(Time.fixedDeltaTime);

            if (Status != BattleStatus.Encounter)
            {
                return;
            }

            Hero.BehaviourController.FixedTick(Time.fixedDeltaTime);
            _encounterEnemy.BehaviourController.FixedTick(Time.fixedDeltaTime);
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
            GameEventManager.Instance.OnRunEncounter += OnRunEncounter;
            GameEventManager.Instance.OnRunReward += OnRunReward;
            GameEventManager.Instance.OnRunContinue += OnRunContinue;
            GameEventManager.Instance.OnEnemyDead += OnBattleEnd;
            // GameEventManager.Instance.OnGameOver += OnGameOver;
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
            // GameEventManager.Instance.OnGameOver -= OnGameOver;
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

        public void ChangeSkillView()
        {
            bool active = SkillView.gameObject.activeSelf;
            SkillView.gameObject.SetActive(!active);
            if (active)
            {
                GameEventManager.Instance.OnGameContinue.Invoke();
            }
            else
            {
                GameEventManager.Instance.OnGamePause.Invoke();
            }
        }

        public void OnRefreshSkills()
        {
            if (!_isRefreshOpen)
            {
                return;
            }

            OnRefreshUseEnergy();
            _isRefreshOpen = false;
            foreach (var skill in _currentSkills)
            {
                skill.OnRefresh();
            }

            _currentSkills.Clear();

            StartCoroutine(RefreshRandomSkills(0.4f));
        }

        private void OnRefreshUseEnergy()
        {
            var cost = Mathf.FloorToInt(_hero.CurrentEnergy / 2);
            _hero.Energy.CostEnemy(cost);
        }

        IEnumerator RefreshRandomSkills(float delay)
        {
            yield return new WaitForSeconds(delay);
            List<SkillComponent> skills = new List<SkillComponent>();
            foreach (var skill in _hero.BehaviourController.CurrentSkills)
            {
                skills.Add(skill);
            }

            skills.Shuffle();
            int num = Mathf.Min(3, skills.Count);
            for (int i = 0; i < num; i++)
            {
                skills[i].gameObject.SetActive(true);
                skills[i].SetFollow(_loopSockets[i]);
                skills[i].ResetStatus();
                _currentSkills.Add(skills[i]);
            }

            _isRefreshOpen = true;
        }
    }
}