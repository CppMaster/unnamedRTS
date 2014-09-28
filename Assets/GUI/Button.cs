using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	public Material inactiveMaterial;
	public Material activeMaterial;

	public virtual void TouchDown()
	{
		renderer.material = activeMaterial;
	}

	void Update()
	{
		if(!InputManager.Instance.Touch) renderer.material = inactiveMaterial;
	}
}
