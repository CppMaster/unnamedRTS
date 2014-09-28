using UnityEngine;
using System.Collections;

public class MinimapRenderer : MonoBehaviour {

	public Material material;
	public RenderTexture texture;
	public Camera guiCamera;
	public Transform targetQuad;

	void Start()
	{
		Set ();
	}

	[ContextMenu ("Set")]
	void Set()
	{
		camera.orthographicSize = Ground.areaSize / 2;
		float finalSize = targetQuad.lossyScale.y / (guiCamera.orthographicSize * 2f);
		camera.rect = new Rect(
			(targetQuad.position.x / (guiCamera.orthographicSize * 2) + (Camera.main.aspect - finalSize) / 2) / Camera.main.aspect
			, targetQuad.position.y / (guiCamera.orthographicSize * 2) + 0.5f - finalSize / 2
			, finalSize / Camera.main.aspect
			, finalSize);
	}
}
