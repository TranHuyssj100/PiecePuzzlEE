#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class ImageCutter : MonoBehaviour
{
    public Sprite[] sprite;
    private GameObject[] all;
    [HideInInspector] public string path;
    [HideInInspector] public string theme;
    int levelIndex;
    // Start is called before the first frame update
    void Start()
    {
        selectedPieces = new List<GameObject>();
        answerPreset = new List<AnswerPreset>();
    }

    // Update is called once per frame
    void Update()
    {
        SelectObject();
        if (Input.GetKeyDown(KeyCode.Return))
            SaveSelectedAsPrefab();
    }

    public void ClearSprite()
    {
        sprite = new Sprite[0];
    }

    public void SpawnGObj()
    {
        index = 0;
        selectedPieces.Clear();
        answerPreset.Clear();
        levelIndex = GetLevelIndexByTheme();
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
            GO.name = i.ToString();
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
    private List<AnswerPreset> answerPreset;

    public void SaveSelectedAsPrefab()
    {
        GameObject sprite = new GameObject();
        //Vector3 meanPos = Vector2.zero;
        string savePath = path + theme + "/" + levelIndex + "/" + index + ".prefab";
        Debug.Log(savePath);
        //foreach (GameObject piece in selectedPieces)
        //    meanPos += piece.transform.position;
        //meanPos /= selectedPieces.Count;
        //gObject.transform.position = meanPos;
        sprite.transform.parent = transform;
        sprite.transform.localPosition = Vector3.zero;
        answerPreset.Add(new AnswerPreset()
        {
            blockIndex = index,
            gridIndex = new List<int>()
        });
        foreach (GameObject grid in selectedPieces)
        {
            GameObject shadow = new GameObject();
            answerPreset[index].gridIndex.Add(int.Parse(grid.name));
            grid.transform.parent = sprite.transform;
            grid.GetComponent<SpriteRenderer>().color = Color.white;
            shadow.transform.parent = grid.transform;
            shadow.name = "Shadow";
            shadow.transform.localPosition = Vector3.zero + new Vector3(.1f,-.1f);
            shadow.AddComponent<SpriteRenderer>().sprite = grid.GetComponent<SpriteRenderer>().sprite;
            shadow.GetComponent<SpriteRenderer>().color = new Color(0,0,0,.7f);
            shadow.GetComponent<SpriteRenderer>().sortingLayerName = "Piece";
            shadow.GetComponent<SpriteRenderer>().sortingOrder = 0;
            DestroyImmediate(grid.GetComponent<BoxCollider2D>());
            sprite.AddComponent<BoxCollider2D>().offset = grid.transform.localPosition;
        }
        sprite.name = index++.ToString();
        sprite.AddComponent<Piece>();
#if UNITY_EDITOR
        if (!System.IO.Directory.Exists(path + theme + "/" + levelIndex))
            System.IO.Directory.CreateDirectory(path + theme + "/" + levelIndex);
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(sprite, savePath);
#endif
        selectedPieces.Clear();
        DestroyImmediate(sprite);
    }

    public void CutImageWithPreset()
    {
        index = 0;
        answerPreset = DataController.ReadAnswerPreset((int)Mathf.Sqrt(sprite.Length));
        for (int i = 0; i < answerPreset.Count; i++)
        {
            GameObject sprite = new GameObject();
            string savePath = path + theme + "/" + levelIndex + "/" + index + ".prefab";
            sprite.transform.parent = transform;
            sprite.transform.localPosition = Vector3.zero;
            for(int j = 0; j < answerPreset[i].gridIndex.Count;j++)
            {
                GameObject shadow = new GameObject();
                GameObject grid = transform.Find(answerPreset[i].gridIndex[j].ToString()).gameObject;
                grid.transform.parent = sprite.transform;
                grid.GetComponent<SpriteRenderer>().color = Color.white;
                shadow.transform.parent = grid.transform;
                shadow.name = "Shadow";
                shadow.transform.localPosition = Vector3.zero + new Vector3(.1f, -.1f);
                shadow.AddComponent<SpriteRenderer>().sprite = grid.GetComponent<SpriteRenderer>().sprite;
                shadow.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, .7f);
                shadow.GetComponent<SpriteRenderer>().sortingLayerName = "Piece";
                shadow.GetComponent<SpriteRenderer>().sortingOrder = 0;
                DestroyImmediate(grid.GetComponent<BoxCollider2D>());
                sprite.AddComponent<BoxCollider2D>().offset = grid.transform.localPosition;
            }
            sprite.name = index++.ToString();
            sprite.AddComponent<Piece>();
#if UNITY_EDITOR
            if (!System.IO.Directory.Exists(path + theme + "/" + levelIndex))
                System.IO.Directory.CreateDirectory(path + theme + "/" + levelIndex);
            UnityEditor.PrefabUtility.SaveAsPrefabAsset(sprite, savePath);
#endif
            selectedPieces.Clear();
            DestroyImmediate(sprite);
        }
    }

    public void SavePresetToJson()
    {
        DataController.SaveAnswerPreset(answerPreset, (int)Mathf.Sqrt(sprite.Length));
    }

    public void CreateFolder()
    {
        System.IO.Directory.CreateDirectory(path + theme + "/");
    }
    public int GetLevelIndexByTheme()
    {
        if (!System.IO.Directory.Exists(path + theme + "/"))
            System.IO.Directory.CreateDirectory(path + theme + "/");
        return System.IO.Directory.GetFiles(path + theme +"/").Length;
    }

    [System.Serializable]
    public struct AnswerPreset
    {
        public int blockIndex;
        public List<int> gridIndex;
    }
}
#endif
