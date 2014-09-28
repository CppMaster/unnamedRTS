using UnityEngine;
using System.Collections;

public class GUICameraScale : MonoBehaviour {

	public AnimationCurve sizeByRealHeight;

	void Awake()
	{
		Set();
	}

	[ContextMenu ("Set")]
	void Set()
	{
		camera.orthographicSize = sizeByRealHeight.Evaluate(ScreenScale.Instance.RealScreenHeight);
	}
}
