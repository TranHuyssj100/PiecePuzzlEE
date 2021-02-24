using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PurchaseButton : MonoBehaviour
{

    public enum ProductType
    {
        GOLD_50,
        GOLD_110,
        GOLD_220,
        GOLD_330,
        GOLD_440,
        GOLD_550,
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
    private void OnDestroy()
    {
        if(productType == ProductType.NO_ADS)
            IAPManager.Instance.onRemoveAds -= HideRemoveAdsButton;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ClickBuy()
    {
        switch(productType)
        {
            case ProductType.GOLD_50:
                IAPManager.Instance.Buy50Gold();
                break;
            case ProductType.GOLD_110:
                IAPManager.Instance.Buy110Gold();
                break;
            case ProductType.GOLD_220:
                IAPManager.Instance.Buy220Gold();
                break;
            case ProductType.GOLD_330:
                IAPManager.Instance.Buy330Gold();
                break;
            case ProductType.GOLD_440:
                IAPManager.Instance.Buy440Gold();
                break;
            case ProductType.GOLD_550:
                IAPManager.Instance.Buy550Gold();
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
            case ProductType.GOLD_50:
                price.text = IAPManager.Instance.GetProductPrice(IAPManager.Instance.GOLD_50);
                break;
            case ProductType.GOLD_110:
                price.text = IAPManager.Instance.GetProductPrice(IAPManager.Instance.GOLD_110);
                break;
            case ProductType.GOLD_220:
                price.text = IAPManager.Instance.GetProductPrice(IAPManager.Instance.GOLD_220);
                break;
            case ProductType.GOLD_330:
                price.text = IAPManager.Instance.GetProductPrice(IAPManager.Instance.GOLD_330);
                break;
            case ProductType.GOLD_440:
                price.text = IAPManager.Instance.GetProductPrice(IAPManager.Instance.GOLD_440);
                break;
            case ProductType.GOLD_550:
                price.text = IAPManager.Instance.GetProductPrice(IAPManager.Instance.GOLD_550);
                break;
            case ProductType.NO_ADS:
                price.text = IAPManager.Instance.GetProductPrice(IAPManager.Instance.NO_ADS);
                break;
        }  
    }    
}
