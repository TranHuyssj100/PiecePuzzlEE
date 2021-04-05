using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionBook : ThemeSelect
{


    
    public override void CreateThemeChild()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < amountTheme; i++)
        {
            GameObject _themeClone = GameObject.Instantiate(themeChild, content);
            _themeClone.GetComponent<ThemeChild>().index = themes[i].idTheme;
            _themeClone.GetComponent<ThemeChild>().titleTxt.text = themes[i].name;
            _themeClone.GetComponent<ThemeChild>().SetRankTheme(themes[i].size);
            _themeClone.GetComponent<ThemeChild>().image.sprite = DataController.LoadSpritePreview(themes[i].idTheme, 0, themes[i].size);
            int x = i;
            if (GameData.GetThemeStatus(i) == 0)
            {
                _themeClone.GetComponent<ThemeChild>().priceTxt.text = "Locked";
                _themeClone.GetComponent<ThemeChild>().priceTxt.rectTransform.anchorMin = new Vector2(.5f, .5f);
                _themeClone.GetComponent<ThemeChild>().priceTxt.rectTransform.anchorMax = new Vector2(.5f, .5f);
                _themeClone.GetComponent<ThemeChild>().priceTxt.rectTransform.anchoredPosition = new Vector2(0,5f);
                _themeClone.GetComponent<ThemeChild>().buyBtn.transform.GetChild(0).gameObject.SetActive(false);
                _themeClone.GetComponent<ThemeChild>().buyBtn.interactable = false;
            }
            else
            {
                _themeClone.GetComponent<ThemeChild>().UnlockTheme();
                _themeClone.GetComponent<ThemeChild>().progressBtn.onClick.AddListener(() => {
                    GameMaster.instance.OpenCollectionGrid();
                    GameData.Theme = x;
                    GridLevel.instance.SpawnGridChild(x, themes[x].size);
                    GameMaster.instance.CloseThemeSelect();
                });
            }
            int siblingIndex = themeOrder.IndexOf(themes[i].idTheme);
            _themeClone.transform.SetSiblingIndex(siblingIndex);
        }
    }
}
