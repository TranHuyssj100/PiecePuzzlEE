using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using TMPro.Examples;

public class AnswerBuilder : MonoBehaviour
{
    public ThemeType theme;
    public int level;
    public int size;

    [Space()]
    public int nameAnswer;
    public GameObject fullSpite;
    public List<Object> listTexture;
    public List<GameObject> listSample;
    



    private void Start()
    {
        listTexture= LoadTextureFromLevel(0, theme, size);
        SpawnPiece(listTexture.Count);


    }

    public List<Object> LoadTextureFromLevel(int _level, ThemeType _themeType, int _sizeLevel)
    {
        string _path = "Themes/" + _themeType.ToString() + "/" + _sizeLevel.ToString() + "x" + _sizeLevel.ToString() + "/" + _level.ToString();
        Debug.Log(_path);
        Object[] _textures = Resources.LoadAll(_path, typeof(Texture2D));
        return _textures.ToList();
    }

    

    public GameObject CreatePiece(Object _textureObj, GameObject _sampleObj, Vector3 _pointSpawn)
    {
        Texture2D _texture = _textureObj as Texture2D;
        Sprite _sprite = Sprite.Create(_texture, new Rect(0f, 0f, _texture.width, _texture.height), new Vector2(0, 0), 100f);
        GameObject _spriteObject = new GameObject(_texture.name);
        GameObject _sampleClone = GameObject.Instantiate(_sampleObj as GameObject, new Vector3(_pointSpawn.x, _pointSpawn.y, 0), Quaternion.identity);
        _spriteObject.AddComponent<SpriteRenderer>().sprite = _sprite;
        _spriteObject.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Piece");
        _spriteObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        _spriteObject.transform.parent = _sampleClone.transform;
        Destroy(_sampleClone.GetComponent<Piece>());
        _sampleClone.AddComponent<PieceBuilder>();
        //_sampleClone.GetComponent<Piece>().sizeSprite = _spriteObject.Ge
        _spriteObject.transform.localPosition = Vector3.zero;
        return _sampleClone;
    }

    public void SpawnPiece(int _numPiece)
    {
        for (int i = 0; i < _numPiece-1; i++)
        {
            CreatePiece(listTexture[i], listSample[i], transform.position);
        }
    }

    public void loadPreview()
    {

    }
   
}
