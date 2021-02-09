using UnityEngine;
using DG.Tweening;
using TMPro;

public class GameMaster : MonoBehaviour
{
    public GameObject winPanel; 
    public GameObject losePanel;
    public GameObject setting;
    public GameObject menu;
    public TextMeshProUGUI moveTxt;
        

    public 
    void Start()
    {
        
    }

    void Update()
    {
        ShowNumMove();
        if (LevelController.instance.NUM_MOVE >= 0)
        {
            if (LevelController.instance.NUM_PIECES_WORNG <= 0)
            {
                WinPhase();
            }
        }
        else
        {
            LosePhase();
        }
    }

    #region Phase
    void WinPhase()
    {
        OpenWinPanel();
    }
    void LosePhase()
    {
        OpenLosePanel() ;
    }

    #endregion

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
    public void ShowNumMove()
    {
        moveTxt.text = LevelController.instance.NUM_MOVE > 0 ? LevelController.instance.NUM_MOVE.ToString() : "0";
    }

    #region OnClick
    void OpenWinPanel()
    {
        if (!winPanel.activeSelf)
        {
            Debug.Log("<color=yellow> YOU WIN ! </color>");
            OpenPanel(winPanel);
            winPanel.GetComponent<WinPanel>().SetImageReview();
        }
    } 

    public void OpenLosePanel()
    {
        Debug.Log("<color=red> YOU LOSE ! </color>");
        OpenPanel(losePanel);
        losePanel.SetActive(true);
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
        LevelController.instance.InitializeGame();
        CloseWinPanel();
        CloseLosePanel();
    }  
    public void Next()
    { 
        GameData.level++;
        EventManager.TriggerEvent("DestroyPiece");
        LevelController.instance.InitializeGame();
        CloseWinPanel();
        CloseLosePanel();
    }

   public void OnStartClick()
    {
        ClosePanel(menu);
    }
   public void OnReturnClick()
    {
        OpenPanel(menu);
    }

   public void OnHintClick()
    {        
        Piece _piece = FindObjectOfType<Piece>();
        if (!_piece.isCorrect)
        {
            LevelController.instance.SetCorrectPiecePos(_piece.gameObject, _piece.startPosition, 0.5f);
        }
        
    }
    #endregion

    

}
