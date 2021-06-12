using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;


public interface IMovementWithCustomPath 
{
    PathManager IntroPath { get; set; }
    PathManager PatrolPath { get; set; }
    PathManager RetreatPath { get; set; }
    void Move();
    void Patrol();
    void Retreat();

    void Stop();

}
