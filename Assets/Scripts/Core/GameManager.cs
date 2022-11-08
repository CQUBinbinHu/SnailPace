using System;
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
        EncounterBattle
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

    public enum MoveStatus
    {
        Idle,
        Run,
        Encounter
    }

    public enum MoveTransition
    {
        StartRun,
        Encounter,
        ContinueRun
    }

    public class GameManager :
        MMPersistentSingleton<GameManager>,
        MMEventListener<CoreGameEvent>
    {
        private float _runClock;
        public GameObject EncounterGo;
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
            idleState.AddTransition(MoveTransition.StartRun, MoveStatus.Run);
            runState.AddTransition(MoveTransition.Encounter, MoveStatus.Encounter);
            encounter.AddTransition(MoveTransition.ContinueRun, MoveStatus.Run);
            _stateMachine.AddState(idleState);
            _stateMachine.AddState(runState);
            _stateMachine.AddState(encounter);
        }

        private void Start()
        {
            _runClock = 0;
            _stateMachine.SetCurrent(MoveStatus.Idle);
        }

        private void Update()
        {
            _stateMachine.Tick(Time.deltaTime);
            CurrentRun = _stateMachine.CurrentStateID;
        }

        private void FixedUpdate()
        {
            if (CurrentRun == MoveStatus.Run)
            {
                //TODO: 计时
                _runClock += Time.fixedDeltaTime;
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
                case CoreGameEventTypes.EncounterBattle:
                    Debug.Log("Debug EncounterBattle");
                    break;
            }
        }

        /// <summary>
        /// OnDisable, we start listening to events.
        /// </summary>
        protected virtual void OnEnable()
        {
            this.MMEventStartListening<CoreGameEvent>();
        }

        /// <summary>
        /// OnDisable, we stop listening to events.
        /// </summary>
        protected virtual void OnDisable()
        {
            this.MMEventStopListening<CoreGameEvent>();
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
            }

            public override void Exit()
            {
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
    }
}