using UnityEngine;
using System.Collections;

public class MinimapClick : MonoBehaviour {

	public Camera cam;
	MapScrolling scroll;

	void Start()
	{
		if(cam == null) cam = Camera.main;
		scroll = cam.GetComponent<MapScrolling>();
	}

	void TouchDrag(RaycastHit hit)
	{
		Vector3 localPos = transform.worldToLocalMatrix.MultiplyPoint(hit.point);
		scroll.LookAt(localPos * Ground.areaSize);
	}
}
