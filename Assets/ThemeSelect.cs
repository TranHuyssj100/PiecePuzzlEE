using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSelect : MonoBehaviour
{
    public Transform content;
    public GameObject themeChild;
    private int amountTheme;
    public List<string> allTheme;
    public ThemeChild.ThemeInfo[] themes;
    private void Start()
    {
        //allTheme = DataController.GetAllTheme();
        amountTheme = themes.Length;
        CreateThemeChild();
    }

    public void CreateThemeChild()
    {
        for (int i = 0; i < amountTheme; i++)
        {
            GameObject _themeClone = GameObject.Instantiate(themeChild, content);
            _themeClone.GetComponent<ThemeChild>().index = themes[i].index;
            _themeClone.GetComponent<ThemeChild>().titleTxt.text = themes[i].name.ToString();/*allTheme[i]*/ /*((ThemeName)i).ToString()*/;
            _themeClone.GetComponent<ThemeChild>().image.sprite = themes[i].image;
            int x = i;
            if (GameData.GetStatusTheme(i) == 0)
            {
                _themeClone.GetComponent<ThemeChild>().priceTxt.text = themes[i].price.ToString();
                _themeClone.GetComponent<ThemeChild>().button.onClick.AddListener(() => UnlockTheme(x));
            }
            else
            {
                _themeClone.GetComponent<ThemeChild>().priceTxt.text = "Open";
                Debug.Log(GridLevel.instance);
                _themeClone.GetComponent<ThemeChild>().button.onClick.AddListener(() => { GameMaster.instance.OpenLevelSelect();
                                                                                          GridLevel.instance.SpawnGridChild((ThemeName)System.Enum.Parse(typeof(ThemeName), themes[x].name.ToString()), themes[x].size);
                                                                                          GameMaster.instance.CloseThemeSelect();
                                                                                          GameMaster.instance.OnStartClick();
                                                                                         });
            }
        }
    }

    private void UnlockTheme(int index)
    {
        if (GameData.gold >= themes[index].price)
        {
            Debug.Log(index);
            GameData.gold -= themes[index].price;
            GameData.SetStatusByTheme(index, 1);
            content.GetChild(index).GetComponent<ThemeChild>().priceTxt.text = "Open";
            content.GetChild(index).GetComponent<ThemeChild>().button.onClick.RemoveAllListeners();
           content.GetChild(index).GetComponent<ThemeChild>().button.onClick.AddListener(() => { GameMaster.instance.OpenLevelSelect();
                                                                                                 GridLevel.instance.SpawnGridChild((ThemeName)System.Enum.Parse(typeof(ThemeName), themes[index].name.ToString()), themes[index].size);
                                                                                                 GameMaster.instance.CloseThemeSelect();
                                                                                                 GameMaster.instance.OnStartClick();

                                                                                                 });
        }
    }
}
