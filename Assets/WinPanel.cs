using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public Image img;

    public Transform reward;
   
    public static WinPanel instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {

    }
    public void SetImageReview()
    {
        int _level = TestLevelCtr.instance.idLevel;
        //Debug.Log(_level);
        img.sprite = DataController.LoadSpritePreview(TestLevelCtr.instance.idTheme, TestLevelCtr.instance.idLevel, TestLevelCtr.instance.sizeLevel);
        TweenCustom.ReWard(reward.GetChild(0).gameObject, reward.GetChild(1).gameObject, 200);
    }

}
