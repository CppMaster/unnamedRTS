using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ground : MonoBehaviour {

	public const int areaSize = 200;
	public const float visibleOversize = 1.25f;
	InputManager input;

	void Start () 
	{
		transform.localScale = Vector3.one * areaSize * visibleOversize;
		input = InputManager.Instance;
	}

	void TouchClick(RaycastHit hit)
	{
		if(input.TouchStayAny) return;
		Vector2 targetPosition = new Vector2(hit.point.x, hit.point.z);
		List<Selectable> selection = SelectionManager.Instance.Selection;
		foreach(Selectable unit in selection)
		{
			unit.GetComponent<UnitMovement>().TargetPosition = targetPosition;
		}
	}

}
