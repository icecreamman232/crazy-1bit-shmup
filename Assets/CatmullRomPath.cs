using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullRomPath : MonoBehaviour
{
    ///Use the transforms of GameObjects in 3d space as your points or define array with desired points
	public Transform[] points;

	// Store points on the Catmull curve so we can visualize them
	List<Vector2> newPoints = new List<Vector2>();

	// How many points you want on the curve
	uint numberOfPoints = 30;

	// Parametric constant: 0.0 for the uniform spline, 0.5 for the centripetal spline, 1.0 for the chordal spline
	public float alpha = 1f;

	/////////////////////////////

	void Update()
	{
		CatmulRom();
	}

	void CatmulRom()
	{
		newPoints.Clear();

		Vector2 p0 = points[0].position; // Vector3 has an implicit conversion to Vector2
		Vector2 p1 = points[1].position;
		Vector2 p2 = points[2].position;
		Vector2 p3 = points[3].position;

		float t0 = 0.0f;
		float t1 = GetT(t0, p0, p1);
		float t2 = GetT(t1, p1, p2);
		float t3 = GetT(t2, p2, p3);

		for (float t = t1; t < t2; t += ((t2 - t1) / (float)numberOfPoints))
		{
			Vector2 A1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
			Vector2 A2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
			Vector2 A3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;

			Vector2 B1 = (t2 - t) / (t2 - t0) * A1 + (t - t0) / (t2 - t0) * A2;
			Vector2 B2 = (t3 - t) / (t3 - t1) * A2 + (t - t1) / (t3 - t1) * A3;

			Vector2 C = (t2 - t) / (t2 - t1) * B1 + (t - t1) / (t2 - t1) * B2;

			newPoints.Add(C);
		}
	}

	float GetT(float t, Vector2 p0, Vector2 p1)
	{
		float a = Mathf.Pow((p1.x - p0.x), 2.0f) + Mathf.Pow((p1.y - p0.y), 2.0f);
		float b = Mathf.Pow(a, alpha * 0.5f);

		return (b + t);
	}

	// Visualize the points
	void OnDrawGizmos()
	{
		CatmulRom();
		Gizmos.color = Color.red;
		foreach (Vector2 temp in newPoints)
		{
			Vector3 pos = new Vector3(temp.x, temp.y, 0);
			//Gizmos.DrawSphere(pos, 0.3f);
			Gizmos.DrawCube(pos, Vector3.one * 0.3f);
		}

		for(int i =0; i < points.Length; i++)
        {
			if(i==0 || i == points.Length-1)
            {
				Gizmos.color = Color.green;
            }
            else
            {
				Gizmos.color = Color.red;
			}
        }
		UnityEditor.Handles.DrawDottedLine(points[0].position, points[1].position, 3f);
		UnityEditor.Handles.DrawDottedLine(points[points.Length-1].position, points[points.Length-2].position, 3f);
	}

}
