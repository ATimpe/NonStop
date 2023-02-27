using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMesh : MonoBehaviour
{
    [SerializeField] Material mat;
    // Start is called before the first frame update
    void Start()
    {
        Vector3[] verts = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        Mesh mesh = new Mesh();

        verts[0] = new Vector3(0, 1, 0);
        verts[1] = new Vector3(1, 1, 0);
        verts[2] = new Vector3(0, 0, 0);
        verts[3] = new Vector3(1, 0, 0);

        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 2;
        triangles[4] = 1;
        triangles[5] = 3;

        mesh.vertices = verts;
        mesh.uv = uv;
        mesh.triangles = triangles;

        gameObject.AddComponent(typeof(MeshFilter)); 
        gameObject.AddComponent(typeof(MeshRenderer)); 

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = mat;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
