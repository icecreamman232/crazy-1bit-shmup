using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithLerp : MonoBehaviour
{
    public float speed;
    public Transform startPos;
    public Transform endPos;
    public float journeyLength;
    public float timeTaken;

    public PathSegment path;
    private float startTime;
    
    public bool isMoving;

    [Range(0f,1f)]
    public float interpolateAmount;

    public float tLerp = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    startTime = Time.time;
        //    startPos.position = transform.position;
        //    journeyLength = Vector3.Distance(endPos.position,startPos.position);
        //    isMoving = true;
        //}
        //if(isMoving)
        //{
        //    float distCovered = (Time.time - startTime) * speed;
        //    timeTaken = Time.time - startTime;
        //    float fractionOfJourney = distCovered / journeyLength;
        //    transform.position = Vector3.Lerp(startPos.position, endPos.position, fractionOfJourney);
        //    if(fractionOfJourney >= 1.0f)
        //    {
        //        isMoving = false;
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.R))
        {
            isMoving = true;
            
        }
        if(isMoving)
        {
            //transform.position = Vector3.MoveTowards(transform.position,
            //    Vector3.Lerp(transform.position, endPos.position, tLerp),
            //Time.deltaTime * speed);
            if (interpolateAmount >= 1.2f)
            {
                isMoving = false;
            }
            transform.position = Vector3.MoveTowards(transform.position,
                CubicLerp(
                path.GetPos(0),
                path.GetPos(1),
                path.GetPos(2),
                path.GetPos(3),
                interpolateAmount),
            Time.deltaTime * speed);
            if(transform.position == CubicLerp(
                path.GetPos(0),
                path.GetPos(1),
                path.GetPos(2),
                path.GetPos(3),
                interpolateAmount))
            {
                interpolateAmount = interpolateAmount + 0.1f;
                
            }

        }
        //transform.position = CubicLerp(
        //        path.GetPos(0),
        //        path.GetPos(1),
        //        path.GetPos(2),
        //        path.GetPos(3),
        //        interpolateAmount);


    }
    private Vector3 QuadratirLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }
    private Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadratirLerp(a, b, c, t);
        Vector3 bc_cd = QuadratirLerp(b, c, d, t);

        return Vector3.Lerp(ab_bc, bc_cd, t);
    }

}
