using UnityEngine;
using System.Collections;

public class PassInput : MonoBehaviour {

	public GameObject target;

	void TouchDown()
	{
		target.SendMessage("TouchDown", SendMessageOptions.DontRequireReceiver); 
	}

	void TouchUp()
	{
		target.SendMessage("TouchUp", SendMessageOptions.DontRequireReceiver); 
	}
}
