using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading;

public class GameMaster : MonoBehaviour
{
    public GameObject winPanel; 
    public GameObject losePanel;
    public GameObject setting;
    public GameObject menu;
    [Header("text")]
    public TextMeshProUGUI moveTxt;
    public TextMeshProUGUI goldTxt;
        

    public 
    void Start()
    {
        AdManager.instance.onRewardAdClosed += RewardAdClosed;
    }

    void Update()
    {

        ShowNumMove();
        ShowGold();
        if (LevelController.isInitializeComplete)
        {
            if (LevelController.instance.NUM_MOVE > 0)
            {
                if (LevelController.instance.NUM_PIECES_WORNG <= 0)
                {
                    WinPhase();
                }
            }
            else
            {
                if (LevelController.instance.NUM_MOVE == 0 && LevelController.instance.NUM_PIECES_WORNG == 0)
                {
                    WinPhase();
                }
                else
                {
                    LosePhase();
                }
            }
        }
    }

    #region PHASE
    void WinPhase()
    {
        if (!winPanel.activeSelf)
        {
            GameData.gold += Config.GOLD_WIN;
            SoundManager.instance.PlayRandom(TypeSFX.Win);
            SoundManager.instance.ClearIndexSquential(TypeSFX.True);
            Debug.Log("<color=yellow> YOU WIN ! </color>");
            OpenPanel(winPanel);
            winPanel.GetComponent<WinPanel>().SetImageReview();
        }
    }
    void LosePhase()
    {
        if (!losePanel.activeSelf)
        {
            Debug.Log("<color=red> YOU LOSE ! </color>");
            OpenPanel(losePanel);
            losePanel.SetActive(true);
        }
    }

    #endregion

    #region CLOSE-OPEN PANEL
    void OpenPanel(GameObject panel)
    {
        if (!panel.activeSelf)
        {

            panel.transform.localScale = Vector3.zero;
            panel.SetActive(true);
            panel.transform.DOScale(Vector3.one, .2f);
        }
    }  
    void ClosePanel(GameObject panel)
    {
        if (panel.activeSelf)
        {
            panel.transform.DOScale(Vector3.zero, .2f).OnComplete(() => {
                panel.SetActive(false);
                panel.transform.localScale = Vector3.one;
            });
        }
    }
    #endregion

    #region UPDATE TEXT
    public void ShowNumMove()
    {
        moveTxt.text = LevelController.instance.NUM_MOVE >= 0 ? LevelController.instance.NUM_MOVE.ToString() : "0";
    }

    public void ShowGold()
    {
        goldTxt.text = GameData.gold.ToString();
    }

    #endregion


    #region OnClick
    void OpenWinPanel()
    {
  
    } 

    public void OpenLosePanel()
    {
        if (!losePanel.activeSelf)
        {
            Debug.Log("<color=red> YOU LOSE ! </color>");
            OpenPanel(losePanel);
            losePanel.SetActive(true);
        }
    }  
    public void OpenSetting()
    {
        OpenPanel(setting);
    }

    public void CloseWinPanel()
    {
        if (winPanel.activeSelf)
        { 
            ClosePanel(winPanel);
        }
    }
    public void CloseLosePanel()
    {
        ClosePanel(losePanel);
    }   
    public void CloseSetting()
    {
        ClosePanel(setting);
    }

    public void Replay()
    {
        EventManager.TriggerEvent("DestroyPiece");
        StartCoroutine(LevelController.instance.InitializeGame());
        CloseWinPanel();
        CloseLosePanel();
        AdManager.instance.checkInterAdsCondition();
    }
    public void Next()
    { 
        GameData.level++;
        EventManager.TriggerEvent("DestroyPiece");
        StartCoroutine(LevelController.instance.InitializeGame());
        CloseWinPanel();
        CloseLosePanel();
        AdManager.instance.checkInterAdsCondition();
    }

    public void OnStartClick()
    {
        ClosePanel(menu);
        AdManager.instance.checkInterAdsCondition();
    }
   public void OnReturnClick()
    {
        //OpenPanel(menu);
        AdManager.instance.showInterstitialAd();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

   public void OnHintClick()
    {
        if (GameData.gold >= Config.COST_HINT)
        {
            GameData.gold -= Config.COST_HINT;
            Piece _piece = LevelController.instance.FindIncorrectPiece();
            if (_piece!=null && !_piece.isCorrect)
            {
                LevelController.instance.SetCorrectPiecePos(_piece.gameObject, _piece.startPosition, 0.5f);
            }
        }
        
    }
    #endregion

    #region Reward
    public void ShowMoreMoveAd()
    {
        AdManager.instance.showRewardedAd(AdManager.RewardType.MoreMove);
    }
    public void GrantMoreMove()
    {
        LevelController.instance.NUM_MOVE += 5;
        CloseLosePanel();
    }    
    private void RewardAdClosed()
    {
        if (AdManager.rewardType == AdManager.RewardType.MoreMove)
            GrantMoreMove();
    }
    #endregion

}
