using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathSegment))]
public class PathSegementEditor : Editor
{
    PathSegment wayPtsObject;
    private void OnEnable()
    {
        wayPtsObject = target as PathSegment;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
     
    }
}
