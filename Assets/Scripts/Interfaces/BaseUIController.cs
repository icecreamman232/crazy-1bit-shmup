using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUIController : MonoBehaviour
{
    protected bool isShow;
    public bool IsShow
    {
        get
        {
            return isShow;
        }
    }
    public abstract void Show();
    public abstract void Hide();
}
