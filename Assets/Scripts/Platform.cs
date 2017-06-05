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

    [SerializeField]
    float TILE_WIDTH = 1.0f;

    public int GetPanelId(Vector3 pos)
    {
        if( ( 0.0f > pos.x ) || (TILE_WIDTH * TILE_X_COUNT) < pos.x )
        {
            return -1;
        }
        if( ( 0.0f > pos.z ) || (TILE_WIDTH * TILE_Z_COUNT) < pos.z )
        {
            return -1;
        }
        int x = (int)(pos.x / TILE_WIDTH);
        int z = (int)(pos.z / TILE_WIDTH);

        return z * TILE_X_COUNT + x;
    }

    public void HilightPanel(int id)
    {
        int TILE_NUM = TILE_X_COUNT * TILE_Z_COUNT;

        Color hilightColor = new Color(0.5f, 0.5f, 0.2f);
        for (var i = 0; i < TILE_NUM; ++i)
        {
            colors[i * 4 + 0] = colors[i * 4 + 1] = colors[i * 4 + 2] = colors[i * 4 + 3] = (i == id) ? hilightColor : Color.black;
        }
        mesh.colors = colors;
    }

    public void GainHeight(int id)
    {
        if( 0 > id )
        {
            return;
        }

        for( int i = id * 4; i < (id + 1) * 4; ++i )
        {
            var p = vertices[i];
            p.y += 0.1f;
            vertices[i] = p;
        }
        mesh.vertices = vertices;

        var collider = GetComponent<MeshCollider>();

        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void Create()
    {
        int TILE_NUM = TILE_X_COUNT * TILE_Z_COUNT;

        vertices = new Vector3[TILE_NUM * 4];
        for( int z = 0; z < TILE_Z_COUNT; ++z )
        {
            for( int x = 0; x < TILE_X_COUNT; ++x )
            {
                int i = z * TILE_X_COUNT + x;
                float y = 0.0f;
                vertices[i * 4 + 0] = new Vector3((0 + x) * TILE_WIDTH, y, (0 + z) * TILE_WIDTH);
                vertices[i * 4 + 1] = new Vector3((0 + x) * TILE_WIDTH, y, (1 + z) * TILE_WIDTH);
                vertices[i * 4 + 2] = new Vector3((1 + x) * TILE_WIDTH, y, (0 + z) * TILE_WIDTH);
                vertices[i * 4 + 3] = new Vector3((1 + x) * TILE_WIDTH, y, (1 + z) * TILE_WIDTH);
            }
        }

        normals = new Vector3[TILE_NUM * 4];
        for( var i = 0; i < TILE_NUM; ++i )
        {
            normals[i * 4 + 0] = normals[i * 4 + 1] = normals[i * 4 + 2] = normals[i * 4 + 3] = Random.insideUnitSphere;
        }

        colors = new Color[TILE_NUM * 4];
        for( var i = 0; i < TILE_NUM; ++i )
        {
            colors[i * 4 + 0] = colors[i * 4 + 1] = colors[i * 4 + 2] = colors[i * 4 + 3] = Color.black;
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


        mesh = new Mesh();
        mesh.name = "Platform";
        mesh.vertices  = vertices;
        mesh.normals   = normals;
        mesh.colors    = colors;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 99999999);
        mesh.MarkDynamic();
        var mf = GetComponent<MeshFilter>();
        mf.sharedMesh = mesh;
    }

    private Mesh      mesh;
    private Vector3[] vertices;
    private Vector3[] normals;
    private Color[]   colors;
    private int[]     triangles;

    IEnumerator Start ()
    {
        var mf = GetComponent<MeshFilter>();
        mesh = mf.mesh;

        vertices = mesh.vertices;
        normals  = mesh.normals;
        colors   = mesh.colors;

        yield return new WaitForSeconds(1.0f);

        /*
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
        */
    }
}
