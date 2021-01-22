
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public int blockPerLine=5;
    //public GameObject block;
    public Texture2D image;
    void Start()
    {
        CreatePuzzle();
    }

    void Update()
    {
        
    }


    void CreatePuzzle()
    {
        Texture2D[,] _imageSlice = ImageSlice.GetSlice(image, blockPerLine);
        for(int x=0; x<blockPerLine; x++)
        {
            for(int y=0; y<blockPerLine; y++)
            {
                GameObject _blockObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //GameObject _blockObject = GameObject.Instantiate(block  , transform.position, Quaternion.identity);
                _blockObject.transform.position = -Vector2.one * (blockPerLine - 1) * 0.5f+ new Vector2(x,y);
                _blockObject.transform.parent = transform;
                _blockObject.AddComponent<Piece>();
                Block _block= _blockObject.AddComponent<Block>();
                _block.coord = new Vector2Int(x, y);
                _block.Init(new Vector2Int(x, y), _imageSlice[x, y]);
                
            }
        }
    }
}
