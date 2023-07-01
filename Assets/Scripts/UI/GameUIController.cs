using System;
using Audio;
using Controllers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameUIController : MonoBehaviour
    {
        public static event Action OnRestart;
        public static event Action OnEnableHintPopup;
        public static event Action OnDisableHintPopup;

        [Header("Objects")]
        [SerializeField] private GameObject _gameEndPopap;
        [SerializeField] private GameObject _hintPopap;
        [SerializeField] private GameObject _quitPopap;
    
        [Header("UI Buttons")]
        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _hintButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _closeHintButton;
        [SerializeField] private Button _closeQuitButton;
        [SerializeField] private Button _gameEndRestartButton;
    
        [Header("Fading")]
        [SerializeField] private CanvasGroup _gameEndCanvasGroup;
        [SerializeField] private CanvasGroup _hintCanvasGroup;
        [SerializeField] private CanvasGroup _quitCanvasGroup;
        [SerializeField] private float _fadeTime = 1f;
    
        [Header("Sound Image")]
        [SerializeField] private Image _soundImage;
        [SerializeField] private Sprite _soundEnabledSprite;
        [SerializeField] private Sprite _soundDisabledSprite;
    
        private GameScoreController _gameScoreController;
        private bool _isPaused;
        private Tween _tween;

        public bool IsPaused => _isPaused;

        private void OnEnable()
        {
            GameFlowController.OnGameEnd += EnableGameEndPopup;
        
            _soundButton.onClick.AddListener(EnableDisableSound);
            _restartButton.onClick.AddListener(RestartGame);
            _hintButton.onClick.AddListener(EnableHintPopap);
            _quitButton.onClick.AddListener(EnableQuitPopap);
            _closeHintButton.onClick.AddListener(DisableHintPopap);
            _closeQuitButton.onClick.AddListener(DisableQuitPopap);
            _gameEndRestartButton.onClick.AddListener(RestartGame);
        }

        private void OnDisable()
        {
            GameFlowController.OnGameEnd -= EnableGameEndPopup;
        
            _soundButton.onClick.RemoveListener(EnableDisableSound);
            _restartButton.onClick.RemoveListener(RestartGame);
            _hintButton.onClick.RemoveListener(EnableHintPopap);
            _quitButton.onClick.RemoveListener(EnableQuitPopap);
            _closeHintButton.onClick.RemoveListener(DisableHintPopap);
            _closeQuitButton.onClick.RemoveListener(DisableQuitPopap);
            _gameEndRestartButton.onClick.RemoveListener(RestartGame);
        }

        private void Start()
        {
            _gameScoreController = FindObjectOfType<GameScoreController>();

            if (AudioManager.Instance.ReturnSoundEnabled())
            {
                _soundImage.sprite = _soundEnabledSprite;
            }
            else if (!AudioManager.Instance.ReturnSoundEnabled())
            {
                _soundImage.sprite = _soundDisabledSprite;
            }
        }

        private void EnableDisableSound()
        {
            AudioManager.Instance.PlayOneShot(("Click"));
            AudioManager.Instance.EnableDisableSoundVolume();

            if (AudioManager.Instance.ReturnSoundEnabled())
            {
                _soundImage.sprite = _soundEnabledSprite;
            }
            else if (!AudioManager.Instance.ReturnSoundEnabled())
            {
                _soundImage.sprite = _soundDisabledSprite;
            }
        }

        private void RestartGame()
        {
            AudioManager.Instance.PlayOneShot(("Click"));
        
            if (_gameScoreController.Score > SaveLoadSystem.SaveLoadSystem.Instance.LoadGame())
            {
                _gameScoreController.SaveScore();
            }

            OnRestart?.Invoke();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
        }
    
        private void EnableHintPopap()
        {
            AudioManager.Instance.PlayOneShot("Click");
        
            if (_tween != null)
            {
                _tween.Kill();
            }
        
            _hintPopap.SetActive(true);
            _isPaused = true;
            Time.timeScale = 0;
            SetButtonsInteractivity(false);
            OnEnableHintPopup?.Invoke();
            _tween = _hintCanvasGroup.DOFade(1f, _fadeTime).SetEase(Ease.Linear).SetUpdate(true);
        }

        private void DisableHintPopap()
        {
            AudioManager.Instance.PlayOneShot("Click");
        
            if (_tween != null)
            {
                _tween.Kill();
            }

            _tween = _hintCanvasGroup.DOFade(0f, _fadeTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                _hintPopap.SetActive(false);
                _isPaused = false;
                Time.timeScale = 1;
                SetButtonsInteractivity(true);
                OnDisableHintPopup?.Invoke();
            }).SetUpdate(true);
        }

        private void EnableQuitPopap()
        {
            AudioManager.Instance.PlayOneShot("Click");
        
            if (_tween != null)
            {
                _tween.Kill();
            }
        
            _quitPopap.SetActive(true);
            _isPaused = true;
            Time.timeScale = 0;
            SetButtonsInteractivity(false);
            _tween = _quitCanvasGroup.DOFade(1f, _fadeTime).SetEase(Ease.Linear).SetUpdate(true);
        }

        private void DisableQuitPopap()
        {
            AudioManager.Instance.PlayOneShot("Click");
        
            if (_tween != null)
            {
                _tween.Kill();
            }

            _tween = _quitCanvasGroup.DOFade(0f, _fadeTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                _quitPopap.SetActive(false);
                _isPaused = false;
                Time.timeScale = 1;
                SetButtonsInteractivity(true);
            }).SetUpdate(true);
        }

        private void EnableGameEndPopup()
        {
            if (_tween != null)
            {
                _tween.Kill();
            }
        
            _gameEndPopap.SetActive(true);
            _isPaused = true;
            Time.timeScale = 0;
            SetButtonsInteractivity(false);
            _tween = _gameEndCanvasGroup.DOFade(1f, _fadeTime).SetEase(Ease.Linear).SetUpdate(true);
        }
    
        private void SetButtonsInteractivity(bool interactivity)
        {
            _soundButton.interactable = interactivity;
            _restartButton.interactable = interactivity;
            _hintButton.interactable = interactivity;
            _quitButton.interactable = interactivity;
        }

        private void OnApplicationQuit()
        {
            if (_gameScoreController.Score > SaveLoadSystem.SaveLoadSystem.Instance.LoadGame())
            {
                _gameScoreController.SaveScore();
            }
        }
    }
}
