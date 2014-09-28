using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour {

	static SelectionManager _instance;
	public static SelectionManager Instance
	{
		get {return _instance;}
	}

	void Awake()
	{
		_instance = this;
	}

	InputManager input;
	List<Selectable> selection;
	public List<Selectable> Selection
	{
		get {return selection;}
	}
	Camera guiCamera;

	void Start()
	{
		input = InputManager.Instance;
		selectionRect = new Rect();
		selection = new List<Selectable>();
		selectionQuad.renderer.enabled = false;
		guiCamera = GameObject.Find("GUICamera").camera;
	}



	Vector2 startTouchPos;
	Vector2 currTouchPos;

	public Transform selectionQuad;
	Rect selectionRect;
	bool selectByBox = false;

	void Update()
	{
		if(input.TouchDown)
		{
			startTouchPos = input.TouchPosition;
		}
		if(input.Touch)
		{
			currTouchPos = input.TouchPosition;
		}

		if(input.TouchStay && !input.MultiTouch && !selectByBox && !MapDragButton.mapDragging)
		{
			selectByBox = true;
			selectionQuad.renderer.enabled = true;
		}
		if(selectByBox)
		{
			SetSelectionBox();
			if(input.TouchUp)
			{
				SelectByBox();
			}
		}
		if((!input.Touch || input.MultiTouch) && selectByBox) 
		{
			selectByBox = false;
			selectionQuad.renderer.enabled = false;
			startTouchPos = currTouchPos;
		}
	}

	void SetSelectionBox()
	{
		selectionRect.Set(startTouchPos.x, startTouchPos.y, 
		                  currTouchPos.x - startTouchPos.x, 
		                  currTouchPos.y - startTouchPos.y);
		if(selectionRect.width < 0)
		{
			selectionRect.x += selectionRect.width;
			selectionRect.width *= -1;
		}
		if(selectionRect.height < 0)
		{
			selectionRect.y += selectionRect.height;
			selectionRect.height *= -1;
		}

		selectionQuad.transform.localPosition = new Vector3(
			selectionRect.center.x / Screen.height - 0.5f * Screen.width / Screen.height,
			selectionRect.center.y / Screen.height - 0.5f, 0
			) * guiCamera.orthographicSize * 2;
		selectionQuad.transform.localScale = new Vector3(
			selectionRect.width / Screen.height,
			selectionRect.height / Screen.height, 0
			) * guiCamera.orthographicSize * 2;
	}

	void SelectByBox()
	{
		DeselectAll();

		SelectableByRect[] toSelect = GameObject.FindObjectsOfType<SelectableByRect>();
		foreach(SelectableByRect s in toSelect)
		{
			Vector2 posOnScreen = Camera.main.WorldToScreenPoint(s.transform.position);
			if(selectionRect.Contains(posOnScreen))
			{
				selection.Add(s);
				s.IsSelected = true;
			}
		}
	}

	void DeselectAll()
	{
		foreach(Selectable s in selection)
		{
			s.IsSelected = false;
		}
		selection.Clear();
	}

	public void Select(Selectable s)
	{
		DeselectAll();
		selection.Add(s);
		s.IsSelected = true;
	}
}
