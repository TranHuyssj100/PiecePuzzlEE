using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Globalization;
using UnityEngine.Purchasing.MiniJSON;

public class DataController : SingletonDontDestroyMonoBehavior<DataController>
{

    private static string SAVESAMPLE;
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
        SAVESAMPLE = Application.streamingAssetsPath +"/Json/Answers";
        SAVETHEME = Application.streamingAssetsPath + "/Json/Themes";
        SAVELEVEL = Application.streamingAssetsPath + "/Json/Levels";
#elif UNITY_ANDROID
        SAVESAMPLE = "jar:file://" + Application.dataPath + "!assets/";
        SAVETHEME =  "jar:file://" + Application.dataPath + "!assets/";
        SAVELEVEL =  "jar:file://" + Application.dataPath + "!assets/";
#endif
        //themeData = LoadThemeData(GameData.Theme);     
        LoadAllThemeData();
        GameData.CreateStatusTheme();
        GameData.CreateCurrentLevelforEachTheme();
    }



    private void Update()
    {

    }


//    public static ThemeData LoadThemeData(int _type)
//    {
//        string loadString ;
//#if UNITY_EDITOR
//        loadString = File.ReadAllText(Path.Combine(SAVETHEME, ((ThemeName)_type).ToString() + JsonSuffix));
//        //Debug.LogError(Path.Combine(SAVETHEME, ((ThemeType)_type).ToString() + JsonSuffix));
//#elif UNITY_ANDROID
//        WWW reader = new WWW(Path.Combine(SAVETHEME, "Json/Themes/" + ((ThemeName)_type).ToString() + JsonSuffix));
//        while (!reader.isDone) { }
//        loadString = reader.text;
//#endif
//        return JsonHelper.FromJson<ThemeData>(loadString)[0];
//    } 
    
    public static void LoadAllThemeData()
    {
        string loadString ;
#if UNITY_EDITOR
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

    public static LevelData LoadLevelData(int idTheme, int idLevel)
    {
        string loadString;
#if UNITY_EDITOR
        loadString = File.ReadAllText(Path.Combine(SAVELEVEL, themeData[idTheme].name  + "/"+ idLevel + JsonSuffix));
        //Debug.LogError((Path.Combine(SAVELEVEL, themeData[idTheme].name + "/" + idLevel + JsonSuffix)));
#elif UNITY_ANDROID
        WWW reader = new WWW(Path.Combine(SAVELEVEL, "Json/Levels/"+ themeData[idTheme].name + "/" + idLevel + JsonSuffix));
        while (!reader.isDone) { }
        loadString = reader.text;
#endif
        return JsonUtility.FromJson<LevelData>(loadString);
    }




    public static SampleAnswer LoadSampleAnswer(int _indexSample, int _sizeLevel)
    {
        string loadString;
#if UNITY_EDITOR
        loadString = File.ReadAllText(Path.Combine(SAVESAMPLE,_sizeLevel.ToString()+"x"+ _sizeLevel.ToString()+"/"+ _indexSample.ToString() + JsonSuffix));
        //Debug.LogError(Path.Combine(SAVESAMPLE, _indexSample.ToString() + JsonSuffix));
#elif UNITY_ANDROID
        WWW reader = new WWW(Path.Combine(SAVESAMPLE, "Json/Answers/" +_sizeLevel.ToString()+"x"+ _sizeLevel.ToString()+"/"+ _indexSample.ToString()  + JsonSuffix));
        while (!reader.isDone) { }
        loadString = reader.text;
#endif
        return JsonHelper.FromJson<SampleAnswer>(loadString)[0];
    }

#if UNITY_EDITOR
    static string AnswerPresetPath = "Assets/_Project/Testing/AnswerPreset/";
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
        File.WriteAllText(Path.Combine(AnswerPresetPath + size, Directory.GetFiles(AnswerPresetPath + size).Length / 2 + JsonSuffix), _strData);
    }
    public static List<ImageCutter.AnswerPreset> ReadAnswerPreset(int _size)
    {
        string loadString;
        string size;
        size = _size + "x" + _size;
        List<ImageCutter.AnswerPreset> answerPreset = new List<ImageCutter.AnswerPreset>();
        int randomPreset = Random.Range(0, Directory.GetFiles(AnswerPresetPath + size).Length / 2);
        loadString = File.ReadAllText(Path.Combine(AnswerPresetPath + size, randomPreset + JsonSuffix));
        foreach(ImageCutter.AnswerPreset answer in JsonHelper.FromJson<ImageCutter.AnswerPreset>(loadString))
        {
            answerPreset.Add(answer);
        }
        return answerPreset;
    }
#endif

    //public static int GetThemeLevelCount(ThemeName themeName,int size)
    //{
    //    //DirectoryInfo dir = new DirectoryInfo("Assets/_Project/Resources/Themes/" + themeName.ToString() + "/" + +size + "x" + size);
    //    Resources.Load("Themes/ " + themeName.ToString() + " / " + +size + "x" + size).
    //    return Mathf.RoundToInt(dir.GetFiles().Length);
    //}

    //public static List<string> GetAllTheme()
    //{
    //    //Debug.LogError(SAVETHEME);
    //    DirectoryInfo dir = new DirectoryInfo(SAVETHEME);
    //    FileInfo[] allFile = dir.GetFiles("*.json");
    //    List<string> allFileName = new List<string>();
    //    for (int i = 0; i < allFile.Length; i++)
    //        allFileName.Add(Path.GetFileName(allFile[i].Name.Remove(allFile[i].Name.Length - 5, 5)));
    //    return allFileName;
    //    //return Mathf.RoundToInt(dir.GetFiles().Length/2);
    //}
    //void CreateDefaultLevelData()
    //{
    //    string _strData;
    //    _strData = "[";
    //    LevelData _levelData = new LevelData();
    //    _levelData.index = 0;
    //    _levelData.sampleIndex = 0;
    //    _levelData.theme = ThemeType.Animal;
    //    _strData += "\n" + JsonUtility.ToJson(_levelData, true);
    //    _strData += "]";
    //    Directory.CreateDirectory(SAVELEVEL);
    //    File.WriteAllText(Path.Combine(SAVELEVEL, "0" + JsonSuffix), _strData);
    //}

    //void CreateSampleAnswer(int _index, int _numPiece, int [] _answer)
    //{
    //    string _strData;
    //    _strData = "[";
    //    SampleAnswer _sample = new SampleAnswer();
    //    _sample.index = _index;
    //    //_sample.numPiece = _numPiece;
    //    _sample.answers = _answer;
    //    _strData += "\n" + JsonUtility.ToJson(_sample, true);
    //    _strData += "]";
    //    Directory.CreateDirectory(SAVESAMPLE);
    //    File.WriteAllText(Path.Combine(SAVESAMPLE, "0" + JsonSuffix), _strData);

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
    //public ThemeName theme;
    public int idTheme;
    public string name;
    public int size;
    public int price;
    public int levelCount;
   
    //public LevelData[] groupLevel;
}

[System.Serializable]
public class LevelData
{
    public int idLevel;
    public int idTheme;
    public int sampleIndex;
    public int pieceDefault;
}

[System.Serializable]
public class SampleAnswer
{
    public int idAnswer;
    public int [] pieceNames;
    public int [] answers;
}


