using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;

public class DataController : SingletonDontDestroyMonoBehavior<DataController>
{

    //private static string SAVEPIECE;
    private static string SAVETHEME;
    private static string SAVELEVEL;
    private static string LEVEL = "Level";
    private static string SAMPLE = "Sample";
    private static string JsonSuffix = ".json";

    //public TextMeshProUGUI txtDebug;
    public static ThemeData[] themeData;

    private void Awake()
    {
        base.Awake();
#if UNITY_EDITOR
        //SAVEPIECE = Application.streamingAssetsPath +"/Json/Answers";
        SAVETHEME = Application.streamingAssetsPath + "/Json/Themes";
        SAVELEVEL = Application.streamingAssetsPath + "/Json/Levels";
#elif UNITY_ANDROID
        //SAVEPIECE = "jar:file://" + Application.dataPath + "!assets/";
        SAVETHEME =  "jar:file://" + Application.dataPath + "!assets/";
        SAVELEVEL =  "jar:file://" + Application.dataPath + "!assets/";
#elif UNITY_IOS
        SAVETHEME =  Application.dataPath + "/Raw/Json/Themes";
        SAVELEVEL =  Application.dataPath + "/Raw/Levels";
#endif
        //themeData = LoadThemeData(GameData.Theme);     
        //LoadAllThemeData();
        //GameData.CreateStatusTheme();
        //GameData.CreateCurrentLevelforEachTheme();
        //GameData.level = 0;
        //for (int i = 0; i < themeData.Length; i++)
        //{
        //    if (GameData.GetThemeStatus(i) == 1)
        //        GameData.level += GameData.GetCurrentLevelByTheme(i) + 1;
        //}
        LoadData();
    }



    public static void LoadData()
    {
        LoadAllThemeData();
        GameData.CreateStatusTheme();
        GameData.CreateCurrentLevelforEachTheme();
        GameData.level = 0;
        for (int i = 0; i < themeData.Length; i++)
        {
            if (GameData.GetThemeStatus(i) == 1)
                GameData.level += GameData.GetCurrentLevelByTheme(i) + 1;
        }
    }
    
    public static void LoadAllThemeData()
    {
        string loadString ;
#if UNITY_EDITOR || UNITY_IOS
        //Debug.LogError(Path.Combine(Path.Combine(SAVETHEME, "Themes" + JsonSuffix)));
        loadString = File.ReadAllText(Path.Combine(SAVETHEME, "Themes"+ JsonSuffix));
#elif UNITY_ANDROID
        WWW reader = new WWW(Path.Combine(SAVETHEME, "Json/Themes/" +"Themes"  + JsonSuffix));
        while (!reader.isDone) { }
        loadString = reader.text;
#endif
        //return JsonHelper.FromJson<ThemeData>(loadString);
        themeData= JsonHelper.FromJson<ThemeData>(loadString);
    }

//    public static LevelData LoadLevelData(int idTheme, int idLevel)
//    {
//        string loadString;
//#if UNITY_EDITOR || UNITY_IOS
//        loadString = File.ReadAllText(Path.Combine(SAVELEVEL, themeData[idTheme].name  + "/"+ idLevel + JsonSuffix));
//        //Debug.LogError((Path.Combine(SAVELEVEL, themeData[idTheme].name + "/" + idLevel + JsonSuffix)));
//#elif UNITY_ANDROID
//        WWW reader = new WWW(Path.Combine(SAVELEVEL, "Json/Levels/"+ themeData[idTheme].name + "/" + idLevel + JsonSuffix));
//        while (!reader.isDone) { }
//        loadString = reader.text;
//#endif
//        return JsonUtility.FromJson<LevelData>(loadString);
//    }
    
   public static List<GameObject> LoadPiece(int idTheme, int idLevel)
    {
        string loadString;
        loadString ="Themes/"+ themeData[idTheme].name.Replace(" ", "") + "/" + idLevel.ToString();
        //Debug.LogError(loadString);
        GameObject[] _result = Resources.LoadAll<GameObject>(loadString);
        //Debug.LogError(_result.Length);
        return _result.ToList();
    }

