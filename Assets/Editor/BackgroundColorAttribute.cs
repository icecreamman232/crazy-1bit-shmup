using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorAttribute : PropertyAttribute
{
    public float r;
    public float g;
    public float b;
    public float a;

    public BackgroundColorAttribute()
    {
        r = g = b = a = 1.0f;
    }

    public BackgroundColorAttribute(float inputR, float inputG, float inputB, float inputA)
    {
        r = inputR;
        g = inputG;
        b = inputB;
        a = inputA;
    }
    public Color color 
    {
        get
        {
            return new Color(r, g, b, a);
        }
    }

}
