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

    float FRAME_WIDTH = 0.2f;
    const int vnum_per_tile = 16;

    public int GetPanelId(Vector3 pos)
    {
        float with_frame_width = TILE_WIDTH + FRAME_WIDTH * 2.0f;
        if ( ( 0.0f > pos.x ) || (with_frame_width * TILE_X_COUNT) < pos.x )
        {
            return -1;
        }
        if( ( 0.0f > pos.z ) || (with_frame_width * TILE_Z_COUNT) < pos.z )
        {
            return -1;
        }
        int x = (int)(pos.x / with_frame_width);
        int z = (int)(pos.z / with_frame_width);

        return z * TILE_X_COUNT + x;
    }

    public void HilightPanel(int id)
    {
        int TILE_NUM = TILE_X_COUNT * TILE_Z_COUNT;

        Color hilightColor = new Color(0.5f, 0.5f, 0.2f);
        /*
        for (var i = 0; i < TILE_NUM; ++i)
        {
            colors[i * 4 + 0] = colors[i * 4 + 1] = colors[i * 4 + 2] = colors[i * 4 + 3] = (i == id) ? hilightColor : Color.black;
        }
        */

        for( int i = 0; i < colors.Length; ++i )
        {
            if( (vnum_per_tile * id <= i) && (vnum_per_tile * (id + 1) > i) )
            {
                colors[i] = hilightColor;
            }
            else
            {
                colors[i] = Color.black;
            }
        }

        mesh.colors = colors;
    }

    public void GainHeight(int id)
    {
        if( 0 > id )
        {
            return;
        }

        int[] indices = new int[4];
        indices[0] = vnum_per_tile * id + 5;
        indices[1] = vnum_per_tile * id + 6;
        indices[2] = vnum_per_tile * id + 9;
        indices[3] = vnum_per_tile * id + 10;

        for ( int i = 0; i < 4; ++i )
        {
            var p = vertices[indices[i]];
            p.y += 0.3f;
            vertices[indices[i]] = p;
        }
        mesh.vertices = vertices;

        var collider = GetComponent<MeshCollider>();

        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void Create()
    {
        int TILE_NUM = TILE_X_COUNT * TILE_Z_COUNT;

        int total_vnum = 9 * TILE_X_COUNT * TILE_Z_COUNT + 3 * TILE_X_COUNT + 3 * TILE_Z_COUNT + 1;

        vertices = new Vector3[total_vnum];


        for( int z = 0; z < TILE_Z_COUNT; ++z )
        {
            for (int x = 0; x < TILE_X_COUNT; ++x)
            {
                int i = z * TILE_X_COUNT + x;
                float y = 0.0f;
                float with_frame_width = TILE_WIDTH + FRAME_WIDTH * 2.0f;

                float x0 = x * with_frame_width;
                float x1 = x0 + FRAME_WIDTH;
                float x2 = x1 + TILE_WIDTH;
                float x3 = x2 + FRAME_WIDTH;

                float z0 = z * with_frame_width;
                float z1 = z0 + FRAME_WIDTH;
                float z2 = z1 + TILE_WIDTH;
                float z3 = z2 + FRAME_WIDTH;

                vertices[i * vnum_per_tile +  0] = new Vector3(x0, y, z0);
                vertices[i * vnum_per_tile +  1] = new Vector3(x1, y, z0);
                vertices[i * vnum_per_tile +  2] = new Vector3(x2, y, z0);
                vertices[i * vnum_per_tile +  3] = new Vector3(x3, y, z0);

                vertices[i * vnum_per_tile +  4] = new Vector3(x0, y, z1);
                vertices[i * vnum_per_tile +  5] = new Vector3(x1, y, z1);
                vertices[i * vnum_per_tile +  6] = new Vector3(x2, y, z1);
                vertices[i * vnum_per_tile +  7] = new Vector3(x3, y, z1);

                vertices[i * vnum_per_tile +  8] = new Vector3(x0, y, z2);
                vertices[i * vnum_per_tile +  9] = new Vector3(x1, y, z2);
                vertices[i * vnum_per_tile + 10] = new Vector3(x2, y, z2);
                vertices[i * vnum_per_tile + 11] = new Vector3(x3, y, z2);

                vertices[i * vnum_per_tile + 12] = new Vector3(x0, y, z3);
                vertices[i * vnum_per_tile + 13] = new Vector3(x1, y, z3);
                vertices[i * vnum_per_tile + 14] = new Vector3(x2, y, z3);
                vertices[i * vnum_per_tile + 15] = new Vector3(x3, y, z3);
            }
        }

        normals = new Vector3[TILE_NUM * vnum_per_tile];
        for (var i = 0; i < TILE_NUM; ++i)
        {
            var normal = Random.insideUnitSphere;
            for (int j = 0; j < vnum_per_tile; ++j)
            {
                normals[i * vnum_per_tile + j] = normal;
            }
            normals[i * vnum_per_tile + 0] = Vector3.down;
            normals[i * vnum_per_tile + 1] = Vector3.down;
            normals[i * vnum_per_tile + 4] = Vector3.down;
            normals[i * vnum_per_tile + 2] = Vector3.down;
            normals[i * vnum_per_tile + 6] = Vector3.down;
        }

        colors = new Color[TILE_NUM * vnum_per_tile];
        var default_color = Color.black;
        for( var i = 0; i < TILE_NUM; ++i )
        {
            for( int j = 0; j < vnum_per_tile; ++j )
            {
                colors[i * vnum_per_tile + j] = default_color;
            }
        }

        const int triangle_per_tile = 54;
        triangles = new int[TILE_NUM * triangle_per_tile];
        for (int i = 0; i < TILE_NUM; ++i)
        {
            triangles[i * triangle_per_tile + 0] = i * vnum_per_tile + 0;
            triangles[i * triangle_per_tile + 1] = i * vnum_per_tile + 4;
            triangles[i * triangle_per_tile + 2] = i * vnum_per_tile + 1;

            triangles[i * triangle_per_tile + 3] = i * vnum_per_tile + 1;
            triangles[i * triangle_per_tile + 4] = i * vnum_per_tile + 4;
            triangles[i * triangle_per_tile + 5] = i * vnum_per_tile + 5;

            triangles[i * triangle_per_tile + 6] = i * vnum_per_tile + 1;
            triangles[i * triangle_per_tile + 7] = i * vnum_per_tile + 5;
            triangles[i * triangle_per_tile + 8] = i * vnum_per_tile + 2;

            triangles[i * triangle_per_tile + 9] = i * vnum_per_tile + 2;
            triangles[i * triangle_per_tile +10] = i * vnum_per_tile + 5;
            triangles[i * triangle_per_tile +11] = i * vnum_per_tile + 6;

            triangles[i * triangle_per_tile + 12] = i * vnum_per_tile + 2;
            triangles[i * triangle_per_tile + 13] = i * vnum_per_tile + 6;
            triangles[i * triangle_per_tile + 14] = i * vnum_per_tile + 3;

            triangles[i * triangle_per_tile + 15] = i * vnum_per_tile + 3;
            triangles[i * triangle_per_tile + 16] = i * vnum_per_tile + 6;
            triangles[i * triangle_per_tile + 17] = i * vnum_per_tile + 7;

            triangles[i * triangle_per_tile + 18] = i * vnum_per_tile + 4;
            triangles[i * triangle_per_tile + 19] = i * vnum_per_tile + 8;
            triangles[i * triangle_per_tile + 20] = i * vnum_per_tile + 5;

            triangles[i * triangle_per_tile + 21] = i * vnum_per_tile + 5;
            triangles[i * triangle_per_tile + 22] = i * vnum_per_tile + 8;
            triangles[i * triangle_per_tile + 23] = i * vnum_per_tile + 9;

            triangles[i * triangle_per_tile + 24] = i * vnum_per_tile + 5;
            triangles[i * triangle_per_tile + 25] = i * vnum_per_tile + 9;
            triangles[i * triangle_per_tile + 26] = i * vnum_per_tile + 6;

            triangles[i * triangle_per_tile + 27] = i * vnum_per_tile + 6;
            triangles[i * triangle_per_tile + 28] = i * vnum_per_tile + 9;
            triangles[i * triangle_per_tile + 29] = i * vnum_per_tile + 10;

            triangles[i * triangle_per_tile + 30] = i * vnum_per_tile + 6;
            triangles[i * triangle_per_tile + 31] = i * vnum_per_tile + 10;
            triangles[i * triangle_per_tile + 32] = i * vnum_per_tile + 7;

            triangles[i * triangle_per_tile + 33] = i * vnum_per_tile + 7;
            triangles[i * triangle_per_tile + 34] = i * vnum_per_tile + 10;
            triangles[i * triangle_per_tile + 35] = i * vnum_per_tile + 11;

            triangles[i * triangle_per_tile + 36] = i * vnum_per_tile + 8;
            triangles[i * triangle_per_tile + 37] = i * vnum_per_tile + 12;
            triangles[i * triangle_per_tile + 38] = i * vnum_per_tile + 9;

            triangles[i * triangle_per_tile + 39] = i * vnum_per_tile + 9;
            triangles[i * triangle_per_tile + 40] = i * vnum_per_tile + 12;
            triangles[i * triangle_per_tile + 41] = i * vnum_per_tile + 13;

            triangles[i * triangle_per_tile + 42] = i * vnum_per_tile + 9;
            triangles[i * triangle_per_tile + 43] = i * vnum_per_tile + 13;
            triangles[i * triangle_per_tile + 44] = i * vnum_per_tile + 10;

            triangles[i * triangle_per_tile + 45] = i * vnum_per_tile + 10;
            triangles[i * triangle_per_tile + 46] = i * vnum_per_tile + 13;
            triangles[i * triangle_per_tile + 47] = i * vnum_per_tile + 14;

            triangles[i * triangle_per_tile + 48] = i * vnum_per_tile + 10;
            triangles[i * triangle_per_tile + 49] = i * vnum_per_tile + 14;
            triangles[i * triangle_per_tile + 50] = i * vnum_per_tile + 11;

            triangles[i * triangle_per_tile + 51] = i * vnum_per_tile + 11;
            triangles[i * triangle_per_tile + 52] = i * vnum_per_tile + 14;
            triangles[i * triangle_per_tile + 53] = i * vnum_per_tile + 15;
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
