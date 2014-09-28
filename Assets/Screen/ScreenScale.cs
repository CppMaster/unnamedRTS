using UnityEngine;
using System.Collections;

public class ScreenScale {

	static ScreenScale _instance = null;
	public static ScreenScale Instance
	{
		get 
		{
			if(_instance == null) _instance = new ScreenScale();
			return _instance;
		}
	}

	float realScreenHeight = 30f;
	public float RealScreenHeight
	{
		get {return realScreenHeight;}
	}

	public ScreenScale()
	{
		realScreenHeight = Screen.dpi > 0f ? 25.4f * Screen.height / Screen.dpi : 100f;
	}

}
