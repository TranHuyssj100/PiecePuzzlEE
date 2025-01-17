﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ImageCutter : MonoBehaviour
{
    public Sprite[] sprite;
    private GameObject[] all;
    public string path = "Assets/_Project/Testing/";
    // Start is called before the first frame update
    void Start()
    {
        selectedPieces = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        SelectObject();
    }

    public void SpawnGObj()
    {
        index = 0;
        Vector2 origin = Vector2.zero;
        int incrementX = 0;
        int incrementY = 0;
        int size = (int)Mathf.Sqrt(sprite.Length);
        all = new GameObject[sprite.Length];
        for (int i = 0; i < sprite.Length; i++)
        {
            GameObject GO = new GameObject();
            GO.transform.parent = transform;
            GO.name = sprite[i].name;
            GO.AddComponent<SpriteRenderer>().sprite = sprite[i];
            if (i % size == 0)
            {
                incrementX = 0;
            }
            GO.transform.localPosition = origin + new Vector2(incrementX++, incrementY);
            if (i % size == size - 1)
            {
                incrementY--;
            }
            GO.AddComponent<BoxCollider2D>();
            all[i] = GO;
        }
    }

    public void RemoveAllGObj()
    {
        foreach (GameObject gObj in all)
            DestroyImmediate(gObj);
    }
    private List<GameObject> selectedPieces;


    void SelectObject()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                GameObject objectOnMouse = hit.collider.gameObject;
                selectedPieces.Add(objectOnMouse);
                objectOnMouse.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }
    private int index;
    public void SaveSelectedAsPrefab()
    {
        GameObject gObject = new GameObject();
        //Vector3 meanPos = Vector2.zero;
        string savePath = path + "/" + index + ".prefab";
        //foreach (GameObject piece in selectedPieces)
        //    meanPos += piece.transform.position;
        //meanPos /= selectedPieces.Count;
        //gObject.transform.position = meanPos;
        gObject.transform.parent = transform;
        gObject.transform.localPosition = Vector3.zero;
        foreach (GameObject piece in selectedPieces)
        {
            piece.transform.parent = gObject.transform;
            piece.GetComponent<SpriteRenderer>().color = Color.white;
        }
        gObject.name = index++.ToString();
        PrefabUtility.SaveAsPrefabAsset(gObject, savePath);
        selectedPieces.Clear();
        DestroyImmediate(gObject);

    }
}
