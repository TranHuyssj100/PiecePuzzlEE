using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public Image img;
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
    public void SetImageReview( )
    {
        int _level = LevelController.level;
        img.sprite = LevelController.LoadSpriteReview(_level,(ThemeType)GameData.Theme, LevelController.instance.sizeLevel);
    }

}
