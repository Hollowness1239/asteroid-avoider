using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{
    [SerializeField] private bool testMode;
    public static AdManager Instance;
    private GameOverHandler gameOverHandler;
#if UNITY_ANDROID
    private string gameId = "5329368";
#elif UNITY_IOS
    private string gameId = "5329369";
#endif
    void Awake()
    {
        Debug.Log("Awakeeeeeeeeeeeeeeeeeee");
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Advertisement.Initialize(gameId, testMode, this);
        }
    }

    public void ShowAd(GameOverHandler gameOverHandler)
    {
        this.gameOverHandler = gameOverHandler;
        Advertisement.Show("rewardedVideo", this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log($"Unity Ads Initialization Complete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error} - {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad Loaded: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error Loading Ad Unit {placementId}: {error} - {message}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"Show click");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"Show complete");
        switch (showCompletionState)
        {
            case UnityAdsShowCompletionState.COMPLETED:
                gameOverHandler.ContiueGame();
                break;
            case UnityAdsShowCompletionState.SKIPPED:
                Debug.Log($"Ads Skipped");
                break;
            case UnityAdsShowCompletionState.UNKNOWN:
                Debug.Log($"Ads Failed");
                break;
        }
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Unity Ads show failure {placementId}:{error} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Show start");
    }
}
