using DefaultNamespace;
using MoreMountains.Tools;
using UnityEngine.Events;

namespace Core
{
    public class GameEventManager : MMPersistentSingleton<GameEventManager>
    {
        public UnityAction OnGameStart;
        public UnityAction OnGamePause;
        public UnityAction OnGameContinue;
        public UnityAction OnRunStart;
        public UnityAction<Character> OnRunEncounter;
        public UnityAction OnRunReward;
        public UnityAction OnRunContinue;
        public UnityAction OnEnemyDead;
        public UnityAction OnGameOver;
        public UnityAction OnGameRestart;
        public UnityAction<SkillReward> OnAddSkill;
    }
}