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



    //Just put the default name so it would not cause error if user forget to put the name
    string prefabName = "New Monster Prefab";
    ItemType itemType;


   
    private void OnEnable()
    {
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
        GameObject prefabInstant = new GameObject();
        var fullPath = assetPath + prefabName + ".prefab";      
        SetupPrefabInformation(prefabInstant);  
        PrefabUtility.SaveAsPrefabAssetAndConnect(prefabInstant, fullPath,InteractionMode.UserAction);
    }
    #region Setup Prefab
    private void SetupPrefabInformation(GameObject prefab)
    {
        prefab.name = prefabName;
        prefab.tag = "Enemy";
        AddHealthBar(prefab);
        SetupSplineMoveComponenet(prefab);
        SetupItemSpawnerComponent(prefab);
        SetupCoinSpawnerComponenet(prefab);
    }
    private void AddHealthBar(GameObject prefab)
    {
        GameObject hp = PrefabUtility.LoadPrefabContents(assetPath + "HealthBar.prefab");
        hp.transform.parent = prefab.transform;
        hp.gameObject.SetActive(false);
    }
    private void SetupSplineMoveComponenet(GameObject prefab)
    {
        prefab.AddComponent<splineMove>();

        //For totally 2D movement
        prefab.GetComponent<splineMove>().pathMode = DG.Tweening.PathMode.Ignore;
    }
    private void SetupItemSpawnerComponent(GameObject prefab)
    {
        prefab.AddComponent<ItemSpawner>();

        GameObject itemPrefab = Resources.Load("ItemPrefabTemplate") as GameObject;
        prefab.GetComponent<ItemSpawner>().itemPrefab = itemPrefab;
        prefab.GetComponent<ItemSpawner>().itemDropRate = 700;
    }
    private void SetupCoinSpawnerComponenet(GameObject prefab)
    {
        prefab.AddComponent<CoinSpawner>();
        GameObject coinPrefab = Resources.Load("CoinPrefabTemplate") as GameObject;
        prefab.GetComponent<CoinSpawner>().coinPrefab = coinPrefab;

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
