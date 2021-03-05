using System;
using System.Collections;
using System.Collections.Generic;
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

    public List<GameObject> listPieces = new List<GameObject>();
    public  Grid[] availableSpace;

    public GameObject allPiece;
    public GameObject[] point;
    public int size;
    public static TestLevelCtr instance;

    private void Start()
    {
        availableSpace = new Grid[size * size];
        CreateAvailableSpaceList();
        IntializeGame();
        instance = this;
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

    public void SpawnPiece( Vector3 _pointSpawn)
    {
        int randomIndex = UnityEngine.Random.Range(0, listPieces.Count);
        GameObject randomPiece = listPieces[randomIndex];
        listPieces.Remove(randomPiece);
        GameObject pieceClone = GameObject.Instantiate(randomPiece, allPiece.transform);
        pieceClone.transform.localScale = Vector3.one * .5f;
        pieceClone.transform.position = _pointSpawn;
        Vector3 offset = Vector3.zero;
        foreach(Transform grid in pieceClone.transform)
        {
            offset += grid.position;
        }
        offset /= pieceClone.transform.childCount;
        offset = (pieceClone.transform.position - offset)/* * pieceClone.transform.localScale.x*/;
        pieceClone.transform.position += offset;
    }

    public void IntializeGame()
    {
        for(int i=0; i<3; i++)
        {
            SpawnPiece(point[i].transform.position);
        }
    }


}
