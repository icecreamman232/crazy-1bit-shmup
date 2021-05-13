using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //Do check null
        if (targetObject.IsNullList())
        {
            EditorGUILayout.HelpBox("There is null member in List", MessageType.Error);
        }
        if(targetObject.IsThereUnusedPath())
        {
            EditorGUILayout.HelpBox("There is a monster without a path attached", MessageType.Error);
        }
        if(targetObject.IsThereMonsterNotInTheList())
        {
            EditorGUILayout.HelpBox("There is a monster not listed", MessageType.Error);
        }
    }

}
