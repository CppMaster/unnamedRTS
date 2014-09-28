using UnityEngine;
using System.Collections;

public class HitGroundTest : MonoBehaviour {

	public GameObject toSpawn;
	public float touchDragTolerance = 5f;
	
	void TouchClick(RaycastHit hit)
	{
		Vector3 newHitPos = hit.point;
		if (!InputManager.Instance.TouchStay) 
		{
			Instantiate(
				toSpawn, 
				newHitPos + toSpawn.transform.position,
				Quaternion.Euler(hit.normal));
		}
	}
	
}
