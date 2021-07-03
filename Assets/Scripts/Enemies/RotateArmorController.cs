using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateArmorController : MonoBehaviour
{
    public float speed;
    public List<CircleArmor> armorList;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Active()
    {
        for(int i =0; i < armorList.Count; i++)
        {
            armorList[i].Active(speed);
            
        }
    }
#if UNITY_EDITOR
    #region Gizmo
    private void OnDrawGizmos()
    {
        //if (armorList != null)
        //{
        //    Debug.Log("enable");
        //    for (int i = 0; i < armorList.Count; i++)
        //    {
        //        Vector3 dir = transform.position - armorList[i].transform.position;
        //        dir = Quaternion.Euler(new Vector3(0, 0, (i + 1) * 60f)) * dir;
        //        armorList[i].transform.position = dir + transform.position;
        //    }
        //}
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
    #endregion
#endif
}
