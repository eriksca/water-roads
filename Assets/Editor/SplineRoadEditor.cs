using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SplineRoad))]
public class SplineRoadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SplineRoad component = (SplineRoad)target;

        if (GUILayout.Button("Generate Mesh"))
        {
            component.BuildMesh();
        }

        if (GUILayout.Button("Reset"))
        {
            component.ResetAll();
        }
    }
}
