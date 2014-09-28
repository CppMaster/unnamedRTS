using UnityEngine;
using System.Collections;

public class MapScrolling : MonoBehaviour {

	public Camera _camera;
	public float scrollFactor = 1f;
	Vector3 minCameraPos;
	Vector3 maxCameraPos;

	void Start()
	{
		if(_camera == null) _camera = Camera.main;
		Update();
		minCameraPos = CameraPositionByGround(Vector2.one * -Ground.areaSize * 0.5f);
		maxCameraPos = CameraPositionByGround(Vector2.one * Ground.areaSize * 0.5f);
	}

	void Update()
	{
		Vector2 scroll = InputManager.Instance.GetScroll();
		if(scroll.sqrMagnitude > 0.000001f)
		{
			_camera.transform.Translate(new Vector3(scroll.x,0f,scroll.y) * -scrollFactor, Space.World);
			BoundToMap();
		}
	}

	void BoundToMap()
	{
		Vector3 cameraPos = _camera.transform.position;
		if(cameraPos.x < minCameraPos.x)
			cameraPos.x = minCameraPos.x;
		if(cameraPos.z < minCameraPos.z)
			cameraPos.z = minCameraPos.z;
		if(cameraPos.x > maxCameraPos.x)
			cameraPos.x = maxCameraPos.x;
		if(cameraPos.z > maxCameraPos.z)
			cameraPos.z = maxCameraPos.z;
		_camera.transform.position = cameraPos;

	}

	public Vector3 CameraPositionByGround(Vector2 groundPosition)
	{
		RaycastHit cameraHit;
		Ray cameraRay = _camera.ScreenPointToRay(new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0));
		Physics.Raycast(
			cameraRay,
			out cameraHit,
			float.PositiveInfinity,
			1 << LayerMask.NameToLayer("Ground"));
		Vector3 cameraOffset = _camera.transform.position - cameraHit.point;
		Vector3 targetPos = new Vector3(groundPosition.x, 0, groundPosition.y);
		return targetPos + cameraOffset;
	}

	public void LookAt(Vector2 groundPosition)
	{
		_camera.transform.position = CameraPositionByGround(groundPosition);
	}
}
