using UnityEngine;
using DG.Tweening;


public class Piece : MonoBehaviour
{
    public int id;
    [Header("Checking Piece")]
    public bool isOnGridBoard=false;
    public bool isMouseDown = false;
    public bool isOnPreSpace = true;
    public bool canSetPosition=true;

    [Space()]
    public bool isCorrect = false;
    public float startScale = .3f; 
    public float selectedScale = 1.2f;
    public float selectedPos = 0.3f;
    public Vector2 sizeSprite;


    
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
        Debug.Log("OnMouseDown");
        if (!isCorrect)
        {
            OnPieceSelecte();
            isMouseDown = true;
        }
        else
        {
            Debug.Log(id + "<color=green> was Correctly </color>");
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
        Debug.Log("OnMouseUp");
        if (!isCorrect)
        {
            OnPieceUnselecte();
            isMouseDown = false;
            if (isOnGridBoard && canSetPosition && !isOnPreSpace)
            {
                //LevelController.indexSpawn--;
                SetPositionPiece();
            }
            else
            {
                canSetPosition = true;
                transform.DOMove(startPosition, 0.5f);
                transform.DOScale(Vector3.one * startScale, .2f);
            }
        }
    }


    void OnPieceSelecte()
    {
        Transform _child = transform.GetChild(transform.childCount - 1);
        Transform _shadown = transform.Find("Shadow");
        if (_child != null)
        {
            Sequence _seq = DOTween.Sequence();
            _child.localPosition = new Vector2(_child.localPosition.x + selectedPos, _child.localPosition.y + selectedPos);
            _seq.Append(transform.DOScale(Vector3.one, .1f));
            _child.localScale = Vector3.one * selectedScale;
            _child.GetComponent<SpriteRenderer>().sortingOrder++;
            _shadown.GetComponent<SpriteRenderer>().sortingOrder++;
        }
        oldMousePos = Input.mousePosition;
       
    }
    void OnPieceDrag()
    {
        Vector2 _directionMouse = Camera.main.ScreenToWorldPoint( Input.mousePosition) - Camera.main.ScreenToWorldPoint(oldMousePos);
        oldMousePos = Input.mousePosition;
        transform.position += (Vector3) _directionMouse;
    }
    void OnPieceUnselecte()
    {
        Transform _child = transform.GetChild(transform.childCount - 1);
        Transform _shadown = transform.Find("Shadow");
        if (_child != null)
        {
            _child.localPosition = Vector2.zero;
            Sequence _seq = DOTween.Sequence();
            _seq.Append(transform.DOScale(Vector3.one, .1f)).OnComplete(()=> {
                _child.GetComponent<SpriteRenderer>().sortingOrder--;
                _shadown.GetComponent<SpriteRenderer>().sortingOrder--;
            });
            _child.localScale = Vector3.one;
        }
    }
    void SetPositionPiece()
    {
        Vector3 _pos = new Vector3(Mathf.Clamp(Mathf.RoundToInt(transform.position.x), limitPosX.x, limitPosX.y),
                                     Mathf.Clamp(Mathf.RoundToInt(transform.position.y), limitPosY.x, limitPosY.y),
                                     transform.position.z);

        transform.DOMove(_pos, 0.2f).OnComplete(() =>
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
                                             isCorrect = new Vector3(id, transform.position.x, transform.position.y) == LevelController.instance.listAnswerForSample[id] ? true:false;
                                             if (isCorrect)
                                             { 
                                                 transform.GetChild(transform.childCount-1).localScale=Vector3.one;
                                                 transform.GetChild(transform.childCount - 1).localPosition = Vector2.zero;
                                                 LevelController.instance.NUM_PIECES_WORNG--;
                                                 LevelController.instance.SpawnRadomPieces(startPosition);
                                                 Debug.Log(id + "<color=green> is Correctly </color>," + "numMove "+ LevelController.instance.NUM_MOVE);
                                             }
                                         }

                                     }) ;
    }

    public void DestroyPiece()
    {
        Destroy(gameObject);
    }
}



