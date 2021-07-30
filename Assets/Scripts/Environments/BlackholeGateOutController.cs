using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BlackholeGateOutController : EnvironmentWithCustomPath
{
    public bool isShowGizmo = true;
    public BlackholeGateInController gateIn;
    public Coroutine pushCoroutine;
    public System.Action StopCurrentFunction;
    public GameObject pointOut;


    SpaceShipController shipController;

    Animator shipAnimator;

    private Coroutine pullingMonsterAtGateCoroutine;
    private Coroutine pushingMonsterAtGateCoroutine;

    public bool isProcessing;
    public bool isProcessingMonster;


    public Transform destShip;
    public Transform destMonster;


    private void Start()
    {
        shipController = GameManager.Instance.spaceShip.GetComponent<SpaceShipController>();
        shipAnimator = GameManager.Instance.spaceShip.GetComponent<Animator>();
        isProcessing = false;
    }
    public void CurrentCoroutine()
    {
        if(pushCoroutine!=null)
        {
            StopCoroutine(pushCoroutine);
            pushCoroutine = null;
        }
    }
    public override void Setup()
    {
        base.Setup();
        StopCurrentFunction += gateIn.CurrentCoroutine;
    }
    public override void Spawn()
    {
        base.Spawn();
        Move();
    }
    public override void Move()
    {
        base.Move();
        bezierMoveController.OnMoveEnd -= OnMoveEnd;
        bezierMoveController.OnMoveEnd += OnMoveEnd;
        bezierMoveController.SetPath(intro);
        bezierMoveController.Stop();
        bezierMoveController.StartMove(LoopType.None);
    }
    #region Push / Pull with Gate In

    public void PushingForShip(GameObject ship)
    {
        LeanTween.rotateAroundLocal(ship, Vector3.forward, 360f, 0.3f);
        LeanTween.scale(ship, Vector3.one, 0.3f);
        LeanTween.move(ship, destShip.position, 0.4f)
            .setOnComplete(() => {
                ship.transform.rotation = Quaternion.identity;
                ship.transform.localScale = Vector3.one;
                shipAnimator.Play("ship_idle");
                shipController.currentStatus = ShipStatus.NORMAL;
                shipController.BeginShoot();
                isProcessing = false;
            });
    }
    public void PushingForMonster(GameObject monster)
    {
        monster.transform.position = transform.position;
        LeanTween.rotateAroundLocal(monster, Vector3.forward, 360f, 0.4f);
        LeanTween.scale(monster, Vector3.one, 0.4f);
        LeanTween.move(monster, destMonster.position, 1.5f)
            .setOnComplete(() => {
                monster.transform.rotation = Quaternion.identity;
                monster.transform.localScale = Vector3.one;
                BaseMonster baseMons = monster.GetComponent<BaseMonster>();
                baseMons.isInteracting = false;
                baseMons.TakeDamage(baseMons.maxHP);              
                isProcessing = false;
            });
    }
    #endregion

    #region Push / Pull with Gate Out itself
    public void PullingForShip(GameObject ship)
    {
        LeanTween.rotateAroundLocal(ship, Vector3.forward, 360f, 0.3f);
        LeanTween.scale(ship, Vector3.zero, 0.3f);
        LeanTween.move(ship, transform.position, 0.4f)
            .setOnComplete(() => {
                ship.transform.rotation = Quaternion.identity;
                ship.transform.localScale = Vector3.zero;
                PushingForShip(ship);
            });
    }
    public void PullingForMonster(GameObject monster)
    {
        LeanTween.rotateAroundLocal(monster, Vector3.forward, 360f, 0.2f);
        LeanTween.scale(monster, Vector3.zero, 0.2f);
        LeanTween.move(monster, transform.position, 0.3f)
            .setOnComplete(() => {
                monster.transform.rotation = Quaternion.identity;
                monster.transform.localScale = Vector3.zero;
                PushingForMonster(monster);
            });
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GameHelper.IsInsideScreenBounds(transform.position))
        {
            if (!isProcessing)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    shipController.currentStatus = ShipStatus.DISABLE;
                    shipController.StopShoot();
                    PullingForShip(collision.gameObject);
                }
            }
            //Layer 8 = Enemy
            if (collision.gameObject.layer == 8)
            {
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    if(!collision.GetComponent<BaseMonster>().isInteracting)
                    {
                        collision.GetComponent<BaseMonster>().isInteracting = true;
                        collision.GetComponent<BezierMoveController>().Pause();
                        PullingForMonster(collision.gameObject);
                    }                  
                }
            }
          
        } 
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (isShowGizmo)
        {
            GUIStyle styleMons = new GUIStyle();
            styleMons.normal.textColor = Color.red;
            var labelPosMons = destMonster.position;
            labelPosMons.y += 1f;
            Handles.Label(labelPosMons, "Dest Mons", styleMons);
            Gizmos.color = new Color(1.0f, 0, 0, 1.0f);
            Gizmos.DrawSphere(transform.position, 0.2f);

            Gizmos.color = Color.yellow;
            
            Gizmos.DrawCube(transform.GetChild(0).position, 0.3f * Vector3.one);
            Gizmos.DrawLine(transform.position, destMonster.position);



            GUIStyle styleShip = new GUIStyle();
            styleShip.normal.textColor = Color.red;
            var labelPosShip = destShip.position;
            labelPosShip.y += 1f;
            Handles.Label(labelPosShip, "Dest Ship", styleShip);
            Gizmos.color = new Color(1.0f, 0, 0, 1.0f);
            Gizmos.DrawSphere(transform.position, 0.2f);

            Gizmos.color = Color.yellow;

            Gizmos.DrawCube(transform.GetChild(0).position, 0.3f * Vector3.one);
            Gizmos.DrawLine(transform.position, destShip.position);
        }
    }
#endif
}
