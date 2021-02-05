using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildLevel : MonoBehaviour
{
    public Transform[] points;
    public List<Object> listTexture = new List<Object>();
    public List<Object> listSamples = new List<Object>();
    public List<Vector3> listAnswerForSample = new List<Vector3>();
    public Stack<int> randIndexPiece;
    public Stack<int> deleIndexPiece;
    [Space(10)]
    [Header("Data")]
    public SampleAnswer curSampleAnswer = new SampleAnswer();
    public ThemeData curThemeData = new ThemeData();


    public static BuildLevel instance;

    LevelData curLevelData;
    int numPiecesWrong;
    int numMove;

    //public int NUM_PIECES_WORNG
    //{
    //    get { return numPiecesWrong; }
    //    set { numPiecesWrong = value; }
    //}
    //public int NUM_MOVE
    //{
    //    get { return numMove; }
    //    set { numMove = value; }
    //}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
 
    }

    void Update()
    {
    }

    public List<Object> LoadTextureFromLevel(int _level, ThemeType _themeType)
    {
        string _path = _themeType.ToString() + "/" + _level.ToString();
        //Debug.Log(_path);
        Object[] _textures = Resources.LoadAll(_path, typeof(Texture2D));
        return _textures.ToList();
    }


    public List<Object> LoadSample(int[] _samples)
    {
        string _path = "Samples/";
        int _id = 0;
        List<Object> _prefabs = new List<Object>();
        foreach (int _child in _samples)
        {
            var _prefab = Resources.Load<Object>(_path + _child.ToString());
            GameObject _prefabGameObj = _prefab as GameObject;
            _prefabGameObj.GetComponent<Piece>().id = _id++;
            _prefabs.Add(_prefab);
        }
        return _prefabs;
    }

    public GameObject CreatePiece(Object _textureObj, Object _sampleObj, Vector3 _pointSpawn)
    {
        Texture2D _texture = _textureObj as Texture2D;
        Sprite _sprite = Sprite.Create(_texture, new Rect(0f, 0f, _texture.width, _texture.height), new Vector2(0, 0), 100f);
        GameObject _spriteObject = new GameObject(_texture.name);
        GameObject _sampleClone = GameObject.Instantiate(_sampleObj as GameObject, _pointSpawn, Quaternion.identity);
        _spriteObject.AddComponent<SpriteRenderer>().sprite = _sprite;
        _spriteObject.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Piece");
        _spriteObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        _spriteObject.transform.parent = _sampleClone.transform;
        _sampleClone.GetComponent<Piece>().sizeSprite = _spriteObject.GetComponent<SpriteRenderer>().size;
        //_sampleClone.GetComponent<Piece>().sizeSprite = _spriteObject.Ge
        _spriteObject.transform.localPosition = Vector3.zero;
        return _sampleClone;
    }

    public void SpawnRadomPieces(Vector3 _pointSpawn)
    {
        if (randIndexPiece.Count > 0)
        {
            int _randIndex = randIndexPiece.Pop();
            CreatePiece(listTexture[_randIndex], listSamples[_randIndex], _pointSpawn);
        }
    }

    //public void InitializeGame()
    //{
    //    curThemeData = DataController.LoadThemeData(GameData.Theme);
    //    if (GameData.level < curThemeData.groupLevel.Length)
    //    {
    //        curLevelData = curThemeData.groupLevel[GameData.level];
    //    }
    //    else
    //    {
    //        curLevelData = curThemeData.groupLevel[curThemeData.groupLevel.Length - 1];
    //    }
    //    curSampleAnswer = DataController.LoadSampleAnswer(curLevelData.sampleIndex);
    //    listTexture = LoadTextureFromLevel(curLevelData.index, curThemeData.theme);
    //    //listSamples = LoadSample(curLevelData.sampleIndex);
    //    listSamples = LoadSample(curSampleAnswer.pieceNames);
    //    randIndexPiece = RandomStackInt(0, listSamples.Count);
    //    for (int i = 0; i < 3; i++)
    //    {
    //        SpawnRadomPieces(points[i].position);
    //    }
    //    numMove = 10;
    //    numPiecesWrong = listSamples.Count;
    //}

    public Stack<int> RandomStackInt(int _min, int _max)
    {
        Stack<int> _randIntStack = new Stack<int>();
        while (_randIntStack.Count < _max)
        {
            int _randInt = Random.Range(_min, _max);
            if (!_randIntStack.Contains(_randInt))
            {
                //Debug.Log(_randInt);
                _randIntStack.Push(_randInt);
            }
        }
        return _randIntStack;
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


}
