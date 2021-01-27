using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public int level = 0;
    public int indexSample = 0;
    public ThemeType type;
    public Transform[] points;
    public  List<Object> listTexture = new List<Object>();
    public  List<Object> listSamples = new List<Object>();
    public  List<GameObject> listPieces= new List<GameObject>();
   
    void Start()
    {
       listTexture= LoadTextureFromLevel(level, type);
       listSamples= LoadSample(indexSample);
     ;
    }

    void Update()
    {
        
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
    
    
    void CreateListiece()
    {
               
    }

    public GameObject CreatePiece(Object _textureObj, Object _sampleObj, Transform _pointSpawn)
    {
        Texture2D _texture = _textureObj as Texture2D;
        Sprite _sprite = Sprite.Create(_texture, new Rect(0f,0f,_texture.width, _texture.height), new Vector2(0.5f,0), 100f);
        GameObject _spriteObject = new GameObject(_texture.name);  
        GameObject _sampleClone = GameObject.Instantiate(_sampleObj as GameObject, _pointSpawn);

        _spriteObject.AddComponent<SpriteRenderer>().sprite = _sprite;
        _spriteObject.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Piece");
        _spriteObject.transform.parent = _sampleClone.transform;
        //_spriteObject.transform.localScale = Vector2.one * .5f;
        _spriteObject.transform.localPosition = Vector3.zero;
        return _sampleClone ;
    }
   
}

public enum ThemeType
{
   Animal,
}
