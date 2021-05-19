using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GunDataObject",menuName ="Gun/Gun Data",order = 0)]
public class GunDO : ScriptableObject
{

    public float fireRate;

    [Tooltip("Số lượt đạn trong 1 lần bắn")]
    public int numWavePerShot;

    public float delayperWaveShot;


    [Tooltip("Thông tin các bullet trong 1 wave")]
    public List<BulletShot> waveShot;

}

[System.Serializable]
public class BulletShot
{
    public Vector3  m_Position;
    public Vector3  m_Rotation;
    public float    m_delay;
        
}


