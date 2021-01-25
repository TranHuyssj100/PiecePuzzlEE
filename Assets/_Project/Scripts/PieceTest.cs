using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceTest : MonoBehaviour
{
    public Texture2D img;

    private void Start()
    {
        Init(ImageSlice.GetPiece(img, Vector2Int.zero, new Vector2Int(5 , 5)), new Vector2(0,0));   
        Init(ImageSlice.GetPiece(img, new Vector2Int(0,1), new Vector2Int(5 , 5)),new Vector2(0, 1));
        Mesh meshCombine = CombineMeshes(new List<Mesh> { transform.GetChild(0).GetComponent<Mesh>(), transform.GetChild(1).GetComponent<Mesh>() });
        GameObject tempQuad= GameObject.CreatePrimitive(PrimitiveType.Quad);
        tempQuad.GetComponent<MeshFilter>().mesh = meshCombine;
    }

    public void Init(Texture2D _image,Vector2 _position)
    {
        if (!GetComponent<SpriteRenderer>())
        {
            gameObject.AddComponent<SpriteRenderer>();
        }
        GameObject _quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        _quad.transform.parent = transform;
        _quad.transform.position = _position;
        _quad.GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
        _quad.GetComponent<MeshRenderer>().material.mainTexture = _image;
    }

    private Mesh CombineMeshes(List<Mesh> meshes)
    {
        var combine = new CombineInstance[meshes.Count];
        for (int i = 0; i < meshes.Count; i++)
        {
            combine[i].mesh = meshes[i];
            combine[i].transform = transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        return mesh;
    }
}
