﻿using System;
using DefaultNamespace;
using MoreMountains.Tools;
using Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public enum GameStatus
    {
        Idle,
        Run,
        Encounter,
        Reward,
        Splash
    }

    public enum GameTransition
    {
        StartRun,
        Encounter,
        ContinueRun,
        Reward,
        StartGame
    }

    public class GameManager : MMPersistentSingleton<GameManager>
    {
        [SerializeField] public BuffShowData BuffShowData;
        [SerializeField] public ShowTipComponent ShowTipComponent;
        private float _runClock;
        public GameStatus CurrentState;
        private StateMachine<GameManager, GameStatus, GameTransition> _stateMachine;
        private bool _isPaused;
        public float RunClock => _runClock;
        public bool IsPaused => _isPaused;
        public float Progress { get; set; }

        public int CountDown;

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine<GameManager, GameStatus, GameTransition>(this);
            var splashState = new Splash(GameStatus.Splash);
            var idleState = new Idle(GameStatus.Idle);
            var runState = new Run(GameStatus.Run);
            var encounter = new Encounter(GameStatus.Encounter);
            var reward = new Reward(GameStatus.Reward);
            splashState.AddTransition(GameTransition.StartGame, GameStatus.Idle);
            idleState.AddTransition(GameTransition.StartRun, GameStatus.Run);
            runState.AddTransition(GameTransition.Encounter, GameStatus.Encounter);
            runState.AddTransition(GameTransition.Reward, GameStatus.Reward);
            encounter.AddTransition(GameTransition.ContinueRun, GameStatus.Run);
            encounter.AddTransition(GameTransition.Reward, GameStatus.Reward);
            reward.AddTransition(GameTransition.ContinueRun, GameStatus.Run);
            _stateMachine.AddState(splashState);
            _stateMachine.AddState(idleState);
            _stateMachine.AddState(runState);
            _stateMachine.AddState(encounter);
            _stateMachine.AddState(reward);
        }

        private void Start()
        {
            BuffShowData.Initialize();
            _runClock = 0;
            _stateMachine.SetCurrent(GameStatus.Splash);
        }

        private void Update()
        {
            if (_isPaused)
            {
                return;
            }

            _stateMachine.Tick(Time.deltaTime);
            CurrentState = _stateMachine.CurrentStateID;
        }

        private void FixedUpdate()
        {
            if (_isPaused)
            {
                return;
            }

            switch (CurrentState)
            {
                case GameStatus.Run:
                case GameStatus.Encounter:
                    _runClock += Time.fixedDeltaTime;
                    break;
            }
        }

        /// <summary>
        /// OnDisable, we start listening to events.
        /// </summary>
        protected virtual void OnEnable()
        {
            GameEventManager.Instance.OnGameStart += OnGameStart;
            GameEventManager.Instance.OnGamePause += OnGamePause;
            GameEventManager.Instance.OnGameContinue += OnGameContinue;
            GameEventManager.Instance.OnRunStart += OnRunStart;
            GameEventManager.Instance.OnRunEncounter += OnRunEncounter;
            GameEventManager.Instance.OnRunReward += OnRunReward;
            GameEventManager.Instance.OnRunContinue += OnRunContinue;
        }

        /// <summary>
        /// OnDisable, we stop listening to events.
        /// </summary>
        protected virtual void OnDisable()
        {
            GameEventManager.Instance.OnGameStart -= OnGameStart;
            GameEventManager.Instance.OnGamePause -= OnGamePause;
            GameEventManager.Instance.OnGameContinue -= OnGameContinue;
            GameEventManager.Instance.OnRunStart -= OnRunStart;
            GameEventManager.Instance.OnRunEncounter -= OnRunEncounter;
            GameEventManager.Instance.OnRunReward -= OnRunReward;
            GameEventManager.Instance.OnRunContinue -= OnRunContinue;
        }

        private void OnRunContinue()
        {
            _stateMachine.PerformTransition(GameTransition.ContinueRun);
        }

        private void OnRunReward()
        {
            _stateMachine.PerformTransition(GameTransition.Reward);
        }

        private void OnRunEncounter(Character target)
        {
            _stateMachine.PerformTransition(GameTransition.Encounter);
        }

        private void OnGameStart()
        {
        }

        private void OnRunStart()
        {
        }

        private void OnGamePause()
        {
            _isPaused = true;
        }

        private void OnGameContinue()
        {
            _isPaused = false;
        }

        private class Splash : FsmState<GameManager, GameStatus, GameTransition>
        {
            public Splash(GameStatus stateId) : base(stateId)
            {
            }

            public override void Enter()
            {
            }

            public override void Exit()
            {
            }

            public override void Reason(float deltaTime = 0)
            {
            }

            public override void Act(float deltaTime = 0)
            {
            }
        }

        private class Idle : FsmState<GameManager, GameStatus, GameTransition>
        {
            private float _timer = 0;
            private const int Duration = 3;

            public Idle(GameStatus stateId) : base(stateId)
            {
            }

            public override void Enter()
            {
                _timer = Duration;
                Context.CountDown = Duration;
                BattleManager.Instance.OnGameStart();
                GameEventManager.Instance.OnGameStart.Invoke();
            }

            public override void Exit()
            {
                GameEventManager.Instance.OnRunStart.Invoke();
            }

            public override void Reason(float deltaTime = 0)
            {
                if (_timer < 0)
                {
                    Context._stateMachine.PerformTransition(GameTransition.StartRun);
                }
            }

            public override void Act(float deltaTime = 0)
            {
                _timer -= deltaTime;
                Context.CountDown = 1 + (int)_timer;
            }
        }

        private class Run : FsmState<GameManager, GameStatus, GameTransition>
        {
            public Run(GameStatus stateId) : base(stateId)
            {
            }

            public override void Enter()
            {
            }

            public override void Exit()
            {
            }

            public override void Reason(float deltaTime = 0)
            {
            }

            public override void Act(float deltaTime = 0)
            {
            }
        }

        private class Encounter : FsmState<GameManager, GameStatus, GameTransition>
        {
            public Encounter(GameStatus stateId) : base(stateId)
            {
            }

            public override void Enter()
            {
            }

            public override void Exit()
            {
            }

            public override void Reason(float deltaTime = 0)
            {
            }

            public override void Act(float deltaTime = 0)
            {
            }
        }

        private class Reward : FsmState<GameManager, GameStatus, GameTransition>
        {
            public Reward(GameStatus stateId) : base(stateId)
            {
            }

            public override void Enter()
            {
            }

            public override void Exit()
            {
            }

            public override void Reason(float deltaTime = 0)
            {
            }

            public override void Act(float deltaTime = 0)
            {
            }
        }

        public void StartGame()
        {
        }
    }
}