using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Linq;

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
    [SerializeField]
    private Button returnBtn, panelBtn;

    [Header("Random Rewards")]
    [SerializeField]
    private bool spinning;
    [SerializeField]
    private float anglePerItem = 45;
    [SerializeField]
    private Button spinButton;
    public TextMeshProUGUI amountSpin;
    public TextMeshProUGUI tileSpinBtn;
    public int maxAmountSpin = 5;
    public List<AnimationCurve> animationCurves;

    [Header("Timer")]
    public DateTime timer;

    [Header("PopUp Results")]
    public GameObject popupResult;
    public Image resultIcon;
    [SerializeField]
    private TextMeshProUGUI resultValue;
    [SerializeField]
    private GameObject gotIt;
    public Button claimx5;


    [SerializeField]
    List<int> randomList = new List<int>();
    [SerializeField]
    private GameObject errorPopUpPanel;
    [SerializeField]
    private TextMeshProUGUI errorText;

    int randomTime;
    int itemNumber;
    int rewardvalue;
    double waitTime;

    public static SpinWheel instance;

    private void Awake()
    {
        ResetSpinWheel();
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
        //spinButton.onClick.AddListener(() => ShowDailyRewardAd());
        spinButton.onClick.AddListener(() => RandomReward());
    }

    private void OnEnable()
    {
        AdManager.Instance.onRewardAdClosed += RewardX5AdClosed;
        //CheckActiveDailyTimer();
        claimx5.transform.localScale = Vector3.zero;
        gotIt.transform.localScale = Vector3.zero;

    }
    private void OnDisable()
    {
        if (AdManager.Instance != null)
            AdManager.Instance.onRewardAdClosed -= RewardX5AdClosed;
    }

    private void Start()
    {
        maxAmountSpin = GameData.maxDailySpinAmount;
        amountSpin.text = GameData.dailySpinAmount + "/" + maxAmountSpin;
        returnBtn.onClick.AddListener(() =>
        {
            GameMaster.instance.CloseDailySpin();
            GameMaster.instance.ChecKSpinDailyNoti();
        });
        panelBtn.onClick.AddListener(() =>
        {
            GameMaster.instance.CloseDailySpin();
            GameMaster.instance.ChecKSpinDailyNoti();
        });

    }
    private void Update()
    {
        //CountDownActiveDailySpin(amountSpin);        
        CheckActiveDailyTimer();
    }
    public void RandomReward()
    {
        if (GameData.dailySpinAmount > 0 && GameData.availableDailySpin == 1)
        {
            if (!spinning)
            {
                GameData.showDailySpin = 0;
                GameData.availableDailySpin = 0;
                GameData.dailySpinAmount--;
                CreateDailyTimer();
                randomTime = UnityEngine.Random.Range(7, 10);
                int crit = UnityEngine.Random.Range(0, 50);
                //Debug.LogError(crit.ToString()+"/" +(randomList.Count-1));

                if (crit == 1) itemNumber = randomList.Count - 1;
                else itemNumber = UnityEngine.Random.Range(0, randomList.Count - 1);

                float maxAngle = 360 * randomTime + (itemNumber * 45);
                StartCoroutine(SpinTheWheel(0.5f * randomTime, maxAngle));
                FirebaseManager.instance.LogDailySpin();
            }
        }
        else
        {
            ShowErrorPopUp("The Spin is not Ready!");
        }
        Debug.LogError(GameData.dailySpinAmount);
        //amountSpin.text = GameData.dailySpinAmount + "/" + maxAmountSpin;

    }
    IEnumerator StartSpin(float maxAngle, float waitSeconds)
    {

        yield return new WaitForSeconds(5f);
        StartCoroutine(SpinTheWheel(0.5f * randomTime, maxAngle));
    }
    IEnumerator SpinTheWheel(float time, float maxAngle)
    {
        spinning = true;
        returnBtn.onClick.RemoveAllListeners();
        panelBtn.onClick.RemoveAllListeners();
        spinButton.interactable = false;

        SoundManager.instance.PlaySFX(TypeSFX.SpinWheel, "Spin");

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
        Debug.Log("<color=green>" + itemNumber + "_Prize: " + itemList[itemNumber] + itemNum[itemNumber] + "</color>");

        AddReward(itemName[itemNumber].ToString(), itemNum[itemNumber]);
        returnBtn.onClick.AddListener(() =>
        {
            GameMaster.instance.CloseDailySpin();
            GameMaster.instance.ChecKSpinDailyNoti();
        });
        panelBtn.onClick.AddListener(() =>
        {
            GameMaster.instance.CloseDailySpin();
            GameMaster.instance.ChecKSpinDailyNoti();
        });
        rewardvalue = itemNum[itemNumber];
        ShowPopUp();
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

    void ShowPopUp()
    {
        //popupResult.transform.DOScale(Vector3.one, 0.2f);
        GameMaster.instance.OpenPanel(popupResult);
        resultIcon.sprite = itemList[itemNumber];
        resultValue.text = itemNum[itemNumber].ToString();

        Sequence seq = DOTween.Sequence();

        seq.Append(claimx5.transform.DOScale(Vector3.one, 0.5f))
        .Append(gotIt.transform.DOScale(Vector3.one, 1));

        claimx5.onClick.RemoveAllListeners();
        claimx5.onClick.AddListener(() => ShowX5CoinAd());

    }


    #region Timer
    void CheckActiveDailyTimer()
    {
        if (GameData.dailyTimer != "")
        {
            DateTime oldDate = GetDailyTimer();
            if (DateTime.Now.CompareTo(oldDate) > 0)
            {
                GameData.availableDailySpin = 1;
                tileSpinBtn.text = "Spin";
                amountSpin.text = GameData.dailySpinAmount + "/" + maxAmountSpin;
                //CreateDailyTimer();
            }
            else
            {
                TimeSpan subTime = oldDate.Subtract(DateTime.Now);
                double temp = (subTime).TotalSeconds - Convert.ToDouble(Time.deltaTime);
                //Debug.LogError(subTime);
                tileSpinBtn.text = "Waiting:";
                amountSpin.text = TimeSpan.FromSeconds(temp).ToString("hh\\:mm\\:ss");
            }
        }
    }


    void CreateDailyTimer()
    {
        if (DateTime.Now.AddMinutes(5f).Day != DateTime.Now.Day)
        {
            GameData.showDailySpin = 1;
            GameData.dailySpinAmount = 5;
            DateTime activeTimer = DateTime.Now.AddMinutes(5f);
            GameData.dailyTimer = activeTimer.ToBinary().ToString();
            Debug.LogError(activeTimer);

        }
        else
        {
            if (GameData.dailySpinAmount > 0)
            {
                DateTime activeTimer = DateTime.Now.AddMinutes(5f);
                GameData.dailyTimer = activeTimer.ToBinary().ToString();
                Debug.LogError(activeTimer);
            }
            else
            {
                GameData.showDailySpin = 1;
                GameData.dailySpinAmount = 5;
                DateTime tororrow = DateTime.Now.AddDays(1);
                DateTime activeTimer = new DateTime(tororrow.Year, tororrow.Month, tororrow.Day, 0, 0, 0);
                GameData.dailyTimer = activeTimer.ToBinary().ToString();
                Debug.LogError(activeTimer);
            }
        }
    }


    public DateTime GetDailyTimer()
    {
        long temp = Convert.ToInt64(GameData.dailyTimer);
        return DateTime.FromBinary(temp);
    }

    public void ResetSpinWheel()
    {
        if (GameData.dailyTimer != "")
        {
            if (DateTime.Now.Day != GetDailyTimer().Day)
            {
                GameData.showDailySpin = 1;
                GameData.availableDailySpin = 1;
                GameData.dailySpinAmount = 5;
                DateTime activeTimer = DateTime.Now;
                GameData.dailyTimer = activeTimer.ToBinary().ToString();
                tileSpinBtn.text = "Spin";
                amountSpin.text = GameData.dailySpinAmount + "/" + maxAmountSpin;
                Debug.LogError(activeTimer);

            }
        }
    }
    #endregion

    #region Reward
    //public void ShowDailyRewardAd()
    //{
    //    CheckActiveDailyTimer();
    //    Debug.LogError(GameData.dailySpinAmount);
    //    if (GameData.dailySpinAmount > 0)
    //    {
    //        AdManager.Instance.showRewardedAd(AdManager.RewardType.DailyReward);
    //#if UNITY_EDITOR
    //        RandomReward();
    //#endif
    //    }
    //    else
    //    {
    //        ShowErrorPopUp("The Spin is not Ready!");
    //    }
    //}
    //private void RewardAdClosed()
    //{
    //    if (AdManager.rewardType == AdManager.RewardType.DailyReward)
    //    {
    //        RandomReward();
    //    }
    //}



    public void ShowX5CoinAd()
    {
        AdManager.Instance.showRewardedAd(AdManager.RewardType.DailyReward);
#if UNITY_EDITOR
        RewardX5AdClosed();
#endif
    }

    private void RewardX5AdClosed()
    {
        if (AdManager.rewardType == AdManager.RewardType.DailyReward)
        {
            Debug.Log(rewardvalue * 4);
            GameData.gold += rewardvalue * 4;
            claimx5.transform.localScale = Vector3.zero;
            gotIt.transform.localScale = Vector3.zero;
            GameMaster.instance.ClosePanel(popupResult);
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
