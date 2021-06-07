using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="EnvironmentStageDO",menuName =GameHelper.dataObjectMenuName+"Environment Stage Data",order =3)]
public class EnvironmentStageDO : ScriptableObject
{
    public List<EnvironmentContainer> listEnvironment;
}

[System.Serializable]
public class EnvironmentContainer
{
    public EnvironmentController environment;
    public float delayTime;
}
