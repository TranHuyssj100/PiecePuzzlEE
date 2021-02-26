using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator : MonoBehaviour
{

    public Transform upperLeft, bottomLeft, bottomMid, upperMid, upperRight, bottomRight;
    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;
    // Start is called before the first frame update
    void Start()
    {

        vertices = new Vector3[6];
        uv = new Vector2[6];
        triangles = new int[12];

        
    }

    private void FixedUpdate()
    {
        Mesh mesh = new Mesh();

        //vertices[0] = bottomLeft.localPosition + leftGroup.localPosition;
        //vertices[1] = upperLeft.localPosition + leftGroup.localPosition;
        //vertices[2] = upperRight.localPosition + rightGroup.localPosition;
        //vertices[3] = bottomRight.localPosition + rightGroup.localPosition;

        vertices[0] = transform.InverseTransformPoint(bottomLeft.position);
        vertices[1] = transform.InverseTransformPoint(upperLeft.position);
        vertices[2] = transform.InverseTransformPoint(upperRight.position);
        vertices[3] = transform.InverseTransformPoint(bottomRight.position);
        vertices[4] = transform.InverseTransformPoint(bottomMid.position);
        vertices[5] = transform.InverseTransformPoint(upperMid.position);

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);
        uv[4] = new Vector2(.5f, 0);
        uv[5] = new Vector2(.5f, 1f);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 5;

        triangles[3] = 0;
        triangles[4] = 5;
        triangles[5] = 4;

        triangles[6] = 4;
        triangles[7] = 5;
        triangles[8] = 2;

        triangles[9] = 4;
        triangles[10] = 2;
        triangles[11] = 3;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;


        GetComponent<MeshFilter>().mesh = mesh;
        //GetComponent<MeshRenderer>().sortingLayerName = "UI_Behind";
    }
}
