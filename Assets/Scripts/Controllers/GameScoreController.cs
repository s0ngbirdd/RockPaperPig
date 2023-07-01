using AdMob;
using Firebase.Analytics;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class GameScoreController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private int _showAdsRate = 10;
        [SerializeField] private int _logScoreRate = 10;
    
        private int _score = 0;

        public int Score => _score;

        private void Update()
        {
            _scoreText.text = _score.ToString();
        }

        public void IncreaseScore()
        {
            _score++;

            LogEventToFirebase();
            
            ShowRewardedAd();
        }

        public void SaveScore()
        {
            SaveLoadSystem.SaveLoadSystem.Instance.SaveGame(_score);
        }

        private void ShowRewardedAd()
        {
            if (_score % _showAdsRate == 0)
            {
                AdRequester.Instance.ShowRewardedAd();
            }
        }

        private void LogEventToFirebase()
        {
            if (_score % _logScoreRate == 0)
            {
                FirebaseAnalytics.LogEvent("scoreProgress", new Parameter("score", "score-" + _score));
            }
        }
    }
}
