using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  ImageSlice 
{
 
    public static Texture2D [,] GetSlice(Texture2D image, int blocksPerLine)
    {
        int _imageSize = Mathf.Min(image.width, image.height);
        int _blockSize = _imageSize / blocksPerLine;

        Texture2D[,] _blocks = new Texture2D[blocksPerLine, blocksPerLine];

        for(int y=0; y<blocksPerLine; y++)
        {
            for(int x=0; x<blocksPerLine; x++)
            {
                Texture2D _block = new Texture2D(_blockSize, _blockSize);
                //_block.wrapMode = TextureWrapMode.Clamp;
                _block.SetPixels(image.GetPixels(x * _blockSize, y * _blockSize, _blockSize, _blockSize));
                _block.Apply();
                _blocks[x, y] = _block;
            }
        }
        return _blocks;
    } 
    
}
