using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class SpinWheel : MonoBehaviour
{
    [SerializeField]
    private Sprite[] itemList;
    [SerializeField]
    private int[] itemNum;
    [SerializeField]
    private string[] itemName;
    [SerializeField]
    private GameObject wheel;
    public Transform arrow;

    [Header("Random Rewards")]
    [SerializeField]
    private bool spinning;
    [SerializeField]
    private float anglePerItem = 45;
    [SerializeField]
    private Button spinButton;
    public List<AnimationCurve> animationCurves;

    [Header("Timer")]
    public DateTime timer;



    [SerializeField]
    List<int> randomList = new List<int>();
    [SerializeField]
    private GameObject errorPopUpPanel;
    [SerializeField]
    private TextMeshProUGUI errorText;

    private int randomTime;
    private int itemNumber;

    public static SpinWheel instance;

    private void Awake()
    {
        transform.localScale = Vector3.one;
        //for (int n = 0; n < itemList.Length; n++)
        //{
        //    if (itemName[n] != "5" && itemName[n] != "6") randomList.Add(n);
        //    else
        //    {
        //        //if (!DataManager.Instance.GetUserCharacterData(n).is_unlocked) randomList.Add(n);

        //    }
        //} 

        var index = 0;
        //adManager = GameObject.Find("AdManager").GetComponent<AdManager>();
        foreach (Transform slot in wheel.transform.Find("rewards"))
        {
            slot.GetComponentInChildren<Image>().sprite = itemList[index];
            slot.GetComponentInChildren<TextMeshProUGUI>().text = itemNum[index].ToString();
            index += 1;
        }
        spinButton.onClick.AddListener(() => ShowDailyRewardAd());
    }

    private void OnEnable()
    {
        AdManager.Instance.onRewardAdClosed += RewardAdClosed;
    }
    private void OnDisable()
    {
        AdManager.Instance.onRewardAdClosed -= RewardAdClosed;
    }

    private void Start()
    {
        //ActiveDailyTimer();
    }
    public void RandomReward()
    {
        if (!spinning)
        {
            GameData.dailySpinAmount--;
            randomTime = UnityEngine.Random.Range(7, 10);
            int crit = UnityEngine.Random.Range(0,17);
            Debug.LogError(crit.ToString()+"/" +(randomList.Count-1));
            
            if (crit == randomList.Count-1) itemNumber = randomList.Count-1;
            else itemNumber = UnityEngine.Random.Range(0, randomList.Count-1);

            float maxAngle = 360 * randomTime + (itemNumber * 45);
            StartCoroutine(SpinTheWheel(0.5f * randomTime, maxAngle));   
        }
    }
    IEnumerator StartSpin(float maxAngle, float waitSeconds)
    {

        yield return new WaitForSeconds(5f);
        StartCoroutine(SpinTheWheel(0.5f * randomTime, maxAngle));
    }
    IEnumerator SpinTheWheel(float time, float maxAngle)
    {
        spinning = true;
        spinButton.interactable = false;
        float timer = 0f;
        float startAngle = wheel.transform.eulerAngles.z;
        maxAngle = maxAngle - startAngle;
        int animationCurveNumber = UnityEngine.Random.Range(0, animationCurves.Count);
        while (timer < time)
        {
            float angle = maxAngle * animationCurves[animationCurveNumber].Evaluate(timer / time);
            wheel.transform.localEulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
            timer += Time.deltaTime;
            yield return 0;
        }
        wheel.transform.localEulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
        spinning = false;
        spinButton.interactable = true;
        Debug.Log("<color=green>" + itemNumber + "_Prize: " + itemList[itemNumber] + itemNum[itemNumber]+"</color>");

        AddReward(itemName[itemNumber].ToString(), itemNum[itemNumber]);
    }

    void AddReward(string itemName, int value)
    {
        switch (itemName)
        {
            case "Golds":
                GameData.gold += value;
                TweenCustom.RangeTextRunner(GameData.gold - value, GameData.gold, 1f, GameMaster.instance.goldTxt.GetComponent<TextMeshProUGUI>());
                break;
            case "Gems":
                break;        
        }
    }

    #region Timer
    void ActiveDailyTimer()
    {
        if (GameData.dailyTimer == "")
        {
            CreateDailyTimer();
        }
        else
        {
            DateTime oldDate = GetDailyTimer();
            TimeSpan subTime = DateTime.Now.Subtract(oldDate);
            if(subTime.CompareTo( new TimeSpan(24,0, 0)) > 0)
            {
                CreateDailyTimer();
                GameData.dailySpinAmount = 3;
            }
            else
            {
                //ShowErrorPopUp()
            }
        }
    }

    void CreateDailyTimer()
    {
        GameData.dailyTimer= DateTime.Now.ToBinary().ToString();
        Debug.LogError(GameData.dailyTimer);
    }

    public DateTime GetDailyTimer()
    {
        long temp = Convert.ToInt64(GameData.dailyTimer);
        Debug.Log("dailyTimer: " + DateTime.FromBinary(temp));
        return DateTime.FromBinary(temp);
    }
    #endregion

    #region Reward
    public void ShowDailyRewardAd()
    {
        ActiveDailyTimer();
        Debug.LogError(GameData.dailySpinAmount);
        if (GameData.dailySpinAmount > 0)
        {
            AdManager.Instance.showRewardedAd(AdManager.RewardType.DailyReward);
    #if UNITY_EDITOR
            RandomReward();
    #endif
        }
    }

    private void RewardAdClosed()
    {
        if (AdManager.rewardType == AdManager.RewardType.DailyReward)
        {
            RandomReward();
        }
    }
    void ShowErrorPopUp(string errorCode)
    {
        errorText.text = errorCode;
        Sequence seq = DOTween.Sequence();
        seq.Pause();
        seq.Append(errorPopUpPanel.transform.DOScale(Vector3.one, .2f))
                 .Append(errorPopUpPanel.transform.DOScale(Vector3.one, 1f))
                 .Append(errorPopUpPanel.transform.DOScale(Vector3.zero, .2f));
        if (errorPopUpPanel.transform.localScale == Vector3.zero)
            seq.Play();
        else
        {
            errorPopUpPanel.transform.localScale = Vector3.zero;
            seq.Restart();
        }
    }

    #endregion





}
