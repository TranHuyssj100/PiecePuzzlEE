using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSelect : CoroutineQueue
{
    public Transform content;
    public GameObject themeChild;
    private int amountTheme;
    public ThemeData[] themes;

    public List<int> themeOrder;

    private void Start()
    {
        //AdManager.Instance.onRewardAdClosed += RewardAdClosed;
        themes = DataController.themeData;
        amountTheme = themes.Length;
        themeOrder =new List<int>{ 0, 1, 18, 2, 3, 4, 5, 6, 7, 19, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        CreateThemeChild();
    }

    private void OnEnable()
    {
        CreateThemeChild();
        AdManager.Instance.onRewardAdClosed += RewardAdClosed;
    }

    private void OnDisable()
    {
        AdManager.Instance.onRewardAdClosed -= RewardAdClosed;
    }

    private void OnDestroy()
    {

    }

    public void CreateThemeChild()
    {
        Queue<IEnumerator> _coroutineQueue = new Queue<IEnumerator>();
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < amountTheme; i++)
        {
            GameObject _themeClone = GameObject.Instantiate(themeChild, content);
            _themeClone.GetComponent<ThemeChild>().index = themes[i].idTheme;
            _themeClone.GetComponent<ThemeChild>().titleTxt.text = themes[i].name;
            _themeClone.GetComponent<ThemeChild>().SetRankTheme(themes[i].size, themes[i].price);
            _themeClone.GetComponent<ThemeChild>().image.sprite = DataController.LoadSpritePreview(themes[i].idTheme, 0, themes[i].size);
            _themeClone.GetComponent<ThemeChild>().SetTypeUnlock(themes[i].price);
            int x = i;
            if (GameData.GetThemeStatus(i) == 0)
            {
                if(themes[i].price>0)
                {
                    
                    _themeClone.GetComponent<ThemeChild>().priceTxt.text = themes[i].price.ToString();
                    _themeClone.GetComponent<ThemeChild>().buyBtn.onClick.AddListener(() => UnlockThemeByCoin(x));
                }
                else
                {
                    _themeClone.GetComponent<ThemeChild>().ad_UnlockBtn.onClick.AddListener(() => ShowUnlockAd(x));
                }
            }
            else
            {
                if (themes[i].price>0)
                {
                    _themeClone.GetComponent<ThemeChild>().UnlockByCoin();
                }
                else 
                {
                    _themeClone.GetComponent<ThemeChild>().UnlockByAd();
                }
                _themeClone.GetComponent<ThemeChild>().progressBtn.onClick.AddListener(() => { GameMaster.instance.OpenLevelSelect();
                                                                                          GameData.Theme = x;
                                                                                          GridLevel.instance.SpawnGridChild(x, themes[x].size);
                                                                                          GameMaster.instance.CloseThemeSelect();
                                                                                          //GameMaster.instance.OnStartClick();
                                                                                         });
            }
            int siblingIndex = themeOrder.IndexOf(themes[i].idTheme);
            _themeClone.transform.SetSiblingIndex(siblingIndex);
        }
    }


    private void UnlockThemeByCoin(int index)
    {
        if (GameData.gold >= themes[index].price)
        {
            GameData.gold -= themes[index].price;
            GameData.level++;
            FirebaseManager.instance.LogUnlockLevel(GameData.level, DataController.themeData[GameData.Theme].name);
            foreach (Transform child in content)
            {
                if (child.GetComponent<ThemeChild>().index == index)
                {
                    child.GetComponent<ThemeChild>().UnlockByCoin();
                    child.GetComponent<ThemeChild>().buyBtn.onClick.RemoveAllListeners();
                    child.GetComponent<ThemeChild>().progressBtn.onClick.AddListener(() => {
                        GameMaster.instance.OpenLevelSelect();
                        GameData.Theme = index;
                        GridLevel.instance.SpawnGridChild(index, themes[index].size);
                        GameMaster.instance.CloseThemeSelect();
                    });
                }
            }
        }
        else
        {
            GameMaster.instance.OpenShopUI();
        }
    }

    int indexAd;
    void UnlockThemeByAd(int index)
    {
        GameData.level++;
        FirebaseManager.instance.LogUnlockLevel(GameData.level, DataController.themeData[GameData.Theme].name);
        //indexAd = index;

        foreach (Transform child in content)
        {
            if (child.GetComponent<ThemeChild>().index == index)
            {
                child.GetComponent<ThemeChild>().UnlockByAd();
                child.GetComponent<ThemeChild>().ad_UnlockBtn.onClick.RemoveAllListeners();
                child.GetComponent<ThemeChild>().progressBtn.onClick.AddListener(() => {
                    GameMaster.instance.OpenLevelSelect();
                    GameData.Theme = index;
                    GridLevel.instance.SpawnGridChild(index, themes[index].size);
                    GameMaster.instance.CloseThemeSelect();
                });
            }
        }
    }

        #region Reward_UnlocK
    public void ShowUnlockAd(int index)
    {
        indexAd = index;
        AdManager.Instance.showRewardedAd(AdManager.RewardType.UnLockTheme);
#if UNITY_EDITOR
        RewardAdClosed();
#endif
    }
    private void RewardAdClosed()
    {
        if (AdManager.rewardType == AdManager.RewardType.UnLockTheme)
            UnlockThemeByAd(indexAd);
    }
    #endregion
}

