using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	static InputManager _instance;
	public static InputManager Instance
	{
		get {return _instance;}
	}

	bool inputMouse = false;
	public bool InputMouse
	{
		get {return inputMouse;}
	}

	public Camera[] cameras;
	public LayerMask[] layerInput;

	Vector2 lastMousePosition;
	Vector2 deltaMousePosition = Vector2.zero;

	void Awake()
	{
		_instance = this;
		if (Application.platform == RuntimePlatform.WindowsEditor 
			|| Application.platform == RuntimePlatform.WindowsPlayer) 
			inputMouse = true;
#if FORCE_TOUCH
		inputMouse = false;
#endif

		if(inputMouse) lastMousePosition = Input.mousePosition;
	}

	bool touch = false;
	bool touchDown = false;
	bool touchUp = false;
	bool touchStay = false;
	bool touchStayAny = false;

	public bool Touch
	{
		get {return touch;}
	}

	public bool TouchDown
	{
		get {return touchDown;}
	}

	public bool TouchUp
	{
		get {return touchUp;}
	}

	public bool TouchStay
	{
		get {return stayTime > stayTimeToDrag;}
	}

	public bool TouchStayAny
	{
		get {return stayTimeAny > stayTimeToDrag;}
	}

	public bool MultiTouch
	{
		get
		{
			if(inputMouse) return Input.GetMouseButton(1);
			return Input.touchCount > 1;
		}
	}

	public float stayTimeToDrag = 0.1f;
	float stayTime = 0f;
	float stayTimeAny = 0f;
	
	GameObject hittedGo;

	Vector2 touchPosition;
	public Vector2 TouchPosition
	{
		get { return touchPosition;}
	}

	void Update()
	{
		deltaMousePosition = (Vector2)Input.mousePosition - lastMousePosition;
		lastMousePosition = Input.mousePosition;

		bool newTouch = Input.GetMouseButton(0) || Input.touchCount > 0;

		touchStay = newTouch && touch ? true && !MultiTouch : false;
		touchStayAny = newTouch && touch;
	
		touch = newTouch;

		if (touch) 
		{
			touchPosition = inputMouse ? (Vector2)Input.mousePosition : Input.touches [0].position;
			touchDown = Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.touches [0].phase == TouchPhase.Began);
			touchUp = (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended);
		} 
		else 
		{
			touchDown = false;
			touchUp = Input.GetMouseButtonUp(0);
		}

		Notify ();

		if(touchStay) stayTime += Time.deltaTime;
		else 
		{
			stayTime = 0f;
		}

		if(touchStayAny) stayTimeAny += Time.deltaTime;
		else 
		{
			stayTimeAny = 0f;
		}
	}

	void Notify()
	{
		if(touchDown)
	   	{
			int layer = 0;
			foreach(Camera cam in cameras)
			{
				RaycastHit hit;
				Physics.Raycast(cam.ScreenPointToRay(touchPosition), out hit, float.PositiveInfinity, layerInput[layer++]);
				if(hit.collider)
				{
					hit.collider.SendMessage("TouchDown", hit, SendMessageOptions.DontRequireReceiver);
					hittedGo = hit.collider.gameObject;
					break;
				}
			}

		}
		else if(touchUp)
		{
			int layer = 0;
			foreach(Camera cam in cameras)
			{
				RaycastHit hit;
				Physics.Raycast(cam.ScreenPointToRay(touchPosition), out hit, float.PositiveInfinity, layerInput[layer++]);
				if(hit.collider)
				{
					hit.collider.SendMessage("TouchUp", hit, SendMessageOptions.DontRequireReceiver);
					if(!TouchStay && hittedGo == hit.collider.gameObject)
					{
						hit.collider.SendMessage("TouchClick", hit, SendMessageOptions.DontRequireReceiver);
					}
					break;
				}
			}
			hittedGo = null;
		}
		else if(touch)
		{
			int layer = 0;
			foreach(Camera cam in cameras)
			{
				RaycastHit hit;
				Physics.Raycast(cam.ScreenPointToRay(touchPosition), out hit, float.PositiveInfinity, layerInput[layer++]);
				if(hit.collider)
				{
					hit.collider.SendMessage("TouchDrag", hit, SendMessageOptions.DontRequireReceiver);
					hittedGo = hit.collider.gameObject;
					break;
				}
			}
		}
	}

	public float GetPinch()
	{
		if(Input.touchCount < 2) return 0f;
		if(Vector2.Dot(Input.touches[0].deltaPosition, Input.touches[1].deltaPosition) > 0f) return 0f;
		float val = Input.touches[0].deltaPosition.magnitude + Input.touches[1].deltaPosition.magnitude;
		Vector2 center = (Input.touches[0].position + Input.touches[1].position) / 2;
		if(Vector2.Dot(Input.touches[0].position - center, Input.touches[0].deltaPosition) > 0) return val;
		return -val;
	}

	public Vector2 GetScroll()
	{
		if(inputMouse && Input.GetMouseButton(1)) return deltaMousePosition;
		if(Input.touchCount < 2) return Vector2.zero;
		if(Vector2.Dot(Input.touches[0].deltaPosition, Input.touches[1].deltaPosition) < 0f) return Vector2.zero;
		return Input.touches[0].deltaPosition + Input.touches[1].deltaPosition;
	}
}
