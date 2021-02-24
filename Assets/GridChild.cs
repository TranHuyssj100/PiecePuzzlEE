using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridChild : MonoBehaviour
{
    public int indexLevel;

    public void OnClick()
    {
        StartCoroutine(LevelController.instance.InitializeGame(indexLevel));
        GameMaster.instance.CloseLevelSelect();
    }
}
