using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleMan : MonoBehaviour
{
    #region test
    //public GunAction gun;
    //public MoveAction move;
    //public Coroutine curCoroutine;
    //public System.Action<int,Coroutine> announce;
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        gun.isRunning = true;
    //        gun.callback = HandleAction;
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        move.isRunning = true;
    //        //move.callback = HandleAction;
    //    }

    //}
    //private IEnumerator HoldMove(float sec)
    //{
    //    yield return new WaitForSeconds(sec);
    //    move.isPaused = false;
    //}
    //public void HandleAction(int code)
    //{
    //    if(code == 1)
    //    {
    //        move.isPaused = true;
    //        //Pause the moving for a sec so the gun can be shot
    //        StartCoroutine(HoldMove(10f));
    //    }
    //}
    //private IEnumerator Wait(Coroutine coroutine)
    //{
    //    yield return new WaitForSeconds(1.0f);

    //}
    #endregion
    public BezierMoveController moveController;
    public EnemyGunController gun;
    public bool readyShoot;

    public float counter;
    private void Start()
    {
        moveController.StartMove(LoopType.PingPong);
        gun.SetupGun();
        gun.Done1Shot += ResumeMoving;
        readyShoot = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            readyShoot = true;
        }
        if(readyShoot)
        {
            if (counter >= 1.5f)
            {
                counter = 0f;
                moveController.Pause();
                gun.ShootTest();
            }
            counter += Time.deltaTime;
        }
        

    }
    IEnumerator WaitForShot()
    {
        yield return new WaitForSeconds(0.08f);
        moveController.Resume();
    }
    public void ResumeMoving()
    {
        //moveController.Resume();
       StartCoroutine(WaitForShot());
    }
}
