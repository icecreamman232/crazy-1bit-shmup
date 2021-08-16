using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;        
    }

    public BaseUIController holdToPlayUI;
    public BaseUIController shipHealthBarUI;
    public BaseUIController endGameUI;

}
