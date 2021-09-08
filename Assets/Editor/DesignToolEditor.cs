using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class DesignToolEditor : EditorWindow
{
    public static DesignToolEditor Instance;


    private List<MonsterDO> monsterDataList;
    private object[] arrDragAndDropObjects;


    bool isSaveAvailable;
    string[] mainTabNames = { "Monster", "Environment" };
    int selectedTabIndex;

    #region UI Rect Values
    Rect mainMenuGroupRect;
    Rect newFileBtnRect;
    Rect openFileBtnRect;
    Rect saveBtnRect;
    Rect mainTabRect;
    private void UpdateRect(ref Rect curRect, float x, float y, float width, float height)
    {
        if(curRect!=null)
        {
            curRect = new Rect(x, y, width, height);
        }
        else
        {
            curRect.x = x;
            curRect.y = y;
            curRect.width = width;
            curRect.height = height;
        }
    }
    private void UpdateAllRects()
    {
        UpdateRect(ref newFileBtnRect, 20, 20, 100, 30);
        UpdateRect(ref openFileBtnRect, newFileBtnRect.x+ newFileBtnRect.width + 20, 20, 100, 30);
        UpdateRect(ref saveBtnRect, position.width / 2 - 100 / 2, position.height - 50, 100, 30);
        UpdateRect(ref mainTabRect, 20, 20, position.width - 40, 30);
    }
    #endregion

    private void OnEnable()
    {
        monsterDataList = new List<MonsterDO>();
        isSaveAvailable = false;
        selectedTabIndex = 0;
        UpdateAllRects();
    }

    [MenuItem("Design/Wave Design")]
    public static void ShowWindow()
    {
        if(Instance = null)
        {
            //Init
            Instance = (DesignToolEditor)EditorWindow.GetWindow(typeof(DesignToolEditor));
        }     
        else
        {
            //Focus
            EditorWindow.GetWindow<DesignToolEditor>();
        }
        
    }
    private void OnGUI()
    {
        UpdateAllRects();
        EventType eventType = Event.current.type;
        if (eventType == EventType.DragUpdated ||
            eventType == EventType.DragPerform)
        {
            // Show a copy icon on the drag
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

            if (eventType == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                arrDragAndDropObjects = DragAndDrop.objectReferences;
                monsterDataList.Add((MonsterDO)arrDragAndDropObjects[0]);
            }
            Event.current.Use();
        }
        
        if (GUI.Button(newFileBtnRect, "New..."))
        {
            CreateNewDesign();
        }

        if (GUI.Button(openFileBtnRect,"Open..."))
        {
            OpenDesign();
        }

 
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
    private void CreateNewDesign()
    {

    }
    private void OpenDesign()
    {

    }
    private void SaveDesign()
    {
        isSaveAvailable = false;
    }

}   
#endif