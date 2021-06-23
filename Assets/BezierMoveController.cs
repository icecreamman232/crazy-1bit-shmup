using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoopType
{
    None = 0,

    //Run from A->B and back from B->A
    Loop = 1,

    //Run from A->B and back from B->A
    PingPong = 2,
}

[AddComponentMenu("SelfComponent/BezierMoveController", 0)]
public class BezierMoveController : MonoBehaviour
{
    //If it's true, object will move step by step to the first point
    public bool moveToPath;

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            if(value <= 0)
            {
                Debug.Log("Zero movespeed");
                //Just for safety
                moveSpeed = 1;
            }
            else
            {
                moveSpeed = value;
            }
        }
    }

    public LoopType loopType;
    public PathSegment path;

    //Call after finish a path
    public System.Action OnMoveEnd;


    private float interpolateAmount;
    private bool isMoving;
    private Coroutine movingCoroutine;
    private bool isGoingBackward;
    private void OnEnable()
    {
        isMoving = false;
        isGoingBackward = false;
       
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetPath(PathSegment newPath)
    {
        path = newPath;
        //path.CopyPath(newPath);
    }
    public void StartMove(LoopType type)
    {
        if(movingCoroutine!=null) { return; }
        //Start new fresh running
        loopType = type;

        interpolateAmount = 0;      
        isMoving = true;
        isGoingBackward = false;
        if(moveToPath)
        {
            StartCoroutine(OnMovingToPath());
        }
        else
        {
            transform.position = path.GetPos(0);
            movingCoroutine = StartCoroutine(OnMoving());
        }
        
    }
    public void Stop()
    {
        //reset position
        transform.position = path.GetPos(0);
        isMoving = false;
        movingCoroutine = null;
        interpolateAmount = 0;       
        isGoingBackward = false;
        
    }
    public void Pause()
    {
        if (movingCoroutine != null)
        {
            StopCoroutine(movingCoroutine);
            movingCoroutine = null;
        }
    }
    public void Resume()
    {
        if (movingCoroutine == null)
        {
            movingCoroutine = StartCoroutine(OnMoving());
        }
    }
    private IEnumerator OnMovingToPath()
    {
        float t = 0;
        while(true)
        {
            if((1f-t) < 0.01f)
            {
                movingCoroutine = StartCoroutine(OnMoving());
                yield break;
            }
            t = (t + moveSpeed * Time.deltaTime) % 1f;
            transform.position = Vector3.Lerp(transform.position, path.GetPos(0), t);
            yield return null;
        }
    }
    private IEnumerator OnMoving()
    {
        while(isMoving)
        {
            //If object move to next chunk of segment then calculate next chunk based on lerp function
            if(transform.position == LerpMoving(isGoingBackward,interpolateAmount))
            {
                interpolateAmount = interpolateAmount + 0.1f;
            }
            
            if (interpolateAmount >= 1.2f)
            {
                if (loopType == LoopType.None)
                {                   
                    Pause();
                }
                else if(loopType == LoopType.Loop)
                {
                    transform.position = path.GetPos(0);
                    interpolateAmount = 0;

                }
                else if(loopType == LoopType.PingPong)
                {
                    isGoingBackward = !isGoingBackward;
                    interpolateAmount = 0;
                }
                OnMoveEnd?.Invoke();
            }
            
            if(loopType == LoopType.PingPong && isGoingBackward)
            {
                //Moving from current postion to next chunk of segment
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    LerpMoving(isGoingBackward, interpolateAmount),
                    Time.deltaTime * moveSpeed);

            }
            else
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    LerpMoving(isGoingBackward, interpolateAmount),
                    Time.deltaTime * moveSpeed);
            }      
            yield return null;
        }
    }
    private Vector3 QuadratirLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }
    private Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c,Vector3 d, float t)
    {
        Vector3 ab_bc = QuadratirLerp(a, b, c, t);
        Vector3 bc_cd = QuadratirLerp(b, c, d, t);

        return Vector3.Lerp(ab_bc, bc_cd, t);
    }
    private Vector3 LerpMoving(bool isBackward, float t)
    {
        //Just helper function to make the core logic easy to look at
        if(isBackward)
        {
           return CubicLerp(
                path.GetPos(3),
                path.GetPos(2),
                path.GetPos(1),
                path.GetPos(0),
                interpolateAmount);
        }
        else
        {
           return CubicLerp(
                path.GetPos(0),
                path.GetPos(1),
                path.GetPos(2),
                path.GetPos(3),
                interpolateAmount);
        }
        
    }
}
