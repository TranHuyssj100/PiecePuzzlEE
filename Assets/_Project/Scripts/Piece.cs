using UnityEngine;
using DG.Tweening;


public class Piece : MonoBehaviour
{

    [Header("Checking Piece")]
    public bool isOnGridBoard=false;
    public bool isMouseDown = false;
    public bool canSetPosition=true;
    [Space()]
    public bool isCorrect = false;
    public float startScale = .3f; 
    public float selectedScale = 1.2f;
    public Vector2 sizeSprite;

    [SerializeField] int index;

    
    Vector3 oldMousePos;

    Vector3 startPosition;
    Vector3 oldPostionOnGridBoard=Vector3.one*10000;
    Vector2 limitPosX= new Vector2(-3,1);
    Vector2 limitPosY= new Vector2(-2,3);


    private void OnEnable()
    {
        EventManager.StartListening("DestroyPiece", DestroyPiece);
    }
    private void OnDisable()
    {
        EventManager.StopListening("DestroyPiece", DestroyPiece);
        
    }

    private void Start()
    {
        canSetPosition = true;
        startPosition = transform.position;
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one * startScale,0.2f);
        limitPosX += new Vector2(0, -1 * (sizeSprite.x - 1));
        limitPosY -= new Vector2(0, (sizeSprite.y));
    }


    private void Update()
    {

    }

    private void OnMouseDown()
    {
        if (!isCorrect)
        {
            isMouseDown = true;
            OnPieceSelected();
        }
        else
        {
            Debug.Log(index + "<color=green> was Correctly </color>");
        }
    }

    private void OnMouseDrag()
    {
        //Debug.Log("OnMouseDrag");
        if (!isCorrect)
            OnPieceDrag();
       
    }
    private void OnMouseUp()
    {
        if (!isCorrect)
        {
            OnPieceUnselected();
            isMouseDown = false;
            if (isOnGridBoard && canSetPosition)
            {
                LevelController.indexSpawn--;
                SetPositionPiece();
            }
            else
            {
                transform.DOMove(startPosition, 0.5f);
                canSetPosition = true;
                transform.DOScale(Vector3.one * startScale, .2f);
            }
        }
    }


    void OnPieceSelected()
    {
        transform.DOScale(Vector3.one, .1f);
        if (transform.GetChild(transform.childCount-1)!=null)
        {
            transform.GetChild(transform.childCount-1).GetComponent<SpriteRenderer>().sortingOrder++;
            transform.GetChild(transform.childCount-1).DOScale(Vector3.one* selectedScale, .2f);
        }
        oldMousePos = Input.mousePosition;
       
    }
    void OnPieceDrag()
    {
        Vector2 _directionMouse = Camera.main.ScreenToWorldPoint( Input.mousePosition) - Camera.main.ScreenToWorldPoint(oldMousePos);
        oldMousePos = Input.mousePosition;
        transform.position += (Vector3) _directionMouse;
    }
    void OnPieceUnselected()
    {
        if (transform.GetChild(transform.childCount-1) != null)
        {
            transform.GetChild(transform.childCount-1).GetComponent<SpriteRenderer>().sortingOrder--;
            transform.GetChild(transform.childCount-1).DOScale(Vector3.one, .1f);
        }
    }
    void SetPositionPiece()
    {
        transform.DOMove(new Vector3(Mathf.Clamp(Mathf.RoundToInt(transform.position.x), limitPosX.x, limitPosX.y),
                                     Mathf.Clamp(Mathf.RoundToInt(transform.position.y), limitPosY.x, limitPosY.y),
                                     transform.position.z), 0.5f).OnComplete(() =>
                                     {
                                         //Debug.Log("check Complete: " +new Vector3(index, transform.position.x, transform.position.y));
                                         //Debug.Log("listIndex:  " + LevelController.instance.listAnswerForSample[index - 1]);
                                         if (transform.position != oldPostionOnGridBoard || oldPostionOnGridBoard== Vector3.one*10000)
                                         {
                                             if (LevelController.instance.NUM_MOVE >0)
                                             { 
                                                LevelController.instance.NUM_MOVE--;
                                             }
                                             else
                                             {
                                                 LevelController.instance.NUM_MOVE=0;

                                             }
                                             //LevelController.instance.NUM_MOVE--;
                                             oldPostionOnGridBoard = transform.position;
                                         }

                                         isCorrect = new Vector3(index, transform.position.x, transform.position.y) == LevelController.instance.listAnswerForSample[index - 1] ? true:false;
                                         if (isCorrect)
                                         { 
                                             LevelController.instance.NUM_PIECES_WORNG--;
                                             LevelController.instance.SpawnRadomPieces(startPosition);
                                             Debug.Log(index + "<color=green> is Correctly </color>," + "numMove "+ LevelController.instance.NUM_MOVE);

                                         }
                                     }) ;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        if (collision.tag == "Piece")
            canSetPosition = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Piece")
            canSetPosition = true;
    }

    public void DestroyPiece()
    {
        Destroy(gameObject);
    }
}



