using UnityEngine;

public static class GameData
{
    private static int THEME;
    private static int LEVEL;
    private static int IS_SOUND;
    private static int PREVIEW;
    private static int AUTO_CORRECT;

    static GameData()
    {
        LEVEL= PlayerPrefs.GetInt("level", 0);
        THEME=PlayerPrefs.GetInt("theme", 0);
        IS_SOUND = PlayerPrefs.GetInt("sound", 1);
        PREVIEW = PlayerPrefs.GetInt("preview", 0);
        AUTO_CORRECT = PlayerPrefs.GetInt("auto_correct", 0);
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

}
