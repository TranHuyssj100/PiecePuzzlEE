using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Piece : MonoBehaviour
{
    public int id;
    [Header("Checking Piece")]
    public bool isOnGridBoard=false;
    public bool isMouseDown = false;
    public bool isOnPreSpace = true;
    public bool canSetPosition=true;
    public bool isPieceTutorial=false;
    public bool isTriggerOtherPiece = false;

    [Space()]
    public bool isCorrect = false;
    public float startScale;
    public float selectedScale;
    public float selectedPos;
    public Vector2 sizeSprite;
    public Vector3 startPosition;
    public int startPointIndex;
    public Vector3 oldPostionOnGridBoard=Vector3.one*10000;


    
    Vector3 oldMousePos;
    Vector2 limitPosX= new Vector2(-3,1);
    Vector2 limitPosY= new Vector2(-2,3);

    int _indexTrueSound = 0;




    private void OnEnable()
    {
        EventManager.StartListening("DestroyPiece", DestroyPiece);
        EventManager.StartListening("CheckTriggerPiece", CheckTriggerPiece);
    }
    private void OnDisable()
    {
        EventManager.StopListening("DestroyPiece", DestroyPiece);
        EventManager.StopListening("CheckTriggerPiece", CheckTriggerPiece);
        
    }

    private void Start()
    {
        startScale = .55f;
        selectedScale = 1f;
        selectedPos = 0.1f;

        canSetPosition = true;

        //Vector3 offset = Vector3.zero;
        //foreach (Transform grid in transform)
        //{
        //    offset += grid.position;
        //}
        //offset /=transform.childCount;
        //offset = (transform.position - offset)/* * pieceClone.transform.localScale.x*/;
        //transform.position += offset;

        //transform.localScale = Vector3.zero;
        //transform.DOScale(Vector3.one * .5f, 0.2f);
        startPosition = transform.position;

    }


    private void Update()
    {

    }

    private void OnMouseDown()
    {
        isMouseDown = true;
        OnPieceSelect();
         
    }

    private void OnMouseDrag()
    {
        //Debug.Log("OnMouseDrag");
        if (!isCorrect)
            OnPieceDrag();
       
    }
    private void OnMouseUp()
    {
        isMouseDown = false;
        if (!isCorrect)
        {
            SetPositionPiece(false);
        }
    }

    public void SetLimitPos(int _sizeLevel)
    {
        switch (_sizeLevel)
        {
            case 5:
                limitPosX = Config.LIMIT_POS_X_5X5;
                limitPosY = Config.LIMIT_POS_Y_5X5;
                break;
            case 6:
                limitPosX = Config.LIMIT_POS_X_6X6;
                limitPosY = Config.LIMIT_POS_Y_6X6;
                break;
        } 
    }
    //void SetScalePieceOnPreSpace()
    //{
    //    Transform _sprite = transform.GetChild(transform.childCount - 1);
    //    Transform _shadow = transform.Find("Shadow");
       
    //    if (isOnPreSpace)
    //    {
    //        transform.localScale = Vector3.zero;
    //        transform.localScale = Vector3.zero;
    //        transform.DOScale(Vector3.one * startScale, 0.2f);
    //        _sprite.localScale = Vector2.one* 0.8f;
    //        _sprite.localPosition = Vector2.zero;
    //        _shadow.localScale = Vector2.one* 0.8f;
    //        _shadow.localPosition = Vector2.zero;
    //    }
    //}

    public void OnPieceSelect()
    {

        transform.DOScale(selectedScale, 0f);
        if (transform.localScale != Vector3.one)
        {
            Vector3 offset = Vector3.zero;
            foreach (Transform grid in transform)
            {
                offset += grid.position;
            }
            offset /= transform.childCount;
            offset = (transform.position - offset)/* * pieceClone.transform.localScale.x*/;
            transform.position += new Vector3(offset.x, offset.y * 2f, 0);
        }
        transform.localScale = selectedScale * Vector3.one;

        foreach (Transform grid in transform)
        {
            for (int i = 0; i < TestLevelCtr.instance.availableSpace.Length; i++)
            {
                if (TestLevelCtr.instance.availableSpace[i].position == (Vector2)TestLevelCtr.instance.allPieces.transform.InverseTransformPoint((Vector2)grid.position))
                    TestLevelCtr.instance.availableSpace[i].available = true;
            }

        }
        oldMousePos = Input.mousePosition;
       
    }
    void OnPieceDrag()
    {
        Vector2 _directionMouse = Camera.main.ScreenToWorldPoint( Input.mousePosition) - Camera.main.ScreenToWorldPoint(oldMousePos);
        oldMousePos = Input.mousePosition;
        transform.position += (Vector3) _directionMouse;

    }
    public void OnPieceUnselect()
    {
        transform.DOScale(startScale, 0.3f);
        if (transform.localScale != Vector3.one)
        {
            Vector3 offset = Vector3.zero;
            foreach (Transform grid in transform)
            {
                offset += grid.position;
            }
            offset /= transform.childCount;
            offset = (transform.position - offset)/* * pieceClone.transform.localScale.x*/;
            transform.position += new Vector3(offset.x, offset.y*2f, 0);
        }
        transform.DOMove(startPosition, 0.3f).OnComplete(() => { 
        
        });
        

    }
    bool CheckAvailableSpace(Vector2 space)
    {
        if (space.x < 0 || space.x > (Mathf.Sqrt(TestLevelCtr.instance.availableSpace.Length) - 1) || space.y > 0 || space.y < -(Mathf.Sqrt(TestLevelCtr.instance.availableSpace.Length) - 1))
        {
            return false;
        }
        for (int i = 0; i < TestLevelCtr.instance.availableSpace.Length; i++)
        {
            if (TestLevelCtr.instance.availableSpace[i].position == space && !TestLevelCtr.instance.availableSpace[i].available)
            {
                return false;
            }
        }
        return true;
    }
    void SetPositionPiece(bool _autoCorrect)
    {
        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x),
                                         Mathf.Round(transform.localPosition.y));
        //transform.position += new Vector3(-.5f, .5f);

        GameMaster.instance.PiecePlaced();
       
        foreach (Transform grid in transform)
        {
            if (!CheckAvailableSpace(TestLevelCtr.instance.allPieces.transform.InverseTransformPoint(grid.position)))
            {
                OnPieceUnselect();
                return;
            }
        }
        if (transform.localPosition != oldPostionOnGridBoard && !_autoCorrect)
        {
            TestLevelCtr.instance.NUM_MOVE--;
            oldPostionOnGridBoard = transform.localPosition;
        }
        foreach (Transform grid in transform)
        {
            for (int i = 0; i < TestLevelCtr.instance.availableSpace.Length; i++)
            {
                if (Vector2.Distance(TestLevelCtr.instance.availableSpace[i].position,(Vector2)TestLevelCtr.instance.allPieces.transform.InverseTransformPoint((Vector2)grid.position)) < .1f)
                {
                    TestLevelCtr.instance.availableSpace[i].available = false;
                }
            }

        }
        if((Vector2)transform.localPosition == Vector2.zero)
        {
                isCorrect = true;
                TestLevelCtr.instance.NUM_PIECES_WRONG--;
                TestLevelCtr.instance.SpawnPiece(startPointIndex,false);

                Collider2D[] colliders = GetComponents<Collider2D>();
                foreach (Collider2D collider in colliders)
                    Destroy(collider);
        }
        //TestLevelCtr.instance.availableSpaceList.Remove(TestLevelCtr.instance.allPiece.transform.InverseTransformPoint(grid.position));

        //Vector3 _pos = new Vector3(Mathf.Clamp((Mathf.RoundToInt(transform.position.x)), limitPosX.x, limitPosX.y),
        //                             Mathf.Clamp(Mathf.RoundToInt(transform.position.y), limitPosY.x, limitPosY.y),
        //                             transform.position.z);

        ////Debug.Log(_pos);
        //if (_pos != oldPostionOnGridBoard || oldPostionOnGridBoard == Vector3.one * 10000)
        //{
        //    oldPostionOnGridBoard =_pos;
        //    //LevelController.instance.NUM_MOVE--;
        //}
        //transform.DOMove(_pos, 0.2f).OnComplete(() =>
        //                             {
        //                                 isCorrect = new Vector3(id, transform.position.x, transform.position.y) == LevelController.instance.listAnswerForSample[id] ? true:false;
        //                                if (isCorrect)
        //                                { 
        //                                    transform.GetChild(transform.childCount-1).localScale=Vector3.one;
        //                                    transform.GetChild(transform.childCount - 1).localPosition = Vector2.zero;
        //                                    LevelController.instance.SpawnRadomPieces(startPosition);
        //                                    SoundManager.instance.playSequential(TypeSFX.True);
        //                                    LevelController.instance.NUM_PIECES_WORNG--;
        //                                     //Debug.Log(id + "<color=green> is Correctly </color>," + "numMove "+ LevelController.instance.NUM_MOVE);
        //                                 }
        //                                else
        //                                {
        //                                    SoundManager.instance.ClearIndexSquential(TypeSFX.True);
        //                                    SoundManager.instance.PlayRandom(TypeSFX.Wrong);
        //                                    EventManager.TriggerEvent("CheckTriggerPiece");
        //                                 }
        //                                 //Debug.LogError(LevelController.instance.NUM_PIECES_WORNG);

        //                             });
    }

    //public void AutoCorrectPiece(Vector2 _correctPos,Vector2 _startPos , float _duration)
    //{
    //    FirebaseManager.instance.LogAutoCorrectHint();
    //    startPosition = _startPos;
    //    Transform _sprite = transform.GetChild(transform.childCount - 1);
    //    Transform _shadow = transform.Find("Shadow");
    //    LevelController.instance.NUM_PIECES_WORNG--;

    //    isCorrect = true;
    //    isOnPreSpace = false;
    //    transform.localScale = Vector2.one;
    //    _sprite.localScale  = Vector2.one;
    //    _sprite.localPosition = Vector2.zero;    
    //    _shadow.localScale  = Vector2.one;
    //    _shadow.localPosition = Vector2.zero;

    //    transform.DOMove(_correctPos, _duration);
    //    LevelController.instance.SpawnRadomPieces(startPosition);   
    //} 
    public void AutoCorrectPiece(int _startPos, float _duration)
    {
        OnPieceSelect();
        FirebaseManager.instance.LogAutoCorrectHint();
        isCorrect = true;
        startPointIndex =  _startPos;
        TestLevelCtr.instance.NUM_PIECES_WRONG--;
        TestLevelCtr.instance.SpawnPiece(_startPos,false);



        transform.DOLocalMove(Vector3.zero, _duration).OnStart(() => CheckAutoCorrect())
                                                      .OnComplete(() => SetPositionPiece(true));
        transform.DOScale(Vector3.one, _duration);
        //transform.localScale = selectedScale * Vector3.one;

        //transform.DOComplete();
        //transform.DOMove(Vector3.zero, _duration);
    }
    public Collider2D[] others;
    private void CheckAutoCorrect()
    {
        foreach (Transform grid in transform)
        {
            for (int i = 0; i < TestLevelCtr.instance.availableSpace.Length; i++)
            {
                if (Vector2.Distance(TestLevelCtr.instance.availableSpace[i].position, grid.localPosition) < .1f && TestLevelCtr.instance.availableSpace[i].available == false)
                {
                    TestLevelCtr.instance.availableSpace[i].available = true;
                }
            }
        }
        Piece[] allPieces = GameObject.FindObjectsOfType<Piece>();
        foreach (Piece piece in allPieces)
            if (!piece.isCorrect && piece != this && piece.transform.localScale == Vector3.one)
            {
                piece.OnPieceSelect();
                piece.OnPieceUnselect();
            }
    }
   
    public void TutorialPieceOnMouseDown()
    {
        Transform _shadow = transform.Find("Shadow");
        Transform _sprite = transform.GetChild(transform.childCount - 1);
        Transform _tutPrefap = transform.GetChild(0);
        _shadow.localScale = Vector3.one;
        _sprite.localScale = Vector3.one;
        _tutPrefap.gameObject.SetActive(false);  
    }
   public void TutorialPieceOnMouseUp()
    {
        Transform _shadow = transform.Find("Shadow");
        Transform _sprite = transform.GetChild(transform.childCount - 1);
        Transform _tutPrefap = transform.GetChild(0);
        _shadow.localScale = Vector3.zero;
        _sprite.localScale = Vector3.zero;
        _tutPrefap.gameObject.SetActive(true);
    }

   public void CheckTriggerPiece()
    {
            //Debug.LogError("Check TRigger");
        if (isTriggerOtherPiece && !isCorrect)
        {
            canSetPosition = true;
            transform.DOMove(startPosition, 0.5f);
            transform.DOScale(Vector3.one * startScale, .2f);
        }
    }

    public void DestroyPiece()
    {
        Destroy(gameObject);
    }
}



