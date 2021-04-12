using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using Firebase;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });


        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;

        System.Collections.Generic.Dictionary<string, object> defaults =
           new System.Collections.Generic.Dictionary<string, object>();

        // These are the values that are used if we haven't fetched data from the
        // server
        // yet, or if we ask for values that the server doesn't have:
        defaults.Add("show_ad_by_time_interval", false);
        defaults.Add("show_ad_time_interval", 40);
        defaults.Add("show_ad_time_interval_day0", 65);
        defaults.Add("show_ad_stage_day0_count", 2);
        defaults.Add("show_ad_stage_count", 1);

        Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defaults);
        // [END set_defaults]
        Debug.Log("RemoteConfig configured and ready!");
        FetchDataAsync();

    }

    #region RemoteConfig
    public Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        System.Threading.Tasks.Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync(
            TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Fetch completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();
                Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                                       info.FetchTime));
                //AdManager.showAdByTimeInterval = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("show_ad_by_time_interval").BooleanValue;
                //AdManager.showAdInterval = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("show_ad_time_interval").StringValue);
                //AdManager.stageToShowAdDay0 = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("show_ad_stage_day0_count").StringValue);
                //AdManager.stageToShowAd = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("show_ad_stage_count").StringValue);
                AdManager.Instance.RefreshConfig(
                    Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("show_ad_by_time_interval").BooleanValue,
                    int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("show_ad_time_interval").StringValue),
                    int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("show_ad_time_interval_day0").StringValue),
                    int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("show_ad_stage_day0_count").StringValue),
                    int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("show_ad_stage_count").StringValue));
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
        Debug.Log(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("config_test_string").StringValue);
    }
#endregion


#region Messaging
public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }
    #endregion


    public void LogShowInter()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("Ads_Inter");
    }
    public void LogShowReward(string reward)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("Ads_Reward","RewardType",reward);
    }
    public void LogRewarded(string reward)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("Ads_Rewarded","RewardType",reward);
    }


    public void LogStartLevel(int level,string theme)
    {
        string trimmed = String.Concat(theme.Where(c => !Char.IsWhiteSpace(c)));
        FirebaseAnalytics.LogEvent("Start_Level_" + trimmed + "_" + (level + 1).ToString("00"));
    }
    public void LogLoseLevel(int level, string theme)
    {
        string trimmed = String.Concat(theme.Where(c => !Char.IsWhiteSpace(c)));
        FirebaseAnalytics.LogEvent("Lose_Level_" + trimmed + "_" + (level + 1).ToString("00"));
    }
    //public void LogResetLevel(int level, string theme)
    //{
    //    string trimmed = String.Concat(theme.Where(c => !Char.IsWhiteSpace(c)));
    //    FirebaseAnalytics.LogEvent("Reset_Level_" + trimmed + "_" + (level + 1).ToString("00"));
    //}
    public void LogUnlockLevel(int level, string theme)
    {
        FirebaseAnalytics.LogEvent("Unlock_Level_" + level.ToString("000"),"Theme",theme);
    }
    public void LogPreviewHint()
    {
        FirebaseAnalytics.LogEvent("Hint_Preview");
    }
    public void LogAutoCorrectHint()
    {
        FirebaseAnalytics.LogEvent("Hint_Auto_Correct");
    }
    public void LogIAP(string product)
    {
        FirebaseAnalytics.LogEvent("Purchae_IAP","Product", product);
    } 
    public void LogDailySpin()
    {
        FirebaseAnalytics.LogEvent("Daily_Spin");
    }
}
