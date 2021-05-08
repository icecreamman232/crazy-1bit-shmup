using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    
    

    private void Awake()
    {
        Instance = this;
    }
    public int GetRandomDropRate()
    {
        return Random.Range(0, 1000);
    }
}
