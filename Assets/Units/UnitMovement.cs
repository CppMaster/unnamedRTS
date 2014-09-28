using UnityEngine;
using System.Collections;

public class UnitMovement : MonoBehaviour {

	const float nearEnough = 0.25f;
	public float speed = 1f;

	Vector2 targetPosition;
	public Vector2 TargetPosition
	{
		set
		{
			targetPosition = value;
			realTargetPosition.x = targetPosition.x;
			realTargetPosition.z = targetPosition.y;
			isOnWay = true;
		}
	}


	Vector3 realTargetPosition = Vector3.zero;
	bool isOnWay = false;

	void Start()
	{
		realTargetPosition = transform.position;
	}

	void FixedUpdate () 
	{
		if(isOnWay)
		{
			Vector3 positionDiff = realTargetPosition - transform.position;
			if(positionDiff.sqrMagnitude < nearEnough) isOnWay = false;
			rigidbody.AddForce((positionDiff.sqrMagnitude > 1f ? positionDiff.normalized : positionDiff)* speed, ForceMode.VelocityChange);
		}
	}
}
