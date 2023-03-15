using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class UGS_Analytics : MonoBehaviour
{
    [SerializeField] private int currentLevel;

    private async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            LevelCompletedCustomEvent();
        }
        catch (ConsentCheckException e) 
        { 
            Debug.LogError(e.ToString());
        }
        
    }

    private void LevelCompletedCustomEvent()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            {"levelName","level" + currentLevel.ToString()}
        };
        AnalyticsService.Instance.CustomData("levelCompleted", parameters);
        AnalyticsService.Instance.Flush();
    }
}
