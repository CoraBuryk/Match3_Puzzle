using Assets.Match.Scripts.InputSystemController;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Assets.Match.Scripts.Ads
{

    public class AdsInit : SingletonPersistent<AdsInit>,IUnityAdsInitializationListener
    {
        [SerializeField] private InterstitialAds _interstitialAds;
        [SerializeField] private bool _testMode = false;

        private readonly string _androidGameID = "4996815";
        private readonly string _iphoneGameID = "4996814";

        private string _gameID = null;

        public override void Awake()
        {
            base.Awake();
            InitializeAds();
        }

        public void InitializeAds()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _gameID = _iphoneGameID;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                _gameID = _androidGameID;
            }
            else
            {
                _gameID = _androidGameID;
            }

            Advertisement.Initialize(_gameID, _testMode, this);
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete");
            _interstitialAds.LoadAd();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads initialization failed: {error} - {message}");
        }

    }
}
