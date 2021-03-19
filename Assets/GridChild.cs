using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridChild : MonoBehaviour
{
    public int indexLevel;
    public GameObject imgLock;
    public bool isUnlock = false;
    public TMPro.TextMeshProUGUI levelIndexTxt;


    private void OnEnable()
    {
    }

    private void Start()
    {
        levelIndexTxt.text = (indexLevel+1).ToString();
    }

    private void Update()
    {
    
    }

    public void OnClick()
    {
        if (isUnlock)
        {
            //StartCoroutine(LevelController.instance.InitializeGame(indexLevel, GameData.Theme));
            //GameMaster.instance.AnimatePlayUI();
           StartCoroutine(TestLevelCtr.instance.InitalizeGame (GameData.Theme, indexLevel));
            //GameMaster.instance.OnStartClick();
            GameMaster.instance.CloseLevelSelect();
        }
    }

    public void UnlockLevel()
    {
        isUnlock = true;
        imgLock.SetActive(false);
    }
}
