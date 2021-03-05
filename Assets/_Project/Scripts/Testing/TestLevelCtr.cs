using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class TestLevelCtr : MonoBehaviour
{
    public List<GameObject> listPieces = new List<GameObject>();
    public  List<Vector2>  availableSpaceList = new List<Vector2>();

    public GameObject allPiece;
    public GameObject[] point;

    public static TestLevelCtr instance;

    private void Start()
    {
        CreateAvailableSpaceList();
        IntializeGame();
        instance = this;

    }


    public void CreateAvailableSpaceList()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j > -5; j--)
            {
                availableSpaceList.Add(new Vector2(i, j));
            }
        }
    }

    public void SpawnPiece( Vector3 _pointSpawn)
    {
        int randomIndex = UnityEngine.Random.Range(0, listPieces.Count);
        GameObject randomPiece = listPieces[randomIndex];
        listPieces.Remove(randomPiece);
        GameObject pieceClone = GameObject.Instantiate(randomPiece, allPiece.transform);
        //pieceClone.transform.localScale = Vector3.one * .5f;
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
