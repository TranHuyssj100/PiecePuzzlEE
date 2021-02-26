using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using System.IO;

public class AnswerBuilder : MonoBehaviour
{
    public ThemeName theme;
    public int level;
    public int size;
    [Space()]
    //public int level;
    public GameObject fullSpite;
    public GameObject allPieces;
    public List<Object> listTexture;
    public List<GameObject> listSample;


    string basePath = Application.streamingAssetsPath + "/Json/Answers";
    string jsonSuffix = ".json";



    private void Start()
    {
        Intial();
        //CreateJson();

    }

    public void Intial()
    {
        Clear();
        loadAnswer();
        listTexture = LoadTextureFromLevel(level, theme, size);
        LoadPreview();
        if(listSample.Count>0)
            SpawnPiece(listTexture.Count);
    }
    
    public List<Object> LoadTextureFromLevel(int _level, ThemeName _themeType, int _sizeLevel)
    {
        string _path = "Themes/" + _themeType.ToString() + "/" + _sizeLevel.ToString() + "x" + _sizeLevel.ToString() + "/" + _level.ToString();
        Debug.Log(_path);
        Object[] _textures = Resources.LoadAll(_path, typeof(Texture2D));
        return _textures.ToList();
    }
    public void LoadPreview()
    {
        string _path = "Themes/" + theme.ToString() + "/" + size.ToString() + "x" + size.ToString() + "/" + level.ToString() + "/full";
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


    public void CreateJson(bool _isOverride)
    {
        string _path = Path.Combine(basePath, size.ToString() + "x" + size.ToString());
        List<int> _finalAnswers = new List<int>();
        SampleAnswer _answer = new SampleAnswer();
        string _strData = "[";

        Debug.LogError(_path);

        _answer.index = level;
        _answer.pieceNames = new int[listSample.Count];
        //_answer.answers = new int[listSample.Count * 3];
        for (int i = 0; i < listSample.Count; i++)
        {
            _answer.pieceNames[i] = System.Int32.Parse(listSample[i].name);
            _finalAnswers.Add(i);
            _finalAnswers.Add(Mathf.RoundToInt(allPieces.transform.GetChild(i).position.x));
            _finalAnswers.Add(Mathf.RoundToInt(allPieces.transform.GetChild(i).position.y));
        }
        _answer.answers = _finalAnswers.ToArray();
        _strData += "\n" + JsonUtility.ToJson(_answer, true);
        _strData += "]";
        Debug.Log(_strData);
        //Directory.CreateDirectory(_path);
        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(basePath);
            Debug.Log("<color=green>Create Directory</color>");

        }

        if (!_isOverride)
        {
            if (!File.Exists(_path + "/" + level + jsonSuffix))
            {
                File.WriteAllText(_path + "/" + level + jsonSuffix, _strData);
                Debug.Log("<color=green>: CREATE file complete </color>" + level);
            }
            else
            {
                Debug.Log("<color=red>Exis file: </color>" + size.ToString() + "x" + size.ToString() + "/" + level + ":Try to UPDATE");
            }
        }
        else
        {
            File.WriteAllText(_path + "/" + level + jsonSuffix, _strData);
            Debug.Log("<color=green>: UPDATE file complete </color>" + level);
        }
    }


    #region CHECK_ANSWER
    public List<Vector3> listAnswerForSample;
    SampleAnswer checkSampleAnswer = new SampleAnswer();

    public void SetCorrectPiecePos(GameObject _pieceObj,int _id , float _duration)
    {
        PieceBuilder _piece = _pieceObj.GetComponent<PieceBuilder>();
        Vector3 _correctPos = listAnswerForSample.Find(a => a.x == _id);
        if (_correctPos != null)
        {
            _piece.AutoCorrectPiece(new Vector2(_correctPos.y, _correctPos.z), _duration);
        }
    }

    public List<Vector3> CreateAnswerForSample(Queue<int> _answers)
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

    public SampleAnswer LoadSampleAnswer()
    {
        string _loadString="";
        string _path = Path.Combine(basePath, size.ToString() + "x" + size.ToString() + "/" + level.ToString() + jsonSuffix);
        if (File.Exists(_path))
        {
            _loadString = File.ReadAllText(_path);
            return JsonHelper.FromJson<SampleAnswer>(_loadString)[0];
        }
        return null;   
    }
    
    public void loadAnswer()
    {
        checkSampleAnswer = LoadSampleAnswer();
        if(checkSampleAnswer!=null)
        {
            listAnswerForSample = CreateAnswerForSample(new Queue<int>(checkSampleAnswer.answers));
            listSample = LoadSample(checkSampleAnswer.pieceNames);
        }
        else
        {
            Debug.LogError("Not Exist json");
            return;
        }
    }
    public void CheckAnswer()
    {
        loadAnswer();
        
        for (int i=0; i<allPieces.transform.childCount; i++)
        {
            SetCorrectPiecePos(allPieces.transform.GetChild(i).gameObject, i,0.5f);
        }
        //listAnswerForSample.Clear();
    }
    
    public void Clear()
    {
        listAnswerForSample.Clear();
        EventManager.TriggerEvent("DestroyPiece");
       
    }
    #endregion
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