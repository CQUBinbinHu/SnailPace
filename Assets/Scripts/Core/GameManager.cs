using System;
using DefaultNamespace;
using MoreMountains.Tools;
using Tools;
using UnityEngine;

namespace Core
{
    public enum CoreGameEventTypes
    {
        Start,
        GameOver,
        Pause,
        Continue,
        EnemyDead,
        AddSkill,
        OnRefreshSkill
    }

    public enum RunEventTypes
    {
        RunStart,
        Encounter,
        Reward,
        Continue
    }

    public struct CoreGameEvent
    {
        public CoreGameEventTypes EventType;

        public CoreGameEvent(CoreGameEventTypes eventType)
        {
            EventType = eventType;
        }

        static CoreGameEvent e;

        public static void Trigger(CoreGameEventTypes eventType)
        {
            e.EventType = eventType;
            MMEventManager.TriggerEvent(e);
        }
    }

    public struct RunGameEvent
    {
        public RunEventTypes EventType;

        public RunGameEvent(RunEventTypes eventType)
        {
            EventType = eventType;
        }

        static RunGameEvent e;

        public static void Trigger(RunEventTypes eventType)
        {
            e.EventType = eventType;
            MMEventManager.TriggerEvent(e);
        }
    }

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

    public class GameManager :
        MMPersistentSingleton<GameManager>,
        MMEventListener<CoreGameEvent>,
        MMEventListener<RunGameEvent>
    {
        [SerializeField] public BuffShowData BuffShowData;
        [SerializeField] public ShowTipComponent ShowTipComponent;
        private float _runClock;
        public MoveStatus CurrentRun;
        private StateMachine<GameManager, MoveStatus, MoveTransition> _stateMachine;
        public float RunClock => _runClock;

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

        private class Idle : FsmState<GameManager, MoveStatus, MoveTransition>
        {
            private float _timer = 0;

            public Idle(MoveStatus stateId) : base(stateId)
            {
            }

            public override void Enter()
            {
                _timer = 0;
                CoreGameEvent.Trigger(CoreGameEventTypes.Start);
            }

            public override void Exit()
            {
                RunGameEvent.Trigger(RunEventTypes.RunStart);
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

        public void OnMMEvent(CoreGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case CoreGameEventTypes.Start:
                    break;
                case CoreGameEventTypes.GameOver:
                    break;
                case CoreGameEventTypes.Pause:
                    break;
            }
        }

        public void OnMMEvent(RunGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case RunEventTypes.RunStart:
                    break;
                case RunEventTypes.Encounter:
                    _stateMachine.PerformTransition(MoveTransition.Encounter);
                    break;
                case RunEventTypes.Reward:
                    _stateMachine.PerformTransition(MoveTransition.Reward);
                    break;
                case RunEventTypes.Continue:
                    _stateMachine.PerformTransition(MoveTransition.ContinueRun);
                    break;
            }
        }
    }
}