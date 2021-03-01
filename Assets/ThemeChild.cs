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
    public UnityEngine.UI.Image image;
    public UnityEngine.UI.Button buyBtn, progressBtn;

    public void UnlockTheme()
    {
        GameData.UnlockTheme(index);
        buyBtn.gameObject.SetActive(false);
        Debug.Log(DataController.themeData[index].levelCount);
        progressTxt.text = (GameData.GetCurrentLevelByTheme(index) + 1).ToString() + "/" + DataController.themeData[index].levelCount.ToString();
    }
}
