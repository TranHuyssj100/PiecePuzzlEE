using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using Firebase;
using UnityEngine;

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

        //Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        //Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    //public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    //{
    //    UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    //}

    //public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    //{
    //    UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    //}


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
        FirebaseAnalytics.LogEvent("Start_Level_" + theme + "_" + (level + 1).ToString("00"));
    }
    public void LogLoseLevel(int level, string theme)
    {
        FirebaseAnalytics.LogEvent("Lose_Level_" + theme + "_" + (level + 1).ToString("00"));
    }
    public void LogResetLevel(int level, string theme)
    {
        FirebaseAnalytics.LogEvent("Reset_Level_" + theme + "_" + (level + 1).ToString("00"));
    }
    public void LogPreviewHint()
    {
        FirebaseAnalytics.LogEvent("Hint_Preview");
    }
    public void LogAutoCorrectHint()
    {
        FirebaseAnalytics.LogEvent("Hint_Auto_Correct");
    }
}
