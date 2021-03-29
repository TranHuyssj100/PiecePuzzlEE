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
        LoadGame(3);
    }

    public void LoadGame(float timeWait)
    {
        TweenCustom.TextAutoComplete(loadingTxt, "loading", "loading..", timeWait/2);
        DOVirtual.Float(0, 1, timeWait, (x) => { fillProgress.fillAmount = x; }).OnComplete(()=> {
            gameObject.SetActive(false);
            if (GameData.firstTimeInGame == 1)
            {
                GameData.firstTimeInGame = 0;
                Debug.LogError(GameData.firstTimeInGame);
                GameMaster.instance.OnStartClick();
            }
            else GameMaster.instance.menu.SetActive(true);
        }); 
    }

}
