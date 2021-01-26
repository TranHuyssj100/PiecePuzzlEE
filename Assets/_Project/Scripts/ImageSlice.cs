using UnityEngine;
using UnityEngine.UI;

public static class  ImageSlice 
{
 
    public static Texture2D [,] GetSlice(Texture2D _image, int _blocksPerLine)
    {
        int _imageSize = Mathf.Min(_image.width, _image.height);
        int _blockSize = _imageSize / _blocksPerLine;

        Texture2D[,] _blocks = new Texture2D[_blocksPerLine, _blocksPerLine];

        for(int y=0; y<_blocksPerLine; y++)
        {
            for(int x=0; x<_blocksPerLine; x++)
            {
                Texture2D _block = new Texture2D(_blockSize, _blockSize);
                //_block.wrapMode = TextureWrapMode.Clamp;
                _block.SetPixels(_image.GetPixels(x * _blockSize, y * _blockSize, _blockSize, _blockSize));
                _block.Apply();
                _blocks[x, y] = _block;
            }
        }
        return _blocks;
    } 

    public static Texture2D GetPiece(Texture2D _image, Vector2Int _pivot, Vector2Int _blocksPerLine ) 
    {
        int _imageSize = Mathf.Min(_image.width, _image.height);
        Vector2Int _blockSize = new Vector2Int (_imageSize / _blocksPerLine.x, _imageSize/_blocksPerLine.y);
        Texture2D _piece = new Texture2D(_blockSize.x, _blockSize.y);
        _piece.SetPixels(_image.GetPixels(_pivot.x * _blockSize.x, _pivot.y * _blockSize.y, _blockSize.x, _blockSize.y));
        _piece.Apply();
        
        return _piece;

    }

    
    
    //public static Texture2D MergeImage(Texture2D _img1, Texture2D _img2)
    //{
        
    //}
}
