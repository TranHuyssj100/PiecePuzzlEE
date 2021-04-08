using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using System.Linq;

public class GameMaster : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject setting;
    public GameObject menu;
    public GameObject levelSelect;
    public GameObject preview;
    public GameObject shopUI;
    public GameObject themeSelect;
    public GameObject loadingPanel;
    public GameObject RatePanel;
    public GameObject collectionGrid;
    public GameObject collectionBook;

    public GameObject noAdAvailablePanel;

    [Space(10)]
    [Header("PlayUI")]
    public GameObject playHeader;
    public GameObject gridBoard;
    public GameObject preSpace;
    public GameObject playFooter;
    [Space(10)]
    [Header("text")]
    public TextMeshProUGUI moveTxt;
    public TextMeshProUGUI goldTxt;

    [Space(10)]
    [Header("Sound Buttons")]
    public Sprite[] SFX;
    public Sprite[] BGM;
    public Button Btn_SFX;
    public Button Btn_BGM;




    public static GameMaster instance;


    public event Action onPiecePlace;
    public void PiecePlaced()
    {
        if (onPiecePlace != null)
            onPiecePlace();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        onPiecePlace += OnPiecePlaced;
        loadingPanel.SetActive(true);
    }
    public void Start()
    {
        Application.targetFrameRate = 60;
        AdManager.Instance.onRewardAdClosed += RewardAdClosed;
        //onPiecePlace += OnPiecePlaced;
        GameData.onGoldValueChanged += ShowGold;
        ShowGold();
        Btn_BGM.GetComponent<UnityEngine.UI.Image>().sprite = BGM[GameData.isBGM];
        Btn_SFX.GetComponent<UnityEngine.UI.Image>().sprite = SFX[GameData.isSFX];

        winPanel.GetComponent<WinPanel>().oldGiftBoxPos = winPanel.GetComponent<WinPanel>().giftBox.localPosition;

    }

    private void OnDestroy()
    {
        if(AdManager.Instance != null)
            AdManager.Instance.onRewardAdClosed -= RewardAdClosed;
        GameData.onGoldValueChanged -= ShowGold;
        onPiecePlace -= OnPiecePlaced;
    }

    void FixedUpdate()
    {
        //ShowNumMove();
        //ShowGold();
    }

    private void checkEndGame()
    {
        //Debug.LogError("NUM_PIECES_WORNG: " + TestLevelCtr.instance.NUM_PIECES_WRONG);
        if (TestLevelCtr.instance.NUM_MOVE > 0)
        {
            if (TestLevelCtr.instance.NUM_PIECES_WRONG <= 0)
            {
                WinPhase();
            }
        }
        else
        {

            if (TestLevelCtr.instance.NUM_MOVE == 0 && TestLevelCtr.instance.NUM_PIECES_WRONG == 0)
            {   
                WinPhase();
            }
            else
            {
                LosePhase();
            }
        }
    }
    private void OnPiecePlaced()
    {
        ShowNumMove();
        Invoke("checkEndGame", .5f);
    }
    #region PHASE
    private bool isWin;
    void WinPhase()
    {
        if (!winPanel.activeSelf)
        {
            GameData.gold += Config.GOLD_WIN;
            SoundManager.instance.PlayRandom(TypeSFX.Win);
            SoundManager.instance.ClearIndexSquential(TypeSFX.True);
            //Debug.Log("<color=yellow> YOU WIN ! </color>");
            GameData.levelReward++;
            winPanel.GetComponent<WinPanel>().SetImageReview();
            OpenPanel(winPanel);
            if (TestLevelCtr.instance.idLevel >= GameData.GetCurrentLevelByTheme(GameData.Theme) && TestLevelCtr.instance.idLevel < DataController.themeData[GameData.Theme].levelCount - 1)
            {
                Debug.Log(GameData.GetCurrentLevelByTheme(GameData.Theme));
                GameData.level++;
                FirebaseManager.instance.LogUnlockLevel(GameData.level,DataController.themeData[GameData.Theme].name);
                GameData.SetCurrentLevelByTheme(GameData.Theme, (TestLevelCtr.instance.idLevel) < (DataController.themeData[GameData.Theme].levelCount - 1) ? TestLevelCtr.instance.idLevel + 1 : TestLevelCtr.instance.idLevel);
            }
            WinPanel.instance.ShowProgress(5);
        }
    }
    void LosePhase()
    {
        if (!losePanel.activeSelf && !isWin)
        {
            Debug.Log("<color=red> YOU LOSE ! </color>");
            SoundManager.instance.PlayRandom(TypeSFX.Lose);
            OpenPanel(losePanel);
            losePanel.SetActive(true);
            FirebaseManager.instance.LogLoseLevel(GameData.GetCurrentLevelByTheme(GameData.Theme), DataController.themeData[GameData.Theme].name);
        }
    }

    #endregion

    #region CLOSE-OPEN PANEL
    public void OpenPanel(GameObject panel)
    {
        if (!panel.activeSelf)
        {
            panel.transform.localScale = Vector3.zero;
            panel.SetActive(true);
            panel.transform.DOScale(Vector3.one, .35f);
        }
    }  
   public void ClosePanel(GameObject panel)
    {
        if (panel.activeSelf)
        {
            panel.transform.DOScale(Vector3.zero, .35f).OnComplete(() => {
                panel.SetActive(false);
                panel.transform.localScale = Vector3.one;
            });
        }
    }
    
    #endregion

    #region UPDATE TEXT
    public void ShowNumMove()
    {
        moveTxt.text = TestLevelCtr.instance.NUM_MOVE >= 0 ? TestLevelCtr.instance.NUM_MOVE.ToString() : "0";
        TweenCustom.ZoomOutandIn(moveTxt.transform, 0.2f ,  0.3f);
    }

    public void ShowGold()
    {
       goldTxt.text = GameData.gold.ToString();
    }

    #endregion

    #region Animation
    public void AnimatePlayUI()
    {
        DOTween.CompleteAll();
        TweenCustom.ToBottom(playHeader.transform, 2f, 0.6f);
        TweenCustom.ToUpper(playFooter.transform, 2f, 0.6f);
        TweenCustom.ZoomOutandIn(gridBoard.transform, 0.1f, 0.3f);
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
            SoundManager.instance.PlayRandom(TypeSFX.Lose);
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
        //levelSelect.transform.Find("GridLevel").GetComponent<GridLevel>().
        OpenPanel(levelSelect);
        
    }
    public void CloseLevelSelect()
    {
        ClosePanel(levelSelect);        
    }
    
    public void ClosePreview()
    {
        ClosePanel(preview);
    }
    public void OpenShopUI()
    {
        OpenPanel(shopUI);

    }
    public void CloseShopUI()
    {
        ClosePanel(shopUI);
    }

    public void OpenThemeSelect()
    {
        OpenPanel(themeSelect);
        TestLevelCtr.instance.DestroyTutorialObj();
    }
    public void CloseThemeSelect()
    {
        ClosePanel(themeSelect);
    }

    public void CloseMenu()
    {
        ClosePanel(menu);
    }

    public void OpenCollectionGrid()
    {
        OpenPanel(collectionGrid);
    }
    public void CloseCollectionGrid()
    {
        ClosePanel(collectionGrid);
    }
    public void OpenCollectionBook()
    {
        OpenPanel(collectionBook);
    }
    public void CloseCollectionBook()
    {
        ClosePanel(collectionBook);
    }

    public void OpenRatePanelByLevel(int [] levelShowRate)
    {
        //Debug.LogError(GameData.level +" :"+ levelShowRate.Contains(GameData.level));;
        if (levelShowRate.Contains(GameData.level))
        {
            OpenPanel(RatePanel);
        }
    }

    public void Replay()
    {
        EventManager.TriggerEvent("DestroyPiece");
        //FirebaseManager.instance.LogResetLevel(TestLevelCtr.instance.idLevel, DataController.themeData[GameData.Theme].name);
        //StartCoroutine(LevelController.instance.InitializeGame(LevelController.idLevel, GameData.Theme));
        StartCoroutine(TestLevelCtr.instance.InitalizeGame(TestLevelCtr.instance.idTheme, TestLevelCtr.instance.idLevel));
        CloseWinPanel();
        CloseLosePanel();
        AdManager.Instance.checkInterAdsCondition();
    }
    public void Next()
    {
        //GameData.level++;
   
        EventManager.TriggerEvent("DestroyPiece");
        if (TestLevelCtr.instance.idLevel >= DataController.themeData[GameData.Theme].levelCount-1)
        {
            OpenThemeSelect();
        }
        else
        {
            //StartCoroutine(TestLevelCtr.instance.InitalizeGame(GameData.Theme, GameData.GetCurrentLevelByTheme(GameData.Theme)));
            StartCoroutine(TestLevelCtr.instance.InitalizeGame(GameData.Theme, ++TestLevelCtr.instance.idLevel));
        }
        CloseWinPanel();
        CloseLosePanel();
        AdManager.Instance.checkInterAdsCondition();
    }

    public void OnStartClick()
    {
        ClosePanel(menu);
        StartCoroutine(TestLevelCtr.instance.InitalizeGame(GameData.Theme, GameData.GetCurrentLevelByTheme(GameData.Theme)));
        //StartCoroutine(LevelController.instance.InitializeGame(GameData.GetCurrentLevelByTheme(GameData.Theme), GameData.Theme));
        //AdManager.Instance.checkInterAdsCondition();
    }
   public void OnReturnMenuClick()
    {
        OpenPanel(menu);
        TestLevelCtr.instance.DestroyTutorialObj();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnHintClick()
    {
        if (GameData.gold >= Config.COST_HINT)
        {
            GameData.gold -= Config.COST_HINT;
            Piece _piece = TestLevelCtr.instance.FindIncorrectPiece();
            if (_piece!=null && !_piece.isCorrect)
            {
                FirebaseManager.instance.LogAutoCorrectHint();
                TestLevelCtr.instance.SetCorrectPiecePos(_piece.gameObject, 0.3f);
                //LevelController.instance.SetCorrectPiecePos(_piece.gameObject, _piece.startPosition, 0.5f);
                //if (_piece.isPieceTutorial) _piece.TutorialPieceOnMouseDown();

            }
        }
        else
        {
            OpenShopUI();
        }
        //StartCoroutine(CorountineCheckPiece());
    }   

    //IEnumerator CorountineCheckPiece()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    EventManager.TriggerEvent("CheckTriggerPiece");
    //}

    public void OnPreviewClick()
    {
        if(GameData.gold>= Config.COST_PREVIEW)
        {
            GameData.gold -= Config.COST_PREVIEW;
            OpenPanel(preview);
            FirebaseManager.instance.LogPreviewHint();
            preview.transform.Find("Bg").Find("Image").GetComponent<Image>().sprite =
                DataController.LoadSpritePreview(TestLevelCtr.instance.idTheme, TestLevelCtr.instance.idLevel, TestLevelCtr.instance.sizeLevel);
                //LevelController.LoadSpritePreview(LevelController.idLevel,DataController.themeData[GameData.Theme].name, LevelController.instance.sizeLevel);
        }
        else
        {
            OpenShopUI();
        }
    }

    public void Rate()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=com.sun.PieceMaster");
#elif UNITY_IPHONE

#endif
    }
    public void MoreGame()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=SUNMOB");
    }

    #endregion

    #region Reward
    public void ShowMoreMoveAd()
    {
        AdManager.Instance.showRewardedAd(AdManager.RewardType.MoreMove);
    }
    public void GrantMoreMove()
    {
        TestLevelCtr.instance.NUM_MOVE += 5;
        ShowNumMove();
        CloseLosePanel();
    }    
    private void RewardAdClosed()
    {
        if (AdManager.rewardType == AdManager.RewardType.MoreMove)
            GrantMoreMove();
    }
    #endregion


    #region Sound
    public void toggleSFX()
    {
        GameData.isSFX = GameData.isSFX == 0 ? 1 : 0;
        Btn_SFX.GetComponent<Image>().sprite = SFX[GameData.isSFX];
    }
    public void toggleBGM()
    {
        GameData.isBGM = GameData.isBGM == 0 ? 1 : 0;
        Btn_BGM.GetComponent<Image>().sprite = BGM[GameData.isBGM];
        if (GameData.isBGM == 0)
            SoundManager.instance.Stop(TypeSFX.BGM, "BGM");
        else
            SoundManager.instance.PlayBGM(TypeSFX.BGM, "BGM");

    }

    #endregion


    public void PlayNoAdAvailableAnim()
    {
        Sequence seq = DOTween.Sequence();
        seq.Pause();
        seq.Append(noAdAvailablePanel.transform.DOScale(Vector3.one, .2f))
                 .Append(noAdAvailablePanel.transform.DOScale(Vector3.one, 1f))
                 .Append(noAdAvailablePanel.transform.DOScale(Vector3.zero, .2f));
        if (noAdAvailablePanel.transform.localScale == Vector3.zero)
            seq.Play();
        else
        {
            noAdAvailablePanel.transform.localScale = Vector3.zero;
            seq.Restart();
        }

    }
    #region Noti
    void ChecKSpinDailyNoti()
    {
        Transform spinWheel = menu.transform.Find("SpinWheel");
        if (GameData.dailySpinAmount > 0)
        {
            spinWheel.Find("Noti").gameObject.SetActive(true);
        }
        else
        {
            spinWheel.Find("Noti").gameObject.SetActive(false);
        }
    }
    #endregion
}
