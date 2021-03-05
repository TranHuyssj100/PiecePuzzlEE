using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class ImageCutter : MonoBehaviour
{
    public Sprite[] sprite;
    private GameObject[] all;
    [HideInInspector] public string path = "Assets/_Project/Testing/";
    // Start is called before the first frame update
    void Start()
    {
        selectedPieces = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        SelectObject();
        if (Input.GetKeyDown(KeyCode.Return))
            SaveSelectedAsPrefab();
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
            GO.GetComponent<SpriteRenderer>().sortingLayerName = "Piece";
            GO.GetComponent<SpriteRenderer>().sortingOrder = 1;
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
                if(!selectedPieces.Contains(objectOnMouse))
                    selectedPieces.Add(objectOnMouse);
                objectOnMouse.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }
    private int index;
    public void SaveSelectedAsPrefab()
    {
        GameObject sprite = new GameObject();
        //Vector3 meanPos = Vector2.zero;
        string savePath = path + "/" + index + ".prefab";
        //foreach (GameObject piece in selectedPieces)
        //    meanPos += piece.transform.position;
        //meanPos /= selectedPieces.Count;
        //gObject.transform.position = meanPos;
        sprite.transform.parent = transform;
        sprite.transform.localPosition = Vector3.zero;
        sprite.name = "Sprite";
        foreach (GameObject piece in selectedPieces)
        {
            GameObject shadow = new GameObject();
            piece.transform.parent = sprite.transform;
            piece.GetComponent<SpriteRenderer>().color = Color.white;
            shadow.transform.parent = piece.transform;
            shadow.name = "Shadow";
            shadow.transform.localPosition = Vector3.zero + new Vector3(.1f,-.1f);
            shadow.AddComponent<SpriteRenderer>().sprite = piece.GetComponent<SpriteRenderer>().sprite;
            shadow.GetComponent<SpriteRenderer>().color = new Color(0,0,0,.7f);
            shadow.GetComponent<SpriteRenderer>().sortingLayerName = "Piece";
            shadow.GetComponent<SpriteRenderer>().sortingOrder = 0;
            DestroyImmediate(piece.GetComponent<BoxCollider2D>());
            sprite.AddComponent<BoxCollider2D>().offset = piece.transform.localPosition;
        }
        sprite.name = index++.ToString();
#if UNITY_EDITOR
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(sprite, savePath);
#endif
        selectedPieces.Clear();
        DestroyImmediate(sprite);

    }

    public void CreateFolder()
    {
        System.IO.Directory.CreateDirectory(path);
    }
}
