using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTargetByTimeComponent : MonoBehaviour
{
    public Transform originPositon;
    public Transform targetTransform;

    public float lastMinDistance;
    public float minDistance;
    public float rangeLimit;
    private float distanceFromObjectToShip;
    private float distance;
    public bool isMoving;
    public Vector3 centerPoint;
    public bool isShowGuideline;
    public float timer;
    private Vector3 lastTargetPosition;

    void Start()
    {
        isMoving = false;
    }

    void Update()
    {
       
        if (!isMoving)
        {
            var delay = Random.Range(0.1f, 0.5f);
            LeanTween.move(gameObject, originPositon.position, delay).setDelay(1.0f).setOnComplete(
                () =>
                {
                    isMoving = true;
                    timer = 0;
                    lastTargetPosition = originPositon.position;
                }
                );
        }
        else
        {
            timer += Time.deltaTime;
            //if(lastTargetPosition!=originPositon.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, 6.0f * Time.deltaTime);
                float distance = Vector3.Distance(transform.position, originPositon.position);
                if (distance > rangeLimit)
                {
                    Vector3 fromOriginToObject = transform.position - originPositon.position;
                    fromOriginToObject *= rangeLimit / distance;
                    transform.position = originPositon.position + fromOriginToObject;
                }

            }
            if (timer >= 1.5f)
            {
                isMoving = false;
            }

        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (isShowGuideline)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(transform.position, Vector3.one * 0.08f);
            Gizmos.DrawWireSphere(originPositon.position, rangeLimit);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(originPositon.position, minDistance);
        }


    }
#endif



}
