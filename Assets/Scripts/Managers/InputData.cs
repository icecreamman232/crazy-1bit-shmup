using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[Serializable]
public class PreDefinedInput
{
    public string hashCode;
    public KeyCode keyCode;
    
}


[CreateAssetMenu(fileName ="Control Input",menuName ="Data Object",order = 3)]
public class InputData : ScriptableObject
{
    public List<PreDefinedInput> preDefinedInputList;

    public Dictionary<string, KeyCode> inputDict;

    

}
