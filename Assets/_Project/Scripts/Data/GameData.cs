﻿using UnityEngine;

public static class GameData
{
    private static int THEME;
    private static int LEVEL;
    private static int IS_SOUND;
    private static int GOLD;
    private static int NO_ADS;

    static GameData()
    {
        LEVEL= PlayerPrefs.GetInt("level", 0);
        THEME=PlayerPrefs.GetInt("theme", 0);
        IS_SOUND = PlayerPrefs.GetInt("sound", 1);
        GOLD = PlayerPrefs.GetInt("gold", 300);
        NO_ADS = PlayerPrefs.GetInt("no_ads", 0);

        //CreateCurrentLevelforEachTheme();
        //CreateStatusTheme();
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
    public static int gold
    {
        get { return GOLD; }
        set { PlayerPrefs.SetInt("gold", (GOLD = value)); }
    }


    public static void CreateCurrentLevelforEachTheme()
    {
        //for (int i = 0; i < (int)ThemeName.NUM_OF_THEME; i++)
        //{
        //    if (!PlayerPrefs.HasKey(((ThemeName)i).ToString() + "Level"))
        //        PlayerPrefs.SetInt(((ThemeName)i).ToString() + "Level", 0);
        //}

        for (int i = 0; i < DataController.themeData.Length; i++)
        {
            if (!PlayerPrefs.HasKey(DataController.themeData[i].name + "Level"))
                PlayerPrefs.SetInt(DataController.themeData[i].name + "Level", 0);
        }
    }

    public static int GetCurrentLevelByTheme(int _idTheme)
    {
        return PlayerPrefs.GetInt(DataController.themeData[_idTheme].name + "Level");
    }
    public static void SetCurrentLevelByTheme(int _idTheme, int _value)
    {

        PlayerPrefs.SetInt(DataController.themeData[_idTheme].name + "Level", _value);
    }

    public static void CreateStatusTheme()
    {
        PlayerPrefs.SetInt(DataController.themeData[0].name + "Unlock", 1);
        for (int i = 1; i < DataController.themeData.Length; i++)
        {
            if (!PlayerPrefs.HasKey(DataController.themeData[i].name + "Unlock"))
                PlayerPrefs.SetInt(DataController.themeData[i].name + "Unlock", 0);
        }
    }

    public static int GetThemeStatus(int _themeID)
    {
        return PlayerPrefs.GetInt(DataController.themeData[_themeID].name + "Unlock");
    }
    public static void UnlockTheme(int _themeID)
    {
        PlayerPrefs.SetInt(DataController.themeData[_themeID].name + "Unlock", 1);
    }

    public static int noAds
    {
        get { return NO_ADS; }
        set { PlayerPrefs.SetInt("no_ads", (NO_ADS = value)); }
    }
}
