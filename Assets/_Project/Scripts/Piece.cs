using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

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


    private void OnEnable()
    {
        EventManager.StartListening("DestroyPiece", DestroyPiece);
        //EventManager.StartListening("CheckTriggerPiece", CheckTriggerPiece);
    }
    private void OnDisable()
    {
        EventManager.StopListening("DestroyPiece", DestroyPiece);
        //EventManager.StopListening("CheckTriggerPiece", CheckTriggerPiece);
        
    }

    private void Start()
    {
        startScale = Config.PIECE_START_SCALE * TestLevelCtr.instance.sizeLevel / 5; // scale size piece for each sizeLevel
        //startScale = Config.PIECE_START_SCALE;// scale size piece for each sizeLevel
        selectedScale = Config.PIECE_SELECTED_SCALE;
        selectedPos = 0.1f;

        canSetPosition = true;
        startPosition = transform.position;
        foreach (Transform grid in transform)
        {
            grid.GetChild(0).gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        
    }

    private void OnMouseDown()
    {
        isMouseDown = true;
        InCreaseSortingLayer(2);
        OnPieceSelect();

        TestLevelCtr.instance.DeativeTutorial();
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
        DecreaseSortingLayer(2);
        if (!isCorrect)
        {
            SetPositionPiece(false);
            //TestLevelCtr.instance.ActiveTutorial();
        }
        GameMaster.instance.PiecePlaced();
    }


    public void OnPieceSelect()
    {

        transform.DOComplete();
        foreach (Transform grid in transform)
        {
            grid.GetChild(0).gameObject.SetActive(true);
            for (int i = 0; i < TestLevelCtr.instance.availableSpace.Length; i++)
            {
                if (TestLevelCtr.instance.availableSpace[i].position == (Vector2)TestLevelCtr.instance.curAllPieces.transform.InverseTransformPoint((Vector2)grid.position))
                    TestLevelCtr.instance.availableSpace[i].available = true;
            }

        }

        if (transform.localScale != Vector3.one)
        {
            Vector3 offset = Vector3.zero;
            foreach (Transform grid in transform)
            {
                offset += grid.position;
            }
            offset /= transform.childCount;
            offset = (transform.position - offset)/* * pieceClone.transform.localScale.x*/;
            Vector3 _temp= transform.position += new Vector3(offset.x, offset.y+2f+ 1*(5-TestLevelCtr.instance.sizeLevel)/2, 0);
            transform.DOMove(_temp, 0.1f);
        }
        else if (isMouseDown)
        {
            transform.position += new Vector3(0,0.5f, 0);
        }
        transform.localScale= Vector3.one;

        //transform.localScale = selectedScale * Vector3.one;
    
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
    TestLevelCtr.Grid[] preState;
    
    void InCreaseSortingLayer(int _value)
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().sortingOrder += _value;
            child.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder += _value;
        }
    }
    void DecreaseSortingLayer(int _value)
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().sortingOrder -= _value;
            child.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder -= _value;
        }
    }
    bool CheckAvailableSpace(Vector2 _space)
    {
        _space = new Vector2(Mathf.Round(_space.x), Mathf.Round(_space.y));
        //Debug.LogError(_space.x);
        if (_space.x < 0 || _space.x > (Mathf.Sqrt(TestLevelCtr.instance.availableSpace.Length) - 1) || _space.y > 0 || _space.y < -(Mathf.Sqrt(TestLevelCtr.instance.availableSpace.Length) - 1))
        {
            return false;
        }
        for (int i = 0; i < TestLevelCtr.instance.availableSpace.Length; i++)
        {
            if (TestLevelCtr.instance.availableSpace[i].position == _space)
            {
                if (TestLevelCtr.instance.availableSpace[i].available)
                    TestLevelCtr.instance.availableSpace[i].available = false;
                else
                {
                    return false;
                }
            }
        }
        return true;
    }
    void SetPositionPiece(bool _autoCorrect)
    {
        transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x),
                                         Mathf.Round(transform.localPosition.y));
        preState = new TestLevelCtr.Grid[(int)Mathf.Pow(TestLevelCtr.instance.sizeLevel, 2)];
        TestLevelCtr.instance.availableSpace.CopyTo(preState,0);
        GameMaster.instance.PiecePlaced();
        foreach (Transform grid in transform)
        {
            grid.GetChild(0).gameObject.SetActive(false);
        }
        foreach (Transform grid in transform)
        {
            if (!CheckAvailableSpace(TestLevelCtr.instance.curAllPieces.transform.InverseTransformPoint(grid.position)))
            {
                TestLevelCtr.instance.availableSpace = preState;
                OnPieceUnselect();
                return;
            }
        }
        if (transform.localPosition != oldPostionOnGridBoard && !_autoCorrect)
        {

            TestLevelCtr.instance.NUM_MOVE--;
            oldPostionOnGridBoard = transform.localPosition;
        }

        if ((Vector2)transform.localPosition == Vector2.zero)
        {
                isCorrect = true;
                SoundManager.instance.PlayRandom(TypeSFX.True);
                //Handheld.Vibrate();
                TestLevelCtr.instance.NUM_PIECES_WRONG--;
                TestLevelCtr.instance.SpawnPiece(startPointIndex,false);
                Collider2D[] colliders = GetComponents<Collider2D>();
                foreach (Collider2D collider in colliders)
                    Destroy(collider);
        }
        else
        {
            SoundManager.instance.PlayRandom(TypeSFX.Wrong);

        }

    }

    public void AutoCorrectPiece(int _startPos, float _duration)
    {
        OnPieceSelect();
        isCorrect = true;
        transform.DOLocalMove(Vector3.zero, _duration).OnStart(() => CheckAutoCorrect())
                                                      .OnComplete(() => {
                                                          SetPositionPiece(true);
                                                      });
    }
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
   
    //public void TutorialPieceOnMouseDown()
    //{
    //    Transform _shadow = transform.Find("Shadow");
    //    Transform _sprite = transform.GetChild(transform.childCount - 1);
    //    Transform _tutPrefap = transform.GetChild(0);
    //    _shadow.localScale = Vector3.one;
    //    _sprite.localScale = Vector3.one;
    //    _tutPrefap.gameObject.SetActive(false);  
    //}
   //public void TutorialPieceOnMouseUp()
   // {
   //     Transform _shadow = transform.Find("Shadow");
   //     Transform _sprite = transform.GetChild(transform.childCount - 1);
   //     Transform _tutPrefap = transform.GetChild(0);
   //     _shadow.localScale = Vector3.zero;
   //     _sprite.localScale = Vector3.zero;
   //     _tutPrefap.gameObject.SetActive(true);
   // }

   //public void CheckTriggerPiece()
   // {
   //         //Debug.LogError("Check TRigger");
   //     if (isTriggerOtherPiece && !isCorrect)
   //     {
   //         canSetPosition = true;
   //         transform.DOMove(startPosition, 0.5f);
   //         transform.DOScale(Vector3.one * startScale, .2f);
   //     }
   // }

    public void DestroyPiece()
    {
        Destroy(gameObject);
    }
}



