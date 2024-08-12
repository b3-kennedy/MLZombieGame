using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(TreePlanter))]
public class TreePlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TreePlanter treePlacer = (TreePlanter)target;

        if (GUILayout.Button("Generate Trees"))
        {
            treePlacer.GenerateTrees();
        }

        if (GUILayout.Button("Clear Trees"))
        {
            treePlacer.ClearTrees();
        }
    }
}
