using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class DesignToolEditor : EditorWindow
{
    bool isSaveAvailable;
    string[] mainTabNames = { "Monster", "Environment" };
    int selectedTabIndex;

    #region UI Rect Values
    Rect saveBtnRect;
    Rect mainTabRect;
    private void LoadRect()
    {
        saveBtnRect = new Rect(position.width / 2 - 100/2, position.height - 50, 100, 30);
        mainTabRect = new Rect(20, 20, position.width - 40, 30);
    }
    private void UpdateRect(ref Rect curRect, float x, float y, float width, float height)
    {
        curRect.x = x;
        curRect.y = y;
        curRect.width = width;
        curRect.height = height;
    }
    private void UpdateAllRects()
    {
        UpdateRect(ref saveBtnRect, position.width / 2 - 100 / 2, position.height - 50, 100, 30);
        UpdateRect(ref mainTabRect, 20, 20, position.width - 40, 30);
    }
    #endregion

    private void OnEnable()
    {
        isSaveAvailable = false;
        selectedTabIndex = 0;
        LoadRect();
    }

    [MenuItem("Design/Wave Design")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(DesignToolEditor));
    }
    private void OnGUI()
    {
        UpdateAllRects();


        selectedTabIndex = GUI.Toolbar(mainTabRect, selectedTabIndex, mainTabNames);

        #region Save Button
        if (GUI.changed)
        {
            isSaveAvailable = true;
        }
        if(!isSaveAvailable)
        {
            GUI.enabled = false;
        }
        if (GUI.Button(saveBtnRect, "Save"))
        {
            SaveDesign();
        }
        GUI.enabled = true;
        #endregion
    }
    private void SaveDesign()
    {
        isSaveAvailable = false;
    }
}   
#endif