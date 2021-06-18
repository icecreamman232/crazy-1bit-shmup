using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SWS;

public class PrefabInstantEditor : EditorWindow
{
    private readonly string assetPath           = "Assets/Prefabs/";
    private readonly float horizontal_padding   = 20;
    private readonly float vertical_padding     = 20;

    GameObject prefabParentGameObject;


    //Just put the default name so it would not cause error if user forget to put the name
    string prefabName = "NewPrefabInstant";
    ItemType itemType;


   
    private void OnEnable()
    {
        prefabParentGameObject = new GameObject();
    }


    [MenuItem("Window/Prefab Instant")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<PrefabInstantEditor>("Prefab Instant");
    }
    

    private void OnGUI()
    {
        

        #region Prefab Basic Information
        var totalRectPrefabName = new Rect(10, 10, GetLabelTextDimension("Prefab Name").x + 150, GetLabelTextDimension("Prefab Name").y);
        var labelRectPrefabName = totalRectPrefabName;
        labelRectPrefabName.width -= 150;
        EditorGUI.HandlePrefixLabel(
            totalRectPrefabName,
            labelRectPrefabName,
            new GUIContent("Prefab Name")
            );
        prefabName = EditorGUI.TextField(
            new Rect(labelRectPrefabName.width + horizontal_padding, totalRectPrefabName.y, 150, totalRectPrefabName.height)
            , prefabName); ;



        var totalRect = new Rect(10, 40, GetLabelTextDimension("Drop Item Type").x + 80, GetLabelTextDimension("Drop Item Type").y);
        var labelRect = totalRect;
        labelRect.width -= 80;
        EditorGUI.HandlePrefixLabel(
            totalRect,
            labelRect,
            new GUIContent("Drop Item Type")
            );
        itemType = (ItemType)EditorGUI.EnumPopup(
            new Rect(labelRect.width + horizontal_padding, totalRect.y, 80, totalRect.height),
            ItemType.ITEM0);
        #endregion

        #region Script required section

        //prefabParentGameObject.AddComponent<ItemSpawner>();

        if(GUI.Button(new Rect(Screen.width/2 - 50, Screen.height-80, 100, 30), "Create prefab"))
        {
            CreateNewPrefabInAsset();
        }
        
        #endregion
        
        
    }
   
    private void CreateNewPrefabInAsset()
    {   
        var fullPath = assetPath + prefabName + ".prefab"; 
        SetupPrefabInformation();
        
        PrefabUtility.SaveAsPrefabAsset(prefabParentGameObject, fullPath);
        prefabParentGameObject = null;
        prefabParentGameObject = new GameObject();
    }
    #region Setup Prefab
    private void SetupPrefabInformation()
    {
        prefabParentGameObject.name = prefabName;
        prefabParentGameObject.tag = "Enemy";
        SetupSplineMoveComponenet();
        SetupItemSpawnerComponent();
        SetupCoinSpawnerComponenet();
    }
    private void SetupSplineMoveComponenet()
    {
        prefabParentGameObject.AddComponent<splineMove>();

        //For totally 2D movement
        prefabParentGameObject.GetComponent<splineMove>().pathMode = DG.Tweening.PathMode.Ignore;
    }
    private void SetupItemSpawnerComponent()
    {
        prefabParentGameObject.AddComponent<ItemSpawner>();
        GameObject itemPrefab = Resources.Load("ItemPrefabTemplate") as GameObject;
        prefabParentGameObject.GetComponent<ItemSpawner>().itemPrefab = itemPrefab;
        prefabParentGameObject.GetComponent<ItemSpawner>().itemDropRate = 700;
    }
    private void SetupCoinSpawnerComponenet()
    {
        prefabParentGameObject.AddComponent<CoinSpawner>();
        GameObject coinPrefab = Resources.Load("CoinPrefabTemplate") as GameObject;
        prefabParentGameObject.GetComponent<CoinSpawner>().coinPrefab = coinPrefab;

    }
    #endregion
    #region Helper Functions
    Vector2 GetLabelTextDimension(string text)
    {
        //Use this to calculate size of content based on its style
        return GUI.skin.label.CalcSize(new GUIContent(text));
    }
    private void DrawLine()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 3);
        rect.height = 3;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
    #endregion



}
