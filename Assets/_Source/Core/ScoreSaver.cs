using UnityEngine;

namespace Core
{
    public class ScoreSaver
    {
        public int MaxScore { get; private set; }

        public ScoreSaver()
        {
            if (PlayerPrefs.HasKey("MaxScore"))
            {
                MaxScore = PlayerPrefs.GetInt("MaxScore");
                Debug.Log("MaxScore: " + MaxScore);
            }
            else
            {
                MaxScore = 0;
                PlayerPrefs.SetInt("MaxScore", MaxScore);
            }
        }
        public void SaveMaxScore(int score)
        {
            PlayerPrefs.SetInt("MaxScore", score);
            Debug.Log($"MaxScored : {GetMaxScore()}");
        }
        public int GetMaxScore()
        {
            return PlayerPrefs.GetInt("MaxScore");
        }
    }
}