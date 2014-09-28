using UnityEngine;
using System.Collections;

public class ActivableButton : Button {
	
	protected bool isActive = false;
	
	void Update()
	{

	}

	public override void TouchDown()
	{

	}
	
	public virtual void TouchClick()
	{
		isActive = !isActive;
		renderer.material = isActive ? activeMaterial : inactiveMaterial;
	}
}
