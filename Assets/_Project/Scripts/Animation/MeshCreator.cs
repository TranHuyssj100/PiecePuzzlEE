using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCreator : MonoBehaviour
{

    public Transform upperLeft, bottomLeft, bottomMid, upperMid, upperRight, bottomRight;
    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;
    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        vertices = new Vector3[6];
        uv = new Vector2[6];
        triangles = new int[12];

        GetComponent<MeshFilter>().mesh = mesh;

    }

    private void FixedUpdate()
    {

        //vertices[0] = bottomLeft.localPosition + leftGroup.localPosition;
        //vertices[1] = upperLeft.localPosition + leftGroup.localPosition;
        //vertices[2] = upperRight.localPosition + rightGroup.localPosition;
        //vertices[3] = bottomRight.localPosition + rightGroup.localPosition;

        vertices[0] = transform.InverseTransformPoint(bottomLeft.position);
        vertices[1] = transform.InverseTransformPoint(upperLeft.position);
        vertices[2] = transform.InverseTransformPoint(bottomMid.position);
        vertices[3] = transform.InverseTransformPoint(upperMid.position);
        vertices[4] = transform.InverseTransformPoint(bottomRight.position);
        vertices[5] = transform.InverseTransformPoint(upperRight.position);

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(.5f, 0);
        uv[3] = new Vector2(.5f, 1);
        uv[4] = new Vector2(1, 0);
        uv[5] = new Vector2(1, 1f);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;

        triangles[3] = 0;
        triangles[4] = 3;
        triangles[5] = 2;

        triangles[6] = 2;
        triangles[7] = 3;
        triangles[8] = 5;

        triangles[9] = 2;
        triangles[10] = 5;
        triangles[11] = 4;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();


        //GetComponent<MeshRenderer>().sortingLayerName = "UI_Behind";
    }
}
