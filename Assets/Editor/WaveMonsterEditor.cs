using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(WaveMonsterController))]
public class WaveMonsterEditor : Editor
{
    WaveMonsterController targetObject;

    private void OnEnable()
    {
        targetObject = target as WaveMonsterController;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.backgroundColor = Color.green;
        if(GUILayout.Button("Snap To Point"))
        {
            targetObject.SnapToControlPoint();
        }
    }

}
#endif