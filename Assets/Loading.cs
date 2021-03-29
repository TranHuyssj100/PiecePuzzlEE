using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Loading : MonoBehaviour
{
    public TextMeshProUGUI loadingTxt;
    public Image fillProgress;

    private void Start()
    {
        LoadGame();
    }

    public void LoadGame()
    {
        TweenCustom.TextAutoComplete(loadingTxt, "", "loading..", 0.5f);
        DOVirtual.Float(0, 1, 0.5f, (x) => { fillProgress.fillAmount = x; });
        if (GameData.firstTimeInGame == 1)
        {
            GameData.firstTimeInGame = 0;
            Debug.LogError(GameData.firstTimeInGame);
            GameMaster.instance.OnStartClick();
        }
        else GameMaster.instance.menu.SetActive(true);
     
    }

}
