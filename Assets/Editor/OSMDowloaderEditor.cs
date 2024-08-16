using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OSMDownloader))]
public class OSMDowloaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        OSMDownloader component = (OSMDownloader)target;

        if (GUILayout.Button("Make Request"))
            component.MakeRequest();
    }
}
