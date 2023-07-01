using UI;
using UnityEngine;

namespace AdMob
{
    [RequireComponent(typeof(GoogleAdMobController))]
    public class AdRequester : MonoBehaviour
    {
        public static AdRequester Instance;

        [SerializeField] private float _timeBeforeShowingBannerAd = 0.5f;

        private GoogleAdMobController _googleAdMobController;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void OnEnable()
        {
            ASyncLoader.OnLoadScene += WaitToShowBannerAd;
            GameUIController.OnRestart += WaitToShowBannerAd;
        }

        private void OnDisable()
        {
            ASyncLoader.OnLoadScene -= WaitToShowBannerAd;
            GameUIController.OnRestart -= WaitToShowBannerAd;
        }

        private void Start()
        {
            _googleAdMobController = GetComponent<GoogleAdMobController>();
            _googleAdMobController.RequestBannerAd();
            _googleAdMobController.RequestAndLoadRewardedAd();
        }

        public void ShowRewardedAd()
        {
            _googleAdMobController.ShowRewardedAd();
        }

        private void WaitToShowBannerAd()
        {
            Invoke(nameof(ShowBannerAd), _timeBeforeShowingBannerAd);
        }

        private void ShowBannerAd()
        {
            _googleAdMobController.RequestBannerAd();
        }
    }
}
