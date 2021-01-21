using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector2Int coord;


    private void Start()
    {
       
    }

    public void Init(Vector2Int startCoord, Texture2D image)
    {
        coord = startCoord;
        GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
        GetComponent<MeshRenderer>().material.mainTexture = image;
    }

}
