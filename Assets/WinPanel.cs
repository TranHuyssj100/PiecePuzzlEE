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
        //Debug.Log(_imgSprite.name);
        int _level = GameData.level;
        img.sprite= LoadSpriteReview(_level,(ThemeType)GameData.Theme);
    }

    public Sprite LoadSpriteReview(int _level, ThemeType _themeType)
    {
        string _path = _themeType.ToString() + "/" + _level.ToString() + "/full";
        //Debug.Log(_path);
        Sprite _sprite = Resources.Load<Sprite>(_path);
        //Debug.Log(_sprite.name);
        return _sprite;
    }
}
