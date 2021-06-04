using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetComponent : MonoBehaviour
{
    [Header("Config Numbers")]
    public float innerRange;
    public float outerRange;
    public float triggeredRange;

    public float scaleSpeed;
    public float minScaleAtCenter;
    public float zoomInSpeed;
    public float zoomOutSpeed;


    [Header("Internal fields")]
    public bool isShowGuideline;
    public Transform originPositon;
    public Transform targetTransform;
    private float distanceFromObjectToShip;
    private float distanceFromCenterToShip;

    private void Start()
    {
        targetTransform = GameManager.Instance.space_ship.transform;
    }

    private void Update()
    {    
        //Zoom in
        if (distanceFromObjectToShip <= triggeredRange)
        {
            #region Scale Codeblock
            transform.localScale -= Vector3.one * scaleSpeed * Time.deltaTime;
            if (transform.localScale.x <= minScaleAtCenter)
            {
                transform.localScale = Vector3.one * minScaleAtCenter;
            }
            #endregion
            Vector3 targetDirect = targetTransform.position - transform.position;
            transform.position = Vector3.MoveTowards(targetDirect, originPositon.position, zoomInSpeed * Time.deltaTime);
            Vector3 direction = transform.position - originPositon.position;
            transform.position = originPositon.position + Vector3.ClampMagnitude(direction, innerRange);
            

        }
        //Zoom out
        else
        {
            #region Scale Codeblock
            transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
            if (transform.localScale.x >= 1.0f)
            {
                transform.localScale = Vector3.one;
            }
            #endregion

            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, zoomOutSpeed * Time.deltaTime);
            distanceFromCenterToShip = Vector3.Distance(transform.position, originPositon.position);
            if (distanceFromCenterToShip > outerRange)
            {
                Vector3 fromOriginToObject = transform.position - originPositon.position;
                fromOriginToObject *= outerRange / distanceFromCenterToShip;
                transform.position = originPositon.position + fromOriginToObject;
            }
        }

        distanceFromObjectToShip = Vector3.Distance(transform.position, targetTransform.position);
    }
   


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(isShowGuideline)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(transform.position, Vector3.one * 0.08f);
            Gizmos.DrawWireSphere(originPositon.position, outerRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(originPositon.position, triggeredRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(originPositon.position, innerRange);
        }
        

    }
#endif
}
