using UnityEngine;
using System.Collections;

public class MapDragButton : ActivableButton {

	public static bool mapDragging = false;

	public override void TouchClick()
	{
		base.TouchClick();
		mapDragging = isActive;
	}

	public float moveFactor = 1f;
	bool isMoving = false;
	Vector2 startPos = Vector2.zero;

	InputManager input;

	void Start()
	{
		input = InputManager.Instance;
	}

	void Update()
	{
		if(!isMoving && mapDragging && input.TouchStay)
		{
			isMoving = true;
			startPos = input.TouchPosition;
		}

		if(!input.Touch) isMoving = false;

		if(isMoving)
		{
			Vector2 currPos = input.TouchPosition - startPos;
			Camera.main.transform.position += new Vector3(currPos.x, 0 , currPos.y) * moveFactor;
			startPos = input.TouchPosition;
		}
	}

}
