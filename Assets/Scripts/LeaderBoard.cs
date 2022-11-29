using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class LeaderBoard : MonoBehaviour
    {
        [SerializeField] private PlayerScoreComponent[] PlayerInfo;
        [SerializeField] private Animator LeaderBoardAnimator;
        private static readonly int IsShow = Animator.StringToHash("IsShow");

        private void Awake()
        {
            LeaderBoardAnimator.SetBool(IsShow, false);
        }

        private void OnEnable()
        {
            GameEventManager.Instance.OnGameWinning += ShowLeaderBoard;
            GameEventManager.Instance.OnFetchScores += OnFetchScores;
            GameEventManager.Instance.OnGameRestart += Restart;
        }

        private void OnDisable()
        {
            GameEventManager.Instance.OnGameWinning -= ShowLeaderBoard;
            GameEventManager.Instance.OnFetchScores -= OnFetchScores;
            GameEventManager.Instance.OnGameRestart -= Restart;
        }

        private void Restart()
        {
            LeaderBoardAnimator.SetBool(IsShow, false);
        }

        private void ShowLeaderBoard()
        {
            LeaderBoardAnimator.SetBool(IsShow, true);
            for (int i = 0; i < PlayerInfo.Length; i++)
            {
                PlayerInfo[i].Rank = i + 1;
                PlayerInfo[i].SetLoading();
            }

            PlayerInfo[10].SetEmpty();
            PlayerInfo[11].SetEmpty();
        }

        private void OnFetchScores()
        {
            foreach (var playerScore in GameManager.Instance.PlayerScores)
            {
                PlayerInfo[playerScore.Rank - 1].SetScore(playerScore.PlayerName, playerScore.Score);
            }

            var score = GameManager.Instance.CurrentScore;
            int index = score.Rank <= 10 ? score.Rank - 1 : 11;
            PlayerInfo[index].SetCurrent(score.Rank);
            PlayerInfo[index].SetScore(score.PlayerName, score.Score);
        }
    }
}