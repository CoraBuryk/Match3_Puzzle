using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using Assets.Match.Scripts.Gameplay;
using Assets.Match.Scripts.UI.Animations;

namespace Assets.Match.Scripts.Ads
{

    public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] private MoveController _moveController;
        [SerializeField] private GameMenuAnimation _gameMenuAnimation;
        [SerializeField] private Button _showAdButton;
        [SerializeField] private GameObject _gamePanel;

#region Android ID

        private readonly string _androidAdsID = "Rewarded_Android";

#endregion


#region iOS ID

        private readonly string _iOSAdsID = "Rewarded_iOS";

#endregion

        private string _adId;

        private void OnEnable()
        {
            _showAdButton.onClick.AddListener(ShowAd);
        }

        private void Awake()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _adId = _iOSAdsID;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                _adId = _androidAdsID;
            }
            else
            {
                _adId = _androidAdsID;
            }

            _showAdButton.interactable = false;
            LoadAd();
        }

        public void LoadAd()
        {
            Debug.Log("Loading rewarded Ad: " + _adId);
            Advertisement.Load(_adId, this);
        }

        public void ShowAd()
        {
            _showAdButton.interactable = false;
            Debug.Log("Showing rewarded Ad: " + _adId);
            Advertisement.Show(_adId, this);
        }

        public void OnUnityAdsAdLoaded(string placementId) 
        { 
            Debug.Log("Rewarded Ad Loaded: " + placementId);
            if(placementId.Equals(_adId))
            {
                _showAdButton.interactable = true;       
            }
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit: {placementId} - {error} - {message}");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {placementId}: {error} - {message}");
        }

        public void OnUnityAdsShowStart(string placementId) { }

        public void OnUnityAdsShowClick(string placementId) { }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) 
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                _gamePanel.SetActive(true);
                _moveController.NumberOfMoves(+5);
                _gameMenuAnimation.ForRestartAndContinue();               
                _showAdButton.interactable = false;
                Debug.Log("Unity Ads Rewarded Ad Completed");
            }
        }

    }
}
