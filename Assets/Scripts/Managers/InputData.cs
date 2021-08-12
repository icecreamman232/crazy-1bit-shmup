using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[Serializable]
public class PreDefinedInput
{
    public KeyBindingAction action;
    public KeyCode keyCode;
    
}


[CreateAssetMenu(fileName ="Control Input",menuName ="Data Object",order = 3)]
public class InputData : ScriptableObject
{
    public List<PreDefinedInput> preDefinedInputList;

    public Dictionary<KeyBindingAction, KeyCode> inputDict;

    private void OnEnable()
    {
        inputDict = new Dictionary<KeyBindingAction, KeyCode>();

        for(int i = 0; i < preDefinedInputList.Count; i++)
        {
            inputDict.Add(preDefinedInputList[i].action, preDefinedInputList[i].keyCode);
        }
    }

}
