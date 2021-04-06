using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSelect : CoroutineQueue
{
    public Transform content;
    public GameObject themeChild;
    protected int amountTheme;
    public ThemeData[] themes;
    public Sprite Ads;
    public Sprite AdsBtn;

    public GridLevel gridLevel;

    public List<int> themeOrder;

    private void Start()
    {
        //allTheme = DataController.GetAllTheme();
        themes = DataController.themeData;
        amountTheme = themes.Length;
        themeOrder =new List<int>{ 0, 1, 18,20, 2, 3, 4, 5, 6, 7, 19, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        CreateThemeChild();

        AdManager.Instance.onRewardAdClosed += RewardAdClosed;
    }

    private void OnEnable()
    {
        CreateThemeChild();
    }
    private void OnDestroy()
    {
        AdManager.Instance.onRewardAdClosed -= RewardAdClosed;
    }
    public virtual void CreateThemeChild()
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
            int x = i;
            if (GameData.GetThemeStatus(i) == 0)
            {
                if (themes[i].price > 0)
                {
                    _themeClone.GetComponent<ThemeChild>().priceTxt.text = themes[i].price.ToString();
                    _themeClone.GetComponent<ThemeChild>().buyBtn.onClick.AddListener(() => UnlockTheme(x));
                }
                else
                {
                    _themeClone.GetComponent<ThemeChild>().priceTxt.text = "Unlock";
                    _themeClone.GetComponent<ThemeChild>().buyBtn.GetComponent<UnityEngine.UI.Image>().sprite = AdsBtn;
                    _themeClone.GetComponent<ThemeChild>().buyBtn.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = Ads;
                    _themeClone.GetComponent<ThemeChild>().buyBtn.onClick.AddListener(() => ShowAdToUnlockTheme(x));
#if UNITY_EDITOR
                    _themeClone.GetComponent<ThemeChild>().buyBtn.onClick.AddListener(() => RewardAdClosed());
#endif
                }
            }
            else
            {
                _themeClone.GetComponent<ThemeChild>().UnlockTheme();
                //_themeClone.GetComponent<ThemeChild>().priceTxt.text = "Open";
                _themeClone.GetComponent<ThemeChild>().progressBtn.onClick.AddListener(() => { GameMaster.instance.OpenLevelSelect();
                                                                                          GameData.Theme = x;
                                                                                          gridLevel.SpawnGridChild(x, themes[x].size);
                                                                                          GameMaster.instance.CloseThemeSelect();
                                                                                          //GameMaster.instance.OnStartClick();
                                                                                         });
            }
            //_coroutineQueue.Enqueue(ShowObjRightToLeft(_themeClone.transform.Find("BG"), 0.1f));
            int siblingIndex = themeOrder.IndexOf(themes[i].idTheme);
            _themeClone.transform.SetSiblingIndex(siblingIndex);
        }
        //StartCoroutine(CoroutineCoordinator(_coroutineQueue));
    }
    
    private void UnlockTheme(int index)
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
                    child.GetComponent<ThemeChild>().UnlockTheme();
                    child.GetComponent<ThemeChild>().buyBtn.onClick.RemoveAllListeners();
                    child.GetComponent<ThemeChild>().progressBtn.onClick.AddListener(() => {
                        GameMaster.instance.OpenLevelSelect();
                        GameData.Theme = index;
                        gridLevel.SpawnGridChild(index, themes[index].size);
                        GameMaster.instance.CloseThemeSelect();
                    });
                }
            }
            //content.GetChild(index).GetComponent<ThemeChild>().UnlockTheme();
            //content.GetChild(index).GetComponent<ThemeChild>().buyBtn.onClick.RemoveAllListeners();
            //content.GetChild(index).GetComponent<ThemeChild>().progressBtn.onClick.AddListener(() => { GameMaster.instance.OpenLevelSelect();
            //                                                                                     GameData.Theme = index;
            //                                                                                     GridLevel.instance.SpawnGridChild(index, themes[index].size);
            //                                                                                     GameMaster.instance.CloseThemeSelect();
            //                                                                                     //GameMaster.instance.OnStartClick();
            //                                                                                     });
        }
        else
        {
            GameMaster.instance.OpenShopUI();
        }
    }


    int themeToUnlock;

    void ShowAdToUnlockTheme(int index)
    {
        themeToUnlock = index;
        AdManager.Instance.showRewardedAd(AdManager.RewardType.UnlockTheme);
    }
    void RewardAdClosed()
    {
        if (AdManager.rewardType == AdManager.RewardType.UnlockTheme)
            UnlockTheme(themeToUnlock);
    }
}
