using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic ;
using System;
using System.Collections;

public class GridLevel : CoroutineQueue
{
    public GameObject gridChild;
    public TextMeshProUGUI title;
    public static GridLevel instance;
    int numLevelofTheme;

    protected Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();

    private void Awake()
    {
        instance = this;

    }

    void Start()
    {
        //numLevelofTheme = DataController.Instance.themeData.groupLevel.Length;
        numLevelofTheme = DataController.themeData[GameData.Theme].levelCount;
        //SpawnGridChild(ThemeName.Dog, 5);
    }

    void Update()
    {
        
    }


    public void SpawnGridChild(int _idTheme, int _sizeLevel)
    {
        title.text = DataController.themeData[_idTheme].name;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        numLevelofTheme = DataController.themeData[GameData.Theme].levelCount;
        for (int i=0; i < numLevelofTheme; i++)
        {
            Sprite _imgSprite = DataController.LoadSpritePreview( DataController.themeData[GameData.Theme].idTheme,i, _sizeLevel);
            if (_imgSprite != null)
            {
                GameObject _gridChildClone = GameObject.Instantiate(gridChild, gameObject.transform);
                
                _gridChildClone.GetComponent<GridChild>().indexLevel = i;
                _gridChildClone.transform.GetChild(0).GetComponent<Image>().sprite = _imgSprite;
                if (i <= GameData.GetCurrentLevelByTheme(GameData.Theme))
                    _gridChildClone.GetComponent<GridChild>().UnlockLevel();
               
                coroutineQueue.Enqueue(ShowObjZoomOut(_gridChildClone.transform, 0.01f));
            }
            else
            {
                Debug.LogError("Sprite Null");
            } 
        }
        StartCoroutine(CoroutineCoordinator(coroutineQueue));
    }




}
