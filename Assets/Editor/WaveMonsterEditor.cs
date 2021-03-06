using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(WaveMonsterController))]
public class WaveMonsterEditor : Editor
{

    WaveMonsterController c;
    SerializedObject targetObject;

    private void OnEnable()
    {
        c = target as WaveMonsterController;
        targetObject = new SerializedObject(c);
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        targetObject.Update();
        GUI.backgroundColor = Color.green;
        if(GUILayout.Button("Snap To Point"))
        {
            c.SnapToControlPoint();
            targetObject.ApplyModifiedProperties();
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(c);
        }
    }

}
#endif