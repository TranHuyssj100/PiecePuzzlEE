﻿using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using System.Collections;

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
    [Header("text")]
    public TextMeshProUGUI moveTxt;
    public TextMeshProUGUI goldTxt;





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

    public void Start()
    {
        menu.SetActive(true);
        AdManager.Instance.onRewardAdClosed += RewardAdClosed;
        onPiecePlace += OnPiecePlaced;
    }

    void FixedUpdate()
    {
        ShowNumMove();
        ShowGold();
        //if (LevelController.isInitializeComplete)
        //{
            
        //}
    }

    private void checkEndGame()
    {
        Debug.LogError("NUM_PIECES_WORNG: " + TestLevelCtr.instance.NUM_PIECES_WRONG);
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
            Debug.Log("<color=yellow> YOU WIN ! </color>");
            OpenPanel(winPanel);
            winPanel.GetComponent<WinPanel>().SetImageReview();
            GameData.SetCurrentLevelByTheme(GameData.Theme, (TestLevelCtr.instance.idLevel) < (DataController.themeData[GameData.Theme].levelCount - 1) ? TestLevelCtr.instance.idLevel + 1 : TestLevelCtr.instance.idLevel);
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
        moveTxt.text = TestLevelCtr.instance.NUM_MOVE >= 0 ? TestLevelCtr.instance.NUM_MOVE.ToString() : "0";
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
    }
    public void CloseThemeSelect()
    {
        ClosePanel(themeSelect);
    }

    public void CloseMenu()
    {
        ClosePanel(menu);
    }

    public void Replay()
    {
        EventManager.TriggerEvent("DestroyPiece");
        FirebaseManager.instance.LogResetLevel(LevelController.idLevel, DataController.themeData[GameData.Theme].name);
        //StartCoroutine(LevelController.instance.InitializeGame(LevelController.idLevel, GameData.Theme));
        TestLevelCtr.instance.InitalizeGame(TestLevelCtr.instance.idTheme, TestLevelCtr.instance.idLevel);
        CloseWinPanel();
        CloseLosePanel();
        AdManager.Instance.checkInterAdsCondition();
    }
    public void Next()
    {
        //GameData.level++;
   
        EventManager.TriggerEvent("DestroyPiece");
        if (TestLevelCtr.instance.idLevel >= DataController.themeData[GameData.Theme].levelCount)
        {
            OpenThemeSelect();
        }
        else
            //StartCoroutine(LevelController.instance.InitializeGame(GameData.GetCurrentLevelByTheme(GameData.Theme), GameData.Theme));
            TestLevelCtr.instance.InitalizeGame(GameData.Theme, GameData.GetCurrentLevelByTheme(GameData.Theme));
        CloseWinPanel();
        CloseLosePanel();
        AdManager.Instance.checkInterAdsCondition();
    }

    public void OnStartClick()
    {
        ClosePanel(menu);
        TestLevelCtr.instance.InitalizeGame(GameData.Theme, GameData.GetCurrentLevelByTheme(GameData.Theme));
        //StartCoroutine(LevelController.instance.InitializeGame(GameData.GetCurrentLevelByTheme(GameData.Theme), GameData.Theme));
        AdManager.Instance.checkInterAdsCondition();
    }
   public void OnReturnMenuClick()
    {
        OpenPanel(menu);
        AdManager.Instance.showInterstitialAd();
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
                TestLevelCtr.instance.SetCorrectPiecePos(_piece.gameObject, 0.3f);
                //LevelController.instance.SetCorrectPiecePos(_piece.gameObject, _piece.startPosition, 0.5f);
                //if (_piece.isPieceTutorial) _piece.TutorialPieceOnMouseDown();
            }
        }
        //StartCoroutine(CorountineCheckPiece());
    }   

    IEnumerator CorountineCheckPiece()
    {
        yield return new WaitForSeconds(0.2f);
        EventManager.TriggerEvent("CheckTriggerPiece");

    }

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


    #region Sound
    public void toggleSFX()
    {
        GameData.isSFX = GameData.isSFX == 0 ? 1 : 0;
        SoundManager.instance.Btn_SFX.GetComponent<Image>().sprite = SoundManager.instance.SFX[GameData.isSFX];
    }
    public void toggleBGM()
    {
        GameData.isBGM = GameData.isBGM == 0 ? 1 : 0;
        SoundManager.instance.Btn_BGM.GetComponent<Image>().sprite = SoundManager.instance.BGM[GameData.isBGM];

        if (GameData.isBGM == 0)
            SoundManager.instance.Stop(TypeSFX.BGM, "BGM");
        else
            SoundManager.instance.PlayBGM(TypeSFX.BGM, "BGM");

    }

    #endregion

}
