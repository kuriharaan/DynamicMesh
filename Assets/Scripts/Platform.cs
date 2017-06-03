using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Platform : MonoBehaviour
{
    [SerializeField]
    int TILE_X_COUNT = 4;

    [SerializeField]
    int TILE_Z_COUNT = 4;

    public void Create()
    {
        int TILE_NUM = TILE_X_COUNT * TILE_Z_COUNT;

        vertices = new Vector3[TILE_NUM * 4];
        for( int z = 0; z < TILE_Z_COUNT; ++z )
        {
            for( int x = 0; x < TILE_X_COUNT; ++x )
            {
                int i = z * TILE_X_COUNT + x;
                vertices[i * 4 + 0] = new Vector3(0.0f + x, 0.0f, 0.0f + z);
                vertices[i * 4 + 1] = new Vector3(0.0f + x, 0.0f, 1.0f + z);
                vertices[i * 4 + 2] = new Vector3(1.0f + x, 0.0f, 0.0f + z);
                vertices[i * 4 + 3] = new Vector3(1.0f + x, 0.0f, 1.0f + z);
            }
        }

        normals = new Vector3[TILE_NUM * 4];
        for( var i = 0; i < normals.Length; ++i )
        {
            //normals[i] = Vector3.up;
            normals[i] = Random.insideUnitSphere;
        }


        triangles = new int[TILE_NUM * 6];
        for (int i = 0; i < TILE_NUM; ++i)
        {
            triangles[i * 6 + 0] = i * 4 + 0;
            triangles[i * 6 + 1] = i * 4 + 1;
            triangles[i * 6 + 2] = i * 4 + 2;
            triangles[i * 6 + 3] = i * 4 + 2;
            triangles[i * 6 + 4] = i * 4 + 1;
            triangles[i * 6 + 5] = i * 4 + 3;
        }

        Mesh mesh = new Mesh();
        mesh.name = "Platform";
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 99999999);
        var mf = GetComponent<MeshFilter>();
        mf.sharedMesh = mesh;

    }

    private Vector3[] vertices;
    private Vector3[] normals;
    private int[]     triangles;

    IEnumerator Start ()
    {
        var mf = GetComponent<MeshFilter>();
        var mesh = mf.mesh;

        vertices = new Vector3[mesh.vertices.Length];
        for( int i = 0; i < mesh.vertices.Length; ++i )
        {
            vertices[i] = mesh.vertices[i];
        }

        yield return new WaitForSeconds(1.0f);

        for ( int i = 0; i < vertices.Length; ++i )
        {
            var v = vertices[i];
            v.y -= 1.0f;
            vertices[i] = v;
        }
        mesh.vertices = vertices;

        var collider = GetComponent<MeshCollider>();

        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
