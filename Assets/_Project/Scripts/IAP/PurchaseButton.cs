using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PurchaseButton : MonoBehaviour
{

    public enum ProductType
    {
        GOLD_ADS,
        GOLD_500,
        GOLD_1100,
        GOLD_2200,
        GOLD_3300,
        GOLD_4400,
        GOLD_5500,
        NO_ADS
    }

    public ProductType productType;
    public TMPro.TextMeshProUGUI price;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadPrice());
        if (productType == ProductType.NO_ADS)
            IAPManager.Instance.onRemoveAds += HideRemoveAdsButton;
        if (GameData.noAds == 1 && productType == ProductType.NO_ADS)
        {
            gameObject.SetActive(false);
        }
    }

    public void HideRemoveAdsButton()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ClickBuy()
    {
        switch(productType)
        {
            case ProductType.GOLD_ADS:
                IAPManager.Instance.Buy50GoldWithAds();
                break;
            case ProductType.GOLD_500:
                IAPManager.Instance.Buy500Gold();
                break;
            case ProductType.GOLD_1100:
                IAPManager.Instance.Buy1100Gold();
                break;
            case ProductType.GOLD_2200:
                IAPManager.Instance.Buy2200Gold();
                break;
            case ProductType.GOLD_3300:
                IAPManager.Instance.Buy3300Gold();
                break;
            case ProductType.GOLD_4400:
                IAPManager.Instance.Buy4400Gold();
                break;
            case ProductType.GOLD_5500:
                IAPManager.Instance.Buy5500Gold();
                break;
            case ProductType.NO_ADS:
                IAPManager.Instance.BuyNoAds();
                break;
        }    
    }
    private IEnumerator LoadPrice()
    {
        while (!IAPManager.Instance.IsInitialized())
            yield return null;
        switch (productType)
        {
            case ProductType.GOLD_500:
                price.text = IAPManager.Instance.GetProductPrice(IAPManager.Instance.GOLD_500);
                break;
            case ProductType.GOLD_1100:
                price.text = IAPManager.Instance.GetProductPrice(IAPManager.Instance.GOLD_1100);
                break;
            case ProductType.GOLD_2200:
                price.text = IAPManager.Instance.GetProductPrice(IAPManager.Instance.GOLD_2200);
                break;
            case ProductType.GOLD_3300:
                price.text = IAPManager.Instance.GetProductPrice(IAPManager.Instance.GOLD_3300);
                break;
            case ProductType.GOLD_4400:
                price.text = IAPManager.Instance.GetProductPrice(IAPManager.Instance.GOLD_4400);
                break;
            case ProductType.GOLD_5500:
                price.text = IAPManager.Instance.GetProductPrice(IAPManager.Instance.GOLD_5500);
                break;
            case ProductType.NO_ADS:
                price.text = IAPManager.Instance.GetProductPrice(IAPManager.Instance.NO_ADS);
                break;
        }  
    }    
}
