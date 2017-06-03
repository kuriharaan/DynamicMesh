using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Platform))]
public class PlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if( GUILayout.Button("Create") )
        {
            Platform platform = target as Platform;
            platform.Create();
        }
    }

    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {

    }
}
