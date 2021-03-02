using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSelect : MonoBehaviour
{
    public Transform content;
    public GameObject themeChild;
    private int amountTheme;
    public ThemeData[] themes;
    private void Start()
    {
        //allTheme = DataController.GetAllTheme();
        themes = DataController.themeData;
        amountTheme = themes.Length;
        CreateThemeChild();
    }

    private void OnEnable()
    {
        CreateThemeChild();
    }

    public void CreateThemeChild()
    {
        foreach(Transform child in content)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < amountTheme; i++)
        {
            GameObject _themeClone = GameObject.Instantiate(themeChild, content);
            _themeClone.GetComponent<ThemeChild>().index = themes[i].idTheme;
            Debug.Log(_themeClone.GetComponent<ThemeChild>().index);
            _themeClone.GetComponent<ThemeChild>().titleTxt.text = themes[i].name;/*allTheme[i]*/ /*((ThemeName)i).ToString()*/;
            _themeClone.GetComponent<ThemeChild>().image.sprite = LevelController.LoadSpritePreview(0, themes[i].name, themes[i].size);
            int x = i;
            if (GameData.GetThemeStatus(i) == 0)
            {
                _themeClone.GetComponent<ThemeChild>().priceTxt.text = themes[i].price.ToString();
                _themeClone.GetComponent<ThemeChild>().buyBtn.onClick.AddListener(() => UnlockTheme(x));
            }
            else
            {
                _themeClone.GetComponent<ThemeChild>().UnlockTheme();
                //_themeClone.GetComponent<ThemeChild>().priceTxt.text = "Open";
                _themeClone.GetComponent<ThemeChild>().progressBtn.onClick.AddListener(() => { GameMaster.instance.OpenLevelSelect();
                                                                                          GameData.Theme = x;
                                                                                          GridLevel.instance.SpawnGridChild(x, themes[x].size);
                                                                                          GameMaster.instance.CloseThemeSelect();
                                                                                          //GameMaster.instance.OnStartClick();
                                                                                         });
            }
        }
    }


    private void UnlockTheme(int index)
    {
        if (GameData.gold >= themes[index].price)
        {
            GameData.gold -= themes[index].price;
            content.GetChild(index).GetComponent<ThemeChild>().UnlockTheme();
            //content.GetChild(index).GetComponent<ThemeChild>().priceTxt.text = "Open";
            content.GetChild(index).GetComponent<ThemeChild>().buyBtn.onClick.RemoveAllListeners();
            content.GetChild(index).GetComponent<ThemeChild>().progressBtn.onClick.AddListener(() => { GameMaster.instance.OpenLevelSelect();
                                                                                                 GameData.Theme = index;
                                                                                                 GridLevel.instance.SpawnGridChild(index, themes[index].size);
                                                                                                 GameMaster.instance.CloseThemeSelect();
                                                                                                 //GameMaster.instance.OnStartClick();
                                                                                                 });
        }
    }
}
