using UnityEngine;
using System.Collections;

public class GameCameraScale : MonoBehaviour {

	public AnimationCurve sizeByRealHeight;
	public AnimationCurve zoomByPinch;
	private float zoom = 1f;
	[Range (0f, 1f)]
	public float currPinch = 0.5f;
	public float pinchFactor = 1f;
	public bool lockZoom = false;

	public float Zoom
	{
		get {return zoom;}
		set 
		{
			zoom = value;
			transform.Translate(Vector3.up * (size * zoom - transform.position.y), Space.World);
		}
	}

	private float size = 10f;
	
	void Awake()
	{
		size = sizeByRealHeight.Evaluate(ScreenScale.Instance.RealScreenHeight);
		Zoom = 1f;
	}

	void Update()
	{
		if(lockZoom) return;
		currPinch = Mathf.Clamp(currPinch + pinchFactor * InputManager.Instance.GetPinch(), 0f, 1f);
		Zoom = zoomByPinch.Evaluate(currPinch);
	}

}
