using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicRebake : MonoBehaviour
{
    public UnityEngine.AI.NavMeshData data;

    UnityEngine.AI.NavMeshBuildSettings settings;

    // Use this for initialization
    void Start ()
    {
        //UnityEngine.AI.NavMeshBuilder.BuildNavMeshAsync();
        //UnityEditor.AI.NavMeshBuilder.BuildNavMeshAsync();
        //UnityEditor.AI.NavMeshBuilder.ClearAllNavMeshes();

        settings = UnityEngine.AI.NavMesh.GetSettingsByIndex(0);


        //UnityEngine.AI.NavMeshBuilder.UpdateNavMeshData(data, settings,  )
    }

    // Update is called once per frame
    void Update ()
    {

    }
}
