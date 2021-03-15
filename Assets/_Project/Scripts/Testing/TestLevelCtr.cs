﻿using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestLevelCtr : MonoBehaviour
{
    [Header("CUSTOM LEVEL")]
    public int idLevel;
    public int idTheme;
    public int sizeLevel;
    public float difficultParam;
    [Space(5f)]
    public GameObject curAllPieces;
    public Transform gridBroad;
    public LevelData curLevelData;
    public Transform[] arrAllPieces;
    public List<GameObject> listPieces = new List<GameObject>();
    public List<Object> listTest = new List<Object>();
    public GameObject[] point;
    public  Grid[] availableSpace;

    [System.Serializable]
    public struct Grid
    {
        public Vector2 position;
        public bool available;
    }
    public static TestLevelCtr instance;

    [SerializeField]
    int numPiecesWrong;
    int numMove;
    Queue<int> sequenceIndex;

    public int NUM_PIECES_WRONG
    {
        get { return numPiecesWrong; }
        set { numPiecesWrong = value; }
    }
    public int NUM_MOVE
    {
        get { return numMove; }
        set { numMove = value; }
    }

    private void Start()
    {
       
        //IntializeGame(idTheme, idLevel);
        instance = this;
       
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            GameData.gold += 2000;
        }
    }

    public void CreateAvailableSpaceList()
    {
        int index = 0;
        for (int i = 0; i < sizeLevel; i++)
        {
            for (int j = 0; j > -sizeLevel; j--)
            {
                availableSpace[index].position = new Vector2(i, j);
                availableSpace[index].available = true;
                index++;
            }
        }
    }

    public void SpawnPiece(int index, bool autoCorrect)
    {

        if (sequenceIndex.Count > 0)
        {
            int randomIndex = sequenceIndex.Dequeue();
            GameObject randomPiece = listPieces[randomIndex];
            GameObject pieceClone =  Instantiate(randomPiece, curAllPieces.transform);            
            if (autoCorrect)
            {
                SetCorrectPiecePos(pieceClone, 0f);
            }
            else
            {
                pieceClone.transform.position = point[index].transform.position;
                pieceClone.transform.localScale = Vector3.one * Config.PIECE_START_SCALE;
                pieceClone.GetComponent<Piece>().startPointIndex = index;
                Vector3 offset = Vector3.zero;
                foreach (Transform grid in pieceClone.transform)
                {
                    offset += grid.position;
                }
                offset /= pieceClone.transform.childCount;
                offset = (pieceClone.transform.position - offset);
                pieceClone.transform.position += offset;
            } 
        }
    }

    public Piece FindIncorrectPiece()
    {
        Piece _piece = null;
        if (curAllPieces != null)
        {
            foreach (Transform child in curAllPieces.transform)
            {
                if (!child.GetComponent<Piece>().isCorrect)
                {
                    _piece = child.GetComponent<Piece>();
                    break;
                }
            }
        }
        return _piece;
    }
    public void SetCorrectPiecePos(GameObject _piece, float _duration)
    {
        _piece.GetComponent<Piece>().AutoCorrectPiece(_piece.GetComponent<Piece>().startPointIndex, _duration);
    }

    public void InitalizeGame(int _idTheme, int _idLevel)
    {
        int _delay = 0;
        EventManager.TriggerEvent("DestroyPiece");
        
        idLevel = _idLevel;
        idTheme = _idTheme;

        listPieces.Clear();
        if (_idLevel > DataController.themeData[idTheme].levelCount)
        { 
            idLevel = DataController.themeData[idLevel].levelCount - 1;
        }
        listPieces = DataController.LoadPiece(idTheme, idLevel);
        sizeLevel = DataController.themeData[idTheme].size;
        numPiecesWrong = listPieces.Count;
        numMove = Mathf.RoundToInt(listPieces.Count + 0.5f * 1 / difficultParam * listPieces.Count * 1);

        SetCamPosition(sizeLevel);

        DOVirtual.Float(_delay, 0.1f, 0.1f, (x) => {
            //Debug.LogError(x);
            if (x >= 0.1f)
            {
                availableSpace = new Grid[sizeLevel * sizeLevel];
                sequenceIndex = new Queue<int>(Enumerable.Range(0, listPieces.Count).ToArray());
                CreateAvailableSpaceList();

                SpawnPiece(0, true);
                for (int i = 1; i < 3; i++)
                {
                    SpawnPiece(i, false);
                }
                _delay = 0;
            }
        });
      
    }

    public static Stack<int> SwapValuetoTopStack(Stack<int> _stack, int _value)
    {
       
        List<int> _temp = _stack.ToList();
        _temp.Remove(_value);
        _temp.Add(_value);
        return new Stack<int>(_temp);
    }

    void SetCamPosition(int _sizeLevel)
    {
        ActiveGridBoardforEachSize(_sizeLevel);
        switch (_sizeLevel)
        {
            case 5:
                Camera.main.transform.position = new Vector3(Config.POSITION_5x5.x, Config.POSITION_5x5.y, -10);
                Camera.main.orthographicSize = Config.POSITION_5x5.z;
                break;
            case 6:
                Camera.main.transform.position = new Vector3(Config.POSITION_6x6.x, Config.POSITION_6x6.y, -10);
                Camera.main.orthographicSize = Config.POSITION_6x6.z;
                break;
        }
    }

    void ActiveGridBoardforEachSize(int _sizeLevel)
    {
        for (int i = 5; i < gridBroad.childCount + 5; i++)
        {
            if (i == sizeLevel)
            {
                gridBroad.GetChild(i - 5).gameObject.SetActive(true);
                curAllPieces = arrAllPieces[i - 5].gameObject;
            }
            else
            {
                gridBroad.GetChild(i - 5).gameObject.SetActive(false);

            }
        }
    }

}
