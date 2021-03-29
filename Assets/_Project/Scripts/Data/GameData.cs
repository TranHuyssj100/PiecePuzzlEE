using UnityEngine;

public static class GameData
{
    private static int THEME;
    private static int LEVEL;
    private static int LEVEL_REWARD;
    private static int IS_BGM;
    private static int IS_SFX;
    private static int GOLD;
    private static int NO_ADS;

    private static int FIRST_TIME_IN_GAME;


    public static event System.Action onGoldValueChanged;
    public static void GoldValueChanged()
    {
        if (onGoldValueChanged != null)
            onGoldValueChanged();
    }


    static GameData()
    {
        LEVEL= PlayerPrefs.GetInt("level", 1);
        THEME=PlayerPrefs.GetInt("theme", 0);
        IS_BGM = PlayerPrefs.GetInt("bgm", 1);
        IS_SFX = PlayerPrefs.GetInt("sfx", 1);
        GOLD = PlayerPrefs.GetInt("gold", 300);
        NO_ADS = PlayerPrefs.GetInt("no_ads", 0);
        FIRST_TIME_IN_GAME = PlayerPrefs.GetInt("first_time_in_game", 1);
        LEVEL_REWARD = PlayerPrefs.GetInt("level_reward", 0);

        //CreateCurrentLevelforEachTheme();
        //CreateStatusTheme();
    }

    public static int level
    {
        get { return LEVEL; }
        set { PlayerPrefs.SetInt("level", (LEVEL = value)); }
    }
    public static int levelReward
    {
        get { return LEVEL_REWARD; }
        set { PlayerPrefs.SetInt("level_reward", (LEVEL_REWARD = value)); }
    }
    public static int Theme
    {
        get { return THEME; }
        set { PlayerPrefs.SetInt("theme", (THEME = value)); }
    }
    public static int isBGM
    {
        get { return IS_BGM; }
        set { PlayerPrefs.SetInt("bgm", (IS_BGM = value)); }
    }
    public static int isSFX
    {
        get { return IS_SFX; }
        set { PlayerPrefs.SetInt("sfx", (IS_SFX = value)); }
    }
    public static int gold
    {
        get { return GOLD; }
        set { PlayerPrefs.SetInt("gold", (GOLD = value));
            GoldValueChanged();
        }
    }
    public static int firstTimeInGame
    {
        get { return FIRST_TIME_IN_GAME; }
        set { PlayerPrefs.SetInt("first_time_in_game", (FIRST_TIME_IN_GAME = value)); }
    }


    public static void CreateCurrentLevelforEachTheme()
    {
    
        for (int i = 0; i < DataController.themeData.Length; i++)
        {
            if (!PlayerPrefs.HasKey(DataController.themeData[i].name + "Level"))
                PlayerPrefs.SetInt(DataController.themeData[i].name + "Level", 0);
        }
    }

    public static int GetCurrentLevelByTheme(int _idTheme)
    {
        return PlayerPrefs.GetInt(DataController.themeData[_idTheme].name + "Level", 0);
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
        return PlayerPrefs.GetInt(DataController.themeData[_themeID].name + "Unlock", 0);
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
