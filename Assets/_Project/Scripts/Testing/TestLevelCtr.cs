using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class TestLevelCtr : MonoBehaviour
{
    [System.Serializable]
    public struct Grid
    {
        public Vector2 position;
        public bool available;
    }

    [Header("CUSTOM LEVEL")]
    public List<GameObject> listPieces = new List<GameObject>();
    public  Grid[] availableSpace;
    public GameObject allPieces;
    public GameObject[] point;
    public int size;
    public float difficultParam;
    [Space(10)]
    public static TestLevelCtr instance;


    int numPiecesWrong;
    int numMove;
    Stack<int> sequenceIndex;

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
       
        IntializeGame();
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
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j > -size; j--)
            {
                availableSpace[index].position = new Vector2(i, j);
                availableSpace[index].available = true;
                index++;
            }
        }
    }

    public void SpawnPiece(int index)
    {

        if (sequenceIndex.Count > 0)
        {
            int randomIndex = sequenceIndex.Pop();
            GameObject randomPiece = listPieces[randomIndex];
            //listPieces.Remove(randomPiece);
            GameObject pieceClone = GameObject.Instantiate(randomPiece, allPieces.transform);
            pieceClone.transform.localScale = Vector3.one * .5f;
            pieceClone.transform.position = point[index].transform.position;
            pieceClone.GetComponent<Piece>().startPointIndex = index;
  
            //Debug.LogError(_pointSpawn);
            //Vector3 offset = Vector3.zero;
            //foreach(Transform grid in pieceClone.transform)
            //{
            //    offset += grid.position;
            //}
            //offset /= pieceClone.transform.childCount;
            //offset = (pieceClone.transform.position - offset)/* * pieceClone.transform.localScale.x*/;
            //pieceClone.transform.position += offset;
        }
    }

    public Piece FindIncorrectPiece()
    {
        Piece _piece = null;
        if (allPieces != null)
        {
            foreach (Transform child in allPieces.transform)
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

    public void IntializeGame()
    {
        sequenceIndex = new Stack<int>(Enumerable.Range(0, listPieces.Count).ToArray());

        availableSpace = new Grid[size * size];
        CreateAvailableSpaceList();


        for (int i=0; i<3; i++)
        {
            SpawnPiece(i);
        }
        numPiecesWrong = listPieces.Count;
        numMove = Mathf.RoundToInt(listPieces.Count + 0.5f * 1 / difficultParam * listPieces.Count * 1);
        Debug.LogError(numPiecesWrong);
        Debug.LogError(numMove);
    }


}
