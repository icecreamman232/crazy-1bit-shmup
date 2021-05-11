using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CollectorRankDO))]
public class RankEditor : Editor
{

    CollectorRankDO c;
    SerializedObject serialized_obj;
    SerializedProperty serialized_list;
    int list_size;
    List<bool> foldout_list;


    private void OnEnable()
    {
        c = target as CollectorRankDO;
        serialized_obj = new SerializedObject(c);
        serialized_list = serialized_obj.FindProperty("list_collector_rank");
        var count = serialized_list.arraySize;
        foldout_list = new List<bool>();
       
        for (int i = 0; i < count; i++)
        {
            foldout_list.Add(false);
        }
    }
    public override void OnInspectorGUI()
    {
        serialized_obj.Update();

        list_size = serialized_list.arraySize;
        list_size = EditorGUILayout.IntField("Size", list_size);
        if(list_size!=serialized_list.arraySize)
        {
            while(list_size > serialized_list.arraySize)
            {
                serialized_list.InsertArrayElementAtIndex(serialized_list.arraySize);
                foldout_list.Add(false);
            }
            while(list_size < serialized_list.arraySize)
            {
                serialized_list.DeleteArrayElementAtIndex(serialized_list.arraySize-1);
                foldout_list.RemoveAt(serialized_list.arraySize - 1);
            }
        }

        for(int i = 0; i < list_size; i++)
        {
            bool isOpenFoldOut;
            if(foldout_list.Count-1 > i)
            {
                foldout_list[i] = EditorGUILayout.Foldout(foldout_list[i], "Rank " + i);
                isOpenFoldOut = foldout_list[i];
            }
            else
            {
                bool foldout = false;
                foldout = EditorGUILayout.Foldout(foldout, "Rank " + i);
                foldout_list.Add(foldout);
                isOpenFoldOut = foldout;              
            }
            if (isOpenFoldOut)
            {
               
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    EditorGUIUtility.labelWidth = 20f;
                    SerializedProperty id = serialized_list.GetArrayElementAtIndex(i).FindPropertyRelative("rank_id");
                    id.intValue = EditorGUILayout.IntField("ID", id.intValue, GUILayout.Width(80));
                    EditorGUIUtility.labelWidth = 50f;
                    SerializedProperty pieces = serialized_list.GetArrayElementAtIndex(i).FindPropertyRelative("pieces_needed");
                    id.intValue = EditorGUILayout.IntField("Pieces", id.intValue, GUILayout.Width(120));
                    SerializedProperty sprite = serialized_list.GetArrayElementAtIndex(i).FindPropertyRelative("rank_sprite");
                    
                    sprite.objectReferenceValue = EditorGUILayout.ObjectField("", sprite.objectReferenceValue, typeof(Sprite),allowSceneObjects:true);
                }
            }
            EditorGUILayout.Space(-1);

        }



        serialized_obj.ApplyModifiedProperties();
    }
}
