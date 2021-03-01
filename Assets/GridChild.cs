using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridChild : MonoBehaviour
{
    public int indexLevel;
    public GameObject imgLock;
    public bool isUnlock = false;

    

    
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
            GameMaster.instance.OnStartClick();
            GameMaster.instance.CloseLevelSelect();
        }
    }

    public void UnlockLevel()
    {
        isUnlock = true;
        imgLock.SetActive(false);
    }
}
