using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;

public class CreatePathRunTimeTest : MonoBehaviour
{
    public splineMove moveController;
    public List<Vector3> positions;
    private PathManager path;


    private void Start()
    {
        moveController = GetComponent<splineMove>();
        path = GetComponent<PathManager>();
        //CreatePathRuntime();

    }
    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            CreatePathRuntime();
            moveController.StartMove();
        }
    }
    public void CreatePathRuntime()
    {
        Transform[] waypoints = new Transform[5];
        for (int i = 0;i <5;i++)
        {
            positions[i] = new Vector3(Random.Range(-2.3f, 2.3f), Random.Range(-2.3f, 2.3f));
            GameObject newPts = new GameObject("waypoint+ "+i);
            waypoints[i] = newPts.transform;
            waypoints[i].position = positions[i];
        }

        path.Create(waypoints, false);

        moveController.pathContainer = path;
    }
}
