﻿using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public Image img;

    public Transform reward;
    public Transform progressBar;
    public Transform giftBox;
    public Transform panelGift;
    public Transform coins;
    public Transform allCoin;
    public Transform claimx5;
    public Transform gotIt;
    public Transform coinTxt;


    int amountCoin=0;
    public Vector3 oldGiftBoxPos;
    public Vector3 oldCoin0Pos;
    public Vector3 oldCoin1Pos;
    public Vector3 oldCoin2Pos;

    public static WinPanel instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        AdManager.Instance.onRewardAdClosed += RewardAdClosed;
        oldGiftBoxPos = giftBox.localPosition;
        oldCoin0Pos = coins.GetChild(0).position;
        oldCoin1Pos = coins.GetChild(1).position;
        oldCoin2Pos = coins.GetChild(2).position;
    }

    private void OnDestroy()
    {
        AdManager.Instance.onRewardAdClosed -= RewardAdClosed;
    }
    public void SetImageReview()
    {
        int _level = TestLevelCtr.instance.idLevel;
        //Debug.Log(_level);
        img.sprite = DataController.LoadSpritePreview(TestLevelCtr.instance.idTheme, TestLevelCtr.instance.idLevel, TestLevelCtr.instance.sizeLevel);
        //TweenCustom.ReWard(reward.GetChild(0).gameObject, reward.GetChild(1).gameObject, Config.GOLD_WIN);
        //ShowProgress(5);
    }

    public void ShowProgress(int _levelReward)
    {
        Transform fill = progressBar.GetChild(1);

        float _surPlus = GameData.levelReward % _levelReward;
        float _value = _surPlus / (float)_levelReward;
        giftBox.GetChild(1).GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
        giftBox.GetChild(2).GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
       

        if (_surPlus != 0)
        {
            DOVirtual.Float(0, _value, 0.5f, (x) => { fill.GetComponent<Image>().fillAmount = x; });
            reward.GetChild(1).GetComponent<TextMeshProUGUI>().text = (_surPlus).ToString() + "/" + _levelReward.ToString();
        }
        else
        {
            DOVirtual.Float(0, 1, 0.5f, (x) => { fill.GetComponent<Image>().fillAmount = x; });
            reward.GetChild(1).GetComponent<TextMeshProUGUI>().text = _levelReward.ToString() + "/" + _levelReward.ToString();
            OpenGift(1.2f, 0.5f);
            GameData.levelReward = 0;
        }
    }

    public void ShowX5CoinAd()
    {
        AdManager.Instance.showRewardedAd(AdManager.RewardType.PentaReward);
#if UNITY_EDITOR
        RewardAdClosed();
#endif
    }

    private void RewardAdClosed()
    {
        if(AdManager.rewardType == AdManager.RewardType.PentaReward)
        {
            GameData.gold += amountCoin * 4;
            claimx5.gameObject.SetActive(false);
            CloseGift();
        }
    }
    public void OpenGift(float strength, float duration)
    {
        GameData.levelReward = 0;
        CloseGift();
        panelGift.gameObject.SetActive(true);
        giftBox.gameObject.SetActive(true);
        claimx5.gameObject.SetActive(true);


        RandomCoin();
        Debug.LogError(amountCoin);
        GameData.gold+= amountCoin;
        allCoin.GetChild(0).GetComponent<TextMeshProUGUI>().text = (GameData.gold - amountCoin).ToString();
        Sequence seq = DOTween.Sequence();
        seq.Append(giftBox.DOMove(panelGift.transform.position, duration))
            .Append(giftBox.DOScale(Vector3.one * 2 + Vector3.one * strength, duration))
            .Append(giftBox.DOScale(Vector3.one * 2, duration / 4))
            .Append(giftBox.GetChild(1).GetComponent<Image>().DOFade(0, duration / 2))
            .Append(giftBox.GetChild(2).GetComponent<Image>().DOFade(0, duration))
            .Append(coins.DOScale(Vector3.one / 2.5f, 0))
            .Append(coins.GetChild(0).DOMove(allCoin.position, duration/1.5f))
            .Append(coins.GetChild(0).DOScale(Vector3.zero, 0))
            .Append(coins.GetChild(1).DOMove(allCoin.position, duration/1.5f))
            .Append(coins.GetChild(1).DOScale(Vector3.zero, 0))
            .Append(coins.GetChild(2).DOMove(allCoin.position, duration/1.5f))
            .Append(coins.GetChild(2).DOScale(Vector3.zero, 0))
            .Append(DOVirtual.Float(GameData.gold - amountCoin, GameData.gold, 1f, ((x) => { allCoin.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)x).ToString(); })))
            .Append(claimx5.DOScale(Vector3.one, duration))
            .Append(gotIt.DOScale(Vector3.one, duration * 2))
            .OnComplete(() => {
                coins.GetChild(0).position = oldCoin0Pos;
                coins.GetChild(1).position = oldCoin1Pos;
                coins.GetChild(2).position = oldCoin2Pos;
            });
        coinTxt.GetComponent<TextMeshProUGUI>().text = "+ " + amountCoin.ToString();
    }

   public void CloseGift()
    {
        DOTween.CompleteAll();
        panelGift.gameObject.SetActive(false);
        coins.localScale = Vector3.zero;
        claimx5.localScale = Vector3.zero;
        gotIt.localScale = Vector3.zero;
        giftBox.localPosition = oldGiftBoxPos;
        giftBox.localScale = Vector3.one;
        coins.GetChild(0).localScale = Vector3.one;
        coins.GetChild(1).localScale = Vector3.one;
        coins.GetChild(2).localScale = Vector3.one;
    }
    public void RandomCoin()
    {
        amountCoin = Random.Range(5, 11) * 10;
    } 
    
}
