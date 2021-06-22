using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathSegment : MonoBehaviour
{
    [SerializeField]
    private Transform[] controlPts = new Transform[4];

    public bool isDrawGizmo;

    [MenuItem("GameObject/Bezier Path/New path", false, 0)]
    static void AddNewPathSegment()
    {
        GameObject newSegment = new GameObject();
        newSegment.name = "NewWayPoint";
        newSegment.AddComponent<PathSegment>();
        for (int i = 0; i < 4; i++)
        {
            GameObject controlPts = new GameObject();
            controlPts.name = "ControlPoint " + i;
            controlPts.transform.parent = newSegment.transform;
            newSegment.GetComponent<PathSegment>().controlPts[i] = controlPts.transform;
        }
        //Setup default position for each control point for better visual
        newSegment.GetComponent<PathSegment>().controlPts[0].transform.position = new Vector3(0f, 2f, 0f);
        newSegment.GetComponent<PathSegment>().controlPts[1].transform.position = new Vector3(-2f, 0f, 0f);
        newSegment.GetComponent<PathSegment>().controlPts[2].transform.position = new Vector3(2f, -2f, 0f);
        newSegment.GetComponent<PathSegment>().controlPts[3].transform.position = new Vector3(0f, -4f, 0f);

        newSegment.GetComponent<PathSegment>().isDrawGizmo = true;
    }
    public Vector3 GetPos(int i) => controlPts[i].position;
    public Transform GetTransform(int i) => controlPts[i];
    
    public void CopyPath(PathSegment newPath)
    {
        for(int i = 0; i< 4; i++)
        {
            controlPts[i] = newPath.GetTransform(i);
        }
    }

    public void ReversePath()
    {
        Transform[] reverseControlPts = new Transform[4];
        for (int i =3,j=0; i >= 0;j++,i--)
        {
            reverseControlPts[j] = controlPts[i];
        }
        for (int n = 0; n < 4; n++)
        {
            controlPts[n] = reverseControlPts[n];
        }
    }
    #region Gizmo
    private void OnDrawGizmos()
    {
        if(isDrawGizmo)
        {
            for (int i = 0; i < controlPts.Length; i++)
            {
                if (i == 1 || i == 2)
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawCube(GetPos(i), Vector3.one * 0.25f);
            }
            UnityEditor.Handles.DrawDottedLine(GetPos(0), GetPos(1), 3f);
            UnityEditor.Handles.DrawDottedLine(GetPos(3), GetPos(2), 3f);
            UnityEditor.Handles.DrawBezier(GetPos(0), GetPos(3), GetPos(1), GetPos(2), Color.yellow, null, 2.0f);
        }
        
    }
    #endregion

}
