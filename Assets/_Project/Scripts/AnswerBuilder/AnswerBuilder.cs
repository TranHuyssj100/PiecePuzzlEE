using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public class AnswerBuilder : MonoBehaviour
{
   
    public string nameTheme;
    public int idTheme; 
    public int idLevel;
    public int size;
    [Space()]
    public int nameAnswerFile;
    public GameObject fullSpite;
    public GameObject allPieces;
    public List<Object> listTexture;
    public List<GameObject> listSample;


    string answerPath = Application.streamingAssetsPath + "/Json/Answers";
    string levelPath = Application.streamingAssetsPath + "/Json/Levels";
    string themePath = Application.streamingAssetsPath + "/Json/Themes";
    string jsonSuffix = ".json";

  



    private void Start()
    {
        Intial();
        //CreateJson();

    }

    public void Intial()
    {
        listAnswerForSample.Clear();
        EventManager.TriggerEvent("DestroyPiece");

        //CreateAnswer();
        listTexture = LoadTextureFromLevel(idLevel, nameTheme, size);
        LoadPreview();
        if(listSample.Count>0) SpawnPiece(listTexture.Count);
        if (listTexture.Count <= 0) Debug.LogError("Texture is not Exsit!");

    }

    public List<Object> LoadTextureFromLevel(int _level, string _nameTheme, int _sizeLevel)
    {
        string _path = "Themes/" +_nameTheme + "/" + _sizeLevel.ToString() + "x" + _sizeLevel.ToString() + "/" + _level.ToString();
        Debug.Log(_path);
        Object[] _textures = Resources.LoadAll(_path, typeof(Texture2D));
        return _textures.ToList();
    }
    public void LoadPreview()
    {
        string _path = "Themes/" + nameTheme.ToString() + "/" + size.ToString() + "x" + size.ToString() + "/" + idLevel.ToString() + "/full";
        //Debug.Log(_path);
        Sprite _sprite = Resources.Load<Sprite>(_path);
        //fullSpite.GetComponent<SpriteRenderer>().sprite = listTexture[listTexture.Count - 1] as Sprite;
        fullSpite.GetComponent<SpriteRenderer>().sprite = _sprite;
    }

    public List<GameObject> LoadSample(int[] _samples)
    {
        string _path = "Samples/";
        List<GameObject> _prefabs = new List<GameObject>();
        foreach (int _child in _samples)
        {
            var _prefab = Resources.Load<Object>(_path + _child.ToString());
            GameObject _prefabGameObj = _prefab as GameObject;
            _prefabs.Add(_prefabGameObj);
        }
        return _prefabs;
    }

    public GameObject CreatePiece(Object _textureObj, GameObject _sampleObj, Vector3 _pointSpawn)
    {
        Texture2D _texture = _textureObj as Texture2D;
        Sprite _sprite = Sprite.Create(_texture, new Rect(0f, 0f, _texture.width, _texture.height), new Vector2(0, 0), 100f);
        GameObject _spriteObject = new GameObject(_texture.name);
        GameObject _sampleClone = GameObject.Instantiate(_sampleObj as GameObject, new Vector3(_pointSpawn.x, _pointSpawn.y, 0), Quaternion.identity);
        _sampleClone.transform.parent = allPieces.transform;
        _spriteObject.AddComponent<SpriteRenderer>().sprite = _sprite;
        _spriteObject.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Piece");
        _spriteObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        _spriteObject.transform.parent = _sampleClone.transform;
        Destroy(_sampleClone.GetComponent<Piece>());
        Destroy(_sampleClone.GetComponentInChildren<TriggerPiece>());
        _sampleClone.AddComponent<PieceBuilder>();
        _spriteObject.transform.localPosition = Vector3.zero;
        return _sampleClone;
    }

    public void SpawnPiece(int _numPiece)
    {

        for (int i = 0; i < _numPiece - 1; i++)
        {
            CreatePiece(listTexture[i], listSample[i], transform.position);
        }
    }

    #region CREATE ANSWER JSON

    //public void CreateJson(bool _isOverride)
    //{
    //    string _path = Path.Combine(answerPath, size.ToString() + "x" + size.ToString());
    //    List<int> _finalAnswers = new List<int>();
    //    SampleAnswer _answer = new SampleAnswer();
    //    string _strData = "[";

    //    //Debug.LogError(_path);

    //    _answer.idAnswer = nameAnswerFile;
    //    _answer.pieceNames = new int[listSample.Count];
    //    //_answer.answers = new int[listSample.Count * 3];
    //    for (int i = 0; i < listSample.Count; i++)
    //    {
    //        _answer.pieceNames[i] = System.Int32.Parse(listSample[i].name);
    //        _finalAnswers.Add(i);
    //        _finalAnswers.Add(Mathf.RoundToInt(allPieces.transform.GetChild(i).position.x));
    //        _finalAnswers.Add(Mathf.RoundToInt(allPieces.transform.GetChild(i).position.y));
    //    }
    //    _answer.answers = _finalAnswers.ToArray();
    //    _strData += "\n" + JsonUtility.ToJson(_answer, true);
    //    _strData += "]";
    //    //Debug.Log(_strData);
    //    //Directory.CreateDirectory(_path);
    //    if (!Directory.Exists(_path))
    //    {
    //        Directory.CreateDirectory(answerPath);
    //        Debug.Log("<color=green>Create Directory</color>");

    //    }

    //    if (!_isOverride)
    //    {
    //        if (!File.Exists(_path + "/" + nameAnswerFile + jsonSuffix))
    //        {
    //            File.WriteAllText(_path + "/" + nameAnswerFile + jsonSuffix, _strData);
    //            Debug.Log("<color=green>: CREATE file complete </color>" + nameAnswerFile);
    //            CreateLevelJson(idLevel, idTheme, nameAnswerFile);
    //        }
    //        else
    //        {
    //            Debug.Log("<color=red>Exis file: </color>" + size.ToString() + "x" + size.ToString() + "/" + nameAnswerFile + ":Try to UPDATE");
    //        }
    //    }
    //    else
    //    {
    //        File.WriteAllText(_path + "/" + nameAnswerFile + jsonSuffix, _strData);
    //        CreateLevelJson(idLevel, idTheme, nameAnswerFile);
    //        Debug.Log("<color=green>: UPDATE file complete </color>" + nameAnswerFile);
    //    }
    //}
    #endregion


    #region CHECK_ANSWER
    public List<Vector3> listAnswerForSample;
    //SampleAnswer checkSampleAnswer = new SampleAnswer();
    LevelData levelData = new LevelData();

    public void SetCorrectPiecePos(GameObject _pieceObj,int _id , float _duration)
    {
        PieceBuilder _piece = _pieceObj.GetComponent<PieceBuilder>();
        Vector3 _correctPos = listAnswerForSample.Find(a => a.x == _id);
        if (_correctPos != null)
        {
            _piece.AutoCorrectPiece(new Vector2(_correctPos.y, _correctPos.z), _duration);
        }
    }

    public List<Vector3> UpdateListAnswerforSample(Queue<int> _answers)
    {
        List<Vector3> _listAnswerforSample = new List<Vector3>();
        List<int> _temp = new List<int>(3);

        while (_answers.Count > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                _temp.Add(_answers.Dequeue());
            }
            _listAnswerforSample.Add(new Vector3(_temp[0], _temp[1], _temp[2]));
            _temp.Clear();
        }
        return _listAnswerforSample;
    }

    //public SampleAnswer LoadAnswerJson(int _jsonName)
    //{
    //    string _loadString="";
    //    string _path = Path.Combine(answerPath, size.ToString() + "x" + size.ToString() + "/" + _jsonName.ToString() + jsonSuffix);
    //    if (File.Exists(_path))
    //    {
    //        _loadString = File.ReadAllText(_path);
    //        return JsonHelper.FromJson<SampleAnswer>(_loadString)[0];
    //    }
    //    return null;   
    //}
    
    //public void CreateAnswer()
    //{
    //    levelData = LoadLevelJson(idLevel) ;
    //    //Debug.LogError(levelData);
    //    if(levelData!=null)
    //    {
    //        nameAnswerFile = levelData.sampleIndex;
    //    }

    //    checkSampleAnswer = LoadAnswerJson(nameAnswerFile);
    //    if(checkSampleAnswer!=null)
    //    {
    //        listAnswerForSample = UpdateListAnswerforSample(new Queue<int>(checkSampleAnswer.answers));
    //        listSample = LoadSample(checkSampleAnswer.pieceNames);
    //    }
    //    else
    //    {
    //        Debug.LogError("Not Exist json");
    //        return;
    //    }
    //}
    //public void CheckAnswer()
    //{
    //    CreateAnswer();
        
    //    for (int i=0; i<allPieces.transform.childCount; i++)
    //    {
    //        SetCorrectPiecePos(allPieces.transform.GetChild(i).gameObject, i,0.5f);
    //    }
    //    //listAnswerForSample.Clear();
    //}
    
    #endregion

    public LevelData LoadLevelJson(int _idLevel)
    {
        string _loadString = "";
        string _path = Path.Combine(levelPath, nameTheme + "/" + _idLevel.ToString() + jsonSuffix);
        //Debug.LogError(_path);
        if (File.Exists(_path))
        {
            _loadString = File.ReadAllText(_path);
            return JsonUtility.FromJson<LevelData>(_loadString) ;
        }
        return null;
    }
   
    public void CreateLevelJson(int _idLevel, int _idTheme, int _sampleIndex)
    {
        string _data = "";
        string _path = Path.Combine(levelPath, nameTheme + "/" + _idLevel.ToString() + jsonSuffix);
        LevelData _levelData = new LevelData();
        _levelData.idLevel = _idLevel;
        _levelData.idTheme = _idTheme;
        //_levelData.sampleIndex = _sampleIndex;

       _data= JsonUtility.ToJson(_levelData,true);

        if(!Directory.Exists(Path.Combine(levelPath, nameTheme)))
        {
            Directory.CreateDirectory(Path.Combine(levelPath, nameTheme));    
        }
       
        if (!File.Exists(_path))
        {
            File.WriteAllText(_path, _data);
            Debug.Log("<color=blue> Create level json success: </color>" + _idLevel);
        }
        else if (CheckThemeExsit(idTheme))
        {
            Debug.LogError("idTheme of level is duplicate, Check and try again");
        }
        else
        {
            Debug.Log("<color=red> Create level json failed, file is Exsit </color>");

        }
    }

    public bool CheckThemeExsit(int _idTheme)
    {
        string _path = Path.Combine(themePath,"Themes" +jsonSuffix);
        string _data = "";
        ThemeData[] _allThemeData;
        if (File.Exists(_path))
        {
            _data= File.ReadAllText(_path);
            _allThemeData = JsonHelper.FromJson<ThemeData>(_data);
            if (_allThemeData.ToList().Find(x => x.idTheme == _idTheme) != null)
                return true;
        }
        return false;
      
    }

    public void AutoFillNamejsonFile()
    {
        string _path = Path.Combine(answerPath, size.ToString() + "x" + size.ToString());
        //int _amountFile = 0;
        if (Directory.Exists(_path))
        {
           nameAnswerFile= Mathf.RoundToInt(Directory.GetFiles(_path).Length/2);   
        }
    } 
    public void AutoIdTheme()
    {
        string _path = Path.Combine(themePath, "Themes" + jsonSuffix);
        string _data = "";
        ThemeData[] _allThemeData;
        if (File.Exists(_path))
        {
            _data = File.ReadAllText(_path);
            _allThemeData = JsonHelper.FromJson<ThemeData>(_data);
            for (int i = 0; i< _allThemeData.Length; i++)
            {
                if(_allThemeData[i].name== nameTheme)
                {
                    idTheme = _allThemeData[i].idTheme; 
                }
            }
                
        }
    }

    public void Clear()
    {
        listSample.Clear();
        listAnswerForSample.Clear();
        EventManager.TriggerEvent("DestroyPiece");
        AutoFillNamejsonFile();
        AutoIdTheme();
    }
}

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