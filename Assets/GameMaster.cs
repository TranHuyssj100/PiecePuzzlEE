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
    public GameObject levelSelect;
    [Header("text")]
    public TextMeshProUGUI moveTxt;
    public TextMeshProUGUI goldTxt;


    public static GameMaster instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        menu.SetActive(true);
        AdManager.Instance.onRewardAdClosed += RewardAdClosed;
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

    public void OpenLevelSelect()
    {
        OpenPanel(levelSelect);
        
    }
    public void CloseLevelSelect()
    {
        ClosePanel(levelSelect);        
    }

    public void Replay()
    {
        //EventManager.TriggerEvent("DestroyPiece");
        StartCoroutine(LevelController.instance.InitializeGame(LevelController.level));
        CloseWinPanel();
        CloseLosePanel();
        AdManager.Instance.checkInterAdsCondition();
    }
    public void Next()
    {
        //GameData.level++;
        GameData.SetCurrentLevelByTheme(GameData.Theme, ++LevelController.level);
        //EventManager.TriggerEvent("DestroyPiece");
        StartCoroutine(LevelController.instance.InitializeGame(GameData.GetCurrentLevelByTheme(GameData.Theme)));
        CloseWinPanel();
        CloseLosePanel();
        AdManager.Instance.checkInterAdsCondition();
    }

    public void OnStartClick()
    {
        ClosePanel(menu);
        StartCoroutine(LevelController.instance.InitializeGame(GameData.GetCurrentLevelByTheme(GameData.Theme)));
        AdManager.Instance.checkInterAdsCondition();
    }
   public void OnReturnMenuClick()
    {
        //OpenPanel(menu);
        AdManager.Instance.showInterstitialAd();
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
        AdManager.Instance.showRewardedAd(AdManager.RewardType.MoreMove);
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
