using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataManager))]
public class DataManagerEditor : Editor
{
    private void OnEnable()
    {
        
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUIStyle style = new GUIStyle();
        if(GUILayout.Button("Reset Data",style))
        {
            
            DataManager.Instance.ResetData();
        }
    }
}
