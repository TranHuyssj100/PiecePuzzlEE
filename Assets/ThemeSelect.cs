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

    public void CreateThemeChild()
    {
        for (int i = 0; i < amountTheme; i++)
        {
            GameObject _themeClone = GameObject.Instantiate(themeChild, content);
            _themeClone.GetComponent<ThemeChild>().index = themes[i].idTheme;
            _themeClone.GetComponent<ThemeChild>().titleTxt.text = themes[i].name;/*allTheme[i]*/ /*((ThemeName)i).ToString()*/;
            //_themeClone.GetComponent<ThemeChild>().image.sprite = themes[i].image;
            int x = i;
            //if (GameData.GetStatusTheme(i) == 0)
            //{
            //    _themeClone.GetComponent<ThemeChild>().priceTxt.text = themes[i].price.ToString();
            //    _themeClone.GetComponent<ThemeChild>().button.onClick.AddListener(() => UnlockTheme(x));
            //}
            //else
            //{
                _themeClone.GetComponent<ThemeChild>().priceTxt.text = "Open";
                _themeClone.GetComponent<ThemeChild>().button.onClick.AddListener(() => { GameMaster.instance.OpenLevelSelect();
                                                                                          GameData.Theme = x;
                                                                                          GridLevel.instance.SpawnGridChild(x, themes[x].size);
                                                                                          GameMaster.instance.CloseThemeSelect();
                                                                                          GameMaster.instance.OnStartClick();
                                                                                         });
            //}
        }
    }


    private void UnlockTheme(int index)
    {
        if (GameData.gold >= themes[index].price)
        {
            Debug.Log(index);
            GameData.gold -= themes[index].price;
            //GameData.SetStatusByTheme(index, 1);
            content.GetChild(index).GetComponent<ThemeChild>().priceTxt.text = "Open";
            content.GetChild(index).GetComponent<ThemeChild>().button.onClick.RemoveAllListeners();
           content.GetChild(index).GetComponent<ThemeChild>().button.onClick.AddListener(() => { GameMaster.instance.OpenLevelSelect();
                                                                                                 GridLevel.instance.SpawnGridChild(index, themes[index].size);
                                                                                                 GameMaster.instance.CloseThemeSelect();
                                                                                                 GameMaster.instance.OnStartClick();

                                                                                                 });
        }
    }
}
