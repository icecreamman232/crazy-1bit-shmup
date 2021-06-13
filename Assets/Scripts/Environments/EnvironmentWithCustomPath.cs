using SWS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentWithCustomPath : BaseEnvironment
{
    [SerializeField]
    protected PathManager introPath;
    [SerializeField]
    protected PathManager patrolPath;
    [SerializeField]
    protected PathManager retreatPath;

    public splineMove moveController;

    public virtual void Move()
    {
        
    }

    public virtual void Patrol()
    {
        
    }

    public virtual void Retreat()
    {
        
    }
    public virtual void Stop()
    {

    }
    public virtual void OnMoveEnd()
    {
        //Entity finished its run and ready to be removed
        OnDestroy?.Invoke();
    }
}
