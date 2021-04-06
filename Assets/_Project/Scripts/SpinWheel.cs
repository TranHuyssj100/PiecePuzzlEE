using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;


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

    [Header("Random Rewards")]
    [SerializeField]
    private bool spinning;
    [SerializeField]
    private float anglePerItem = 45;
    [SerializeField]
    private Button spinButton;
    public List<AnimationCurve> animationCurves;

    private int randomTime;
    private int itemNumber;

    [SerializeField]
    private TextMeshProUGUI requirementText;

    [Header("PopUp Results")]
    [SerializeField]
    private GameObject popupResult;
    [SerializeField]
    private Image resultIcon;
    [SerializeField]
    private TextMeshProUGUI resultValue;
    [SerializeField]
    private GameObject nonCharacterReward;
    [SerializeField]
    private GameObject characterReward;
    [SerializeField]
    private TextMeshProUGUI noThanksText;
    [SerializeField]
    public bool isAdsWatched;
    [SerializeField]
    public bool isX2Watched;
    [SerializeField]
    private Button x2Rewards;
    [SerializeField]
    private Button claimCharacter;
    [SerializeField] AdManager adManager;

    [SerializeField]
    List<int> randomList = new List<int>();
    [SerializeField]
    private GameObject errorPopUpPanel;
    [SerializeField]
    private TextMeshProUGUI errorText;

    public static SpinWheel instance;

    private void Awake()
    {
        //transform.localScale = Vector3.zero;
        //for (int n = 0; n < itemList.Length; n++)
        //{
        //    if (itemName[n] != "5" && itemName[n] != "6") randomList.Add(n);
        //    else
        //    {
        //        //if (!DataManager.Instance.GetUserCharacterData(n).is_unlocked) randomList.Add(n);

        //    }
        //}
        //var index = 0;
        //adManager = GameObject.Find("AdManager").GetComponent<AdManager>();
        //foreach (Transform slot in wheel.transform)
        //{
        //    slot.GetComponentInChildren<Image>().sprite = itemList[index];
        //    slot.GetComponentInChildren<TextMeshProUGUI>().text = itemNum[index].ToString();
        //    index += 1;
        //}
        //spinButton.onClick.AddListener(() => RandomReward());

    }
    public void RandomReward()
    {
        if (!spinning /*&& GameData.PreStars >= 10*/ )
        {
            //if (GameData.PreStars >= 10)
            //{
                randomTime = Random.Range(7, 10);
                itemNumber = Random.Range(0, randomList.Count);
                float maxAngle = 360 * randomTime + (itemNumber * 45);
                //wheel.transform.DORotate(new Vector3(0, 0, 30), 0.05f).SetLoops(90, LoopType.Incremental);
                StartCoroutine(SpinTheWheel(0.5f * randomTime, maxAngle));
            //}
            //else
            //{
            //    ShowErrorPopUp("Not Enough Stars");
            //}
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
        //spinButton.interactable = false;
        float timer = 0f;
        float startAngle = wheel.transform.eulerAngles.z;
        maxAngle = maxAngle - startAngle;
        int animationCurveNumber = Random.Range(0, animationCurves.Count);
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
        //GameData.PreStars -= 10;
        //GameData.Chest_Opened_No += 1;
        //requirementText.text = GameData.PreStars.ToString() + "/" + 10;
        Debug.Log("< color = green >" + itemNumber + "Prize: " + itemList[itemNumber] + itemNum[itemNumber]);

        AddReward(itemName[itemNumber].ToString(), itemNum[itemNumber]);
        Invoke("ShowPopUp", 0.35f);
        //ShowPopUp();
    }

    void AddReward(string itemName, int value)
    {
        switch (itemName)
        {
            case "Golds":
                //GameData.Golds += value;
                break;
            case "Gems":
                //GameData.Gems += value;
                break;
            case "Shield":
                //GameData.AmountShield += value;
                break;
            case "Boost":
                //GameData.AmountBoost += value;
                break;
            case "5":
                break;
            case "6":
                break;
        }
    }
    void ShowPopUp()
    {
        popupResult.transform.DOScale(Vector3.one, 0.2f);
        resultIcon.sprite =  itemList[itemNumber];
        resultValue.text = itemNum[itemNumber].ToString();
        //if (itemName[itemNumber] != "5" && itemName[itemNumber] != "6")
        //{
            //nonCharacterReward.SetActive(true);
            x2Rewards.onClick.AddListener(() => ShowAds(x2Rewards));
        //}
        //else
        //{
        //    characterReward.SetActive(true);
        //    claimCharacter.onClick.AddListener(() => ShowAds(claimCharacter));
            noThanksText.GetComponent<RectTransform>().DOScale(Vector3.one, 0.5f);
        //}
    }

    void ShowAds(Button button)
    {
        button.interactable = false;
        if (button == x2Rewards)
        {
            if (adManager.rewardedAd.IsLoaded())
            {
                //adManager.showRewardedAd(AdManager.RewardType.DoubleWheelReward);
            }
            else
            {
                ShowErrorPopUp("No ads available at the moment!");
            }
        }
        else if (button == claimCharacter)
        {
            if (adManager.rewardedAd.IsLoaded())
            {
                //adManager.showRewardedAd(AdManager.RewardType.CharacterWheelReward);
            }
            else
            {
                ShowErrorPopUp("No ads available at the moment!");
            }
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

    public void OnAdClose()
    {
        if (isAdsWatched)
        {
            //DataManager.Instance.UnlockChar(System.Int32.Parse(itemName[itemNumber]));
            randomList.Remove(itemNumber);
            //GameData.Chest_Opened_No += 1;
            ClosePopUp();
        }
        else if (isX2Watched)
        {
            AddReward(itemName[itemNumber].ToString(), itemNum[itemNumber]);
            ClosePopUp();
        }
        else
        {
            x2Rewards.interactable = true;
            claimCharacter.interactable = true;
        }
    }

    public void ClosePopUp()
    {
        nonCharacterReward.SetActive(false);
        characterReward.SetActive(false);
        popupResult.transform.DOScale(Vector3.zero, 0.2f);
        isAdsWatched = false;
        isX2Watched = false;
        x2Rewards.interactable = true;
        claimCharacter.interactable = true;
    }
    private void Update()
    {

    }

    public void OpenUI()
    {
        transform.DOScale(Vector3.one, 0.2f);
        //requirementText.text = GameData.PreStars.ToString() + "/" + 10;
    }

    public void CloseUI()
    {
        transform.DOScale(Vector3.zero, 0.2f);
    }
}
