using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(BackgroundColorAttribute))]
public class BackgroundColorDecorator : DecoratorDrawer
{
    BackgroundColorAttribute attr
    {
        get 
        {
            return (BackgroundColorAttribute)attribute;
        }
    }
    public override float GetHeight()
    {
        return 0;
    }
    public override void OnGUI(Rect position)
    {
        GUI.backgroundColor = attr.color;
        //EditorStyles.label.normal.textColor = Color.yellow;
    }

}
