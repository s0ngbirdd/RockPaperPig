using TMPro;
using UnityEngine;

namespace Controllers
{
    public class MainMenuScoreController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
    
        private int _score = 0;

        private void Start()
        {
            _score = SaveLoadSystem.SaveLoadSystem.Instance.LoadGame();
            _scoreText.text = _score.ToString();
        }
    }
}
