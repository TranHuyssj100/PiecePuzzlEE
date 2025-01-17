﻿using UnityEngine;

public class QuadCreator : MonoBehaviour
{
    public float width = 1;
    public float height = 1;

    public void Start()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[6]
        {
            new Vector3(0, 0, 0),
            new Vector3(width *2, 0, 0),
            new Vector3(0, height*2, 0),
            new Vector3(width, height*2, 0),
            new Vector3(width, height, 0),
            new Vector3(width*2, height, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[15]
        {
            // lower left triangle
            0, 1, 4,
            1, 5, 4,
            // upper right triangle
            4, 3, 2,
            4, 3, 2,
            2,0,4

        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[6]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[6]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 1),
            new Vector2(1, 1),
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;
    }
}
