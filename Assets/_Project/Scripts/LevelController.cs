using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public int level = 0;
    public int indexSample = 0;
    public ThemeType type;
    public Transform[] points;
    public List<Object> listTexture = new List<Object>();
    public List<Object> listSamples = new List<Object>();
    public Stack<int> randIndexPiece;
    public Stack<int> deleIndexPiece;
   
    public static int indexSpawn= 3;
    public static LevelController instance;


    private void Awake()
    {
        if (instance != null)
        {
            instance = this;
        }
    }

    void Start()
    {

       listTexture= LoadTextureFromLevel(level, type);
       listSamples = LoadSample(indexSample);
       randIndexPiece = RandomStackInt(0, listSamples.Count);
       CreateRadomPiece();

    }

    void Update()
    {
       if(indexSpawn==0)
        CreateRadomPiece();
        
    }

   public List<Object> LoadTextureFromLevel(int _level, ThemeType _themeType)
   {
        string _path = _themeType.ToString() + "/" + "Pieces" + "/" + _level.ToString();
        Debug.Log(_path);
        Object[] _textures= Resources.LoadAll(_path, typeof(Texture2D));
        return _textures.ToList();
   }
    public List<Object> LoadSample(int _indexSample)
   {
        string _path = "Samples" + "/" + _indexSample.ToString();
        Debug.Log(_path);
        Object[] _prefabs= Resources.LoadAll(_path);
        return _prefabs.ToList();
   }
    
    public GameObject CreatePiece(Object _textureObj, Object _sampleObj, Transform _pointSpawn)
    {
        Texture2D _texture = _textureObj as Texture2D;
        Sprite _sprite = Sprite.Create(_texture, new Rect(0f,0f,_texture.width, _texture.height), new Vector2(0,0), 100f);
        GameObject _spriteObject = new GameObject(_texture.name);  
        GameObject _sampleClone = GameObject.Instantiate(_sampleObj as GameObject, _pointSpawn.position, Quaternion.identity);
        _spriteObject.AddComponent<SpriteRenderer>().sprite = _sprite;
        _spriteObject.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Piece");
        _spriteObject.transform.parent = _sampleClone.transform;
        _sampleClone.GetComponent<Piece>().sizeSprite = _spriteObject.GetComponent<SpriteRenderer>().size;
        _spriteObject.transform.localPosition = Vector3.zero;
        return _sampleClone ;
    }

    void CreateRadomPiece()
    {
        indexSpawn = 3;
        for (int i = 0; i < 3; i++)
        {
            if (randIndexPiece.Count > 0)
            {
                int _randIndex = randIndexPiece.Pop();
                CreatePiece(listTexture[_randIndex], listSamples[_randIndex], points[i]);
            }
            else break;
        }
    }

    


    public Stack<int> RandomStackInt( int _min, int _max)
    {
        Stack<int> _randIntStack = new Stack<int>();
        while (_randIntStack.Count < _max)
        {
            int _randInt = Random.Range(_min, _max);
            if (!_randIntStack.Contains(_randInt))
            {
                Debug.Log(_randInt);
                _randIntStack.Push(_randInt);
            }
        }
        return _randIntStack;
    }
   
}

public enum ThemeType
{
   Animal,
}