    public static Sprite LoadSpritePreview(int _idTheme, int _idLevel, int _sizeLevel)
    {
        string _path = "Themes/" +themeData[_idTheme].name.Replace(" ", "") + "/Full/" +_idLevel.ToString();
        Sprite[] _sprite = Resources.LoadAll<Sprite>(_path);
        //Debug.Log(_path);
        //Sprite _sprite = Resources.Load<Sprite>(_path);
        return _sprite[_sizeLevel*_sizeLevel];
    }


#if UNITY_EDITOR
    static string AnswerPresetPath = "Assets/_Project/Resources/AnswerPreset/";
    public static void SaveAnswerPreset(List<ImageCutter.AnswerPreset> _answerPreset,int _size)
    {
        string _strData;
        string size;
        size = _size + "x" + _size;
        _strData = "[";
        for (int i = 0; i< _answerPreset.Count;i++)
        {
            ImageCutter.AnswerPreset block = new ImageCutter.AnswerPreset
            {
                gridIndex = _answerPreset[i].gridIndex,
                blockIndex = _answerPreset[i].blockIndex
            };
            _strData += (i!=0?"\n":"") + JsonUtility.ToJson(block, true);
            if (i == _answerPreset.Count - 1)
                break;
            _strData += ",";
        }
        _strData += "]";
        Debug.Log(_strData);
        if (!Directory.Exists(AnswerPresetPath + size))
        {
            Directory.CreateDirectory(AnswerPresetPath + size);
        }
        //Debug.Log(AnswerPresetPath + size);
        AssetDatabase.Refresh();
        //File.WriteAllText(Path.Combine(AnswerPresetPath + size, Directory.GetFiles(AnswerPresetPath + size).Length / 2 + JsonSuffix), _strData);
        string fileName = GenerateRandomFileName();
        File.WriteAllText(Path.Combine(AnswerPresetPath + size, fileName + JsonSuffix), _strData);
        Debug.Log("<color=green>Saved successful to: </color>" + AnswerPresetPath + size +"/" + fileName + JsonSuffix);
    }
    private static string GenerateRandomFileName()
    {
        const string glyphs = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
        string fileName = "";
        for (int i = 0; i < 16; i++)
        {
            fileName += glyphs[Random.Range(0, glyphs.Length)];
        }
        return fileName;
    }
    public static List<ImageCutter.AnswerPreset> ReadAnswerPreset(int _size)
    {
        string loadString;
        string size;
        size = _size + "x" + _size;
        DirectoryInfo directoryInfo = new DirectoryInfo(AnswerPresetPath + size);
        FileInfo[] fileInfo = directoryInfo.GetFiles("*.json");
        List<ImageCutter.AnswerPreset> answerPreset = new List<ImageCutter.AnswerPreset>();
        //int randomPreset = Random.Range(0, Directory.GetFiles(AnswerPresetPath + size).Length / 2);
        //loadString = File.ReadAllText(Path.Combine(AnswerPresetPath + size, randomPreset + JsonSuffix));
        int randomPreset = Random.Range(0, fileInfo.Length);
        loadString = File.ReadAllText(Path.Combine(AnswerPresetPath + size, fileInfo[randomPreset].Name));
        foreach(ImageCutter.AnswerPreset answer in JsonHelper.FromJson<ImageCutter.AnswerPreset>(loadString))
        {
            answerPreset.Add(answer);
        }
        Debug.Log("<color=green>Cut successful from preset: </color>" + fileInfo[randomPreset].Name);
        return answerPreset;
    }
    //public static void RenameFile()
    //{
    //    string fileName = "";
    //    string size;
    //    for (int i = 5; i < 9; i++)
    //    {
    //        size = i + "x" + i;
    //        DirectoryInfo directoryInfo = new DirectoryInfo(AnswerPresetPath + size);
    //        FileInfo[] fileInfo = directoryInfo.GetFiles("*.json");
    //        foreach (FileInfo file in fileInfo)
    //        {
    //            Debug.Log("<color=red>From: </color>" + file.FullName);
    //            File.Move(file.FullName, Path.Combine(AnswerPresetPath + size, GenerateRandomFileName() + JsonSuffix));
    //            Debug.Log("<color=Green>To: </color>" + file.FullName);
    //        }
    //    }
    //}    
#endif

    //public static int GetThemeLevelCount(ThemeName themeName,int size)
    //{
    //    //DirectoryInfo dir = new DirectoryInfo("Assets/_Project/Resources/Themes/" + themeName.ToString() + "/" + +size + "x" + size);
    //    Resources.Load("Themes/ " + themeName.ToString() + " / " + +size + "x" + size).
    //    return Mathf.RoundToInt(dir.GetFiles().Length);
    //}





    #region JSONHELPER
    //----------------------------JsonHelp---------DONT TOUCH------------------
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string jsonArray)
        {
            jsonArray = WrapArray(jsonArray);
            return FromJsonWrapped<T>(jsonArray);
        }

        public static T[] FromJsonWrapped<T>(string jsonObject)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(jsonObject);
            return wrapper.items;
        }

        private static string WrapArray(string jsonArray)
        {
            return "{ \"items\": " + jsonArray + "}";
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }
#endregion
}

[System.Serializable]
public class ThemeData
{
    public int idTheme;
    public string name;
    public int size;
    public int price;
    public int levelCount;
   
}

[System.Serializable]
public class LevelData
{
    public int idLevel;
    public int idTheme;
    public int pieceDefault;
}

//[System.Serializable]
//public class SampleAnswer
//{
//    public int idAnswer;
//    public int [] pieceNames;
//    public int [] answers;
//}


