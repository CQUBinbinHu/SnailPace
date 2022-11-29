using Core;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerScoreComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI PlayerDataText;
        [SerializeField] private TextMeshProUGUI ScoreText;
        [SerializeField] private Color PlayerColor;
        public int Rank;

        public void SetLoading()
        {
            PlayerDataText.color = Color.white;
            ScoreText.color = Color.white;
            PlayerDataText.fontStyle = FontStyles.Normal;
            ScoreText.fontStyle = FontStyles.Normal;

            PlayerDataText.text = Rank.ToString()
                                  + "." + "Loading...";
            ScoreText.text = string.Empty;
        }

        public void SetScore(string playerName, int score)
        {
            if (!string.IsNullOrEmpty(playerName))
            {
                PlayerDataText.text = Rank.ToString()
                                      + "." + playerName;
                // TODO: use static method
                ScoreText.text = GameManager.Instance.GetStringScore(score);
            }
            else
            {
                PlayerDataText.text = string.Empty;
                ScoreText.text = string.Empty;
            }
        }

        public void SetCurrent(int rank)
        {
            Rank = rank;
            PlayerDataText.fontStyle = FontStyles.Bold;
            ScoreText.fontStyle = FontStyles.Bold;
            PlayerDataText.color = PlayerColor;
            ScoreText.color = PlayerColor;
        }

        public void SetEmpty()
        {
            PlayerDataText.text = string.Empty;
            ScoreText.text = string.Empty;
        }
    }
}