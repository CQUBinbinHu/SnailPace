using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerScoreComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI PlayerDataText;
        [SerializeField] private TextMeshProUGUI ScoreText;
        public int Rank;

        public void SetLoading()
        {
            PlayerDataText.text = Rank.ToString()
                                  + "." + "Loading...";
            ScoreText.text = string.Empty;
        }

        public void SetScore(string playerName, int score)
        {
            PlayerDataText.text = Rank.ToString()
                                  + "." + playerName;
            ScoreText.text = score.ToString();
        }
    }
}