using UnityEngine;
using UnityEngine.Advertisements;

namespace Assets.Match.Scripts.Ads
{

    public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {

#region Android ID

        private readonly string _androidAdsID = "Interstitial_Android";

#endregion


#region iOS ID

        private readonly string _iOSAdsID = "Interstitial_iOS";

#endregion

        private string _adId;

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
        }

        public void LoadAd()
        {
            Debug.Log("Loading Ad:" + _adId);
            Advertisement.Load(_adId, this);
        }

        public void OnUnityAdsAdLoaded(string placementId) 
        {
            Debug.Log("Ad Loaded:" + _adId);
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit: {placementId} - {error} - {message}");
        }

        public void OnUnityAdsShowClick(string placementId) { }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            LoadAd();
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {placementId}: {error} - {message}");
        }

        public void OnUnityAdsShowStart(string placementId) { }

        public void ShowAd()
        {
            Debug.Log("Showing Ad:" + _adId);
            Advertisement.Show(_adId, this);
        }

    }
}
