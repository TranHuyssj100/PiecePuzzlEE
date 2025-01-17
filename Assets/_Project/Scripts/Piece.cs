﻿using UnityEngine;
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
    public float startScale = .6f; 
    public float selectedScale = 1.2f;
    public float selectedPos = 0.3f;
    public Vector2 sizeSprite;
    public Vector3 startPosition;
    public Vector3 oldPostionOnGridBoard=Vector3.one*10000;


    
    Vector3 oldMousePos;
    Vector2 limitPosX= new Vector2(-3,1);
    Vector2 limitPosY= new Vector2(-2,3);

    int _indexTrueSound = 0;


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
        startScale = .55f;
        selectedScale = 1f;
        selectedPos = 0.1f;

        canSetPosition = true;
        startPosition = transform.position;

        //transform.DOScale(Vector3.one * startScale,0.2f);
        SetScalePieceOnPreSpace();

        limitPosX += new Vector2(0, -1 * (sizeSprite.x - 1));
        limitPosY -= new Vector2(0, (sizeSprite.y));
    }


    private void Update()
    {

    }

    private void OnMouseDown()
    {
        //Debug.Log("OnMouseDown");
        if (!isCorrect)
        {
            OnPieceSelect();
            isMouseDown = true;
            if (isOnPreSpace)
            {
                oldPostionOnGridBoard = startPosition;
            }
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
        //Debug.Log("OnMouseUp");
        if (!isCorrect)
        {
            OnPieceUnselect();
            isMouseDown = false;
            if (isOnGridBoard && canSetPosition && !isOnPreSpace)
            {
                //LevelController.indexSpawn--;
                SetPositionPiece();
            }
            else
            {
                canSetPosition = true;
                //SetScalePieceOnPreSpace();
                transform.DOMove(startPosition, 0.5f);
                transform.DOScale(Vector3.one * startScale, .2f);
            }
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
    void SetScalePieceOnPreSpace()
    {
        Transform _sprite = transform.GetChild(transform.childCount - 1);
        Transform _shadow = transform.Find("Shadow");
       
        if (isOnPreSpace)
        {
            transform.localScale = Vector3.zero;
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one * startScale, 0.2f);
            _sprite.localScale = Vector2.one* 0.8f;
            _sprite.localPosition = Vector2.zero;
            _shadow.localScale = Vector2.one* 0.8f;
            _shadow.localPosition = Vector2.zero;
        }
    }

    public void OnPieceSelect()
    {
        Transform _sprite = transform.GetChild(transform.childCount - 1);
        Transform _shadown = transform.Find("Shadow");
        if (_sprite != null)
        {

            Sequence _seq = DOTween.Sequence();
            _seq.Append(transform.DOScale(Vector3.one, .1f));

            _sprite.localPosition = new Vector2(_sprite.localPosition.x + selectedPos, _sprite.localPosition.y + selectedPos);
            _sprite.localScale = Vector2.one * selectedScale;
            _shadown.localScale = Vector2.one * selectedScale;

            _sprite.GetComponent<SpriteRenderer>().sortingOrder++;
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
    public void OnPieceUnselect()
    {
        Transform _sprite = transform.GetChild(transform.childCount - 1);
        Transform _shadown = transform.Find("Shadow");
        if (_sprite != null)
        {
            _sprite.localPosition = Vector2.zero;

            Sequence _seq = DOTween.Sequence();
            _seq.Append(transform.DOScale(Vector3.one, .1f))
                .OnComplete(()=> {
                _sprite.GetComponent<SpriteRenderer>().sortingOrder--;
                _shadown.GetComponent<SpriteRenderer>().sortingOrder--;
            });
           
            _sprite.localScale = isOnPreSpace ?Vector2.one*1f:Vector2.one;
            _shadown.localScale = isOnPreSpace ?Vector2.one*1f:Vector2.one;
        }
    }
    void SetPositionPiece()
    {
        Vector3 _pos = new Vector3(Mathf.Clamp((Mathf.RoundToInt(transform.position.x)), limitPosX.x, limitPosX.y),
                                     Mathf.Clamp(Mathf.RoundToInt(transform.position.y), limitPosY.x, limitPosY.y),
                                     transform.position.z);

        //Debug.Log(_pos);
        if (_pos != oldPostionOnGridBoard || oldPostionOnGridBoard == Vector3.one * 10000)
        {
            oldPostionOnGridBoard =_pos;
            LevelController.instance.NUM_MOVE--;
        }
        transform.DOMove(_pos, 0.2f).OnComplete(() =>
                                     {
                                         isCorrect = new Vector3(id, transform.position.x, transform.position.y) == LevelController.instance.listAnswerForSample[id] ? true:false;
                                        if (isCorrect)
                                        { 
                                            transform.GetChild(transform.childCount-1).localScale=Vector3.one;
                                            transform.GetChild(transform.childCount - 1).localPosition = Vector2.zero;
                                            LevelController.instance.SpawnRadomPieces(startPosition);
                                            SoundManager.instance.playSequential(TypeSFX.True);
                                            LevelController.instance.NUM_PIECES_WORNG--;
                                             //Debug.Log(id + "<color=green> is Correctly </color>," + "numMove "+ LevelController.instance.NUM_MOVE);
                                         }
                                        else
                                        {
                                            SoundManager.instance.ClearIndexSquential(TypeSFX.True);
                                            SoundManager.instance.PlayRandom(TypeSFX.Wrong);
                                        }
                                         //Debug.LogError(LevelController.instance.NUM_PIECES_WORNG);

                                     });
    }

    public void AutoCorrectPiece(Vector2 _correctPos,Vector2 _startPos , float _duration)
    {
        FirebaseManager.instance.LogAutoCorrectHint();
        startPosition = _startPos;
        Transform _sprite = transform.GetChild(transform.childCount - 1);
        Transform _shadow = transform.Find("Shadow");
        LevelController.instance.NUM_PIECES_WORNG--;

        isCorrect = true;
        isOnPreSpace = false;
        transform.localScale = Vector2.one;
        _sprite.localScale  = Vector2.one;
        _sprite.localPosition = Vector2.zero;    
        _shadow.localScale  = Vector2.one;
        _shadow.localPosition = Vector2.zero;

        transform.DOMove(_correctPos, _duration);
        LevelController.instance.SpawnRadomPieces(startPosition);   
    }

   
    public void DestroyPiece()
    {
        Destroy(gameObject);
    }
}



