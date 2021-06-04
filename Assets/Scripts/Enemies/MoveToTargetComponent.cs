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
        if(targetTransform==null)
        {
            targetTransform = GameManager.Instance.space_ship.transform;
        }
    }

    private void Update()
    {
        float distance_from_center_to_ship = Vector3.Distance(originPositon.position, targetTransform.position);
        if(distance_from_center_to_ship < triggeredRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, originPositon.position, zoomInSpeed * Time.deltaTime);
            float distance_from_object_to_center = Vector3.Distance(transform.position, originPositon.position);
            if(distance_from_object_to_center < innerRange)
            {
                Vector3 direction = targetTransform.position - originPositon.position;
                transform.position = originPositon.position + Vector3.ClampMagnitude(direction, innerRange);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, zoomOutSpeed * Time.deltaTime);
            float distance_from_center_to_object = Vector3.Distance(originPositon.position, transform.position);
            if(distance_from_center_to_object > outerRange)
            {
                Vector3 direction = targetTransform.position - transform.position;
                transform.position = originPositon.position + Vector3.ClampMagnitude(direction, outerRange);
            }
               
        }
    }
    void minorUpdate()
    {
        //Zoom in
        if (distanceFromObjectToShip <= triggeredRange)
        {
            //Vector3 targetDirect = targetTransform.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, originPositon.position, zoomInSpeed * Time.deltaTime);
            float distanceFromObjecToCenter = Vector3.Distance(transform.position, originPositon.position);
            if (distanceFromObjecToCenter <= innerRange)
            {
                Debug.Log("here");
                Vector3 direction = transform.position - originPositon.position;
                transform.position = originPositon.position + Vector3.ClampMagnitude(direction, innerRange);
            }
            //Vector3 direction = transform.position - originPositon.position;
            //transform.position = originPositon.position + Vector3.ClampMagnitude(direction, innerRange);          
        }
        //Zoom out
        else
        {
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
