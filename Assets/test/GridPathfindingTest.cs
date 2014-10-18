using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using PathFinding;

public class GridPathfindingTest : MonoBehaviour {

	public IntVector2D start;
	public IntVector2D end;
	public int mode = 2;
	public List<Vector3> path;

	void Start()
	{
		Grid grid = GameObject.FindObjectOfType<PathFinding.Grid>();
		path = grid.GridToWorldPosition(grid.Search(start, end, mode));

		StartCoroutine(CheckTime());
	}

	void OnDrawGizmos()
	{
		if(path == null) return;
		for(int a = 0; a < path.Count - 1; ++a)
		{
			Debug.DrawLine(path[a] + Vector3.up, path[a+1] + Vector3.up);
		}
	}

	IEnumerator CheckTime()
	{
		yield return new WaitForSeconds(1f);
		Grid grid = GameObject.FindObjectOfType<PathFinding.Grid>();
		for(int a = 0; a < 3; ++a)
		{
			float milis = Time.realtimeSinceStartup;
			grid.GridToWorldPosition(grid.Search(start, end, a));
			float newMilis = Time.realtimeSinceStartup;
			Debug.Log("Mode: " + a + ", time: " + (newMilis - milis).ToString());
		}
	}
}
