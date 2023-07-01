using Audio;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        [Header("UI Buttons")]
        [SerializeField] private Button _quitButton;

        private void OnEnable()
        {
            _quitButton.onClick.AddListener(QuitGame);
        }

        private void OnDisable()
        {
            _quitButton.onClick.RemoveListener(QuitGame);
        }
        private void QuitGame()
        {
            AudioManager.Instance.PlayOneShot("Click");
        
            Application.Quit();
        }
    }
}
