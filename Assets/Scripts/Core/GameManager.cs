using System;
using DefaultNamespace;
using MoreMountains.Tools;
using Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public enum MoveStatus
    {
        Idle,
        Run,
        Encounter,
        Reward
    }

    public enum MoveTransition
    {
        StartRun,
        Encounter,
        ContinueRun,
        Reward
    }

    public class GameManager : MMPersistentSingleton<GameManager>
    {
        [SerializeField] public BuffShowData BuffShowData;
        [SerializeField] public ShowTipComponent ShowTipComponent;
        private float _runClock;
        public MoveStatus CurrentRun;
        private StateMachine<GameManager, MoveStatus, MoveTransition> _stateMachine;
        private bool _isPaused;
        public float RunClock => _runClock;
        public bool IsPaused => _isPaused;

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine<GameManager, MoveStatus, MoveTransition>(this);
            var idleState = new Idle(MoveStatus.Idle);
            var runState = new Run(MoveStatus.Run);
            var encounter = new Encounter(MoveStatus.Encounter);
            var reward = new Reward(MoveStatus.Reward);
            idleState.AddTransition(MoveTransition.StartRun, MoveStatus.Run);
            runState.AddTransition(MoveTransition.Encounter, MoveStatus.Encounter);
            runState.AddTransition(MoveTransition.Reward, MoveStatus.Reward);
            encounter.AddTransition(MoveTransition.ContinueRun, MoveStatus.Run);
            encounter.AddTransition(MoveTransition.Reward, MoveStatus.Reward);
            reward.AddTransition(MoveTransition.ContinueRun, MoveStatus.Run);
            _stateMachine.AddState(idleState);
            _stateMachine.AddState(runState);
            _stateMachine.AddState(encounter);
            _stateMachine.AddState(reward);
        }

        private void Start()
        {
            _runClock = 0;
            BuffShowData.Initialize();
            _stateMachine.SetCurrent(MoveStatus.Idle);
        }

        private void Update()
        {
            _stateMachine.Tick(Time.deltaTime);
            CurrentRun = _stateMachine.CurrentStateID;
        }

        private void FixedUpdate()
        {
            switch (CurrentRun)
            {
                case MoveStatus.Run:
                case MoveStatus.Encounter:
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
            _stateMachine.PerformTransition(MoveTransition.ContinueRun);
        }

        private void OnRunReward()
        {
            _stateMachine.PerformTransition(MoveTransition.Reward);
        }

        private void OnRunEncounter(Character target)
        {
            _stateMachine.PerformTransition(MoveTransition.Encounter);
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

        private class Idle : FsmState<GameManager, MoveStatus, MoveTransition>
        {
            private float _timer = 0;

            public Idle(MoveStatus stateId) : base(stateId)
            {
            }

            public override void Enter()
            {
                _timer = 0;
                BattleManager.Instance.OnGameStart();
                GameEventManager.Instance.OnGameStart.Invoke();
            }

            public override void Exit()
            {
                GameEventManager.Instance.OnRunStart.Invoke();
            }

            public override void Reason(float deltaTime = 0)
            {
                if (_timer > 3)
                {
                    Context._stateMachine.PerformTransition(MoveTransition.StartRun);
                }
            }

            public override void Act(float deltaTime = 0)
            {
                _timer += deltaTime;
            }
        }

        private class Run : FsmState<GameManager, MoveStatus, MoveTransition>
        {
            public Run(MoveStatus stateId) : base(stateId)
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

        private class Encounter : FsmState<GameManager, MoveStatus, MoveTransition>
        {
            public Encounter(MoveStatus stateId) : base(stateId)
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

        private class Reward : FsmState<GameManager, MoveStatus, MoveTransition>
        {
            public Reward(MoveStatus stateId) : base(stateId)
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
    }
}