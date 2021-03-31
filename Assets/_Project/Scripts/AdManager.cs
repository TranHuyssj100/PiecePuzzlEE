using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : SingletonDontDestroyMonoBehavior<AdManager>
{
    public event Action onRewardAdClosed;


    bool isDay0;

    public bool showAdByTimeInterval;

    public int showAdInterval = 20;
    private float showAdTimer;


    public int stagePlayed = 0;
    private static int stageToShowAdDay0 = 3;
    private static int stageToShowAd = 2;

    public void RefreshConfig(bool _showAdByTime, int _showInterval, int _stageToShowDay0,int _stageToShow)
    {
        Debug.Log("asdasdasd");
        showAdByTimeInterval = _showAdByTime;
        showAdInterval = _showInterval;
        stageToShowAdDay0 = _stageToShowDay0;
        stageToShowAd = _stageToShow;
        Debug.Log(showAdByTimeInterval);
        Debug.Log(showAdInterval);
        Debug.Log(stageToShowAdDay0);
        Debug.Log(stageToShowAd);

    }

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

    public InterstitialAd interstitialAd;

    public BannerView bannerView;

    string rewardedAdUnitId, interstitialAdUnitId, bannerAdUnitId;


    //public static AdManager instance;
    // Start is called before the first frame update

    void Start()
    {
        if (CheckIsDay0())
            stageToShowAd = 3;
        else
            stageToShowAd = 2;
        showAdTimer = showAdInterval;
#if UNITY_ANDROID
        rewardedAdUnitId = "ca-app-pub-9179752697212712/9650286780";
        interstitialAdUnitId = "ca-app-pub-9179752697212712/7215695137";
        bannerAdUnitId = "ca-app-pub-9179752697212712/2320437582";
#elif UNITY_IOS
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
#if !UNITY_IOS
        if(GameData.noAds != 1)
            loadBannerAds();
#endif
        loadInterstitialAd();


        stagePlayed = 0;

        
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

    private void Update()
    {
        showAdTimer -= Time.deltaTime;
    }

    bool CheckIsDay0()
    {
        if (!PlayerPrefs.HasKey("Day0"))
        {
            PlayerPrefs.SetString("Day0", DateTime.Now.Date.ToString());
            return true;
        }
        if (DateTime.Now.Date > DateTime.Parse(PlayerPrefs.GetString("Day0", DateTime.Now.Date.ToString())))
            return false;
        else
            return true;
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
#region banner
    void loadBannerAds()
    {
    //    AdRequest request = new AdRequest.Builder().Build();
    //    bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
#if UNITY_ANDROID
        string adUnitId = bannerAdUnitId;
#elif UNITY_IOS
            string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }
#endregion
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

    public void checkInterAdsCondition()
    {
        stagePlayed++;
        if (showAdByTimeInterval)
        {
            if (showAdTimer <= 0)
            {
                showAdTimer = showAdInterval;
                showInterstitialAd();
            }
        }
        else
        {
            if (stagePlayed >= stageToShowAd)
            {
                stagePlayed = 0;
                showInterstitialAd();
            }
        }
        
    }    

    public void showInterstitialAd()
    {
#if !UNITY_IOS
        if (this.interstitialAd.IsLoaded() && GameData.noAds != 1)
        {
            FirebaseManager.instance.LogShowInter();
            interstitialAd.Show();
        }
#endif
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
#if !UNITY_IOS
        if (this.rewardedAd.IsLoaded())
        {
            isRewarded = false;
            rewardType = _rewardType;
            FirebaseManager.instance.LogShowReward(rewardType.ToString());
            this.rewardedAd.Show();
        }
#endif
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
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        StartCoroutine("waitAndReloadRewardedAd");
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        loadRewardedAd();
        Time.timeScale = 1;
        if (isRewarded)
        {
            showAdTimer = showAdInterval;
            RewardAdClosed();
        }
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        isRewarded = true;
        FirebaseManager.instance.LogRewarded(rewardType.ToString());
    }

#endregion

}
