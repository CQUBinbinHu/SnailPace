﻿using System.Collections;
using DefaultNamespace;
using DefaultNamespace.Tools.IncrementScoreCharacters;
using LootLocker;
using LootLocker.Requests;
using MoreMountains.Tools;
using Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public enum GameStatus
    {
        Idle,
        Run,
        Encounter,
        Reward,
        Splash,
        GameOver,
        GameWining
    }

    public enum GameTransition
    {
        StartRun,
        Encounter,
        ContinueRun,
        Reward,
        StartGame,
        OnGameOver,
        Restart,
        WinGame
    }

    public class GameManager : MMPersistentSingleton<GameManager>
    {
        [SerializeField] public int MaxEncounters;
        [SerializeField] public BuffShowData BuffShowData;
        [SerializeField] public ShowTipComponent ShowTipComponent;
        [SerializeField] private float MinLoadDuration;
        public string LeaderBoardKey;
        private float _runClock;
        public GameStatus CurrentState;
        private StateMachine<GameManager, GameStatus, GameTransition> _stateMachine;
        private bool _isPaused;
        public string RunClock => (0.01f * GetScore()).ToString("0.00");
        public bool IsPaused => _isPaused;
        public float ProgressValue { get; set; }
        public int CountDown;
        private float _loadTimer;
        public bool LoggedIn;
        private int PlayerScore { get; set; }
        public bool IsSuccessRegistered { get; set; }

        protected override void Awake()
        {
            base.Awake();
            LoggedIn = false;
            IsSuccessRegistered = false;
            _stateMachine = new StateMachine<GameManager, GameStatus, GameTransition>(this);
            var splashState = new Splash(GameStatus.Splash);
            var idleState = new Idle(GameStatus.Idle);
            var runState = new Run(GameStatus.Run);
            var encounter = new Encounter(GameStatus.Encounter);
            var reward = new Reward(GameStatus.Reward);
            var gameOverState = new GameOverState(GameStatus.GameOver);
            var gameWining = new GameWiningState(GameStatus.GameWining);

            gameWining.AddTransition(GameTransition.Restart, GameStatus.Idle);
            splashState.AddTransition(GameTransition.StartGame, GameStatus.Idle);
            idleState.AddTransition(GameTransition.StartRun, GameStatus.Run);
            runState.AddTransition(GameTransition.Encounter, GameStatus.Encounter);
            runState.AddTransition(GameTransition.Reward, GameStatus.Reward);
            runState.AddTransition(GameTransition.WinGame, GameStatus.GameWining);
            encounter.AddTransition(GameTransition.ContinueRun, GameStatus.Run);
            encounter.AddTransition(GameTransition.Reward, GameStatus.Reward);
            reward.AddTransition(GameTransition.ContinueRun, GameStatus.Run);
            runState.AddTransition(GameTransition.OnGameOver, GameStatus.GameOver);
            encounter.AddTransition(GameTransition.OnGameOver, GameStatus.GameOver);
            gameOverState.AddTransition(GameTransition.Restart, GameStatus.Idle);
            _stateMachine.AddState(splashState);
            _stateMachine.AddState(idleState);
            _stateMachine.AddState(runState);
            _stateMachine.AddState(encounter);
            _stateMachine.AddState(reward);
            _stateMachine.AddState(gameOverState);
            _stateMachine.AddState(gameWining);
        }

        private void Start()
        {
            _runClock = 0;
            _stateMachine.SetCurrent(GameStatus.Splash);
        }

        private int GetScore()
        {
            return (int)(_runClock * 100);
        }

        public IEnumerator LoginRoutine()
        {
            bool done = false;
            LootLockerSDKManager.StartGuestSession((response) =>
            {
                if (response.success)
                {
                    Debug.Log("Successfully started LootLocker session");
                    LoggedIn = true;
                    done = true;
                    // Save the player ID for use in the leaderboard
                    PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                    Debug.Log("UserLoggedIn:" + PlayerPrefs.GetString("PlayerID"));
                }
                else
                {
                    Debug.Log("Error starting LootLocker session");
                    done = true;
                }
            });
            yield return new WaitWhile(() => done == false);
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
            // GameEventManager.Instance.OnGameStart += OnGameStart;
            GameEventManager.Instance.OnGamePause += OnGamePause;
            GameEventManager.Instance.OnGameContinue += OnGameContinue;
            // GameEventManager.Instance.OnRunStart += OnRunStart;
            GameEventManager.Instance.OnRunEncounter += OnRunEncounter;
            GameEventManager.Instance.OnRunReward += OnRunReward;
            GameEventManager.Instance.OnRunContinue += OnRunContinue;
            GameEventManager.Instance.OnGameOver += OnGameOver;
            GameEventManager.Instance.OnGameWinning += OnGameWinning;
        }

        /// <summary>
        /// OnDisable, we stop listening to events.
        /// </summary>
        protected virtual void OnDisable()
        {
            // GameEventManager.Instance.OnGameStart -= OnGameStart;
            GameEventManager.Instance.OnGamePause -= OnGamePause;
            GameEventManager.Instance.OnGameContinue -= OnGameContinue;
            // GameEventManager.Instance.OnRunStart -= OnRunStart;
            GameEventManager.Instance.OnRunEncounter -= OnRunEncounter;
            GameEventManager.Instance.OnRunReward -= OnRunReward;
            GameEventManager.Instance.OnRunContinue -= OnRunContinue;
            GameEventManager.Instance.OnGameOver -= OnGameOver;
            GameEventManager.Instance.OnGameWinning -= OnGameWinning;
        }

        private void OnGameWinning()
        {
            _stateMachine.PerformTransition(GameTransition.WinGame);
        }

        private void OnGameOver()
        {
            _stateMachine.PerformTransition(GameTransition.OnGameOver);
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

        // private void OnGameStart()
        // {
        // }

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
                Context.BuffShowData.Initialize();
                BattleManager.Instance.OnGameStart();
                // GameEventManager.Instance.OnGameStart.Invoke();
            }

            public override void Exit()
            {
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
                Context._isPaused = true;
            }

            public override void Exit()
            {
                Context._isPaused = false;
            }

            public override void Reason(float deltaTime = 0)
            {
            }

            public override void Act(float deltaTime = 0)
            {
            }
        }

        private class GameOverState : FsmState<GameManager, GameStatus, GameTransition>
        {
            public GameOverState(GameStatus stateId) : base(stateId)
            {
            }

            public override void Enter()
            {
                Context._isPaused = true;
            }

            public override void Exit()
            {
                Context._isPaused = false;
                Context._runClock = 0;
            }

            public override void Reason(float deltaTime = 0)
            {
            }

            public override void Act(float deltaTime = 0)
            {
            }
        }

        private class GameWiningState : FsmState<GameManager, GameStatus, GameTransition>
        {
            public GameWiningState(GameStatus stateId) : base(stateId)
            {
            }

            public override void Enter()
            {
                Context._isPaused = true;
                Context.LoggedIn = false;
                Context.PlayerScore = Context.GetScore();
                Context.OnSubmitScore();
            }

            public override void Exit()
            {
                Context._isPaused = false;
                Context._runClock = 0;
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
            StartCoroutine(LoadLeaver());
        }

        IEnumerator LoadLeaver()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync("Scenes/Main");
            operation.allowSceneActivation = false;
            while (!operation.isDone) //当场景没有加载完毕
            {
                _loadTimer += Time.fixedDeltaTime;
                if (_loadTimer > MinLoadDuration
                    && Mathf.Approximately(operation.progress, 0.9f))
                {
                    operation.allowSceneActivation = true;
                }

                ProgressValue = Mathf.Min(operation.progress, _loadTimer / MinLoadDuration);
                yield return new WaitForFixedUpdate();
            }

            yield return StartCoroutine(OnSceneLoaded());
        }

        IEnumerator OnSceneLoaded()
        {
            yield return new WaitForSeconds(0.2f);
            _stateMachine.PerformTransition(GameTransition.StartGame);
        }

        public void Restart()
        {
            GameEventManager.Instance.OnGameRestart.Invoke();
            _stateMachine.PerformTransition(GameTransition.Restart);
        }

        private void OnSubmitScore()
        {
            StartCoroutine(SetupRoutine());
        }

        IEnumerator SetupRoutine()
        {
            // Set the info text to loading
            // InfoText.text = "Logging in...";
            // Wait while the login is happening
            yield return LoginRoutine();
            // If the player couldn't log in, let them know, and then retry
            if (!LoggedIn)
            {
                float loginCountdown = 4;
                float timer = loginCountdown;
                while (timer >= -1f)
                {
                    timer -= Time.deltaTime;
                    // Update the text when we get to a new number
                    if (Mathf.CeilToInt(timer) != Mathf.CeilToInt(loginCountdown))
                    {
                        var info = "Failed to login retrying in " + Mathf.CeilToInt(timer).ToString();
                        Debug.Log(info);
                        loginCountdown -= 1f;
                    }

                    yield return null;
                }

                StartCoroutine(SetupRoutine());
                yield break;
            }

            SubmitScore();
            yield return null;
        }

        private void SubmitScore()
        {
            // Get the players saved ID, and add the incremental characters
            string playerID = PlayerPrefs.GetString("PlayerID") + "_" + IncrementCharacters.GetStr();
            string metadata = PlayerPrefs.GetString("PlayerName");

            Debug.Log("PlayerID " + playerID);
            LootLockerSDKManager.SubmitScore(playerID, PlayerScore, LeaderBoardKey, metadata, (response) =>
            {
                if (response.statusCode == 200)
                {
                    Debug.Log("Successful Submit Score " + PlayerScore);
                    // Only let the player upload score once until we reset it
                }
                else
                {
                    Debug.Log("failed: " + response.Error);
                }
            });
        }
    }
}