﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    public event Action onRewardAdClosed;

    public void RewardAdClosed()
    {
        if (onRewardAdClosed != null)
            onRewardAdClosed();
    }
     public enum RewardType
    {
        Gold,
        DoubleReward,
        PentaReward,
        MoreMove
    }

    public static RewardType rewardType;


    public RewardedAd rewardedAd;
    private bool isSecondChanceRewarded;

    public InterstitialAd interstitialAd;

    public BannerView bannerView;

    string rewardedAdUnitId, interstitialAdUnitId, bannerAdUnitId;


    public static AdManager instance;
    // Start is called before the first frame update

    void Start()
    {
        if (instance == null)
            instance = this;
        DontDestroyOnLoad(this);
#if UNITY_ANDROID
        rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
        interstitialAdUnitId = "ca-app-pub-3940256099942544/8691691433";
        bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IOS
            //rewardedAdUnitId = "ca-app-pub-9179752697212712/7094033959";
            //interstitialAdUnitId = "ca-app-pub-9179752697212712/7828686655";
            rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
            interstitialAdUnitId = "ca-app-pub-3940256099942544/8691691433";
            bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";

#else
		rewardedAdUnitId = "unexpected_platform";
            interstitialAdUnitId = "unexpected_platform";
#endif

        MobileAds.Initialize(initStatus => { });

        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

#if UNITY_ANDROID
        deviceIds.Add("75EF8D155528C04DACBBA6F36F433035");
        deviceIds.Add("864816020118760");
        deviceIds.Add("864816020118778");
        deviceIds.Add("495ECB59AE9A355C");
#endif
        RequestConfiguration requestConfiguration =
         new RequestConfiguration.Builder()
         .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
         .SetTestDeviceIds(deviceIds).build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        loadRewardedAd();

        loadInterstitialAd();

        //// Called when an ad request has successfully loaded.
        //this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        //// Called when an ad request failed to load.
        //this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        //// Called when an ad is shown.
        //this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        //// Called when an ad request failed to show.
        //this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        //// Called when the user should be rewarded for interacting with the ad.
        //this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        //// Called when the ad is closed.
        //this.rewardedAd.OnAdClosed += HandleRewardedAdClosed; interstitialAdEventSubscribe();



        //// Called when an ad request has successfully loaded.
        //this.interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        //// Called when an ad request failed to load.
        //this.interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        //// Called when an ad is shown.
        //this.interstitialAd.OnAdOpening += HandleOnAdOpened;
        //// Called when the ad is closed.
        //this.interstitialAd.OnAdClosed += HandleOnAdClosed;
        //// Called when the ad click caused the user to leave the application.
        //this.interstitialAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;





    }

    public void rewardedAdEventSubscribe()
    {
        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }


    public void interstitialAdEventSubscribe()
    { 
        // Called when an ad request has successfully loaded.
        this.interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitialAd.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitialAd.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitialAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;
    }



    //--------------------------------------------------------------------------------------------------------
    void loadBannerAds()
    {
        AdRequest request = new AdRequest.Builder().Build();
        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
    }
    //--------------------------------------------------------------------------------------------------------
    #region InterstitialAd
    void loadInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
        this.interstitialAd = new InterstitialAd(interstitialAdUnitId);
        interstitialAdEventSubscribe();
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitialAd.LoadAd(request);
    }

    public void showInterstitialAd()
    {
        if (this.interstitialAd.IsLoaded())
        {
            //FirebaseManager.instance.LogShowInter();
            interstitialAd.Show();
        }
    }
    public IEnumerator waitAndReloadInterstitialAd()
    {
        yield return new WaitForSecondsRealtime(10f);
        loadInterstitialAd();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        StartCoroutine("waitAndReloadRewardedAd");
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        Time.timeScale = 0;
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        loadInterstitialAd();
        Time.timeScale = 1f;
    }

public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
    }
    #endregion
    //--------------------------------------------------------------------------------------------------------
    #region RewardedAd
    private bool isRewarded;
    void loadRewardedAd()
    {
        this.rewardedAd = new RewardedAd(rewardedAdUnitId);
        rewardedAdEventSubscribe();
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);

    }
    public void showRewardedAd(RewardType _rewardType)
    {
        if (this.rewardedAd.IsLoaded())
        {
            isRewarded = false;
            rewardType = _rewardType;
            //FirebaseManager.instance.LogShowReward();
            this.rewardedAd.Show();
        }
    }
    public IEnumerator waitAndReloadRewardedAd()
    {
        yield return new WaitForSecondsRealtime(10f);
        loadRewardedAd();
    }
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        StartCoroutine("waitAndReloadRewardedAd");
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Time.timeScale = 0;
        isSecondChanceRewarded = false;
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        StartCoroutine("waitAndReloadRewardedAd");
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        loadRewardedAd();
        Time.timeScale = 1;
        if(isRewarded)
            RewardAdClosed();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        isRewarded = true;
        //FirebaseManager.instance.LogRewarded(rewardType.ToString());
    }
    #endregion

}
