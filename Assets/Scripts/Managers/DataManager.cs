using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class SaveFile
{
    public int current_rank;
    public int current_points;

    public SaveFile()
    {
        current_rank = 0;
        current_points = 0;
    }
}


public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public RankManager rank_manager;

    public string save_path ;

    public SaveFile save_data;

    void Awake()
    {
        save_data = new SaveFile();
        save_path = Application.persistentDataPath + "/SpaceGuadians.sgs";
        Instance = this;
    }
    void Start()
    {
        LoadDataFromLocalStorage();
    }
    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reset save!!");
            ResetData();
        }
#endif
    }

    public void LoadDataFromLocalStorage()
    {
        if (File.Exists(save_path))
        {
            BinaryFormatter binary_formatter = new BinaryFormatter();
            FileStream stream = new FileStream(save_path, FileMode.Open);
            save_data = binary_formatter.Deserialize(stream) as SaveFile;
            stream.Close();
        }
        else
        {
            Debug.LogError("Save file not Found in" + save_path);
          
        }
    }
    public void SaveDataToLocalStorage()
    {
        BinaryFormatter binary_formatter = new BinaryFormatter();
        FileStream stream = new FileStream(save_path, FileMode.Create);
        save_data.current_rank = rank_manager.current_rank;
        save_data.current_points = rank_manager.total_rank_points;

        binary_formatter.Serialize(stream, save_data);
        stream.Close();
    }
#if UNITY_EDITOR
    [ContextMenu("ResetData")]
    public void ResetData()
    {
        save_data.current_rank = 0;
        save_data.current_points = 0;
        BinaryFormatter binary_formatter = new BinaryFormatter();
        FileStream stream = new FileStream(save_path, FileMode.Create);
        binary_formatter.Serialize(stream, save_data);
        stream.Close();
    }
#endif
}
