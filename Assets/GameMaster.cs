using UnityEngine;
using DG.Tweening;
using TMPro;

public class GameMaster : MonoBehaviour
{
    public GameObject winPanel; 
    public GameObject losePanel;
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


    void WinPhase()
    {
        OpenWinPanel();

    }
    void LosePhase()
    {
        OpenLosePanel();
    }


   void OpenWinPanel()
    {
        if (!winPanel.activeSelf)
        {
            Debug.Log("<color=yellow> YOU WIN ! </color>");
            winPanel.transform.localScale = Vector3.zero;
            winPanel.SetActive(true);
            winPanel.transform.DOScale(Vector3.one, .2f);
            winPanel.GetComponent<WinPanel>().SetImageReview();
            GameData.level++;
        }
    } 

    public void CloseWinPanel()
    {
        if (winPanel.activeSelf)
        {

            winPanel.transform.DOScale(Vector3.zero, .2f).OnComplete(()=> {
                winPanel.SetActive(false);
                winPanel.transform.localScale = Vector3.one;
            });
        }
    }
    public void OpenLosePanel()
    {
        Debug.Log("<color=red> YOU LOSE ! </color>");
        losePanel.transform.localScale = Vector3.zero;
        losePanel.transform.DOScale(Vector3.one, .2f);
        losePanel.SetActive(true);
    }
    public void CloseLosePanel()
    {
        losePanel.transform.DOScale(Vector3.zero, .2f).OnComplete(()=> {
            losePanel.SetActive(false);
            losePanel.transform.localScale = Vector3.one;
        });
    }

    public void ShowNumMove()
    {
        moveTxt.text = LevelController.instance.NUM_MOVE>0 ? LevelController.instance.NUM_MOVE.ToString(): "0";
    }
    

    public void Replay()
    { 
        EventManager.TriggerEvent("DestroyPiece");
        LevelController.instance.InitializeGame();
        CloseWinPanel();
        CloseLosePanel();
    }
}
