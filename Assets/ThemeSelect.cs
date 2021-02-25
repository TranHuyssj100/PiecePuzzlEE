using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSelect : MonoBehaviour
{
    public Transform content;
    public GameObject themeChild;
    public int amountTheme;
    private void Start()
    {
        amountTheme = DataController.GetAmountTheme();
        CreateThemeChild();
    }

    public void CreateThemeChild()
    {
        for(int i=0; i<amountTheme; i++)
        {
            GameObject _themeClone = GameObject.Instantiate(themeChild, content);
            _themeClone.GetComponent<ThemeChild>().index = i;
            _themeClone.GetComponent<ThemeChild>().title.text = ((ThemeType)i).ToString() ;
        }
    }
   
   
}
