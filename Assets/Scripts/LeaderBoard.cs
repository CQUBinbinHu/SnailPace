using System;
using Core;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class LeaderBoard : MonoBehaviour
    {
        [SerializeField] private PlayerScoreComponent[] PlayerInfo;
        [SerializeField] private Animator LeaderBoardAnimator;

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
            LeaderBoardAnimator.SetTrigger("");
        }

        private void ShowLeaderBoard()
        {
            LeaderBoardAnimator.SetTrigger("");
            for (int i = 0; i < PlayerInfo.Length; i++)
            {
                PlayerInfo[i].Rank = i;
                PlayerInfo[i].SetLoading();
            }
        }

        private void OnFetchScores()
        {
            // TODO: 更新排行榜
            for (int i = 0; i < PlayerInfo.Length; i++)
            {
                // PlayerInfo[i].SetScore(GameManager.Instance.PlayerScores[i].);
            }
        }
    }
}