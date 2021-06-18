using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MonsterWithCustomPath))]
public class MonsterWithCustomPathEditor : Editor
{
    MonsterWithCustomPath monster;
    int currentHP;

    private void OnEnable()
    {
        monster = target as MonsterWithCustomPath;

    }
    public override void OnInspectorGUI()
    {
        UpdateInformation();
        EditorGUILayout.LabelField("Current HP");
        EditorGUILayout.HelpBox(new GUIContent(currentHP.ToString()));
        //base.OnInspectorGUI();

    }
    private void UpdateInformation()
    {
        currentHP = monster.currentHP;
    }
}
