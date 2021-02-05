using UnityEngine;

public class GameData
{
    private static int THEME;
    private static int LEVEL;
    static GameData()
    {
        PlayerPrefs.GetInt("level", 0);
        PlayerPrefs.GetInt("theme", 0);
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
}
