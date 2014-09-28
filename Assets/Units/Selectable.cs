using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour 
{
	bool isSelected = false;
	GameObject selectionCircle = null;
	public bool IsSelected
	{
		set 
		{
			isSelected = value;
			if(isSelected && selectionCircle == null)
			{
				selectionCircle = Instantiate(Resources.Load("Selection/SelectionCircle")) as GameObject;
				selectionCircle.transform.parent = transform;
				selectionCircle.transform.localPosition = Vector3.zero;
				selectionCircle.transform.Translate(Vector3.forward * (transform.position.y - 0.1f));
				selectionCircle.transform.localScale = Vector3.one * GetSelectionCirleScale();
			}
			else if(!isSelected && selectionCircle != null)
			{
				Destroy(selectionCircle);
				selectionCircle = null;
			}
		}
	}

	protected float GetSelectionCirleScale()
	{
		return GetComponent<SphereCollider>().radius * 2;
	}

	void TouchClick()
	{
		SelectionManager.Instance.Select(this);
	}
}
