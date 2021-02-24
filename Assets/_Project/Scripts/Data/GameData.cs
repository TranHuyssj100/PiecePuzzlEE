﻿using UnityEngine;

public static class GameData
{
    private static int THEME;
    private static int LEVEL;
    private static int IS_SOUND;
    private static int PREVIEW;
    private static int AUTO_CORRECT;
    private static int GOLD;

    static GameData()
    {
        LEVEL= PlayerPrefs.GetInt("level", 0);
        THEME=PlayerPrefs.GetInt("theme", 0);
        IS_SOUND = PlayerPrefs.GetInt("sound", 1);
        PREVIEW = PlayerPrefs.GetInt("preview", 0);
        AUTO_CORRECT = PlayerPrefs.GetInt("auto_correct", 0);
        GOLD = PlayerPrefs.GetInt("gold", 0);


        CreateCurrentLevelforEachTheme();
    }  
    
    //public static int level
    //{
    //    get { return LEVEL; }
    //    set { PlayerPrefs.SetInt("level", (LEVEL = value)); }
    //}  
    public static int Theme
    {
        get { return THEME; }
        set { PlayerPrefs.SetInt("theme", (THEME = value)); }
    }
    public static int isSound
    {
        get { return IS_SOUND; }
        set { PlayerPrefs.SetInt("sound", (IS_SOUND = value)); }
    }
    public static int preview
    {
        get { return PREVIEW; }
        set { PlayerPrefs.SetInt("preview", (PREVIEW = value)); }
    }
    public static int auto_correct
    {
        get { return AUTO_CORRECT; }
        set { PlayerPrefs.SetInt("auto_correct", (AUTO_CORRECT = value)); }
    }   
    public static int gold
    {
        get { return GOLD; }
        set { PlayerPrefs.SetInt("gold", (GOLD = value)); }
    }

    public static void CreateCurrentLevelforEachTheme()
    {
        for(int i=0; i< (int) ThemeType.NUM_OF_THEME; i++)
        {
            if(!PlayerPrefs.HasKey(((ThemeType)i).ToString() + "Level"))
                PlayerPrefs.SetInt(((ThemeType)i).ToString() + "Level", 0);
        }
    }
    public static int GetCurrentLevelByTheme( int _themeType)
    {
        return PlayerPrefs.GetInt(((ThemeType)_themeType).ToString() + "Level");
    }
    public static void SetCurrentLevelByTheme( int _themeType, int _value)
    {
         PlayerPrefs.SetInt(((ThemeType)_themeType).ToString() + "Level", _value);
    }


}
