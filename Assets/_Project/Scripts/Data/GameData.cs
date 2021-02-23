using UnityEngine;

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
        GOLD = PlayerPrefs.GetInt("gold", 0);
    }  
    
    public static int level
    {
        get { return LEVEL; }
        set { PlayerPrefs.SetInt("level", (LEVEL = value)); }
    }  
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
}
