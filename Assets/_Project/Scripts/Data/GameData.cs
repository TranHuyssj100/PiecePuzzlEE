using UnityEngine;

public static class GameData
{
    private static int THEME;
    private static int LEVEL;
    private static int IS_SOUND;

    static GameData()
    {
        LEVEL= PlayerPrefs.GetInt("level", 0);
        THEME=PlayerPrefs.GetInt("theme", 0);
        IS_SOUND = PlayerPrefs.GetInt("sound", 1);
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
        set { PlayerPrefs.SetInt("theme", (IS_SOUND = value)); }
    }

}
