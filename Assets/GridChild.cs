using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridChild : MonoBehaviour
{
    public int indexLevel;
    public GameObject imgLock;
    public bool isUnlock = false;
    public TMPro.TextMeshProUGUI levelIndexTxt;


    private void Start()
    {
        levelIndexTxt.text = (indexLevel+1).ToString();
    }

    private void Update()
    {
        //isUnlock = indexLevel <= GameData.GetCurrentLevelByTheme(GameData.Theme);
        //if (isUnlock)     
        //{
        //    if (imgLock.activeSelf) imgLock.SetActive(false);
        //}
        //else
        //{
        //    if (!imgLock.activeSelf) imgLock.SetActive(true);
        //}
    }

    public void OnClick()
    {
        if (isUnlock)
        {
            //StartCoroutine(LevelController.instance.InitializeGame(indexLevel, GameData.Theme));
            TestLevelCtr.instance.InitalizeGame (GameData.Theme, indexLevel);
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
