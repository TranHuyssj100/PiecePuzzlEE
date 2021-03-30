using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThemeChild : MonoBehaviour
{
    public int index;
    public bool isUnlock;
    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI priceTxt;
    public TextMeshProUGUI progressTxt;
    public Transform rankObj;
    public UnityEngine.UI.Image image;
    public UnityEngine.UI.Button buyBtn, progressBtn;

    public void UnlockTheme()
    {
        GameData.UnlockTheme(index);
        buyBtn.gameObject.SetActive(false);
        progressTxt.text = (GameData.GetCurrentLevelByTheme(index) + 1).ToString() + "/" + DataController.themeData[index].levelCount.ToString();
    }

    public void SetRankTheme( int sizeLevel)
    {
        for (int i = 5; i < rankObj.childCount + 5; i++)
        {
            if (i == sizeLevel)
            {
                rankObj.GetChild(i - 5).gameObject.SetActive(true);
            }
            else
            {
                rankObj.GetChild(i - 5).gameObject.SetActive(false);

            }
        }
    }
}
